
using UnityEngine;
using UnityEngine.UI;

public class MonsterIndividualController : MonoBehaviour
{
    public float originalMonsterSpeed;
    public float twitchMonsterSpeed;
    public float twitchEffectiveTime;
    public float tempDisappearTime;
    public float attractiveRadius;
    public CircleCollider2D bodyCollider;

    private Rigidbody2D monsterRB;
    private float driftChangeTimer = 3.0f;
    private float driftLocalTimer;
    private bool twitchFlag;
    private float twicthLocalTimer;
    private GameObject signalPrefab;
    private GameObject signalDisplay;
    private bool chaseFlag;
    private GameObject player;
    private int moveChanceX;
    private int moveChanceY;
    private GameObject lightCamera;
    private Vector2 birthPlace;
    private bool disappearFlag;
    private float disappearLocalTimer;

    void Start()
    {
        birthPlace = transform.position;
        monsterRB = GetComponent<Rigidbody2D>();
        driftLocalTimer = driftChangeTimer;
        signalPrefab = Resources.Load("Signal") as GameObject;
        signalDisplay = Instantiate(signalPrefab);
        signalDisplay.SetActive(false);
        player = GameObject.FindGameObjectWithTag("player");
        lightCamera = null;
    }

    void FixedUpdate()
    {
        if (!twitchFlag && !chaseFlag && !disappearFlag)
        {
            Drift();
        }
        else if (disappearFlag)
        {
            if (disappearLocalTimer > 0)
                disappearLocalTimer -= Time.deltaTime;
            else
            {
                Regenerate();

            }

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

        if (!twitchFlag && Vector2.Distance(transform.position, player.transform.position) < attractiveRadius)
        {
            Chase(true);
        }
        else
        {
            Chase(false);
        }


    }

    //check light intensity in one direction
    private bool CheckOneDirection(int x, int y)
    {
        if (x == 0 && y == 0)
            lightCamera = transform.GetChild(0).gameObject;
        else if (x == 0 && y == 1)
            lightCamera = transform.GetChild(1).gameObject;
        else if (x == 0 && y == 2)
            lightCamera = transform.GetChild(2).gameObject;
        else if (x == 1 && y == 0)
            lightCamera = transform.GetChild(3).gameObject;
        else if (x == 1 && y == 1)
            lightCamera = transform.GetChild(4).gameObject;
        else if (x == 1 && y == 2)
            lightCamera = transform.GetChild(5).gameObject;
        else if (x == 2 && y == 0)
            lightCamera = transform.GetChild(6).gameObject;
        else if (x == 2 && y == 1)
            lightCamera = transform.GetChild(7).gameObject;
        else if (x == 2 && y == 2)
            lightCamera = transform.GetChild(8).gameObject;

        //Debug.Log(lightCamera.GetComponent<LightCheckController>().lightLevel);
        return lightCamera.GetComponent<LightCheckController>().CheckLightIntensity();
    }


    private bool CheckAllDirection()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //if one direction is available
                if (CheckOneDirection(i, j))
                    return true;
            }

        }
        //if all directions are invalid
        return false;
    }

    //drift randomly within dark space
    private void Drift()
    {
        if (driftLocalTimer <= 0)
        {
            driftLocalTimer = driftChangeTimer;

            moveChanceX = Random.Range(-1, 2);
            moveChanceY = Random.Range(-1, 2);

            if (CheckAllDirection())
            {
                while (moveChanceX == 0 && moveChanceY == 0 || !CheckOneDirection(moveChanceX + 1, moveChanceY + 1))
                {
                    moveChanceX = Random.Range(-1, 2);
                    moveChanceY = Random.Range(-1, 2);
                }

                //Debug.Log("x: " + moveChanceX + " y: " + moveChanceY);
                //CheckOneDirection(moveChanceX + 1, moveChanceY + 1);
                //Debug.Log(lightCamera.GetComponent<LightCheckController>().lightLevel);

                monsterRB.velocity = new Vector2(moveChanceX * originalMonsterSpeed, moveChanceY * originalMonsterSpeed);
                monsterRB.velocity = Vector2.ClampMagnitude(monsterRB.velocity, originalMonsterSpeed);
            }
            else
            {
                Regenerate();
                driftLocalTimer = 0;
            }
        }
        else
        {
            driftLocalTimer -= Time.deltaTime;
            if (!CheckOneDirection(moveChanceX + 1, moveChanceY + 1))
            {
                //Debug.Log("false");
                driftLocalTimer = 0;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            if (bodyCollider.IsTouching(collision.gameObject.GetComponent<PlayerController>().monsterCollider))
                Catch();
        }
        else if (bodyCollider.IsTouching(collision))
            Teleport(collision);

    }

    private void Teleport(Collider2D collision)
    {

        if (collision.gameObject.tag == "LeftWall")
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + 14, gameObject.transform.position.y);
        }
        else if (collision.gameObject.tag == "RightWall")
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - 14, gameObject.transform.position.y);
        }
        else if (collision.gameObject.tag == "BottomWall")
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 14);
        }
        else if (collision.gameObject.tag == "TopWall")
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 14);
        }

    }


    //follow the player within a certain radius
    private void Chase(bool attract)
    {
        if (attract)
        {
            chaseFlag = true;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, originalMonsterSpeed * Time.deltaTime);
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
        //play animation
    }

    //disappear when surrounded by light
    private void Regenerate()
    {
        //if (!disappearFlag)
        //{
        //    //play disappear animation
        //    gameObject.SetActive(false);
        //    disappearFlag = true;
        //    disappearLocalTimer = tempDisappearTime;
        //}
        //else
        //{

        //regenerate
        transform.position = birthPlace;
        //disappearFlag = false;
        //gameObject.SetActive(true);
        //}
    }

    //move based on twitch command
    public void TwitchMove(string direction)
    {
        //if no command is received
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