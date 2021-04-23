/*
 * File:        CrawAster.cs
 * Date:        22 April 2021
 * 
 * Purpose:     Land masses.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawAster : MonoBehaviour
{
    //physical
    [SerializeField] Vector3 spawnLocation;

    //animations (shaders)
    [SerializeField] Material mat;

    [SerializeField] float dissolve_factor_init;
    [SerializeField] float dissolve_factor_target;
    float dissolve_factor_current;
    [SerializeField] int height_scale_mult;


    /// <summary>
    /// init shader values (because shader is shared by different structures that requires different init values)
    /// </summary>
    public void Start()
    {
        mat.SetFloat("float_dissolve_factor", dissolve_factor_init);
        mat.SetFloat("height_scale_mult", height_scale_mult);
    }

    /// <summary>
    /// animates the spawning of a structure, called by CrawManager Update()
    /// </summary>
    /// <returns>true: not done; false: done</returns>
    public bool AnimateSpawn()
    {
        //done animating
        if (Mathf.Approximately(dissolve_factor_current, dissolve_factor_target)) return false;

        //animate
        dissolve_factor_current = Mathf.Lerp(dissolve_factor_current, dissolve_factor_target, Time.deltaTime);
        mat.SetFloat("float_dissolve_factor", dissolve_factor_current);

        return true;
    }
}
