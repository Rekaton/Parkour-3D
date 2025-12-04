using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    public void ButtonMoveScene(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void Quitgame()
    {
        Application.Quit();
    }
}
