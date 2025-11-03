using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContinuePromptScript : MonoBehaviour
{
    public float timer;
    public bool rising;
    public bool flashing;
    public bool stoppedFlashing;
    public TextMeshProUGUI prompt;
    public Color currColor;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
        rising = true;
        flashing = false;
        stoppedFlashing = false;
        currColor = prompt.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashing)
        {
            if (rising)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer -= Time.deltaTime;
            }
            prompt.color = new Color(currColor.r, currColor.g, currColor.b, timer);
        }

        if(timer > 1.0)
        {
            rising = false;
        } else if (timer < 0)
        {
            rising = true;
        }
    }

    public void beginFlashing()
    {
        flashing = true;
    }

    public void stopFlashing()
    {
        stoppedFlashing = true;
        prompt.color = new Color(currColor.r, currColor.g, currColor.b, 1);
    }
}
