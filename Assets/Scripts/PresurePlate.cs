using UnityEngine;
using System.Collections;

public class PresurePlate : MonoBehaviour
{
    GameObject player2;
    GameObject topLeft;
    public GameObject powerUp;
    private bool pressed = false;

    void OnCollisionEnter(Collision collider)
    {
        player2 = GameObject.FindGameObjectWithTag("Player2");
        topLeft = GameObject.FindGameObjectWithTag("TopLeft");

        if (collider.gameObject == player2 && pressed == false)
        {
            transform.Translate(0,-0.9f,0);
            pressed = true;
            
            Instantiate(powerUp, topLeft.transform.position, powerUp.transform.rotation);
        }
    }
}
