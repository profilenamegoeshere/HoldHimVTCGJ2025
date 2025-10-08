using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    Tutorial2,
    TopazRes21,
    TopazRes22,
    TopazRes23
}

public enum Speakers
{
    Topaz,
    Doku,
    None
}

/*
 * TODO
 * Add voices lines
 * 
 * Add some images
 * 
 * Add a title screen (I think that would be a different scene?)
 * 
 * Make tutorial better
 * 
 * Then you can fix all the little things that bother you
 */


public class DialogueSystem : MonoBehaviour
{   
    public TextMeshProUGUI dialogueTextGUI;
    public TextMeshProUGUI speakerNameGUI;
    public TextMeshProUGUI dialogueTimerGUI;
    public TextMeshProUGUI dialogueIterGUI;
    public TextMeshProUGUI branchGUI;
    public TextMeshProUGUI displayIterGUI;
    public TextMeshProUGUI isChoosingGUI;
    public TextMeshProUGUI soundIter;
    public float timer;
    public float timerSpeed;
    public float timerLimit;

    public string[] linesTopaz1;
    public string[] linesPlayer1;
    public string[] linesTopazRes11;
    public string[] linesTopazRes12;
    public string[] linesTopazRes13;
    public string[] linesTopaz2;
    public string[] linesPlayer2;
    public string[] linesTopazRes21;
    public string[] linesTopazRes22;
    public string[] linesTopazRes23;

    private DialogueHandler dialogueHandler;
    public string[] displayLines;
    public int displayIter;
    public int diplayIterOffset;

    public AudioSource audioSource;
    public AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        timerLimit = 0;
        dialogueHandler = new DialogueHandler(this);
        displayLines = new string[80];
        for (int i = 0; i < 80; i++)
        {
            displayLines[i] = " ";
        }

        displayIter = 0;

        linesTopaz1 = Resources.Load<TextAsset>("Dialogue/Topaz 1").text.Split('\n');
        linesPlayer1 = Resources.Load<TextAsset>("Dialogue/Player 1").text.Split('\n');
        linesTopazRes11 = Resources.Load<TextAsset>("Dialogue/Topaz Response 1-1").text.Split('\n');
        linesTopazRes12 = Resources.Load<TextAsset>("Dialogue/Topaz Response 1-2").text.Split('\n');
        linesTopazRes13 = Resources.Load<TextAsset>("Dialogue/Topaz Response 1-3").text.Split('\n');
        linesTopaz2 = Resources.Load<TextAsset>("Dialogue/Topaz 2").text.Split('\n');
        linesPlayer2 = Resources.Load<TextAsset>("Dialogue/Player 2").text.Split('\n');
        linesTopazRes21 = Resources.Load<TextAsset>("Dialogue/Topaz Response 2-1").text.Split('\n');
        linesTopazRes22 = Resources.Load<TextAsset>("Dialogue/Topaz Response 2-2").text.Split('\n');
        linesTopazRes23 = Resources.Load<TextAsset>("Dialogue/Topaz Response 2-3").text.Split('\n');

        displayLines[0] = linesTopaz1[0];
        speakerNameGUI.SetText("Topaz");

