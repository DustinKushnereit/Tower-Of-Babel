using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FlipSwitch : MonoBehaviour
{
    public bool flipped = false;
    public GameObject pivot;

    public void flip()
    {
        flipped = true;
        transform.RotateAround(pivot.transform.position, new Vector3(0, 0, 1), -90);
    }

    public void UnFlip()
    {
        flipped = false;
        transform.RotateAround(pivot.transform.position, new Vector3(0, 0, 1), 90);
    }
}
