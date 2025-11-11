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
    TopazRes11,
    TopazRes12,
    TopazRes13,
    Topaz2,
    Player2,
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
    public TextMeshProUGUI soundIterGUI;
    public TextMeshProUGUI ending3TimerGUI;
    public TextMeshProUGUI endingGUI;
    public GameObject dialogueBackground;
    public GameObject playerDialogueBackground;
    public float timer;
    public float timerSpeed;
    public float timerLimit;
    public float ending3Timer;
    public Boolean ending3CanPlay;
    public Boolean[] ending3Triggers;
    public Boolean canChoose;

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

    public int whichEnding;

    // Start is called before the first frame update
    void Start()
    {
        whichEnding = 0;
        
        timer = 0;
        timerLimit = 0;
        ending3Timer = 0;
        dialogueHandler = new DialogueHandler(this);
        displayLines = new string[80];
        ending3CanPlay = true;
        ending3Triggers = new bool[6];
        canChoose = false;
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

        dialogueTextGUI.rectTransform.position = new Vector2(960f, (-220f+500f));
    }

    // Update is called once per frame
    void Update()
    {
        whichEnding = dialogueHandler.whichEnding;
        timerLimit = 3;
        if ((timer > .8 && timer < 1) |
                (timer > 1.8 && timer < 2) |
                (timer > 2.8 && timer < 3))
        {
            canChoose = false;
        } else
        {
            dialogueTextGUI.color = Color.white;
            canChoose = true;
        }

        if (dialogueHandler.isChoosing())
        {
            dialogueTextGUI.rectTransform.position = new Vector2(960f, (100f + 500));
            if ((Input.GetKeyDown(KeyCode.Space)|(timer > timerLimit))
                && canChoose)
            {
                dialogueHandler.madeChoice();
                timer = 0;
            }
            else if (timer < timerLimit)
            {
                timer += (Time.deltaTime * (timerSpeed / 100));
                dialogueHandler.checkChoosing(timer);
            }
        }
        else
        {
            dialogueTextGUI.rectTransform.position = new Vector2(960f, (-220f + 500));
            if (Input.GetKeyDown(KeyCode.Space) && ending3CanPlay)
            {
                dialogueHandler.advanceDialogue();
            }
        }

        if (dialogueHandler.whichBranch() == Branch.TopazRes23 && 
            dialogueHandler.getIter()>=9 &&
            dialogueHandler.getIter()<=15)
        {
            ending3();
        }
        updateScreen();
    }

    void updateScreen()
    {
        if (dialogueHandler.isChoosing())
        {
            if(dialogueHandler.whichBranch() == Branch.Player2)
            {
                playerDialogueBackground.transform.localScale = new Vector3(20.0f, 2.7f, 1.0f);
                playerDialogueBackground.transform.localPosition = new Vector2(0f, 1.63f);
                Debug.Log("Should have switched to the wide");
            } else
            {
                playerDialogueBackground.transform.localScale = new Vector3(16.0f, 1.3f, 1.0f);
                playerDialogueBackground.transform.localPosition = new Vector2(0f, 2.14f);
                Debug.Log("Should have switched to the skinny");
            }
            dialogueBackground.SetActive(false);
            playerDialogueBackground.SetActive(true);
        } else
        {
            dialogueBackground.SetActive(true);
            playerDialogueBackground.SetActive(false);
        }
        dialogueTextGUI.SetText(displayLines[displayIter]);
        if (dialogueHandler.whichBranch() == Branch.Player2)
        {
            speakerNameGUI.gameObject.SetActive(false);
        }
        else
        {
            speakerNameGUI.gameObject.SetActive(true);
        }
        if (dialogueHandler.whichBranch() == Branch.Topaz2||
           dialogueHandler.whichBranch() == Branch.TopazRes11||
           dialogueHandler.whichBranch() == Branch.TopazRes12||
           dialogueHandler.whichBranch() == Branch.TopazRes13)
        {
            speakerNameGUI.SetText("Topaz");
        } else if(dialogueHandler.whichBranch() == Branch.Topaz1)
        {
            if(dialogueHandler.dialogueIter == 3)
            {
                speakerNameGUI.SetText("");
            } else
            {
                speakerNameGUI.SetText("Topaz");
            }
        } else if(dialogueHandler.whichBranch() == Branch.Player1)
        {
            speakerNameGUI.SetText("");
        }
        else if (dialogueHandler.whichBranch() == Branch.TopazRes21)
        {
            if (dialogueHandler.dialogueIter == 13 ||
               dialogueHandler.dialogueIter >= 19)
            {
                speakerNameGUI.gameObject.SetActive(false);
            } else
            {
                speakerNameGUI.gameObject.SetActive(true);

            }
            if (dialogueHandler.dialogueIter == 14 ||
               dialogueHandler.dialogueIter == 15 ||
               dialogueHandler.dialogueIter == 17)
            {
                speakerNameGUI.SetText("Doku");
            }
            else
            {
                speakerNameGUI.SetText("Topaz");
            }
        } else if (dialogueHandler.whichBranch() == Branch.TopazRes22)
        {
            if (dialogueHandler.dialogueIter >= 14)
            {
                speakerNameGUI.gameObject.SetActive(false);
            } else
            {
                speakerNameGUI.gameObject.SetActive(true);
            }
            if (dialogueHandler.dialogueIter == 12 ||
               dialogueHandler.dialogueIter == 13)
            {
                speakerNameGUI.SetText("Doku");
            }
            else
            {
                speakerNameGUI.SetText("Topaz");
            }
        } else if (dialogueHandler.whichBranch() == Branch.TopazRes23)
        {
            speakerNameGUI.SetText("Topaz");
            if ((dialogueHandler.dialogueIter >= 6 &&
               dialogueHandler.dialogueIter <= 8) ||
               dialogueHandler.dialogueIter >= 15)
            {
                speakerNameGUI.gameObject.SetActive(false);
            } else
            {
                speakerNameGUI.gameObject.SetActive(true);
            }
        }

        //Debug.Log("ending: " + dialogueHandler.whichEnding);
        if(dialogueHandler.whichEnding != 0)
        {
            Debug.Log("is it working?: " + dialogueHandler.whichEnding);
        }

        if(dialogueHandler.whichEnding == 1)
        {
            endingGUI.gameObject.SetActive(true);
            endingGUI.SetText("Ending 1/3: Hold Him Tight");
            progressTrackerScript.Instance.gotAnEnding(1);
        } else if(dialogueHandler.whichEnding == 2)
        {
            endingGUI.gameObject.SetActive(true);
            endingGUI.SetText("Ending 2/3: Him Him Accountable");
            progressTrackerScript.Instance.gotAnEnding(2);
        } else if (dialogueHandler.whichEnding == 3)
        {
            endingGUI.gameObject.SetActive(true);
            endingGUI.SetText("Ending 3/3: Hold Him Back");
            progressTrackerScript.Instance.gotAnEnding(3);
        } else
        {
            endingGUI.gameObject.SetActive(false);
        }

        if (dialogueHandler.isChoosing())
        {
            if ((timer > .8 && timer < 1)|
                (timer > 1.8 && timer < 2))
            {
                string newText = "<s>" + dialogueTextGUI.GetComponent<TMP_Text>().text + "</s>";
                dialogueTextGUI.SetText(newText);
                dialogueTextGUI.color = Color.red;
            }
            if (timer > 2.8 && timer < 3)
            {
                string newText = "<u>" + dialogueTextGUI.GetComponent<TMP_Text>().text + "</u>";
                dialogueTextGUI.SetText(newText);
                dialogueTextGUI.color = Color.green;
            }
        }
        

        // debug guis
        dialogueTimerGUI.SetText(timer.ToString());
        dialogueIterGUI.SetText(dialogueHandler.getIter().ToString());
        branchGUI.SetText(dialogueHandler.whichBranch().ToString());
        displayIterGUI.SetText(displayIter.ToString());
        isChoosingGUI.SetText(dialogueHandler.isChoosing().ToString());
        soundIterGUI.SetText(dialogueHandler.getSoundIter().ToString());
        ending3TimerGUI.SetText(ending3Timer.ToString());
    }

    public void ending3()
    {
        ending3CanPlay = false;
        ending3Timer += Time.deltaTime;
        if (ending3Timer > 3.15 && !ending3Triggers[0])
        {
            ending3Triggers[0] = true;
            dialogueHandler.advanceDialogue();
            Debug.Log("Ending 3 play line 1");
        }
        if (ending3Timer > 4.7 && !ending3Triggers[1])
        {
            ending3Triggers[1] = true;
            dialogueHandler.advanceDialogue();
            Debug.Log("Ending 3 play line 2");
        }
        if (ending3Timer > 6.3 && !ending3Triggers[2])
        {
            ending3Triggers[2] = true;
            dialogueHandler.advanceDialogue();
            Debug.Log("Ending 3 play line 3");
        }
        if (ending3Timer > 7.9 && !ending3Triggers[3])
        {
            ending3Triggers[3] = true;
            dialogueHandler.advanceDialogue();
            Debug.Log("Ending 3 play line 4");
        }
        if (ending3Timer > 9.6 && !ending3Triggers[4])
        {
            ending3Triggers[4] = true;
            dialogueHandler.advanceDialogue();
            Debug.Log("Ending 3 play line 5");
        }
        if (ending3Timer > 12.0 && !ending3Triggers[5])
        {
            ending3Triggers[5] = true;
            dialogueHandler.advanceDialogue();
            Debug.Log("Ending 3 play line 6");
        }
        if(ending3Timer > 12.0 && ending3Triggers[5])
        {
            ending3CanPlay = true;
        }
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

    public float getTimer()
    {
        return timer;
    }

    public int getDialogueIter()
    {
        return dialogueHandler.dialogueIter;
    }

    public Branch getBranch()
    {
        return dialogueHandler.whichBranch();
    }

    public int getWhichEnding()
    {
        return dialogueHandler.whichEnding;
    }

    public class DialogueHandler : ScriptableObject
    {
        public DialogueSystem dialogueSystem;

        public int dialogueIter = 0;
        public Branch currBranch = Branch.Topaz1;
        public Branch nextBranch = Branch.Player1;
        public Boolean choosing = false;
        public int soundIter = 0;

        [SerializeField]
        public int whichEnding = 0;

        public DialogueHandler(DialogueSystem newDialogueSystem)
        {
            dialogueSystem = newDialogueSystem;
        }

        public void advanceDialogue()
        {
            if(dialogueIter < 2 && currBranch == Branch.Topaz1)
            {
                dialogueIter++;
                dialogueSystem.addToDisplayLines(dialogueSystem.linesTopaz1[dialogueIter]);
                if(dialogueIter < 2)
                {
                    soundIter++;
                    playSound();
                }
            } 
            else if (dialogueIter >= 2 && currBranch == Branch.Topaz1)
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
                if (dialogueIter <= 20 && whichEnding == 0)
                {
                    dialogueIter++;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesTopazRes21[dialogueIter]);
                    if (soundIter < 14)
                    {
                        Debug.Log("soundIter = 13");
                        soundIter = 13;
                        playSound();
                        soundIter++;
                    } else if(!((dialogueIter>=13)&&(dialogueIter<=15))&&
                              !(dialogueIter==17)&&
                              !(dialogueIter>=19))
                    {
                        Debug.Log("sound Iter is now " + soundIter.ToString());
                        playSound();
                        soundIter++;
                    }
                }
                if(dialogueIter == 20)
                {
                    whichEnding = 1;
                }
            }
            else if (currBranch == Branch.TopazRes22)
            {
                if (dialogueIter <= 15 && whichEnding == 0)
                {
                    dialogueIter++;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesTopazRes22[dialogueIter]);
                    if (soundIter < 14)
                    {
                        Debug.Log("soundIter = 28");
                        soundIter = 28;
                        playSound();
                        soundIter++;
                    } else if(!(dialogueIter>=12))
                    {
                        Debug.Log("sound Iter is now " + soundIter.ToString());
                        playSound();
                        soundIter++;
                    }
                }
                if(dialogueIter == 15)
                {
                    whichEnding = 2;
                }
            }
            else if (currBranch == Branch.TopazRes23)
            {
                if (dialogueIter <= 23 && whichEnding == 0)
                {
                    dialogueIter++;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesTopazRes23[dialogueIter]);
                    if (soundIter < 14)
                    {
                        Debug.Log("soundIter = 40");
                        soundIter = 40;
                        playSound();
                        soundIter++;
                    }
                    else if(!(dialogueIter>=6&&dialogueIter<=8)&&
                            !(dialogueIter>=15))
                    {
                        Debug.Log("sound Iter is now " + soundIter.ToString());
                        playSound();
                        soundIter++;
                    }
                }
                if(dialogueIter == 23)
                {
                    whichEnding = 3;
                }
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
                nextBranch = Branch.Player2;
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
                if (nextBranch == Branch.Player1)
                {
                    nextBranch = Branch.TopazRes11;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer1[0]);
                }
                else if (nextBranch == Branch.Player2)
                {
                    nextBranch = Branch.TopazRes21;
                    dialogueSystem.addToDisplayLines(dialogueSystem.linesPlayer2[0]);
                }
            } else if (timer < 2)
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
                newName = "You";
            }
            else
            {
                newName = " ";
            }
            dialogueSystem.updateSpeakerDisplay(newName);
        }

        public void playSound()
        {
            dialogueSystem.audioSource.clip = dialogueSystem.audioClips[soundIter];
            dialogueSystem.audioSource.Play();
            Debug.Log(soundIter.ToString());
            Debug.Log(dialogueSystem.audioSource.clip.name);
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