using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopazSpriteScript : MonoBehaviour
{
    public GameObject dialogueSystem;
    public DialogueSystem dialogueScript;
    public SpriteRenderer spriteRenderer;
    public Branch branch;
    public int dialogueIter;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        dialogueScript = dialogueSystem.GetComponent<DialogueSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        dialogueIter = dialogueScript.getDialogueIter();
        branch = dialogueScript.getBranch();

        if (branch == Branch.Topaz1)
        {
            if (dialogueIter == 1)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
            else if (dialogueIter == 2)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Cheeky");
            }
            else if (dialogueIter == 4)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
        }
        else if (branch == Branch.TopazRes11)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Cheeky");
        }
        else if (branch == Branch.TopazRes12)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Closed Eyes");
        }
        else if (branch == Branch.TopazRes13)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Cheeky");
        }
        else if (branch == Branch.Topaz2)
        {
            if (dialogueIter == 1)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Closed Eyes");
            }
            else if (dialogueIter == 2)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
            else if (dialogueIter == 3)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Sus");
            }
            else if (dialogueIter == 4)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Closed Eyes");
            }
            else if (dialogueIter == 5)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
            else if (dialogueIter == 7)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Sus");
            }
        }
        else if (branch == Branch.TopazRes21)
        {
            if (dialogueIter == 1)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Angry");
            }
            else if (dialogueIter == 4)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
            else if (dialogueIter == 5)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Angry");
            }
            else if (dialogueIter == 6)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Sus");
            }
            else if (dialogueIter == 9)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Angry");
            }
            else if (dialogueIter == 10)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Desperate");
            }
            else if (dialogueIter == 11)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Angry");
            }
            else if (dialogueIter == 13)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Sus");
            }
            else if (dialogueIter == 17)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Desperate");
            }
            else if (dialogueIter == 19)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Sus");
            }
        }
        else if (branch == Branch.TopazRes22)
        {
            if (dialogueIter == 1)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
            else if (dialogueIter == 2)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Closed Eyes");
            }
            else if (dialogueIter == 4)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
            else if (dialogueIter == 6)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Closed Eyes");
            }
            else if (dialogueIter == 7)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
            else if (dialogueIter == 8)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Desperate");
            }
            else if (dialogueIter == 9)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
            else if (dialogueIter == 10)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Angry");
            }
            else if (dialogueIter == 12)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Desperate");
            }
        }
        else if (branch == Branch.TopazRes23)
        {
            if (dialogueIter == 1)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Normal");
            }
            else if (dialogueIter == 2)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Angry");
            }
            else if (dialogueIter == 10)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Desperate");
            }
            else if (dialogueIter == 11)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Yelling");
            }
            else if (dialogueIter == 15)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Images/Topaz Desperate");
            }
        }
    }
}
