using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class TriggerSwitch : MonoBehaviour
{
    bool here = false;
    GameObject player;

    void Update()
    {
        if (!transform.parent.gameObject.GetComponent<FlipSwitch>().flipped && here)
        {
            if (player.gameObject.GetComponent<PlayerScript>().gamePad.GetButton("X"))
            {
                Debug.Log("Hit");
                transform.parent.gameObject.GetComponent<FlipSwitch>().flip();
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
