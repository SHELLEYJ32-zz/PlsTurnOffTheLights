using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject twitchControl;
    private GameObject[] switches;

    void Start()
    {
        if (GameplayController.instance.twitchName != "")
            twitchControl.SetActive(true);
        else
            twitchControl.SetActive(false);

        switches = GameObject.FindGameObjectsWithTag("Switch");
    }

    private void FixedUpdate()
    {
        if (LvlComplete())
            SceneManager.LoadScene("LvCompleteScene");

    }

    public bool LvlComplete()
    {
        for (int i = 0; i < switches.Length; i++)
        {
            if (switches[i].GetComponent<SwitchController>().IsSwitchOn())
                return false;
        }
        return true;
    }
}
