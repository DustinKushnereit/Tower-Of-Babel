using UnityEngine;
using System.Collections;

public class DungeonEntranceLogic : MonoBehaviour
{
    public bool DungeonCheck()
    {
        bool temp = true;
        foreach (GameObject obj in GameObject.Find("ActivePlayers").GetComponent<ActivePlayers>().characters)
        {
            if (obj.GetComponent<PlayerScript>().inDungeon || GameObject.Find("WaveManager").GetComponent<WaveManager>().waveStart)
            {
                temp = false;
            }
        }
        return temp;
    }

    public bool DungeonEmpty()
    {
        bool temp = true;
        foreach (GameObject obj in GameObject.Find("ActivePlayers").GetComponent<ActivePlayers>().characters)
        {
            if (obj.GetComponent<PlayerScript>().inDungeon)
            {
                temp = false;
            }
        }
        return temp;
    }
}
