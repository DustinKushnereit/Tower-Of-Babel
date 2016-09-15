using UnityEngine;
using System.Collections;

public class Sword : Bullet
{
    GameObject closestPlayer;

	void Start ()
    {
        closestPlayer = getClosestPlayer();
	}
	
	void Update ()
    {

	}
    
    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(closestPlayer.gameObject.GetComponent<PlayerScript>().playerID);

        //0 = cosette, 1 = skylar , 2 = leon, 3 = ronan
        if (collider.tag == "Enemy" || collider.tag == "EnemyBullet")
        {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(damage, closestPlayer.gameObject.GetComponent<PlayerScript>().playerID);

            GameObject explosion = (GameObject)Instantiate(gotHitExplosion, transform.position, gotHitExplosion.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
        }
        else if (collider.tag == "Boss")
        {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(damage, closestPlayer.gameObject.GetComponent<PlayerScript>().playerID);

            GameObject explosion = (GameObject)Instantiate(gotHitExplosion, transform.position, gotHitExplosion.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
        }

        if (collider.tag != "Player" && collider.tag != "Bullet" && collider.tag != "Lava")
        {
            Destroy(this.gameObject);
        }
    }
     
}
