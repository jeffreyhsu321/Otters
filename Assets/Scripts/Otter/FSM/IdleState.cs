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
    int timerThreshold = 0;
    float timer = 0;
    int numOfDecisions = 3;

    //anim
    [SerializeField] int anim_state = 1;


    /// <summary>
    /// main behavior loop: runs decision tree
    /// </summary>
    /// <param name="otter"></param>
    /// <returns></returns>
    public override State RunCurrentState(ProtoOtter otter)
    {
        doDecision = otter.doDecision;

        //interruption
        if (isClicked) {
            isClicked = !isClicked;
        }

        //placeholder: input
        if (Input.GetKeyDown(KeyCode.A) || Input.touchCount > 1)
        {
            return wanderState;
        }

        //animation
        otter.anim.SetInteger("State", anim_state);

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
        if (timerThreshold == 0) {
            timerThreshold = (int)(Random.value * 20);
            timer = 0;
        }

        //timer increment
        timer += Time.deltaTime;

        //decision
        if (doDecision && timer >= timerThreshold) {
            timerThreshold = 0;  //resets timer
            switch (Random.Range(0, numOfDecisions)) {
                case 0:
                    //IDLE
                    anim_state = 1;
                    return null;
                case 1:
                    //IDLE
                    anim_state = 4;
                    return null;
                case 2:
                    //WANDER
                    //anim_state = 1;
                    return wanderState;
                default:
                    return null;
            }
        }
        return null;
    }
}
