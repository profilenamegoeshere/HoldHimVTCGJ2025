using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;



public class DialogueSystem : MonoBehaviour
{
    public int beginningDialogueIter;
    public TextMeshProUGUI dialogueTextGUI;
    public TextMeshProUGUI dialogueTimerGUI;
    public TextMeshProUGUI dialogueIterGUI;
    public Boolean isChoosing;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        beginningDialogueIter = 0;
        timer = 0;
        isChoosing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChoosing)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                timer += Time.deltaTime;
            }
            else
            {
                beginningDialogueIter += (int)Mathf.Floor(timer);
                timer = 0;
                isChoosing = false;
            }
        }

        if (!isChoosing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                beginningDialogueIter++;
            }
        }

        if (beginningDialogueIter == 3)
        {
            isChoosing = true;
        }

        updateScreen();
    }

    void updateScreen()
    {
        dialogueTextGUI.SetText(timer.ToString());
        dialogueIterGUI.SetText(beginningDialogueIter.ToString());
    }
}
