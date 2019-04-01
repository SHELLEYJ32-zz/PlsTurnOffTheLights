using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwitchNameController : MonoBehaviour
{
    public Text userNameDisplay;

    public void Display(string userName)
    {
        //Debug.Log(userName);
        userNameDisplay.text = userName;
    }

}
