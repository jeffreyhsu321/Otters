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
    //physical
    //[SerializeField] GameObject model;
    [SerializeField] Vector3 spawnLocation;

    //stats
    public float baseAmount;
    public float upgradeModIndex;
    [SerializeField] float[] upgradeModList;

    public void Awake()
    {
        transform.position = spawnLocation;
    }
}
