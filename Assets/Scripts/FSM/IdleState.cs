/*
 * File:        IdleState.cs
 * Date:        4 April 2021
 * 
 * Purpose:     Idle State
 *              Decision tree -> (idle)
 *                            -> (wander)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    //states
    [SerializeField] WanderState wanderState;
    [SerializeField] ClickState clickState;

    //decision
    bool doDecision;
    int timeUntilNextDecision = 0;
    float timeToNextDecision = 0;
    int numOfDecisions = 3;

    //anim
    int anim_state = 0;


    /// <summary>
    /// main behavior loop: runs decision tree
    /// </summary>
    /// <param name="otter"></param>
    /// <returns></returns>
    public override State RunCurrentState(ProtoOtter otter)
    {
        doDecision = otter.doDecision;

        //placeholder: input
        if (Input.GetKeyDown(KeyCode.A) || Input.touchCount > 1)
        {
            return wanderState;
        }

        //animation
        otter.anim.SetInteger("State", 1);
        otter.anim.SetFloat("state_idle", anim_state);

        //run decision tree
        return Decision();
    }
    public override void StopCurrentState(ProtoOtter otter)
    {
        return;
    }

    /// <summary>
    /// decision tree: 
    /// randomly chooses next action after time elasped
    /// </summary>
    /// <returns></returns>
    private State Decision()
    {
        //run timer until threshold
        if (timeUntilNextDecision == 0) {
            timeUntilNextDecision = (int)(Random.value * 20);
            timeToNextDecision = 0;
        }

        //timer increment
        timeToNextDecision += Time.deltaTime;

        //decision
        if (doDecision && timeToNextDecision >= timeUntilNextDecision) {
            timeUntilNextDecision = 0;  //resets timer
            switch (Random.Range(0, numOfDecisions)) {
                case 0:
                    //IDLE
                    anim_state = 0;
                    return this;
                case 1:
                    //IDLE
                    anim_state = 1;
                    return this;
                case 2:
                    //WANDER
                    return wanderState;
                default:
                    return null;
            }
        }
        return null;
    }
}
