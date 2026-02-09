//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EndingSpritesManager : MonoBehaviour
{
    public GameObject walls;
    public GameObject table;
    public GameObject Topaz;
    public GameObject Ending;
    public GameObject dialogueSystem;
    public DialogueSystem dialogueScript;
    public GameObject restartButtonGUI;
    public GameObject mainMenuButtonGUI;
    public int whichEnding;

    // Start is called before the first frame update
    void Start()
    {
        dialogueScript = dialogueSystem.GetComponent<DialogueSystem>();
        whichEnding = 0;
        walls.SetActive(true);
        table.SetActive(true);
        Topaz.SetActive(true);
        Ending.SetActive(false);
        restartButtonGUI.SetActive(false);
        mainMenuButtonGUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        whichEnding = dialogueScript.getWhichEnding();
        if (whichEnding == 1 || whichEnding == 2 || 
            (whichEnding == 3 && dialogueScript.getDialogueIter() > 21))
        {
            walls.SetActive(false);
            table.SetActive(false);
            Topaz.SetActive(false);
            Ending.SetActive(true);
            restartButtonGUI.SetActive(true);
            mainMenuButtonGUI.SetActive(true);
            if (whichEnding == 1)
            {
                Ending.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/Ending 1");
            }
            else if (whichEnding == 2)
            {
                Ending.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/Ending 2");
            }
        }
        if (dialogueScript.isEnding3)
        {
            if (dialogueScript.getDialogueIter() == 15)
            {
                walls.SetActive(false);
                table.SetActive(false);
                Topaz.SetActive(false);
                Ending.SetActive(true);
                Ending.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/Ending 3-1");
            } 
            else if(dialogueScript.getDialogueIter() == 16)
            {
                Ending.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/Ending 3-2");
            }
            else if (dialogueScript.getDialogueIter() == 19)
            {
                Ending.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/Ending 3-3");
            }
            else if (dialogueScript.getDialogueIter() == 20)
            {
                Ending.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/Ending 3-4");
            }
            else if (dialogueScript.getDialogueIter() == 21)
            {
                Ending.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/Ending 3-5");
            }
        }
    }
}
