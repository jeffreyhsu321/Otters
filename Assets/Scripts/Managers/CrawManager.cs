/*
 * File:        CrawManager.cs
 * Date:        7 April 2021
 * 
 * Purpose:     Manager for CRAWs (colonial resource acquisition workshops) - logic and UI
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawManager : Singleton<CrawManager>
{
    public bool doGenerateFish;

    Data data = new Data();     //game data that holds player stats

    [SerializeField] GameObject[] pf_asters;        //list of all the land masses
    [SerializeField] GameObject[] pf_structures;   //list of all the possible structures

    //timer
    int timerThreshold;
    float timer;

    //lands
    bool spawningAster;
    CrawAster currentAster;

    [SerializeField] Transform asters_hierachy_folder;

    //structures
    bool spawningStructure;
    CrawStructure currentStructure;
    public GameObject pf_corridor;

    [SerializeField] Transform structures_hierachy_folder;

    //tmp
    public bool doSpawnStruct;


    public void SpawnAster(int aster_index) {
        currentAster = Instantiate(pf_asters[aster_index], asters_hierachy_folder).GetComponent<CrawAster>();
        spawningAster = true;
    }

    public void SpawnStructure()
    {
        //tmp var inits
        int corridor_length = 2;
        Vector3 base_spawn_pos = Vector3.zero;

        if (structures_hierachy_folder.childCount == 0) {
            //initial base spawn
            currentStructure = Instantiate(pf_structures[0], base_spawn_pos, Quaternion.identity, structures_hierachy_folder).GetComponent<CrawStructure>();
        } else {
            //spawn room and corridor from random currentStructure
            currentStructure = structures_hierachy_folder.GetChild(Random.Range(0, structures_hierachy_folder.childCount)).GetComponent<CrawStructure>();

            //get availabilty
            Vector3 dir = currentStructure.GetAvailableDir();
            if (dir == Vector3.zero) {
                Debug.Log("aLL POINTS EXHAUSTED");
                return;
            }

            //spawn corridor
            GameObject corridor = Instantiate(pf_corridor, currentStructure.transform.position + dir * currentStructure.radius, Quaternion.LookRotation(dir, Vector3.up), currentStructure.transform);

            //spawn room
            GameObject room = Instantiate(pf_structures[0], currentStructure.transform.position + dir * (currentStructure.radius + corridor_length + pf_structures[0].GetComponent<CrawStructure>().radius), Quaternion.identity, structures_hierachy_folder);
            room.GetComponent<CrawStructure>().SetAvailability(dir);


            //currentStructure.SpawnAdjacentRoom(pf_structures[0], pf_corridor, structures_hierachy_folder);
        }
    }

    /// <summary>
    /// increment fish count by amount and update UI
    /// </summary>
    /// <param name="amount"></param>
    public void GenerateFish(int amount)
    {
        data.fish += amount;
        CanvasManager.Instance.t_count_fish.text = data.fish.ToString() + " Fish";
    }

    /// <summary>
    /// runs passive craw income
    /// </summary>
    private void RunCrawTimer() {
        //run timer until threshold
        if (timerThreshold == 0)
        {
            timerThreshold = (int)(Random.value * 20 / CritterManager.Instance.otters.Count) + 2;
            timer = 0;
        }

        //timer increment
        timer += Time.deltaTime;

        //generate fish notif
        if (timer >= timerThreshold)
        {
            timerThreshold = 0;

            CritterManager.Instance.otters_script[(int)Random.Range(0, CritterManager.Instance.otters_script.Count)].GenerateFishNotif();
        }
    }

    public void Update()
    {
        if (doGenerateFish) RunCrawTimer();
        if (spawningStructure) spawningStructure = currentStructure.AnimateSpawn();
        if (spawningAster) spawningAster = currentAster.AnimateSpawn();

        //tmp button in inspector
        if (doSpawnStruct) {
            //doSpawnStruct = false;
            //spawningStructure = true;
            //SpawnStructure();
            Invoke("SpawnStructure", 0.1f);
        }
    }
}
