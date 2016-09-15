using UnityEngine;
using System.Collections;

public class Claymore : Bullet
{
    
    void Start()
    {

    }
    
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(collider);
        if (collider.tag == "Enemy" || collider.tag == "EnemyBullet")
        {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(damage * 2.0f, 0);

            GameObject explosion = (GameObject)Instantiate(gotHitExplosion, transform.position, gotHitExplosion.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
        }
        else if (collider.tag == "Boss")
        {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(damage * 2.0f, 0);

            GameObject explosion = (GameObject)Instantiate(gotHitExplosion, transform.position, gotHitExplosion.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
        }
    }
}
