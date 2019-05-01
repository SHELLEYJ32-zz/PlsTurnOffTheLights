using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnController : MonoBehaviour
{
    private GameObject[] monsters;
    public float regenerateTimer;
    private float[] individualTimer;

    void Start()
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");
        individualTimer = new float[monsters.Length];
        for (int i = 0; i < monsters.Length; i++)
        {
            individualTimer[i] = regenerateTimer;
        }

    }

    void FixedUpdate()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            if (!monsters[i].activeSelf)
            {
                if (individualTimer[i] <= 0)
                {
                    monsters[i].GetComponent<SpriteRenderer>().enabled = true;
                    monsters[i].GetComponent<MonsterIndividualController>().bodyCollider.enabled = true;
                    monsters[i].SetActive(true);
                    individualTimer[i] = regenerateTimer;
                }
                else
                    individualTimer[i] -= Time.deltaTime;
            }
        }
    }
}
