/*
 * File:        CrawManager.cs
 * Date:        7 April 2021
 * 
 * Purpose:     Manager for CRAWs (colonial resource acquisition workshops) - logic and UI
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawManager : MonoBehaviour
{
    
    Data data = new Data();     //game data that holds player stats

    [SerializeField] GameObject[] struct_list;  //list of all the possible structures
    
    [SerializeField] CanvasManager canvas;      //ref to canvas (UI)



    public void GenerateFish(int amount)
    {
        data.fish += amount;
        canvas.t_count_fish.text = data.fish.ToString() + " Fish";
    }

    public void BuildStructure(int struct_index) {
        Instantiate(struct_list[struct_index]);
    }

    public void Update()
    {
        //GenerateFish(1);
    }
}
