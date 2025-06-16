using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UIElements;



// TODO
// add progress bar, then make it so that choosing tutorial relies on whether or not the progress bar is shown
// add a tutorial message that you have to press and hold space for a couple seconds to start the actual choosing

// CURRENT PROBLEM
// I think its a problem with generalDialogueIter and switching, iter is not being updated correctly, like when choosing, its
// staying at 4 which is making it switch to the "press and hold" dialogue, so need to check the cases for when dialogueIter should
// be different numbers and such



public class DialogueScript : MonoBehaviour
{
    public float timer;
    public float masterInternalTimer;
    public float tempTime;
    public int generalDialogueIter;
    public int playerDialogueIter;
    public TextMeshProUGUI timerGUI;
    public TextMeshProUGUI dialogueGUI;
    public TextMeshProUGUI speakerNameGUI;
    public TextAsset topazDialogueLinesData;
    public TextAsset playerDialogueLinesData;
    private string[] topazDialogueLines;
    private string[] playerDialogueLines;
    public Boolean isPlayerControlling;
    public Boolean hasPlayerStartedControlling;
    public int whichChoiceMade;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        masterInternalTimer = 0;
        tempTime = 0;
        generalDialogueIter = 0;
        playerDialogueIter = 0;
        topazDialogueLines = topazDialogueLinesData.text.Split("\n");
        playerDialogueLines = playerDialogueLinesData.text.Split("\n");
        isPlayerControlling = false;
        hasPlayerStartedControlling = false;
        speakerNameGUI.SetText("Topaz");
        advanceGeneralDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        masterInternalTimer += Time.deltaTime;
        if (isPlayerControlling) {

            // transitioning from general to choosing
            if (!hasPlayerStartedControlling) {
                dialogueGUI.SetText("Press and hold space to start selecting dialogue");
                if ((Input.GetKeyDown(KeyCode.Space)||Input.GetKey(KeyCode.Space))&&
                    (masterInternalTimer-tempTime)>=.1) {
                    hasPlayerStartedControlling = true;
                    advancePlayerDialogueToNextChoice(); 
                }
            } 

            // if held for too long
            else if (timer > 5) { 
                if (playerDialogueIter == 4) {
                    timer = 0;
                    isPlayerControlling = false;
                    whichChoiceMade = 3;
                    advanceGeneralDialogueAfterChoose();
                } else if (playerDialogueIter == 7) {
                    timer = 0;
                    isPlayerControlling = false;
                    whichChoiceMade = 6;
                    advanceGeneralDialogueAfterChoose();
                } else {
                    timer = 0;
                    advancePlayerDialogueToNextChoice();
                }
            } 
            
            // in the middle of choosing
            else { 
                // if holding space or first half second
                if (Input.GetKey(KeyCode.Space)||(timer<=.5&&timer>=0)){
                    timer += Time.deltaTime;
                } 
                
                // let go of space bar
                else
                {
                    print("you let go of space");
                    timer = 0;
                    hasPlayerStartedControlling = false;
                    isPlayerControlling = false;
                    whichChoiceMade = playerDialogueIter;
                    advanceGeneralDialogueAfterChoose();
                }
            }
        }
        
        // while in general dialogue 
        else { 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                advanceGeneralDialogue();
            }
        }
        timerGUI.SetText(timer.ToString());
    }

    void advanceGeneralDialogue()
    {
        generalDialogueIter++;

        // if topaz said his response and need to skip past other responses
        if (checkSkipTo(generalDialogueIter-1) > 0) 
        {
            dialogueGUI.SetText(topazDialogueLines[checkSkipTo(generalDialogueIter)-1]);
        } 

        else 
        { 
            dialogueGUI.SetText(topazDialogueLines[generalDialogueIter - 1]);
            if (checkSwitch(generalDialogueIter) == 0) {
                speakerNameGUI.SetText("Topaz");
            } else if (checkSwitch(generalDialogueIter) == 1) {
                speakerNameGUI.SetText("Narrator");
            } else if(checkSwitch(generalDialogueIter) == 2) {
                speakerNameGUI.SetText("Doku");
            } else if (checkSwitch(generalDialogueIter) == 3) {
                speakerNameGUI.SetText("Choosing");
                isPlayerControlling = true;
                tempTime = masterInternalTimer;
            }
        }
    }

    void advancePlayerDialogueToNextChoice()
    {
        playerDialogueIter++;
        dialogueGUI.SetText(playerDialogueLines[playerDialogueIter-1]);
    }
    
    void advanceGeneralDialogueAfterChoose()
    {
        isPlayerControlling = false;
        hasPlayerStartedControlling = false;
        if (whichChoiceMade == 1)
        {
            generalDialogueIter = 5;
            advanceGeneralDialogue();
        } else if (whichChoiceMade == 2)
        {
            generalDialogueIter = 6;
            advanceGeneralDialogue();
        } else if (whichChoiceMade == 3)
        {
            generalDialogueIter = 7;
            advanceGeneralDialogue();
        }
    }

    // 0 = switch to topaz, 1 = switch to narrator, 2 = switch to doku, 3 = switch to choosing
    int checkSwitch(int n)
    {
        // I messed up the counting and this is easier than going through and fixing it
        n--;
        if (n == 4 || n == 14) {
            return 3;
        } else if (n == 29 || n == 32 || n == 47) {
            return 2;
        } else if (n == 28 || n == 34 || n == 49 || n == 56 || n == 64) {
            return 1;
        } else if (n == 31 || n == 58 || n==7) { 
            return 0;
        } else {
            return -1;
        }
    }

    // returns which general dialogue line should skip to
    int checkSkipTo(int n)
    {
        if(n == 5 || n == 6 || n == 7)
        {
            return 8;
        } else
        {
            return -1;
        }
    }
}
