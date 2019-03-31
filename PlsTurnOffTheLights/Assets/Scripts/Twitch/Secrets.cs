using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secrets : MonoBehaviour
{
    public static Secrets Instance;
    //Holds info of the twitch bot
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject); //makes instance persist across scenes
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); //deletes copies of global which do not need to exist, so right version is used to get info from
        }
    }

    public string clientID = "vrd9jxv1kwcz7x5shslac31ob6pjx6";
    public string secret = "diagij48v92mnowq13gci7cl1d1tj7";
    public string accessToken = "abqxt096j5gcnysc7w8meq7ogxiuwu";
    public string refreshToken = "h46w3wm071sv1m21k9o7mfkpfo80wz5gqssfo3z9rp6htdzvcx";
}
