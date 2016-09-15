using UnityEngine;
using System.Collections;

public class EnemyBullet : Enemy
{
    float currentDistance;

    void Update()
    {
        GameObject closestPlayer = base.getClosestPlayer();

        currentDistance = Vector3.Distance(closestPlayer.transform.position, transform.position);

        if (currentDistance <= 4.0f && closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive == false)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag.Equals("Player") || collider.gameObject.tag.Equals("Skylar"))
        {
            Destroy(gameObject);
            collider.gameObject.GetComponent<PlayerScript>().TakeDamage(5);
            collider.gameObject.GetComponent<PlayerScript>().Rumble();
        }
        else
        {
            Explode();
            Destroy(gameObject);
        }
    }
}
