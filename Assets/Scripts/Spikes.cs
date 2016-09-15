using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

    private bool isTriggered = false;
    private Vector3 moveAmount = new Vector3(0, 1.35f, 0);
    private Vector3 origPos;
    private float speedMultUp = 10;
    private float speedMultDown = 2;

    void Start()
    {
        origPos = transform.position;
    }

	public void Raise()
    {
        //transform.Translate(new Vector3(0, 0, 1.85f));
        isTriggered = true;
    }
    public void Lower()
    {
        //transform.Translate(new Vector3(0, 0, -1.85f));
        isTriggered = false;
    }

    void Update()
    {
        if(isTriggered)
        {
            transform.position = Vector3.Lerp(transform.position, origPos + moveAmount, Time.deltaTime * speedMultUp);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, origPos - moveAmount, Time.deltaTime * speedMultDown);
        }
    }
}
