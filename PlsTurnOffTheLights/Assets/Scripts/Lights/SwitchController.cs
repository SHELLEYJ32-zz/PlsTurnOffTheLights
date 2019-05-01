using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public Sprite on;
    public Sprite off;
    public AudioSource clickSound;

    private bool switchIsOn;

    void Start()
    {
        switchIsOn = true;
        GetComponent<SpriteRenderer>().sprite = on;
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
            GetComponent<SpriteRenderer>().sprite = off;
        else
            GetComponent<SpriteRenderer>().sprite = on;
        switchIsOn = !switchIsOn;
        clickSound.Play();
    }

}
