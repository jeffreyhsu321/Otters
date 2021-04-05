/*
 * File:        ClickState.cs
 * Date:        4 April 2021
 * 
 * Purpose:     Click State
 *              Trigger startle animation and return to idle (started by ClickManager)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickState : State
{
    [SerializeField] IdleState idleState;

    public override State RunCurrentState(ProtoOtter otter)
    {
        otter.anim.SetInteger("State", 3);
        return idleState;
    }

    public override void StopCurrentState(ProtoOtter otter)
    {
        return;
    }
}
