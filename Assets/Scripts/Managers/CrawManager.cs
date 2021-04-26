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
    [SerializeField] GameObject[] pf_structures;    //list of all the possible structures

    public Vector3[] camp_spawnInfo;                                    //[camp_i][0 = structure_i; 1 = dir_i; 2 = pf_structures_i]
    int current_structure_build_i;                                      //current structure built index for this camp (camp_spawnInfo use)
    List<CrawStructure> structures = new List<CrawStructure>();         //instantiated list of structures

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
    public float buildSpeed;
    public bool spawningInterval;


    public void SpawnAster(int aster_index) {
        currentAster = Instantiate(pf_asters[aster_index], asters_hierachy_folder).GetComponent<CrawAster>();
        spawningAster = true;
    }

    /// <summary>
    /// (depreciated) spawn structures randomly
    /// </summary>
    /// <returns></returns>
    public bool SpawnStructureRandom()
    {
        //tmp var inits
        int corridor_length = 2;
        Vector3 base_spawn_pos = Vector3.zero;
        int pf_struct_index = Random.Range(0, 2);

        if (structures_hierachy_folder.childCount == 0)
        {
            //initial base spawn
            currentStructure = Instantiate(pf_structures[pf_struct_index], base_spawn_pos, Quaternion.identity, structures_hierachy_folder).GetComponent<CrawStructure>();
        }
        else
        {
            //spawn room and corridor from random currentStructure
            currentStructure = structures_hierachy_folder.GetChild(Random.Range(0, structures_hierachy_folder.childCount)).GetComponent<CrawStructure>();

            //get and check availabilty
            Vector3 dir = currentStructure.GetAvailableDir();
            if (dir == Vector3.zero) {
                Debug.Log("Availability Check Fails!");
                return false;
            }

            //check collision validity
            if (    currentStructure.CheckCollisionValidity(dir, corridor_length, pf_structures[pf_struct_index])) {
                Debug.Log("Collision Validity Fails!");
                return false;
            }

            //spawn corridor
            GameObject corridor = Instantiate(pf_corridor, currentStructure.transform.position + dir * currentStructure.radius, Quaternion.LookRotation(dir, Vector3.up), currentStructure.transform);

            //spawn room
            GameObject room = Instantiate(pf_structures[pf_struct_index], currentStructure.transform.position + dir * (currentStructure.radius + corridor_length + pf_structures[pf_struct_index].GetComponent<CrawStructure>().radius), Quaternion.identity, structures_hierachy_folder);
            room.GetComponent<CrawStructure>().SetAvailability(dir);

            Debug.Log("Spawn Successful!");
        }

        //successful spawning
        return true;
        
    }

    /// <summary>
    /// spawn structures (corridor -> room) per spawn camp info list (pre-determined design)
    /// </summary>
    public void SpawnStructure() {
        //tmp var inits
        int corridor_length = 2;
        Vector3 base_spawn_pos = Vector3.zero;
        int base_room_pf_i = 0;

        if (structures_hierachy_folder.childCount == 0)
        {
            //initial base spawn and append to list of instaniated
            structures.Add(Instantiate(pf_structures[base_room_pf_i], base_spawn_pos, Quaternion.identity, structures_hierachy_folder).GetComponent<CrawStructure>());
            current_structure_build_i = 0;
        }
        else
        {
            current_structure_build_i++;

            //get designated currentStructure
            currentStructure = structures[(int)camp_spawnInfo[current_structure_build_i].x];

            //spawn corridor
            Vector3 dir = currentStructure.GetDir((int)camp_spawnInfo[current_structure_build_i].y);
            Instantiate(pf_corridor, currentStructure.transform.position + dir * currentStructure.radius, Quaternion.LookRotation(dir, Vector3.up), currentStructure.transform);

            //spawn room
            structures.Add(Instantiate(pf_structures[(int)camp_spawnInfo[current_structure_build_i].z], currentStructure.transform.position + dir * (currentStructure.radius + corridor_length + pf_structures[(int)camp_spawnInfo[current_structure_build_i].z].GetComponent<CrawStructure>().radius), Quaternion.identity, structures_hierachy_folder).GetComponent<CrawStructure>());
        }
    }

    private void EndSpawningInterval() {
        spawningInterval = false;
        Debug.Log("spawningInterval reset");
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
            if (!spawningInterval)
            {
                spawningInterval = true;

                //SpawnStructureRandom();
                SpawnStructure();

                //tmp: start timer to count till next rapid spawn
                Invoke("EndSpawningInterval", buildSpeed);
            }
        }
    }
}
