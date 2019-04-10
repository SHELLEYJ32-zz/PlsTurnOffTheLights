using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public SwitchController pairedSwitch;

    private void FixedUpdate()
    {
        if (pairedSwitch.IsSwitchOn())
        {
            GetComponent<Light>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
        }

        else
        {
            GetComponent<Light>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }


    }

}
