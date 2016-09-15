using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour
{
    public int minDelay, maxDelay;
    public int delay;
    public bool isRandom = true;
    public GameObject bullet;
    public GameObject spawn;

	void Start ()
    {
        Invoke("Shoot", delay);//only for the first time it shoots
    }
	
	void Update ()
    {
	}

    void Shoot()
    {
        Instantiate(bullet, spawn.transform.position, spawn.transform.rotation);
        if (isRandom)
        {
            float rand;
            rand = Random.Range(minDelay, maxDelay);
            Invoke("Shoot", rand);
        }
        else
        {
            Invoke("Shoot", maxDelay);
        }
    }
}
