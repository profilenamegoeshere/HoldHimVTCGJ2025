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
    public DialogueSystem dialoguSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temp = dialoguSystem.timer;
        if(temp == 0)
        {
            gameObject.GetComponent<Image>().color = Color.clear;
        } else
        {
            gameObject.GetComponent<Image>().color = new Color(45f/255f,63f/255f,94f/255f);
        }

            curr = temp % 1;
        float fill = (curr / max) * 500;
        image.rectTransform.sizeDelta = new Vector2(fill, 75);
    }
}
