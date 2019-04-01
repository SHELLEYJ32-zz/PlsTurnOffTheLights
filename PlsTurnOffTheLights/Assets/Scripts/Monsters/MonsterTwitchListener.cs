
using UnityEngine;

public class MonsterTwitchListener : MonoBehaviour
{
    public void SelectRandomMonster(string direction, string twitchName)
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        int r = Random.Range(0, monsters.Length);
        GameObject selectedMonster = monsters[r];
        //Debug.Log(selectedMonster);
        selectedMonster.GetComponent<MonsterIndividualController>().TwitchMove(direction);
        selectedMonster.GetComponent<MonsterIndividualController>().DisplayTwitchName(twitchName);
    }
}
