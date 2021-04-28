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

    //camp
    public Vector3 initial_base_pos;                                            //position of the first spawned structure
    public Vector3[] camp_BuildInfo;                                           //[structure_buildlist_i][0 = structure_i (room to spawn the offshoot of); 1 = dir_i (facing direction); 2 = pf_structures_i (prefab of the offshoot room)]
    public int current_structure_i;                                            //current structure built index for this camp (camp_spawnInfo use)
    public List<CrawStructure> structures = new List<CrawStructure>();         //instantiated list of structures

    public Transform structures_hierachy_folder;


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

    public bool AnimateStructureSpawn() {
        return structures[current_structure_i].AnimateSpawn();
    }

    public CrawStructure GetBuildInfo_Structure() {
        return structures[(int)camp_BuildInfo[current_structure_i].x];
    }

    public Vector3 GetBuildInfo_Dir() {
        int dir = (int)camp_BuildInfo[current_structure_i].y;
        switch (dir)
        {
            case 0:
                return Vector3.forward;
            case 1:
                return Vector3.right;
            case 2:
                return Vector3.back;
            case 3:
                return Vector3.left;
            default:
                return Vector3.zero;
        }
    }

    public int GetBuildInfo_Prefab_i() {
        return (int)camp_BuildInfo[current_structure_i].z;
    }
}
