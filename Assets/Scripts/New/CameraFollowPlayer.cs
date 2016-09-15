using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 playerOffset;

    private bool attached;
    public Vector3 cameraSpawnPosition;
    public Quaternion cameraSpawnRotation;
    
	void Start ()
    {
        cameraSpawnPosition = transform.position;
        cameraSpawnRotation = transform.rotation;
	}
	
	void Update ()
    {
        if (attached)
        {
            float desiredAngle = player.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
            transform.position = player.transform.position - (rotation * playerOffset);
            transform.LookAt(player.transform.GetChild(0));
            transform.Rotate(5, 0, 0);
        }
	}

    public void AssignPlayer(GameObject obj)
    {
        if (obj.GetComponent<PlayerScript>().inDungeon)
        {
            player = obj;
            attached = true;
            transform.position = player.transform.position - playerOffset;
            transform.rotation = player.transform.rotation;
        }
    }

    public void UnassignPlayer()
    {
        player = null;
        attached = false;
        transform.position = cameraSpawnPosition;
        transform.rotation = cameraSpawnRotation;

    }

}
