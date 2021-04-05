using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 target_offset;

    private void Start()
    {
        target_offset = transform.position - target.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + target_offset, 0.1f);
    }
}