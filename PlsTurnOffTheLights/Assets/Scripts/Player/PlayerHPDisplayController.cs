using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPDisplayController : MonoBehaviour
{
    public GameObject HPPrefab;
    private GameObject player;
    private GameObject[] healthBar;
    private int fullHP;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        fullHP = player.GetComponent<PlayerController>().playerHP;
        healthBar = new GameObject[fullHP];

        //instantiate health bar
        for (int i = 0; i < fullHP; i++)
        {
            GameObject temp = Instantiate(HPPrefab);
            healthBar[i] = temp;
            temp.transform.position = new Vector2(11 + 2 * i, 7);
        }
    }

    void FixedUpdate()
    {
        //update health bar
        for (int i = fullHP; i > player.GetComponent<PlayerController>().playerHP; i--)
        {
            healthBar[i - 1].SetActive(false);
        }
    }

}
