
using UnityEngine;

public class MonsterTwitchListener : MonoBehaviour
{
    private int leftVote;
    private int rightVote;
    private int upVote;
    private int downVote;

    public float voteCountdown = 10.0f;
    private float localTimer;
    private string finalVote;

    private void Start()
    {
        localTimer = voteCountdown;
    }

    private void FixedUpdate()
    {
        if (localTimer <= 0)
        {
            //SelectRandomMonster(finalVote);
            ClearVotes();
            localTimer = voteCountdown;
        }
        else
        {
            localTimer -= Time.deltaTime;
        }
    }

    private void ClearVotes()
    {
        leftVote = 0;
        rightVote = 0;
        upVote = 0;
        downVote = 0;
        finalVote = "";
    }

    public void CollectVotes(string direction)
    {
        if (direction == "u")
            upVote++;
        else if (direction == "d")
            downVote++;
        else if (direction == "l")
            leftVote++;
        else if (direction == "r")
            rightVote++;

        //Debug.Log("received: " + direction);
        if (upVote != 0 || downVote != 0 || leftVote != 0 || rightVote != 0)
        {
            //pick the most vote
            if (upVote >= downVote && upVote >= leftVote && upVote >= rightVote)
            {
                finalVote = "u";
            }
            else if (downVote >= upVote && downVote >= leftVote && downVote >= rightVote)
            {
                finalVote = "d";
            }
            else if (leftVote >= upVote && leftVote >= downVote && leftVote >= rightVote)
            {
                finalVote = "l";
            }
            else if (rightVote >= upVote && rightVote >= downVote && rightVote >= leftVote)
            {
                finalVote = "r";
            }
        }

    }

    public void SelectRandomMonster(string direction)
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        int r = Random.Range(0, monsters.Length);
        GameObject selectedMonster = monsters[r];
        //Debug.Log(selectedMonster);
        selectedMonster.GetComponent<MonsterIndividualController>().TwitchMove(direction);
        selectedMonster.GetComponent<MonsterIndividualController>().TwitchActiveSignal(direction);
    }
}
