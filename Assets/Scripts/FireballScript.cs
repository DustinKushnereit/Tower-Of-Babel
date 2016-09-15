using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour {

    public float speed;
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0,0,speed));
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerScript>().TakeDamage(10);
            other.gameObject.GetComponent<PlayerScript>().Rumble();
        }
        
        Destroy(gameObject);
        //Debug.Log("Collider");

    }
    void OnCollisionEnter(Collision other) { Debug.Log("Collision"); Destroy(gameObject); }
}
