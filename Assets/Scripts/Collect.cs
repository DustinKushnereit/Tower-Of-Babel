using UnityEngine;
using System.Collections;

public class Collect : MonoBehaviour
{
    bool here = false;
    GameObject player;

    void Update()
    {
        if (here)
        {
            if (player.gameObject.GetComponent<PlayerScript>().gamePad.GetButton("X"))
            {
                player.gameObject.GetComponent<PlayerScript>().treasure++;
                player.gameObject.GetComponent<PlayerScript>().playSound(SoundLibrary.PLAYER_SOUND_INDEX.FOUND_TREASURE);
                GameObject.Find("LevelHandler").GetComponent<LevelHandler>().nextLevel();
                player.GetComponent<PlayerScript>().LeaveDungeon();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            here = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            here = false;
        }
    }
}
