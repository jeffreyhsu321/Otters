/*
 * File:        CrawStructure.cs
 * Date:        9 April 2021
 * 
 * Purpose:     Structures that passively generate income and hold upgrades
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawStructure : MonoBehaviour
{
    public float radius;
    public int length;

    /* (depreciated) direction avaliability for pcg spawning
    public bool east = true, west = true;
    public bool north = true, south = true;
    */

    /* (depreciated) get avaliable direction (for spawning corridors)
    public Vector3 GetAvailableDir()
    {
        if (north == false && east == false && south == false && west == false) return Vector3.zero;

        while (true)
        {
            int dir = Random.Range(0, 4);
            switch (dir)
            {
                case 0:
                    if (north) { north = false; return Vector3.forward; }
                    break;
                case 1:
                    if (east) { east = false; return Vector3.right; }
                    break;
                case 2:
                    if (south) { south = false; return Vector3.back; }
                    break;
                case 3:
                    if (west) { west = false; return Vector3.left; }
                    break;
                default:
                    return Vector3.zero;
            }
        }
    }
    */

    /* (depreciated) set direction avaliability after spawning corridors
    public void SetAvailability(Vector3 buttDir) {
        if (buttDir == Vector3.forward)
        {
            south = false;
        }
        else if (buttDir == Vector3.right)
        {
            west = false;
        }
        else if (buttDir == Vector3.back)
        {
            north = false;
        }
        else {
            east = false;
        }
    }
    */

    /* (depreciated) checks for collision validity for spawning new rooms and corridors
    public bool CheckCollisionValidity(Vector3 dir, float corridor_length, GameObject pf_room) {
        Debug.DrawLine(transform.position + dir * (radius + 0.5f), dir * (radius + corridor_length + pf_room.GetComponent<CrawStructure>().radius), Color.red, 2);
        return Physics.Raycast(transform.position + dir * (radius + 0.5f), dir, radius + corridor_length + pf_room.GetComponent<CrawStructure>().radius, 12);
    }
    */



    //physical
    [SerializeField] Vector3 spawnLocation;

    //stats
    public float baseAmount;
    public float upgradeModIndex;
    [SerializeField] float[] upgradeModList;

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
    public bool AnimateSpawn() {

        //done animating
        if (dissolve_factor_current > dissolve_factor_target - 0.1f) {
            Debug.Log("reached approximate target");
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
    private void SetStableMat() {
        this.GetComponent<Renderer>().material = m_stable;
    }
}
