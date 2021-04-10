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

    public GameObject tab_default;
    public GameObject tab_structures;

    public void SwitchToTab(int tab) {
        switch (tab) {
            case 0:
                tab_default.SetActive(true);
                tab_structures.SetActive(false);
                break;
            case 1:
                tab_default.SetActive(false);
                tab_structures.SetActive(true);
                break;
            default:
                Debug.Log("ERROR: Invalid tab number");
                break;
        }
    }
}
