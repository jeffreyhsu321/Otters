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
    //main camera
    [SerializeField] Camera cam;

    //level (position and rotation data)
    [SerializeField] List<Transform> cam_posRot_list = new List<Transform>();   //list of camera position and rotation (stored as GameObjects under Levels)
    private int current_i = 0;                                                  //current index in list of camera pos and rot

    //camera lerp speed
    [SerializeField] int speed_cam_pos;
    [SerializeField] int speed_cam_rot;

    //camera follow
    private Transform target;                   //target GameObject to follow (focus on)
    [SerializeField] Vector3 target_offset;     //offset to keep the target in view
    private Vector3 pos_neutral;                //default position when not following a target

    //rendering (scene specific)
    [SerializeField] float[] fogStartDistance_targets;

    //flags
    bool finishedMoving;


    public void Start()
    {
        pos_neutral = cam_posRot_list[0].position;
    }

    public void Update()
    {
        //PLACEHOLDER DEBUG (go to next or previous level for camera)
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
            //lerp camera pos and rot
            cam.transform.position = Vector3.Slerp(cam.transform.position, pos_neutral, Time.deltaTime * speed_cam_pos);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, cam_posRot_list[current_i].rotation, Time.deltaTime * speed_cam_rot);

            //lerp fog distance
            RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, fogStartDistance_targets[current_i], Time.deltaTime * 1f);
            RenderSettings.fogEndDistance = fogStartDistance_targets[current_i] + 20;
        }
    }

    /// <summary>
    /// set new neutral position to next or previous pre-defined values
    /// </summary>
    /// <param name="goNext">true: go to next; false: go to previous</param>
    public void SetNextPosRot(bool goNext)
    {
        //increment index in cam_posRot_list
        if (goNext) current_i++;
        else current_i--;

        //set new neutral pos, set moving flag, invoke moving couroutine
        pos_neutral = cam_posRot_list[current_i].position;
        finishedMoving = false;
        Invoke("FinishedMoving", 10);
    }

    /// <summary>
    /// invoked in SetNextPosRot() to reset moving flag
    /// </summary>
    private void FinishedMoving() {
        finishedMoving = true;
    }

    /// <summary>
    /// set follow target (called by InputManager)
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