        audioClips = Resources.LoadAll<AudioClip>("Sounds/").OrderBy(clip => clip.name).ToArray();
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timerLimit = 4;
        if (dialogueHandler.whichBranch() == Branch.PreChoice2)
        {
            timerLimit = 3;
        }
        if (dialogueHandler.isChoosing())
        {
            // TODO if less than 1 and let go of space just reset timer and dont choose
            if (timer < timerLimit && (Input.GetKey(KeyCode.Space) || timer <= 1))
            {
                timer += (Time.deltaTime) / 2;
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
        displayIterGUI.SetText(displayIter.ToString());
        isChoosingGUI.SetText(dialogueHandler.isChoosing().ToString());
        soundIter.SetText(dialogueHandler.getSoundIter().ToString());
    }

    public void addToDisplayLines(string add)
    {
        displayLines[displayIter + 1] = add;
        displayIter++;
    }

    public void updateSpeakerDisplay(string newName)
    {
        speakerNameGUI.SetText(newName);
    }

    public class AudioNameSort : Comparer<AudioClip>
    {
        public override int Compare(AudioClip x, AudioClip y)
        {
            return String.Compare(x.name, y.name, StringComparison.Ordinal);
        }
    }

    public class DialogueHandler
    {
        public DialogueSystem dialogueSystem;

        public int dialogueIter = 0;
        public Branch currBranch = Branch.Topaz1;
        public Branch nextBranch = Branch.PreChoice1;
        public Boolean choosing = false;
        public int soundIter = 0;
        public bool playSoundOnNextLine = false;

        public DialogueHandler(DialogueSystem newDialogueSystem)
        {
            dialogueSystem = newDialogueSystem;
        }

        public void advanceDialogue()
        {
            if(dialogueIter < 3 && currBranch == Branch.Topaz1)
            {
                dialogueIter++;
                dialogueSystem.addToDisplayLines(dialogueSystem.linesTopaz1[dialogueIter]);
                playSoundOnNextLine = true;
                if(dialogueIter < 3)
                {
                    soundIter++;
                    playSound();
                }
            } 
            else if (dialogueIter >= 3 && currBranch == Branch.Topaz1)
            {
                currBranch = Branch.Player1;
                dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer1[0]);
                dialogueIter = 0;
                choosing = true;
            } 
            else if(currBranch == Branch.TopazRes11)
            {
                soundIter = 3;
                playSound();

                dialogueIter++;
                dialogueSystem.addToDisplayLines(dialogueSystem.linesTopazRes11[dialogueIter]);
                currBranch = Branch.Topaz2;
                dialogueIter = 0;
            }
            else if (currBranch == Branch.TopazRes12)
            {
                soundIter = 4;
                playSound();

                dialogueIter++;
                dialogueSystem.addToDisplayLines(dialogueSystem.linesTopazRes12[dialogueIter]);
                currBranch = Branch.Topaz2;
                dialogueIter = 0;
            }
            else if (currBranch == Branch.TopazRes13)
            {
                soundIter = 5;
                playSound();

                dialogueIter++;
                dialogueSystem.addToDisplayLines(dialogueSystem.linesTopazRes13[dialogueIter]);
                currBranch = Branch.Topaz2;
                dialogueIter = 0;
            }
            else if (dialogueIter < 7 && currBranch == Branch.Topaz2)
            {
                if(soundIter < 6)
                {
                    soundIter = 6;
                }
                playSound();
                soundIter++;
                dialogueSystem.addToDisplayLines(dialogueSystem.linesTopaz2[dialogueIter]);
                dialogueIter++;
            }
            else if(dialogueIter >= 7 && currBranch == Branch.Topaz2)
            {
                currBranch = Branch.Player2;
                dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer2[0]);
                dialogueIter = 0;
                choosing = true;
            }
            else if (currBranch == Branch.TopazRes21)
            {
                if (dialogueIter < 21)
                {
                    dialogueIter++;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesTopazRes21[dialogueIter]);
                    playSoundOnNextLine = true;
                }
            }
            else if (currBranch == Branch.TopazRes22)
            {
                if (dialogueIter <15)
                {
                    dialogueIter++;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesTopazRes22[dialogueIter]);
                    playSoundOnNextLine = true;
                }
            }
            else if (currBranch == Branch.TopazRes23)
            {
                if (dialogueIter < 23)
                {
                    dialogueIter++;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesTopazRes23[dialogueIter]);
                    playSoundOnNextLine = true;
                }
            } else
            {
                playSoundOnNextLine = false;
            }
            callUpdateSpeakerDisplay();
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
                    dialogueSystem.addToDisplayLines("Press and hold space to start selecting. Let go of space to choose displayed dialogue choice.");
                } else if(nextBranch == Branch.PreChoice2)
                {
                    nextBranch = Branch.Tutorial2;
                    dialogueSystem.addToDisplayLines("Press and hold space to choose next dialogue.");
                }
            } else if (timer < 2)
            {
                if (nextBranch == Branch.Tutorial)
                {
                    nextBranch = Branch.TopazRes11;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer1[0]);
                }
                else if (nextBranch == Branch.Tutorial2)
                {
                    nextBranch = Branch.TopazRes21;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer2[0]);
                }
            } else if (timer < 3)
            {
                if (nextBranch == Branch.TopazRes11)
                {
                    nextBranch = Branch.TopazRes12;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer1[1]);
                }
                else if (nextBranch == Branch.TopazRes21)
                {
                    nextBranch = Branch.TopazRes22;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer2[1]);
                }
            } else
            {
                if (nextBranch == Branch.TopazRes12)
                {
                    nextBranch = Branch.TopazRes13;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer1[2]);
                }
                else if (nextBranch == Branch.TopazRes22)
                {
                    nextBranch = Branch.TopazRes23;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer2[2]);
                }
            }
        }

        public void callUpdateSpeakerDisplay()
        {
            string newName = "";
            if (currBranch == Branch.Topaz1 ||
                currBranch == Branch.Topaz2 ||
                currBranch == Branch.TopazRes11 ||
                currBranch == Branch.TopazRes12 ||
                currBranch == Branch.TopazRes13 ||
                currBranch == Branch.TopazRes21 ||
                currBranch == Branch.TopazRes22 ||
                currBranch == Branch.TopazRes23 )
            {
                newName = "Topaz";
            }
            else if (currBranch == Branch.Player1 ||
                     currBranch == Branch.Player2)
            {
                newName = "Topaz";
            }
            else
            {
                newName = " ";
            }
            dialogueSystem.updateSpeakerDisplay(newName);
        }

        public void playSound()
        {
            if (playSoundOnNextLine)
            {
                dialogueSystem.audioSource.clip = dialogueSystem.audioClips[soundIter];
                dialogueSystem.audioSource.Play();
                Debug.Log(dialogueSystem.audioSource.clip.name);
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

        public int getSoundIter()
        {
            return soundIter;
        }
    }
}