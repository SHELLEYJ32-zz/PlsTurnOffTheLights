using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed;
    public int playerHP;
    public float monsterCaughtTime;
    public float interactTimer;
    public PolygonCollider2D wallCollider;
    public PolygonCollider2D monsterCollider;
    public CircleCollider2D interactCollider;
    public Animator animator;
    public AudioSource footstepSound;
    public AudioSource loseLifeSound;
    public Material normalMat;
    public Material hurtMat;

    private Rigidbody2D playerRB;
    private float interactLocalTimer;
    private float monsterLocalTimer;
    private float deathLocalTimer;
    private bool caught;
    private bool inLight;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().material = normalMat;
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

        //interact
        if (Input.GetKeyDown("space"))
        {
            //reset timer
            interactLocalTimer = interactTimer;
            Interact();
        }


        //flip
        if (playerRB.velocity.x > 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

        //set animator
        animator.SetFloat("speedX", Mathf.Abs(playerRB.velocity.x));

        if (Mathf.Abs(playerRB.velocity.x) <= 0)
        {
            if (playerRB.velocity.y < 0)
            {
                animator.SetBool("static", false);
                animator.SetBool("verticalDown", true);
                animator.SetBool("verticalUp", false);
            }
            else if (playerRB.velocity.y > 0)
            {
                animator.SetBool("static", false);
                animator.SetBool("verticalDown", false);
                animator.SetBool("verticalUp", true);
            }
            else
            {
                animator.SetBool("static", true);
                animator.SetBool("verticalDown", false);
                animator.SetBool("verticalUp", false);
            }
        }
        else
        {
            animator.SetBool("static", false);
            animator.SetBool("verticalDown", false);
            animator.SetBool("verticalUp", false);
        }


    }

    //take keyboard command to move
    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        playerRB.velocity = new Vector2(moveHorizontal * playerSpeed, moveVertical * playerSpeed);

        if (Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0)
            footstepSound.Play();

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
            GetComponent<SpriteRenderer>().material = normalMat;
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
            GetComponent<SpriteRenderer>().material = hurtMat;
            loseLifeSound.Play();
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
        playerRB.constraints = RigidbodyConstraints2D.FreezePosition;
        SceneManager.LoadScene("GameOverScene");
    }

}
