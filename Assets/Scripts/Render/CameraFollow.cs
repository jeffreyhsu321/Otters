/*
 * File:        CameraFollow.cs
 * Date:        6 April 2021
 * 
 * Purpose:     Sets camera follow target
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    [SerializeField] Vector3 target_offset;
    private Vector3 pos_neutral;

    /// <summary>
    /// get offset and neutral cam pos
    /// </summary>
    private void Start()
    {
        if(target != null) target_offset = transform.position - target.position;
        pos_neutral = transform.position;
    }

    /// <summary>
    /// lerps cam pos between target and neutral
    /// </summary>
    private void Update()
    {
        if (target != null) { transform.position = Vector3.Lerp(transform.position, target.position + target_offset, 0.1f); }
        else { transform.position = Vector3.Lerp(transform.position, pos_neutral, 0.1f); }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }


}