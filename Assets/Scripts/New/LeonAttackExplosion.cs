using UnityEngine;
using System.Collections;

public class LeonAttackExplosion : MonoBehaviour
{
    public GameObject particleSystemExplosion;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            GameObject explosion = (GameObject)Instantiate(particleSystemExplosion, transform.position, particleSystemExplosion.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
        }
        else
        {
            GameObject explosion = (GameObject)Instantiate(particleSystemExplosion, transform.position, particleSystemExplosion.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
        }
    }
}
