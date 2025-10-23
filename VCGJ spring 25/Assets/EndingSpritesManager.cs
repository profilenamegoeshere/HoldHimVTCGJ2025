using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        if (whichEnding != 0)
        {
            walls.SetActive(false);
            table.SetActive(false);
            Topaz.SetActive(false);
            Ending.SetActive(true);
            restartButtonGUI.SetActive(true);
            mainMenuButtonGUI.SetActive(true);
            if (whichEnding == 1)
            {
                Ending.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Ending 1");
            } 
            //else if(whichEnding == 2)
            //{
            //    Ending.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Ending 2");
            //}
            //else if (whichEnding == 3)
            //{
            //    Ending.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Ending 3");
            //}
        }
    }
}
