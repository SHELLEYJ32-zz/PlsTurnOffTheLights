using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed = 2.0f;
    public int playHP = 3;
    public float monsterCaughtTime;
    public float interactTimer = 0.3f;
    //temp var
    public Material normalMat;
    public Material interactMat;

    private Rigidbody2D playerRB;
    private float localTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        //count down interact time if effective
        if (localTimer > 0)
            localTimer -= Time.deltaTime;
        else
        {
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<MeshRenderer>().material = normalMat;
        }

        if (Input.GetKeyDown("space"))
        {
            //reset timer
            localTimer = interactTimer;
            //Debug.Log("triggered");
            Interact();
        }

    }

    //take keyboard command to move
    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        playerRB.velocity = new Vector2(moveHorizontal * playerSpeed, moveVertical * playerSpeed);

        //max speed constrained
        playerRB.velocity = Vector2.ClampMagnitude(playerRB.velocity, playerSpeed);
    }

    //interact with objects around
    void Interact()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<MeshRenderer>().material = interactMat;
    }

    //lose one life
    void LoseLife()
    {

    }

    //trigger death
    void Die()
    {

    }

}
