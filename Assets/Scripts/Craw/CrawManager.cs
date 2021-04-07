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
    Data data = new Data();
    
    [SerializeField] CanvasManager canvas;

    public void GenerateFish(int amount)
    {
        data.fish += amount;
        canvas.t_count_fish.text = data.fish.ToString() + " Fish";
    }

    public void Update()
    {
        GenerateFish(1);
    }
}
