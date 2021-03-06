/*
 * File:        InputManager.cs
 * Date:        1 April 2021
 * 
 * Purpose:     Detect and execute inputs
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    bool isInMenu;

    CameraManager cam;
    [SerializeField] bool doCamClickFollow;

    public void Start()
    {
        cam = CameraManager.Instance;
    }

    /// <summary>
    /// raycast on input and interrupts with next state
    /// </summary>
    private void Update()
    {
        KeyPressInputs();
        if (!isInMenu) ClickInputs();
    }

    private void ClickInputs() {
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
                    //set cam follow target
                    if (doCamClickFollow) cam.SetTarget(hit.transform);

                    //clicked otter is now clicked
                    hit.transform.GetComponent<ProtoOtter>().Clicked();
                }
                else
                {
                    //reset cam follow target
                    if (doCamClickFollow) cam.SetTarget(null);
                }
            }
            else
            {
                //reset cam follow target
                if (doCamClickFollow) cam.SetTarget(null);
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
                    if (doCamClickFollow) cam.SetTarget(hit.transform);

                    //switch hit otter to click state
                    hit.transform.GetComponent<ProtoOtter>().currentState.Clicked();
                }
                else
                {
                    if (doCamClickFollow) cam.SetTarget(null);
                }
            }
            else
            {
                if (doCamClickFollow) cam.SetTarget(null);
            }
        }
    }

    private void KeyPressInputs() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isInMenu = false;
            CanvasManager.Instance.SwitchToTab(0);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            isInMenu = true;
            CanvasManager.Instance.SwitchToTab(1);
        }
    }
}
