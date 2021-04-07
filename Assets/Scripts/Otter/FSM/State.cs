/*
 * File:        State.cs
 * Date:        01 April 2021
 * 
 * Purpose:     Abstract State class for the Finite State Machine
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract State RunCurrentState(ProtoOtter otter);
    public abstract void StopCurrentState(ProtoOtter otter);
}
