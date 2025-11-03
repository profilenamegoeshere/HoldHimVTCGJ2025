using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCanvasScript : MonoBehaviour
{
    public bool fadeIning;
    public bool fadeOuting;
    public float timer;
    public float opacity;
    public CanvasGroup canvas;
    public ContinuePromptScript promptScript;

    // Start is called before the first frame update
    void Start()
    {
        fadeIning = true;
        fadeOuting = false;
        timer = 0;
        opacity = 0;
        canvas = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (!fadeIning) && (!fadeOuting))
        {
            fadeOuting = true;
            fadeIning = false;
            promptScript.stopFlashing();
            timer = 0;
        }

        if (fadeIning)
        {
            if(opacity < 100)
            {
                timer += Time.deltaTime * 50;
                opacity = timer;
                canvas.alpha = opacity/100;
            } else
            {
                fadeIning = false;
                promptScript.beginFlashing();
            }
        } else if (fadeOuting)
        {
            if (opacity > 0)
            {
                timer += Time.deltaTime * 50;
                opacity = 100 - timer;
                canvas.alpha = opacity/100;
            }
            else
            {
                SceneManager.LoadScene("Game");
                Debug.Log("Do a scene");
            }
        }
    }
}
