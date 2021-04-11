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
    Data data = new Data();     //game data that holds player stats

    [SerializeField] GameObject[] structures;  //list of all the possible structures

    //timer
    int timerThreshold;
    float timer;

    public void BuildStructure(int struct_index)
    {
        Instantiate(structures[struct_index]);
    }

    public void GenerateFish(int amount)
    {
        data.fish += amount;
        CanvasManager.Instance.t_count_fish.text = data.fish.ToString() + " Fish";
    }

    public void Update()
    {
        //run timer until threshold
        if (timerThreshold == 0)
        {
            timerThreshold = (int)(Random.value * 20 / CritterManager.Instance.otters.Count) + 2;
            timer = 0;
        }

        //timer increment
        timer += Time.deltaTime;

        //generate fish notif
        if (timer >= timerThreshold) {
            timerThreshold = 0;

            CritterManager.Instance.otters_script[(int)Random.Range(0, CritterManager.Instance.otters_script.Count)].GenerateFishNotif();
        }
    }
}
