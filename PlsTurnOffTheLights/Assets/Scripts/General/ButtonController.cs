using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void LoadInstruction()
    {
        Debug.Log("clicked I");
        SceneManager.LoadScene("InstructionScene");
    }

    public void LoadTwitchSetting()
    {
        Debug.Log("clicked T");
        SceneManager.LoadScene("TwitchInputScene");
    }

    public void Submit()
    {

    }

    public void LoadOption()
    {
        SceneManager.LoadScene("OptionScene");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadFirstLv()
    {
        SceneManager.LoadScene("FirstLvScene");
    }

    public void Pause()
    {

    }

    public void Resume() { }

    public void Exit()
    {
        Application.Quit();
    }
}
