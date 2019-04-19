using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwitchNameController : MonoBehaviour
{
    public Text userNameDisplay;

    public void Display(string direction)
    {
        //Debug.Log(userName);
        userNameDisplay.text = direction;
    }

}
