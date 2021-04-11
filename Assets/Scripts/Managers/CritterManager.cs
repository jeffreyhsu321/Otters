/*
 * File:        CritterManager.cs
 * Date:        4 April 2021
 * 
 * Purpose:     Manage critters (spawning)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterManager : Singleton<CritterManager>
{
    public List<GameObject> otters = new List<GameObject>();
    public List<ProtoOtter> otters_script = new List<ProtoOtter>();
    [SerializeField] Transform actor_hierarchy_folder;

    [SerializeField] bool isRandomizeSpawnPosRot;
    [SerializeField] int spawnLocationRadius;


    /// <summary>
    /// spawns otter with random position and rotation
    /// </summary>
    /// <param name="otterType_i"></param>
    public void SpawnOtter(int otterType_i)
    {
        GameObject otter = Instantiate(otters[otterType_i], actor_hierarchy_folder);

        if (isRandomizeSpawnPosRot) {
            otter.transform.position = Random.insideUnitSphere * spawnLocationRadius;
            otter.transform.rotation = Random.rotation;
        }

        otters.Add(otter);
        otters_script.Add(otter.GetComponent<ProtoOtter>());
    }
}
