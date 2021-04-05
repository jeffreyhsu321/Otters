/*
 * File:        CritterManager.cs
 * Date:        4 April 2021
 * 
 * Purpose:     Manage critters (spawning)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterManager : MonoBehaviour
{
    public GameObject critter1;
    public GameObject critter2;

    [SerializeField] int spawnSize;

    [SerializeField] bool isRandomizeSpawnLocation;
    [SerializeField] int spawnLocationRadius;

    public void Start()
    {
        for (int i = 0; i < spawnSize / 2; i++) {
            GameObject c1 = Instantiate(critter1);
            GameObject c2 = Instantiate(critter2);

            if (isRandomizeSpawnLocation) {
                c1.transform.position = Random.insideUnitSphere * spawnLocationRadius;
                c2.transform.position = Random.insideUnitSphere * spawnLocationRadius;

                c1.transform.rotation = Random.rotation;
                c2.transform.rotation = Random.rotation;
            }
        }
    }
}
