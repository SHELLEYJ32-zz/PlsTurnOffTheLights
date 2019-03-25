using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIndividualController : MonoBehaviour
{
    public float originalMonsterSpeed = 1.5f;
    public float twitchMonsterSpeed;
    public float twitchEffectiveTime;
    public float maxLightIntensity;
    public float tempDisappearTime;

    private Rigidbody2D monsterRB;
    private float directionChangeTimer = 3.0f;
    private float localTimer;

    void Start()
    {
        monsterRB = GetComponent<Rigidbody2D>();
        localTimer = directionChangeTimer;
    }

    void FixedUpdate()
    {

        Drift();

    }

    //check light intensity and decide drift direction
    private void CheckLightIntensity()
    {

    }

    //drift randomly within dark space
    private void Drift()
    {
        if (localTimer < 0)
        {
            localTimer = directionChangeTimer;
            int moveChanceX = Random.Range(-1, 2);
            int moveChanceY = Random.Range(-1, 2);
            //float moveMagnitudeX = Random.Range(0, 2);
            //float moveMagnitudeY = Random.Range(0, 2);

            //prevent monster from not moving
            while (moveChanceX == 0 && moveChanceY == 0)
            {
                moveChanceX = Random.Range(-1, 1);
                moveChanceY = Random.Range(-1, 1);
            }

            monsterRB.velocity = new Vector2(moveChanceX * originalMonsterSpeed, moveChanceY * originalMonsterSpeed);
            monsterRB.velocity = Vector2.ClampMagnitude(monsterRB.velocity, originalMonsterSpeed);
        }
        else
        {
            localTimer -= Time.deltaTime;
        }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Teleport(collision);

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

    }

    //display twitch name when command received
    public void DisplayTwitchName(string name)
    {

    }
}
