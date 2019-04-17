using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed;
    public int playerHP;
    public float monsterCaughtTime;
    public float interactTimer;
    public PolygonCollider2D wallCollider;
    public PolygonCollider2D monsterCollider;
    public CircleCollider2D interactCollider;

    private Rigidbody2D playerRB;
    private float interactLocalTimer;
    private float monsterLocalTimer;
    private bool caught;
    private bool inLight;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().enabled = false;
        caught = false;
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
            if (monsterCollider.IsTouching(collision.gameObject.GetComponent<MonsterIndividualController>().bodyCollider))
            {
                if (!caught)
                {
                    caught = true;
                    monsterLocalTimer = monsterCaughtTime;
                }
            }
        }
        if (collision.gameObject.tag == "Light")
        {
            inLight = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            inLight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster" && caught)
        {
            caught = false;
        }
        if (collision.gameObject.tag == "Light")
        {
            inLight = false;
        }
    }

    public bool InLight()
    {
        return inLight;
    }

    //lose one life
    void LoseLife()
    {
        if (playerHP - 1 > 0)
        {
            playerHP--;
            monsterLocalTimer = monsterCaughtTime;
        }
        else
        {
            playerHP--;
            Die();
        }

    }

    //trigger death
    void Die()
    {
        SceneManager.LoadScene("GameOverScene");
    }

}
