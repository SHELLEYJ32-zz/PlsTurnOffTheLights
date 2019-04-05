using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed;
    public int playerHP;
    public float monsterCaughtTime;
    public float interactTimer;
    public BoxCollider2D wallCollider;
    public BoxCollider2D monsterCollider;
    public CircleCollider2D interactCollider;
    //public CircleCollider2D attractiveCollider;
    //public BoxCollider2D interactCollider;

    private Rigidbody2D playerRB;
    private float interactLocalTimer;
    private float monsterLocalTimer;
    private bool caught;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().enabled = false;
        caught = false;
        Debug.Log("player HP left: " + playerHP);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        //if caught, count down caught time
        if (caught)
        {
            if (monsterLocalTimer < 0)
                LoseLife();
            else
                monsterLocalTimer -= Time.deltaTime;
        }

        //if interact, count down interact time 
        if (interactLocalTimer > 0)
            interactLocalTimer -= Time.deltaTime;
        else
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }

        if (Input.GetKeyDown("space"))
        {
            //reset timer
            interactLocalTimer = interactTimer;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            if (monsterCollider.IsTouching(collision.gameObject.GetComponent<CircleCollider2D>()))
            {

                if (!caught)
                {
                    caught = true;
                    monsterLocalTimer = monsterCaughtTime;
                }
                else
                {
                    //if caught by multiple monsters, player dies immediately
                    Die();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && caught)
        {
            caught = false;
        }
    }

    //lose one life
    void LoseLife()
    {
        playerHP--;
        monsterLocalTimer = monsterCaughtTime;
        Debug.Log("player HP left: " + playerHP);

        if (playerHP == 0)
            Die();
    }

    //trigger death
    void Die()
    {

    }

}
