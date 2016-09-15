using UnityEngine;
using System.Collections;

public class KeyCollection : MonoBehaviour {

    float rot = 0;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0,rot,0);
        rot++;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            other.gameObject.GetComponent<PlayerScript>().hasKey = true;
        }
    }
}
