using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public Sprite on;
    public Sprite off;
    private bool switchIsOn;

    // Start is called before the first frame update
    void Start()
    {
        switchIsOn = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
