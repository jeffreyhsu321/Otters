/*
 * File:        CollisionState.cs
 * Date:        4 April 2021
 * 
 * Purpose:     CollisionState
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionState : State
{
    //states
    [SerializeField] IdleState idleState;

    ProtoOtter otter;

    public override State RunCurrentState(ProtoOtter otter)
    {
        otter.anim.SetInteger("State", 3);

        /*
        //go flying lul 
        otter.agent.enabled = false;
        otter.GetComponent<CapsuleCollider>().isTrigger = false;
        otter.rb.isKinematic = false;
        otter.rb.AddForce((otter.transform.position - otter.other.transform.position) * 100 + Vector3.up * 10);

        this.otter = otter;
        Invoke("restore", 2);
        */


        return idleState;
    }

    public override void StopCurrentState(ProtoOtter otter)
    {
        return;
    }

    private void restore() {
        otter.rb.isKinematic = true;
        otter.GetComponent<CapsuleCollider>().isTrigger = true;
        //otter.transform.rotation = Vector3.zero;
        otter.agent.enabled = true;
    }
    
}
