using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float damage;

    public GameObject gotHitExplosion;

    void Start ()
    {
        damage = 1;
    }
	
	void Update ()
    {

    }

    public void UseTheForce(Vector3 dir, float speed)
    {
        GetComponent<Rigidbody>().AddForce(dir * speed,ForceMode.VelocityChange);
    }

    protected GameObject getClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float distanceToPlayer = 10000000000;
        GameObject player = players[0];

        foreach (GameObject obj in players)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) < distanceToPlayer)
            {
                distanceToPlayer = Vector3.Distance(transform.position, obj.transform.position);
                player = obj;
            }
        }

        if (player.gameObject.GetComponent<PlayerScript>().isActive && player.gameObject.GetComponent<PlayerScript>().isAlive)
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= 4.0f && player.gameObject.GetComponent<PlayerScript>().isActive && player.gameObject.GetComponent<PlayerScript>().isAlive)
        {
            return player;
        }

        return player;
     }
}
