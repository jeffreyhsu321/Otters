/*
 * File:        HeadTracking.cs
 * Date:        5 April 2021
 * 
 * Purpose:     Head tracking by moving HeadTrackTarget (read and controlled by Animation Rigging: HeadTrackLogic)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(RigBuilder))]

public class HeadTracking : MonoBehaviour
{

    [SerializeField] bool doHeadTrack;

    [SerializeField] List<PointOfInterest> POIs;    //list of point of interests

    [SerializeField] Rig HeadRig;
    [SerializeField] Transform HeadTrackTarget;     //HeadTrackTarget under Model > Rig, as referenced by HeadTrackLogic's Multi-Aim Constraint component

    [SerializeField] float interestRadius = 10f;    //activate head track when poi is within this radius
    float interestRadiusSqr;                        //square of interestRadius, for optimization

    [SerializeField] float maxTurnAngle = 80;
    [SerializeField] float headTurnSpeed = 6;


    private void Start()
    {
        interestRadiusSqr = interestRadius * interestRadius;
    }

    void Update()
    {
        if (!doHeadTrack) return;

        //check proximity and angle to each POI
        PointOfInterest targetPOI = null;
        foreach (PointOfInterest poi in POIs)
        {
            Vector3 v_toTarget = poi.transform.position - transform.position;
            if (v_toTarget.sqrMagnitude < interestRadiusSqr)
            {
                float angle_toTarget = Vector3.Angle(transform.forward, v_toTarget);
                if (angle_toTarget <= maxTurnAngle)
                {
                    targetPOI = poi;
                    break;
                }
            }
        }

        //set HeadTrackTarget to the poi's position
        if (targetPOI != null)
        {
            HeadTrackTarget.position = Vector3.Lerp(HeadTrackTarget.position, targetPOI.transform.position + targetPOI.offset, Time.deltaTime);
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, 1, Time.deltaTime * headTurnSpeed);
        }
        //reduce rig weight to 0 to ignore HeadTrackTarget
        else
        {
            HeadRig.weight = Mathf.Lerp(HeadRig.weight, 0, Time.deltaTime * headTurnSpeed);
        }
    }
}
