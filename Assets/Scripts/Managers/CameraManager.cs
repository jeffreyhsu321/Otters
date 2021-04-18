/*
 * File:        CameraManager.cs
 * Date:        13 April 2021
 * 
 * Purpose:     Controls Camera pos rot and follow
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] Camera cam;
    [SerializeField] List<Transform> cam_posRot_list = new List<Transform>();
    private int current_i = 0;

    private Transform target;
    [SerializeField] Vector3 target_offset;
    private Vector3 pos_neutral;

    [SerializeField] float[] fogStartDistance_targets;

    bool finishedMoving;


    public void Start()
    {
        //GoToNextPosRot();
        pos_neutral = cam_posRot_list[0].position;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SetNextPosRot(true);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SetNextPosRot(false);
        }

        //camera follow if target is not null, else return to neutral pos
        if (target != null)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, target.position - cam.transform.forward * 20, 0.1f);
        }
        else if (!finishedMoving)
        {
            cam.transform.position = Vector3.Slerp(cam.transform.position, pos_neutral, Time.deltaTime * 2);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, cam_posRot_list[current_i].rotation, Time.deltaTime * 20);
            RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, fogStartDistance_targets[current_i], Time.deltaTime * 1f);
            RenderSettings.fogEndDistance = fogStartDistance_targets[current_i] + 20;
        }
    }

    public void SetNextPosRot(bool isNext)
    {
        if (isNext)
        {
            current_i++;
        }
        else
        {
            current_i--;
        }

        pos_neutral = cam_posRot_list[current_i].position;
        finishedMoving = false;
        Invoke("FinishedMoving", 10);
    }

    private void FinishedMoving() {
        finishedMoving = true;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
