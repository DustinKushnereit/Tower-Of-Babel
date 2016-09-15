using UnityEngine;
using System.Collections;

public class TrapDamage : MonoBehaviour
{
    public float damage;

	void OnTriggerEnter (Collider other)
    {

        if (other.gameObject.tag == "Player" || other.gameObject.tag ==  "Skylar")
        {
            other.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            other.gameObject.GetComponent<PlayerScript>().Rumble();
        }
	}
}
