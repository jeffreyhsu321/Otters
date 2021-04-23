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
        public bool isBase;

        public bool east = true, west = true;
        public bool north = true, south = true;

    public int buttDir; //0 = north, 1 = east, 2 = south, 3 = west

    public Vector3 GetAvailableDir()
    {
        if (north == false && east == false && south == false && west == false) return Vector3.zero;

        while (true)
        {
            int dir = Random.Range(0, 4);
            Debug.Log("dir: " + dir);
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
                    Debug.Log("returning zero");
                    return Vector3.zero;
            }
        }
    }

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

    //physical
    [SerializeField] Vector3 spawnLocation;

    //stats
    public float baseAmount;
    public float upgradeModIndex;
    [SerializeField] float[] upgradeModList;

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
    public bool AnimateSpawn() {

        //done animating
        if (Mathf.Approximately(dissolve_factor_current, dissolve_factor_target)) return false;

        //animate
        dissolve_factor_current = Mathf.Lerp(dissolve_factor_current, dissolve_factor_target, Time.deltaTime);
        mat.SetFloat("float_dissolve_factor", dissolve_factor_current);

        return true;
    }

    /*
    public void SpawnAdjacentRoom(GameObject pf_room, GameObject pf_corridor, Transform structures_hierachy_folder) {
        //spawn corridor
        Vector3 dir = GetAvailableDir();
        GameObject corridor = Instantiate(pf_corridor, transform.position + dir * radius, Quaternion.LookRotation(dir), transform);

        //spawn room
        int corridor_length = 2;
        GameObject room = Instantiate(pf_room, transform.position + dir * (radius + corridor_length + pf_room.GetComponent<CrawStructure>().radius), Quaternion.identity, structures_hierachy_folder);
    }
    */


}
