using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [Header("State Variables")]
    public GameInfo gameInfo;

    public string deathMatchScene = "Arena DeathMatch";
    public string championshipScene = "Arena DeathMatch";

    public void OnDeathMatchClick()
    {
        gameInfo.gameMode = GameMode.DeathMatch;
        SceneManager.LoadScene(deathMatchScene);
    }

    public void OnChampionshipClick()
    {
        gameInfo.gameMode = GameMode.Championship;
        SceneManager.LoadScene(championshipScene);
    }

    public void OnQuitGameClick()
    {
        Application.Quit();
    }

    public void OnCreditsClick()
    {
    }
}
