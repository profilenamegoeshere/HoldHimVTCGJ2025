using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class progressTrackerScript : MonoBehaviour
{
    public static progressTrackerScript Instance { get; private set; }
    public Boolean[] endingsGotten;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        endingsGotten = new bool[3];
    }

    public void gotAnEnding(int whichEnding)
    {
        if (whichEnding == 1)
        {
            endingsGotten[0] = true;
        }
        else if (whichEnding == 2)
        {
            endingsGotten[1] = true;
        }
        else if (whichEnding == 3)
        {
            endingsGotten[2] = true;
        } else
        {
            Debug.Log("Tried to award an incorrect ending.");
        }
    }
}
