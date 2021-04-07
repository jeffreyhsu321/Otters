/*
 * File:        ClickManager.cs
 * Date:        1 April 2021
 * 
 * Purpose:     Detect and manage clicks through screen raycast
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    [SerializeField] ClickState clickState;

    [SerializeField] CameraFollow cam;
    [SerializeField] bool doCamClickFollow;

    /// <summary>
    /// raycast on input and interrupts with next state
    /// </summary>
    private void Update()
    {
        //mouse click
        if (Input.GetMouseButtonDown(0))
        {
            //raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //on hit, compare tags
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Otter"))
                {
                    if (doCamClickFollow) cam.target = hit.transform;

                    //switch hit otter to click state
                    hit.transform.GetComponent<ProtoOtter>().SwitchToNextState(clickState, true);
                }
                else
                {
                    if (doCamClickFollow) cam.target = null;
                }
            }
            else
            {
                if (doCamClickFollow) cam.target = null;
            }
        }

        //touch click
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            //raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            //on hit, compare tags
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Otter"))
                {
                    //switch hit otter to click state
                    hit.transform.GetComponent<ProtoOtter>().SwitchToNextState(clickState, true);
                }
                else
                {
                    if (doCamClickFollow) cam.target = null;
                }
            }
            else
            {
                if (doCamClickFollow) cam.target = null;
            }
        }
    }
}
