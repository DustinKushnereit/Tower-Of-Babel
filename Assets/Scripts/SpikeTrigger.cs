using UnityEngine;
using System.Collections;

public class SpikeTrigger : MonoBehaviour {
    public float delay = 1.0f;
    public float raiseDelay = 1.0f;
    public float lowerDelay = 2.0f;
	// Use this for initialization
	void Start () {
        Invoke("triggerTrap",delay);
	}
    void triggerTrap()
    {
        transform.GetComponentInChildren<Spikes>().Raise();
        Invoke("stopTrap", lowerDelay);
    }
 
    void stopTrap()
    {
        transform.GetComponentInChildren<Spikes>().Lower();
        Invoke("triggerTrap", raiseDelay);
    }
}
