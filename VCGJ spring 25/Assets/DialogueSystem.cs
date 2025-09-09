using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum Branch
{
    Topaz1,
    Player1,
    Tutorial,
    PreChoice1,
    TopazRes11,
    TopazRes12,
    TopazRes13,
    Topaz2,
    Player2,
    PreChoice2,
    TopazRes21,
    TopazRes22,
    TopazRes23
}

/*
 * TODO
 * Other function builds array of dialogue lines ahead of it while update and 
 * update display just use that array and handle button pressing and calling
 * the other function
 * 
 * Figure out file parsing
 */


public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueTextGUI;
    public TextMeshProUGUI dialogueTimerGUI;
    public TextMeshProUGUI dialogueIterGUI;
    public TextMeshProUGUI branchGUI;
    public float timer;
    public float timerSpeed;
    public TextAsset dataTopaz1;

    public static string[] linesTopaz1;
    public static string[] linesPlayer1;
    public static string[] linesTopazRes11;
    public static string[] linesTopazRes12;
    public static string[] linesTopazRes13;
    public static string[] linesTopaz2;
    public static string[] linesPlayer2;
    public static string[] linesTopazRes21;
    public static string[] linesTopazRes22;
    public static string[] linesTopazRes23;

    private DialogueHandler dialogueHandler;
    public static string[] displayLines;
    public static int displayIter;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        dialogueHandler = new DialogueHandler();
        displayLines = new string[80];
        for (int i = 0; i < 80; i++)
        {
            displayLines[i] = " ";
        }
        displayIter = 0;

        linesTopaz1 = Resources.Load<TextAsset>("Topaz 1").text.Split('\n');
        linesPlayer1 = Resources.Load<TextAsset>("Player 1").text.Split('\n');
        linesTopazRes11 = Resources.Load<TextAsset>("Topaz Response 1-1").text.Split('\n');
        linesTopazRes12 = Resources.Load<TextAsset>("Topaz Response 1-2").text.Split('\n');
        linesTopazRes13 = Resources.Load<TextAsset>("Topaz Response 1-3").text.Split('\n');
        linesTopaz2 = Resources.Load<TextAsset>("Topaz 2").text.Split('\n');
        linesPlayer2 = Resources.Load<TextAsset>("Player 2").text.Split('\n');
        linesTopazRes21 = Resources.Load<TextAsset>("Topaz Response 2-1").text.Split('\n');
        linesTopazRes22 = Resources.Load<TextAsset>("Topaz Response 2-2").text.Split('\n');
        linesTopazRes23 = Resources.Load<TextAsset>("Topaz Response 2-3").text.Split('\n');

        displayLines[0] = linesTopaz1[0];

    }

    // Update is called once per frame
    void Update()
    {
        int timerLimit = 4;
        if (dialogueHandler.whichBranch() == Branch.PreChoice2)
        {
            timerLimit = 3;
        }
        if (dialogueHandler.isChoosing())
        {
            // TODO if less than 1 and let go of space just reset timer and dont choose
            if (timer < timerLimit && (Input.GetKey(KeyCode.Space)||timer<=1))
            {
                timer += (Time.deltaTime)/2;
                dialogueHandler.checkChoosing(timer);
            }
            else if (timer > 1)
            {
                dialogueHandler.madeChoice();
                timer = 0;
            }
        } 
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueHandler.advanceDialogue();
            }
        }
        updateScreen();
    }

    void updateScreen()
    {
        dialogueTimerGUI.SetText(timer.ToString());
        dialogueTextGUI.SetText(displayLines[displayIter]);
        dialogueIterGUI.SetText(dialogueHandler.getIter().ToString());
        branchGUI.SetText(dialogueHandler.whichBranch().ToString());
    }

    public static void addToDisplayLines(string add)
    {
        displayLines[displayIter+1] = add;
        displayIter++;
    }

    public class DialogueHandler
    {
        public int dialogueIter = 0;
        public Branch currBranch = Branch.Topaz1;
        public Branch nextBranch = Branch.PreChoice1;
        public Boolean choosing = false;

        public void advanceDialogue()
        {
            if(dialogueIter < 4 && currBranch == Branch.Topaz1)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopaz1[dialogueIter]);
            } 
            else if (dialogueIter >= 4 && currBranch == Branch.Topaz1)
            {
                currBranch = Branch.Player1;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesPlayer1[0]);
                dialogueIter = 0;
                choosing = true;
            } 
            else if(currBranch == Branch.TopazRes11)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes11[dialogueIter]);
                currBranch = Branch.Topaz2;
                dialogueIter = 0;
            }
            else if (currBranch == Branch.TopazRes12)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes12[dialogueIter]);
                currBranch = Branch.Topaz2;
                dialogueIter = 0;
            }
            else if (currBranch == Branch.TopazRes13)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes13[dialogueIter]);
                currBranch = Branch.Topaz2;
                dialogueIter = 0;
            }
            else if (dialogueIter < 7 && currBranch == Branch.Topaz2)
            {
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopaz2[dialogueIter]);
                dialogueIter++;
            }
            else if(dialogueIter >= 7 && currBranch == Branch.Topaz2)
            {
                currBranch = Branch.Player2;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesPlayer2[0]);
                dialogueIter = 0;
                choosing = true;
            }
            else if (currBranch == Branch.TopazRes21)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes21[dialogueIter]);
            }
            else if (currBranch == Branch.TopazRes22)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes22[dialogueIter]);
            }
            else if (currBranch == Branch.TopazRes23)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes23[dialogueIter]);
            }
        }

        public void madeChoice()
        {
            choosing = false;
            // based on nextBranch, change currBranch
            currBranch = nextBranch;
            if (nextBranch == Branch.TopazRes11 || nextBranch == Branch.TopazRes12 || nextBranch == Branch.TopazRes13)
            {
                nextBranch = Branch.PreChoice2;
            }
            dialogueIter = -1;
            advanceDialogue();
        }

        public void checkChoosing(float timer)
        {
            // change nextBranch based on timer and advance dialogue while currBranch is still in player choosing
            Debug.Log(nextBranch.ToString());
            if (timer < 1)
            {
                if (nextBranch == Branch.PreChoice1)
                {
                    nextBranch = Branch.Tutorial;
                    DialogueSystem.addToDisplayLines("Press and hold space to start selecting. Let go of space to choose displayed dialogue choice.");
                } else if(nextBranch == Branch.PreChoice2)
                {
                    nextBranch = Branch.TopazRes21;
                    DialogueSystem.addToDisplayLines(DialogueSystem.linesPlayer2[0]);
                }
            } else if (timer < 2)
            {
                if (nextBranch == Branch.Tutorial)
                {
                    nextBranch = Branch.TopazRes11;
                    DialogueSystem.addToDisplayLines(DialogueSystem.linesPlayer1[0]);
                }
                else if (nextBranch == Branch.TopazRes21)
                {
                    nextBranch = Branch.TopazRes22;
                    DialogueSystem.addToDisplayLines(DialogueSystem.linesPlayer2[1]);
                }
            } else if (timer < 3)
            {
                if (nextBranch == Branch.TopazRes11)
                {
                    nextBranch = Branch.TopazRes12;
                    DialogueSystem.addToDisplayLines(DialogueSystem.linesPlayer1[1]);
                }
                else if (nextBranch == Branch.TopazRes22)
                {
                    nextBranch = Branch.TopazRes23;
                    DialogueSystem.addToDisplayLines(DialogueSystem.linesPlayer2[2]);
                }
            } else
            {
                if (nextBranch == Branch.TopazRes12)
                {
                    nextBranch = Branch.TopazRes13;
                    DialogueSystem.addToDisplayLines(DialogueSystem.linesPlayer1[2]);
                }
            }
        }

        public bool isChoosing()
        {
            return (choosing);
        }

        public int getIter()
        {
            return dialogueIter;
        }

        public Branch whichBranch()
        {
            return currBranch;
        }
    }
}