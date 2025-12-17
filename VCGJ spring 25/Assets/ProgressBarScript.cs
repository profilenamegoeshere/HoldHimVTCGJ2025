using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarScript : MonoBehaviour
{
    public Image unMask;
    public Image mask;
    public Image inner;
    public Image outer;
    public float max;
    public float temp;
    public float curr;
    public float fill;
    public DialogueSystem dialoguSystem;
    public Color unmaskedColor;
    public Color outlineColor;
    
    // Start is called before the first frame update
    void Start()
    {
        curr = 0;
        fill = 0;
        unmaskedColor = new Color(1f, 1f, 1f);
        outlineColor = new Color(56f / 255f, 56f / 255f, 56f / 255f);
    }

    // Update is called once per frame
    void Update()
    {
        temp = dialoguSystem.timer;
        curr = temp % 1;
        fill = (curr / max);
        if (temp == 0)
        {
            unMask.GetComponent<Image>().color = Color.clear;
            mask.GetComponent<Image>().color = Color.clear;
            inner.GetComponent<Image>().color = Color.clear;
            outer.GetComponent<Image>().color = Color.clear;

        } else
        {
            if (fill < .5)
            {
                mask.GetComponent<Image>().color = new Color(fill*2, 1f, 0f);
            } else
            {
                mask.GetComponent<Image>().color = new Color(1f, (1f - ((fill-.5f)*2)), 0f);
            }
            unMask.GetComponent<Image>().color = unmaskedColor;
            inner.GetComponent<Image>().color = outlineColor;
            outer.GetComponent<Image>().color = outlineColor;
        }
        mask.GetComponent<Image>().fillAmount = fill;
    }
}
