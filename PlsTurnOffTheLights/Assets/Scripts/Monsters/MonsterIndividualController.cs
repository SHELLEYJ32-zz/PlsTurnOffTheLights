
using UnityEngine;
using UnityEngine.UI;

public class MonsterIndividualController : MonoBehaviour
{
    public float originalMonsterSpeed = 1.5f;
    public float twitchMonsterSpeed = 4.0f;
    public float twitchEffectiveTime = 0.5f;
    public float maxLightIntensity;
    public float tempDisappearTime;

    private Rigidbody2D monsterRB;
    private float driftChangeTimer = 3.0f;
    private float driftLocalTimer;
    private bool twitchFlag;
    private float twicthLocalTimer;
    private GameObject twitchNamePrefab;
    private GameObject twitchNameDisplay;

    void Start()
    {
        monsterRB = GetComponent<Rigidbody2D>();
        driftLocalTimer = driftChangeTimer;
        twitchFlag = false;
        twitchNamePrefab = Resources.Load("TwitchName") as GameObject;
    }

    void FixedUpdate()
    {

        if (!twitchFlag)
        {
            Drift();
        }
        else
        {
            if (twicthLocalTimer > 0)
            {
                twicthLocalTimer -= Time.deltaTime;
                twitchNameDisplay.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1);
            }
            else
            {
                twitchFlag = false;
                twitchNameDisplay.SetActive(false);
                //if twitch commond controls the monster, reset drift timer
                driftLocalTimer = driftChangeTimer;
                monsterRB.velocity = Vector2.ClampMagnitude(monsterRB.velocity, originalMonsterSpeed);
            }

        }

    }

    //check light intensity and decide drift direction
    private void CheckLightIntensity()
    {

    }

    //drift randomly within dark space
    private void Drift()
    {
        if (driftLocalTimer < 0)
        {
            driftLocalTimer = driftChangeTimer;
            int moveChanceX = Random.Range(-1, 2);
            int moveChanceY = Random.Range(-1, 2);
            //float moveMagnitudeX = Random.Range(0, 2);
            //float moveMagnitudeY = Random.Range(0, 2);

            //prevent monster from not moving
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Teleport(collision);

    }

    private void Teleport(Collider2D collision)
    {
        if (collision.gameObject.tag == "LeftWall")
        {
            //Debug.Log("left wall");
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + 14, gameObject.transform.position.y);
        }
        else if (collision.gameObject.tag == "RightWall")
        {
            //Debug.Log("right wall");
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - 14, gameObject.transform.position.y);
        }
        else if (collision.gameObject.tag == "BottomWall")
        {
            //Debug.Log("bottom wall");
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 14);
        }
        else if (collision.gameObject.tag == "TopWall")
        {
            //Debug.Log("top wall");
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 14);
        }
    }


    //follow the player within a certain radius
    private void Chase() { }

    //catch the player if it touches the player
    private void Catch() { }

    //disappear when player interacts
    private void Disappear() { }

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

            if (direction == "!u")
            {
                moveChanceX = 0;
                moveChanceY = 1;
            }
            else if (direction == "!d")
            {
                moveChanceX = 0;
                moveChanceY = -1;
            }
            else if (direction == "!l")
            {
                moveChanceX = -1;
                moveChanceY = 0;
            }
            else if (direction == "!r")
            {
                moveChanceX = 1;
                moveChanceY = 0;
            }

            monsterRB.velocity = new Vector2(moveChanceX * twitchMonsterSpeed, moveChanceY * twitchMonsterSpeed);
        }
        //Debug.Log("twitch command received");
    }

    ////display twitch name when command received
    //public void DisplayTwitchName(string name)
    //{
    //    twitchNameDisplay = Instantiate(twitchNamePrefab);
    //    twitchNameDisplay.GetComponent<TwitchNameController>().Display(name);
    //    twitchNameDisplay.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1);
    //    twitchNameDisplay.SetActive(true);
    //    //Debug.Log("name received");
    //}
}