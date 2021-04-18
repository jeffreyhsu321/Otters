/*
 * File:        WanderState.cs
 * Date:        4 April 2021
 * 
 * Purpose:     Wander State
 *              Randomly chooses a destination and delegates movement to navmesh agent
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : State
{
    //states
    [SerializeField] IdleState idleState;
    [SerializeField] CollisionState collisionState;

    //wander params
    [SerializeField] bool isWandering;
    [SerializeField] Vector3 wanderTargetLocation;

    //anim calcs
    Vector3 lastFacing;     //vector of last facing direction to calculate angular velocity
    float ang_v;            //angular velocity for animation

    //debug
    public bool debug;


    /// <summary>
    /// main behavior loop: 
    /// set destination on init and delegate movement to navmesh agent
    /// calculates angular velocity and updates animator with ang_v and v
    /// </summary>
    /// <param name="otter"></param>
    /// <returns></returns>
    public override State RunCurrentState(ProtoOtter otter)
    {
        //interruption
        if (isClicked)
        {
            isClicked = !isClicked;
        }

        //wander (navmesh agent)
        if (!isWandering)
        {
            //get random location
            wanderTargetLocation = Random.insideUnitSphere * 20;
            wanderTargetLocation.y = 0;

            otter.agent.speed = Random.Range(3,12);

            //start wander
            otter.agent.SetDestination(wanderTargetLocation);
            isWandering = true;
        }
        
        //stop wander and switch to idle state if reach target location
        if (!otter.agent.pathPending && otter.agent.remainingDistance < 2)
        {
            StopCurrentState(otter);
            return idleState;
        }


        //animation
        otter.anim.SetInteger("State", 2);

        //calculates and updates anim with angular velcoity
        ang_v = Vector3.SignedAngle(otter.transform.forward, lastFacing, Vector3.down) / Time.deltaTime;
        lastFacing = otter.transform.forward;
        otter.anim.SetFloat("ang_v", ang_v);

        //updates anim with velocity params and controls animation speed
        otter.anim.SetFloat("v", otter.agent.velocity.sqrMagnitude);
        otter.anim.speed = Mathf.Clamp(otter.agent.velocity.sqrMagnitude / 5, 1, 1);

        return null;
    }

    /// <summary>
    /// reset values on interruption before exiting current state
    /// </summary>
    /// <param name="otter"></param>
    public override void StopCurrentState(ProtoOtter otter)
    {
        //reset anim params
        otter.anim.SetFloat("ang_v", 0);
        otter.anim.SetFloat("v", 0);

        //reset navmesh agent path
        otter.agent.ResetPath();

        //reset init flag
        isWandering = false;
    }

    /// <summary>
    /// debug visuals
    /// </summary>
    void OnDrawGizmos()
    {
        //wander target
        if (debug)
        {
            if (isWandering)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(wanderTargetLocation, 1);
            }
        }
    }

    
}
