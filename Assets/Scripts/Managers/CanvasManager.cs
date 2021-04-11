/*
 * File:        CanvasManager.cs
 * Date:        7 April 2021
 * 
 * Purpose:     Manager for all UI
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : Singleton<CanvasManager>
{
    public TMP_Text t_count_fish;

    [SerializeField] GameObject[] tabs;

    public void SwitchToTab(int tab_i) {

        for(int i = 0; i < tabs.Length; i++) {
            if (i == tab_i)
            {
                tabs[0].SetActive(tabs[i].activeSelf);      //toggle default UI panel
                tabs[i].SetActive(!tabs[i].activeSelf);     //toggle target tab
            }
            else
            {
                tabs[i].SetActive(false);       //disable all other tabs
            }
        }
    }
}
