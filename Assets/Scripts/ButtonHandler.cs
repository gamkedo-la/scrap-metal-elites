using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void NewGameBtn(string startNewGame)
    {
        SceneManager.LoadScene(startNewGame);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
