using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void PressedStartButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void PressedQuitButton()
    {
        Application.Quit();
    }
}
