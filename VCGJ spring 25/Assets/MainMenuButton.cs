using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void clicked()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
