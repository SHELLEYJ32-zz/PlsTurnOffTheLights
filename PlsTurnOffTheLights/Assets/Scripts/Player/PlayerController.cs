using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D playerRB;
    public float playerSpeed = 2.0f;
    public int playHP = 3;
    public float monsterCaughtTime;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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

    void Interact()
    {

    }

    void LoseLife()
    {

    }

    void Die()
    {

    }

}
