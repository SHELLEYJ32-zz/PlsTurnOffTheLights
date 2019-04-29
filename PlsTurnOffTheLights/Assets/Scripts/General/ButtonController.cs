using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void LoadInstruction()
    {
        SceneManager.LoadScene("InstructionScene");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("CreditScene");
    }

    public void LoadTwitchSetting()
    {
        SceneManager.LoadScene("TwitchInputScene");
    }

    public void Submit(string text)
    {
        GameplayController.instance.twitchName = text;
        Debug.Log(GameplayController.instance.twitchName);
    }

    public void LoadOption()
    {
        SceneManager.LoadScene("OptionScene");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameplayController.instance.paused = false;
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadFirstLv()
    {
        SceneManager.LoadScene("FirstLvScene");
    }

    public void Resume()
    {
        GameplayController.instance.Resume();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
