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
    public Vector3 spawnLocation;
    public Vector3 spawnRotation;

    //animations (shaders)
    [SerializeField] Material m_spawning;
    [SerializeField] Material m_stable;

    [SerializeField] float dissolve_factor_init;
    [SerializeField] float dissolve_factor_target;
    float dissolve_factor_current;
    [SerializeField] int height_scale_mult;


    /// <summary>
    /// init shader values (because shader is shared by different structures that requires different init values)
    /// </summary>
    public void Start()
    {
        m_spawning.SetFloat("float_dissolve_factor", dissolve_factor_init);
        m_spawning.SetFloat("height_scale_mult", height_scale_mult);
    }

    /// <summary>
    /// animates the spawning of a structure, called by CrawManager Update()
    /// </summary>
    /// <returns>true: not done; false: done</returns>
    public bool AnimateSpawn()
    {
        //done animating
        if (dissolve_factor_current > dissolve_factor_target - 0.1f) {
            SetStableMat();
            return false;
        }

        //animate
        dissolve_factor_current = Mathf.Lerp(dissolve_factor_current, dissolve_factor_target, Time.deltaTime);
        m_spawning.SetFloat("float_dissolve_factor", dissolve_factor_current);

        return true;
    }

    /// <summary>
    /// switches material to post-instantiated material after spawn animation is done
    /// </summary>
    private void SetStableMat()
    {
        this.GetComponent<Renderer>().material = m_stable;
    }
}
