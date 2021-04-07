using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 target_offset;
    private Vector3 pos_neutral;

    private void Start()
    {
        if(target != null) target_offset = transform.position - target.position;
        pos_neutral = transform.position;
    }

    private void Update()
    {
        if (target != null) { transform.position = Vector3.Lerp(transform.position, target.position + target_offset, 0.1f); }
        else { transform.position = Vector3.Lerp(transform.position, pos_neutral, 0.1f); }
    }


}