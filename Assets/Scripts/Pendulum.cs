using UnityEngine;
using System.Collections;

public class Pendulum : MonoBehaviour {

    public float startDelay, pullDelay, fireDelay;
    public float fireSpeed, pullSpeed;
    public bool swung = false;
    public bool fire = false;
    public Quaternion fromAng, toAng;
    public float smooth = 1.0F;
    public float tiltAngle = 45.0F;

    void Start()
    {
        fromAng = Quaternion.Euler(0,0,-89);
        toAng = Quaternion.Euler(0, 0, 89);
        Invoke("Fire", startDelay);
    }
    void Update()
    {
        if (fire)
        {
            transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, toAng, Time.deltaTime * fireSpeed);
            Invoke("PullBack", pullDelay);
        }
        if (swung)
        {
            transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, fromAng, Time.deltaTime * pullSpeed);
            Invoke("Fire", fireDelay);
        }
      
    }
    void Fire()
    {
        fire = true;
        swung = false;
    }
    void PullBack()
    {
        fire = false;
        swung = true;
    }
}
