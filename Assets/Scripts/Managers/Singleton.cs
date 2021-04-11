/*
 * File:        Singleton.cs
 * Date:        10 April 2021
 * 
 * Purpose:     Singleton Design Pattern. Only one instance for each manager.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                GameObject obj = GameObject.Find(typeof(T).Name);
                //obj.hideFlags = HideFlags.HideAndDontSave;      //hide this obj from the hierarchy and don't save it to scene
                _instance = obj.GetComponent<T>();
                if (_instance == null) Debug.Log("ERROR: FAILED TO GET SINGLETON INSTANCE");
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this) {
            _instance = null;
        }
    }
}
