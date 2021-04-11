/*
 * File:      ProtoOtter.cs
 * Date:      31 March 2021
 * 
 * Purpose:   Behavior controller for ProtoOtters (finite state machine)
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class ProtoOtter : MonoBehaviour
{   
    //components
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody rb;

    [SerializeField] GameObject fish_notif;

    public State currentState;

    //states
    public State collisionState;

    //decision
    public bool doDecision;

    //collision
    [HideInInspector] public Collider other;

    //debug
    [SerializeField] bool debug;


    /// <summary>
    /// retrieve ref to components
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// call to run state machine every frame
    /// </summary>
    void Update()
    {
        RunStateMachine();
    }

    /// <summary>
    /// runs state machine, call switch to next state when current state returns a non-null next state
    /// </summary>
    private void RunStateMachine()
    {
        //run current state and get next state
        State nextState = currentState?.RunCurrentState(this);

        //switch to next state if applicable
        if (nextState != null)
        {
            SwitchToNextState(nextState, false);
        }
    }

    /// <summary>
    /// set current state to next state, stops current state if needed (interruption)
    /// </summary>
    /// <param name="nextState">next state to run</param>
    /// <param name="needStop">true: will run StopCurrentState() on current state before assigning new state</param>
    public void SwitchToNextState(State nextState, bool needStop)
    {
        if (needStop) { currentState.StopCurrentState(this); }
        currentState = nextState;

        //debug: print current state name
        if(debug) Debug.Log(currentState.name);
    }


    /// <summary>
    /// set current state isClicked flag and collect fish
    /// </summary>
    public void Clicked()
    {
        if (fish_notif.activeSelf) { 
            currentState.isClicked = true;          //state interruption
            CrawManager.Instance.GenerateFish(1);   //collect fish
            fish_notif.SetActive(false);            //hide fish notif
        }
    }

    public void GenerateFishNotif()
    {
        Debug.Log("generated fish notif!" + fish_notif);
        fish_notif.SetActive(true);
    }

    
    /// <summary>
    /// collision detection
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //compare for navmesh agent priority and switch lower priority (higher avoidancePriority value) to collision state 
        if (agent.avoidancePriority > other.GetComponent<ProtoOtter>().agent.avoidancePriority)
        {
            this.other = other;
            SwitchToNextState(collisionState, true);
        }
    }

    /// <summary>
    /// debug visuals
    /// </summary>
    void OnDrawGizmos()
    {
        //draws a ray to indicate facing direction
        if(debug) Debug.DrawRay(transform.position, transform.forward, Color.green);
    }
}
