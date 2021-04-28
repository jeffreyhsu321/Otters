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
    //triggers (debug)
    public bool doGenerateFish;


    //data
    Data data = new Data();     //game data that holds player stats
     
    [SerializeField] GameObject[] pf_asters;        //list of all the aster prefabs
    [SerializeField] GameObject[] pf_structures;    //list of all the structure prefabs
    
    int current_aster_i = 0;                                            //current aster built index
    List<CrawAster> asters = new List<CrawAster>();                     //instantiated list of asters

    //timer for resource generation
    int timerThreshold;
    float timer;

    //lands
    bool spawningAster;                 //animation flag
    CrawAster currentSpawningAster;     //current aster that is spawning
    [SerializeField] Transform asters_hierachy_folder;      //transform folder to store instantiated asters

    //structures
    bool spawningStructure;             //animation flag
    CrawStructure currentStructure;
    public GameObject pf_corridor;      //corridor prefab

    //tmp
    public bool btnSpawnStruct;
    public bool btnSpawnAster;


    /// <summary>
    /// spawns asters
    /// </summary>
    /// <param name="aster_i"></param>
    public void SpawnAster(int aster_i) {
        currentSpawningAster = Instantiate(pf_asters[aster_i], pf_asters[aster_i].GetComponent<CrawAster>().spawnLocation, Quaternion.Euler(pf_asters[aster_i].GetComponent<CrawAster>().spawnRotation), asters_hierachy_folder).GetComponent<CrawAster>();
        asters.Add(currentSpawningAster);
        spawningAster = true;
    }
    public void SpawnAster() {
        currentSpawningAster = Instantiate(pf_asters[current_aster_i], pf_asters[current_aster_i].GetComponent<CrawAster>().spawnLocation, Quaternion.Euler(pf_asters[current_aster_i].GetComponent<CrawAster>().spawnRotation), asters_hierachy_folder).GetComponent<CrawAster>();
        spawningAster = true;
        asters.Add(currentSpawningAster);
        current_aster_i++;
    }



    /* (depreciated) spawn structures randomly
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
    */


    /// <summary>
    /// spawn structures (corridor -> room) per spawn camp info list (pre-determined design)
    /// </summary>
    public void SpawnStructure(int aster_i) {
        CrawAster currentAster = asters[aster_i];

        //tmp var inits
        int corridor_length = 10;
        int base_room_pf_i = 0;
        spawningStructure = true;

        if (currentAster.structures_hierachy_folder.childCount == 0)
        {
            //initial base spawn and append to list of instaniated
            currentAster.structures.Add(Instantiate(pf_structures[base_room_pf_i], currentAster.structures_hierachy_folder, false).GetComponent<CrawStructure>());
            currentAster.structures[0].transform.localPosition = currentAster.initial_base_pos;
            currentAster.current_structure_i = 0;
        }
        else
        {
            currentAster.current_structure_i++;

            //get base structure to offshoot the new room off of
            currentStructure = currentAster.GetBuildInfo_Structure();

            //spawn corridor
            Vector3 dir = currentAster.GetBuildInfo_Dir();
            Instantiate(pf_corridor, currentStructure.transform.position + dir * currentStructure.radius, Quaternion.LookRotation(dir, Vector3.up), currentStructure.transform);

            //spawn room
            GameObject pf = pf_structures[currentAster.GetBuildInfo_Prefab_i()];
            currentAster.structures.Add(Instantiate(pf, currentStructure.transform.position + dir * (currentStructure.radius + corridor_length + pf.GetComponent<CrawStructure>().radius), Quaternion.identity, currentAster.structures_hierachy_folder).GetComponent<CrawStructure>());
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
        if (spawningStructure) spawningStructure = currentSpawningAster.AnimateStructureSpawn();
        if (spawningAster) spawningAster = currentSpawningAster.AnimateSpawn();

        //tmp button in inspector
        if (btnSpawnStruct) {
            btnSpawnStruct = false;
            SpawnStructure(0);
        }
        if (btnSpawnAster) {
            btnSpawnAster = false;
            SpawnAster();
        }
    }
}
