using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum Branch
{
    Topaz1,
    Player1,
    TopazRes1,
    Topaz2,
    Player2,
    TopazRes21,
    TopazRes22,
    TopazRes23
}

/*
 * TODO
 * Other function builds array of dialogue lines ahead of it while update and 
 * update display just use that array and handle button pressing and calling
 * the other function
 */

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueTextGUI;
    public TextMeshProUGUI dialogueTimerGUI;
    public TextMeshProUGUI dialogueIterGUI;
    public Boolean isChoosing;
    public float timer;
    public float timerSpeed;
    public TextAsset dataTopaz1;
    private string[] linesTopaz1;
    public TextAsset dataPlayer1;
    private string[] linesPlayer1;
    public TextAsset dataTopazRes1;
    private string[] linesTopazRes1;
    public TextAsset dataTopaz2;
    private string[] linesTopaz2;
    public TextAsset dataPlayer2;
    private string[] linesPlayer2;
    public TextAsset dataTopazRes21;
    private string[] linesTopazRes21;
    public TextAsset dataTopazRes22;
    private string[] linesTopazRes22;
    public TextAsset dataTopazRes23;
    private string[] linesTopazRes23;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        isChoosing = false;

        string sTopaz1 = dataTopaz1.text;
        linesTopaz1 = sTopaz1.Split('\n');

        string sPlayer1 = dataPlayer1.text;
        linesPlayer1 = sPlayer1.Split('\n');

        string sTopazRes1 = dataTopazRes1.text;
        linesTopazRes1 = sTopazRes1.Split('\n');

        string sTopaz2 = dataTopaz2.text;
        linesTopaz2 = sTopaz2.Split('\n');

        string sPlayer2 = dataPlayer2.text;
        linesPlayer2 = sPlayer2.Split('\n');

        string sTopazRes21 = dataTopazRes21.text;
        linesTopazRes21 = sTopazRes21.Split('\n');

        string sTopazRes22 = dataTopazRes22.text;
        linesTopazRes22 = sTopazRes22.Split('\n');

        string sTopazRes23 = dataTopazRes23.text;
        linesTopazRes23 = sTopazRes23.Split('\n');
    }

    // Update is called once per frame
    void Update()
    {
        if (isChoosing)
        {
            if ((Input.GetKey(KeyCode.Space)||timer<=1)&&timer<4)
            {
                timer += (Time.deltaTime)/2;
            }
            else if (timer > 1)
            {
                //dialogueIter += (int)Mathf.Floor(timer);
                timer = 0;
                isChoosing = false;
            }
        }

        if (!isChoosing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
            }
        }

        updateScreen();
    }

    void updateScreen()
    {
        dialogueTimerGUI.SetText(timer.ToString());
    }
}

public class DialogueHandler
{
    public int dialogueIter = 1;
    public Branch currBranch = Branch.Topaz1;
    public string[] displayLines = new string[80];

    public void advanceDialogue()
    {

    }
}