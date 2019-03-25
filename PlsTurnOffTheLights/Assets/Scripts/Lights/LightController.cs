using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public SwitchController pairedSwitch;

    private void FixedUpdate()
    {
        if (pairedSwitch.IsSwitchOn())
            GetComponent<Light>().enabled = true;
        else
            GetComponent<Light>().enabled = false;
    }

}
