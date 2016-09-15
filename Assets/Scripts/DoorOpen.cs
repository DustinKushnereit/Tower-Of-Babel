using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class DoorOpen : MonoBehaviour
{
    GameObject player;
    bool open = false;
    bool here = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            here = true;

        }
    }

    void openDoor()
    {
        transform.Rotate(new Vector3(0, -90, 0));
        open = true;
        player.GetComponent<PlayerScript>().hasKey = false;
    }

    void Update()
    {
        if (player != null)
        {
            if (player.GetComponent<PlayerScript>().hasKey && here)
            {
                if (player.gameObject.GetComponent<PlayerScript>().gamePad.GetButton("X"))
                {
                    openDoor();
                }
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            here = false;
        }
    }

    public void CloseDoor()
    {
        if (open)
        {
            transform.Rotate(new Vector3(0, 90, 0));
            open = false;
        }
    }
}
