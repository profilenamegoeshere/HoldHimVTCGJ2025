using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarScript : MonoBehaviour
{
    public Image image;
    public float max;
    public float temp;
    public float curr;
    public float fill;
    public DialogueSystem dialoguSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        curr = 0;
        fill = 0;
    }

    // Update is called once per frame
    void Update()
    {
        temp = dialoguSystem.timer;
        if(temp == 0)
        {
            gameObject.GetComponent<Image>().color = Color.clear;
            image.GetComponent<Image>().color = Color.clear;

        } else
        {
            gameObject.GetComponent<Image>().color = new Color(45f/255f,63f/255f,94f/255f);
            image.GetComponent<Image>().color = new Color(74f/255f,144f/255f,241f/255f);
        }
        curr = temp % 1;
        fill = (curr / max);
        image.GetComponent<Image>().fillAmount = fill;
    }
}
