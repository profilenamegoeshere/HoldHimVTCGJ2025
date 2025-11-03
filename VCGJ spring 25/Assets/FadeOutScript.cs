using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOutScript : MonoBehaviour
{
    public Image image;
    public Color currColor;
    public float timer;
    public float opacity;
    public bool fading;
    
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        timer = 0;
        opacity = 0;
        fading = false;
        image.color = new Color(currColor.r, currColor.g, currColor.b, opacity);
    }
    public void fadeOut()
    {
        fading = true;
    }

    private void Update()
    {
        if (fading)
        {
            currColor = image.color;
            timer += (Time.deltaTime * 33);
            opacity = timer;
            image.color = new Color(currColor.r, currColor.g, currColor.b, (opacity/100));
            if(opacity > 100)
            {
                SceneManager.LoadScene("Tutorial");
            }
        }
    }
}
