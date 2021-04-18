/*
 * File:        FishNotif.cs
 * Date:        11 April 2021
 * 
 * Purpose:     Fixate Fish Notif UI element to the parent (otter)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNotif : MonoBehaviour
{
    public Transform parent;
    public Vector3 v_offset;


    public void OnEnable()
    {
        this.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, parent.position + v_offset);
    }
    public void FixedUpdate()
    {
        //transform.position = parent.transform.position + v_offset;
        //this.GetComponent<RectTransform>().anchoredPosition = parent.transform.position + v_offset;
        this.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, parent.position + v_offset);
    }
}
