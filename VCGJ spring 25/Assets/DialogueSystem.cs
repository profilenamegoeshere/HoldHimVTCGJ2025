using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum Branch
{
    Null,
    Topaz1,
    Player1,
    TopazRes11,
    TopazRes12,
    TopazRes13,
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
        linesPlayer2 = Resources.Load<TextAsset>("Player 1").text.Split('\n');
        linesTopazRes21 = Resources.Load<TextAsset>("Topaz Response 2-1").text.Split('\n');
        linesTopazRes22 = Resources.Load<TextAsset>("Topaz Response 2-2").text.Split('\n');
        linesTopazRes23 = Resources.Load<TextAsset>("Topaz Response 2-3").text.Split('\n');

        displayLines[0] = linesTopaz1[0];
        //for(int i=0; i < linesTopaz1.Length; i++)
        //{
        //    Debug.Log(linesTopaz1[i]);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueHandler.isChoosing())
        {
            if (timer < 4 && (Input.GetKey(KeyCode.Space)||timer<=1))
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
        int dialogueIter = 0;
        Branch currBranch = Branch.Topaz1;
        Branch nextBranch = Branch.Null;
        Boolean choosing = false;

        public void advanceDialogue()
        {
            if(dialogueIter < 3 && currBranch == Branch.Topaz1)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopaz1[dialogueIter]);
            } else if (dialogueIter >= 3 && currBranch == Branch.Topaz1)
            {
                currBranch = Branch.Player1;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesPlayer1[0]);
                dialogueIter = 0;
                choosing = true;
            } else if(currBranch == Branch.TopazRes11)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes11[dialogueIter]);
            }
            else if (currBranch == Branch.TopazRes12)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes12[dialogueIter]);
            }
            else if (currBranch == Branch.TopazRes13)
            {
                dialogueIter++;
                DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes13[dialogueIter]);
            }
        }

        public void madeChoice()
        {
            choosing = false;
            // based on nextBranch, change currBranch
            currBranch = nextBranch;
            dialogueIter = -1;
            advanceDialogue();
            //if(currBranch == Branch.TopazRes11)
            //{
            //    dialogueIter = 0;
            //    DialogueSystem.addToDisplayLines(DialogueSystem.linesTopazRes13[dialogueIter]);
            //    dialogueIter++;
            //} else if (currBranch == Branch.TopazRes12)
            //{
            //    dialogueIter = 0;
            //    DialogueSystem.displayLines[dialogueIter + 1] = DialogueSystem.linesTopazRes12[0];
            //    dialogueIter++;
            //} else
            //{
            //    dialogueIter = 0;
            //    DialogueSystem.displayLines[dialogueIter + 1] = DialogueSystem.linesTopazRes13[0];
            //    dialogueIter++;
            //}
        }

        public void checkChoosing(float timer)
        {
            // change nextBranch based on timer and advance dialogue while currBranch is still in player choosing
            if (timer < 2)
            {
                if (nextBranch == Branch.Null)
                {
                    nextBranch = Branch.TopazRes11;
                    advanceDialogue();
                }
            } else if (timer < 3)
            {
                if (nextBranch == Branch.TopazRes11)
                {
                    nextBranch = Branch.TopazRes12;
                    advanceDialogue();
                }
            } else
            {
                if (nextBranch == Branch.TopazRes12)
                {
                    nextBranch = Branch.TopazRes13;
                    advanceDialogue();
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