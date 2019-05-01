
using UnityEngine;
using UnityEngine.UI;

public class MonsterIndividualController : MonoBehaviour
{
    public float originalMonsterSpeed;
    public float twitchMonsterSpeed;
    public float twitchEffectiveTime;
    public float tempDisappearTime;
    public float attractiveRadius;
    public PolygonCollider2D bodyCollider;
    public AudioSource attackSound;
    public AudioSource deathSound;
    public Animator animator;

    private Rigidbody2D monsterRB;
    private float driftChangeTimer = 3.0f;
    private float driftLocalTimer;
    private bool twitchFlag;
    private float twicthLocalTimer;
    private GameObject signalPrefab;
    private GameObject signalDisplay;
    private bool chaseFlag;
    private bool disappearFlag;
    private float disappearTimer = 2.0f;
    private float disappearLocalTimer;
    private GameObject player;
    private int moveChanceX;
    private int moveChanceY;
    private Vector2 birthPlace;
    private Vector2 originalVelocity;

    void Start()
    {
        birthPlace = transform.position;
        monsterRB = GetComponent<Rigidbody2D>();
        driftLocalTimer = 0;
        signalPrefab = Resources.Load("Signal") as GameObject;
        signalDisplay = Instantiate(signalPrefab);
        signalDisplay.SetActive(false);
        player = GameObject.FindGameObjectWithTag("player");
    }

    void FixedUpdate()
    {
        if (disappearFlag)
        {
            if (disappearLocalTimer > 0)
                disappearLocalTimer -= Time.deltaTime;
            else
            {
                disappearFlag = false;
                disappearLocalTimer = disappearTimer;
                transform.position = birthPlace;
                gameObject.SetActive(false);
            }

        }
        else if (!twitchFlag && !chaseFlag)
        {
            Drift();
        }
        else if (twitchFlag)
        {
            if (twicthLocalTimer > 0)
            {
                twicthLocalTimer -= Time.deltaTime;
                signalDisplay.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1);
            }
            else
            {
                twitchFlag = false;
                signalDisplay.SetActive(false);
                //if twitch commond controls the monster, reset drift timer
                driftLocalTimer = driftChangeTimer;
                monsterRB.velocity = Vector2.ClampMagnitude(monsterRB.velocity, originalMonsterSpeed);
            }
        }
        bool closeEnough = Vector2.Distance(transform.position, player.transform.position) <= attractiveRadius;
        bool playerInLight = player.GetComponent<PlayerController>().InLight();

        if (!disappearFlag && !twitchFlag && !playerInLight && closeEnough)
        {
            Chase(true);
            //Debug.Log("chase");
        }
        else
        {
            Chase(false);
            //Debug.Log("abort");
        }

        //flip
        if (monsterRB.velocity.x > 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

        //set animator
        animator.SetBool("Horizontal", Mathf.Abs(monsterRB.velocity.x) > 0);

        if (Mathf.Abs(monsterRB.velocity.x) <= 0)
        {
            animator.SetBool("verticalDown", monsterRB.velocity.y < 0);
            animator.SetBool("verticalUp", monsterRB.velocity.y > 0);
        }
        else
        {
            animator.SetBool("verticalDown", false);
            animator.SetBool("verticalUp", false);
        }
    }

    //drift randomly within dark space
    private void Drift()
    {
        if (driftLocalTimer <= 0)
        {
            driftLocalTimer = driftChangeTimer;

            moveChanceX = Random.Range(-1, 2);
            moveChanceY = Random.Range(-1, 2);

            //avoid it from not moving
            while (moveChanceX == 0 && moveChanceY == 0)
            {
                moveChanceX = Random.Range(-1, 2);
                moveChanceY = Random.Range(-1, 2);
            }

            monsterRB.velocity = new Vector2(moveChanceX * originalMonsterSpeed, moveChanceY * originalMonsterSpeed);
            monsterRB.velocity = Vector2.ClampMagnitude(monsterRB.velocity, originalMonsterSpeed);

        }
        else
        {
            driftLocalTimer -= Time.deltaTime;
        }
    }

    //with player, lights
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            if (bodyCollider.IsTouching(collision.gameObject.GetComponent<PlayerController>().monsterCollider))
                Catch();
        }
        else if (bodyCollider.IsTouching(collision))
        {
            TurnBack(collision);
        }

    }

    //if lights are turned back on when monster is in, monsters regenerate
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            driftLocalTimer = 0;
            Disappear();
        }
    }

    //with other monsters, wall, switches, lightbulbs
    private void TurnBack(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "LeftWall" || collision.gameObject.tag == "RightWall")
        {
            monsterRB.velocity = new Vector2(-monsterRB.velocity.x, monsterRB.velocity.y);
        }
        else if (collision.gameObject.tag == "TopWall" || collision.gameObject.tag == "BottomWall")
        {
            monsterRB.velocity = new Vector2(monsterRB.velocity.x, -monsterRB.velocity.y);
        }
        else
        {
            monsterRB.velocity = new Vector2(-monsterRB.velocity.x, -monsterRB.velocity.y);
        }
        monsterRB.velocity = Vector2.ClampMagnitude(monsterRB.velocity, originalMonsterSpeed);
    }

    //follow the player within a certain radius
    private void Chase(bool attract)
    {
        if (attract)
        {
            chaseFlag = true;
            monsterRB.velocity = new Vector2(0, 0);
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position, originalMonsterSpeed * Time.deltaTime);
            driftLocalTimer = 0;
        }

        else
        {
            chaseFlag = false;
        }

        //Debug.Log(chaseFlag);
    }

    //catch the player if it touches the player
    private void Catch()
    {
        attackSound.Play();
        //play animation
    }

    //disappear when surrounded by light
    public void Disappear()
    {
        deathSound.Play();
        GetComponent<ParticleSystem>().Play();
        GetComponent<SpriteRenderer>().enabled = false;
        bodyCollider.enabled = false;
        disappearLocalTimer = disappearTimer;
        disappearFlag = true;
    }

    //move based on twitch command
    public void TwitchMove(string direction)
    {
        //if command is received
        if (direction != "")
        {
            twitchFlag = true;
            twicthLocalTimer = twitchEffectiveTime;

            int moveChanceX = 0;
            int moveChanceY = 0;

            if (direction == "u")
            {
                moveChanceX = 0;
                moveChanceY = 1;
            }
            else if (direction == "d")
            {
                moveChanceX = 0;
                moveChanceY = -1;
            }
            else if (direction == "l")
            {
                moveChanceX = -1;
                moveChanceY = 0;
            }
            else if (direction == "r")
            {
                moveChanceX = 1;
                moveChanceY = 0;
            }

            monsterRB.velocity = new Vector2(moveChanceX * twitchMonsterSpeed, moveChanceY * twitchMonsterSpeed);

        }
        //Debug.Log("twitch command received");
    }

    //display twitch name when command received
    public void TwitchActiveSignal(string text)
    {
        signalDisplay.GetComponent<TwitchNameController>().Display(text);
        signalDisplay.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1);
        signalDisplay.SetActive(true);
        //Debug.Log("final vote: " + text);
    }
}