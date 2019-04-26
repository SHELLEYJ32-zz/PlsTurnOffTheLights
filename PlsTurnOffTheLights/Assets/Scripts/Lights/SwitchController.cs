using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public Sprite on;
    public Sprite off;
    //temp var
    public Material onMat;
    public Material offMat;
    public AudioSource clickSound;

    private bool switchIsOn;

    void Start()
    {
        switchIsOn = true;
        GetComponent<MeshRenderer>().material = onMat;
    }

    //return switch state
    public bool IsSwitchOn()
    {
        return switchIsOn;
    }

    //detect player interaction
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player" && collision.gameObject.GetComponent<CircleCollider2D>().enabled)
            ChangeState();
    }

    //change state
    private void ChangeState()
    {
        if (switchIsOn)
            GetComponent<MeshRenderer>().material = offMat;
        else
            GetComponent<MeshRenderer>().material = onMat;
        switchIsOn = !switchIsOn;
        clickSound.Play();
    }

}
