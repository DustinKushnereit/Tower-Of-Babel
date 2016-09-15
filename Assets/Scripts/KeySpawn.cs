using UnityEngine;
using System.Collections;

public class KeySpawn : MonoBehaviour {
    public GameObject lever;
    bool spawned = false;
    public GameObject key;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (lever.GetComponent<FlipSwitch>().flipped && !spawned)
        {
            spawnKey();
        }
	}
    void spawnKey()
    {
        spawned = true;
        Instantiate(key, transform.position, Quaternion.identity);
    }
    public void unSpawn()
    {
        spawned = false;
    }
}
