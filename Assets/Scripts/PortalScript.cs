using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {

    bool inCollider = false;
    GameObject player;

    void OnTriggerEnter(Collider collider)
    {
        DungeonEntranceLogic dungeonEntranceLogic = GameObject.Find("DungeonEntrance").GetComponent<DungeonEntranceLogic>();
        //Debug.Log(collider);
        if (collider.tag == "Player")
        {
            inCollider = true;
            player = collider.gameObject;
            if (dungeonEntranceLogic.DungeonCheck() && 
                !player.GetComponent<PlayerScript>().inDungeon)
            {
                Invoke("goToDungeon",3);
            }
            else if (player.GetComponent<PlayerScript>().inDungeon)
            {
                Invoke("leaveDungeon", 3);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inCollider = false;
        }
    }

    void goToDungeon()
    {
        if (inCollider)
        {
            player.GetComponent<PlayerScript>().EnterDungeon();
        }
    }

    void leaveDungeon()
    {
        if (inCollider)
        {
            player.GetComponent<PlayerScript>().LeaveDungeon();
        }
    }
}
