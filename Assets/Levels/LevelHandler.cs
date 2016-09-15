using UnityEngine;
using System.Collections;

public class LevelHandler : MonoBehaviour
{

    public int currLevel;
    public GameObject[] quests;
    public float readTime;
    public GameObject spawn;
    public GameObject cam;

    public Transform questSpawn;
    public Transform[] spawnLoc;
    public Transform[] camSpawns;
    public GameObject lava;
    public GameObject lever1, lever2, lever3, lever4;

    private GameObject currScreen;
    private int tracker = 0;
    // Use this for initialization
    void Start()
    {
        currLevel = 1;
        SpawnLevel();
        lava.SetActive(false);
    }
    void Update()
    {
        switch (tracker)
        {
            case 0:
                if (lever1.GetComponentInChildren<FlipSwitch>().flipped )
                {
                    lava.SetActive(true);
                    tracker++;
                }
                break;
            case 1:
                if (lever2.GetComponentInChildren<FlipSwitch>().flipped)
                {
                    lava.SetActive(false);
                    tracker++;
                }
                break;
            case 2:
                if (lever3.GetComponentInChildren<FlipSwitch>().flipped)
                {
                    lava.SetActive(true);
                    tracker++;
                }
                break;
            case 3:
                if (lever4.GetComponentInChildren<FlipSwitch>().flipped)
                {
                    lava.SetActive(false);
                    tracker++;
                }
                break;
            default:
                break;
        }
    }
    void SpawnLevel()
    {
        currScreen = Instantiate(quests[currLevel - 1], questSpawn.position, questSpawn.rotation) as GameObject;
        currScreen.SetActive(true);
        Invoke("LevelStart", readTime);
    }

    void LevelStart()
    {
        currScreen.SetActive(false);
    }
    public void nextLevel()
    {
        currLevel++;
        SwitchLevels();
    }

    void SwitchLevels()
    {
        if (currLevel <= 3)
        {
            spawn.transform.position = spawnLoc[currLevel - 1].position;
            spawn.transform.rotation = spawnLoc[currLevel - 1].rotation;

            cam.GetComponent<CameraFollowPlayer>().cameraSpawnPosition = camSpawns[currLevel - 1].position;
            cam.GetComponent<CameraFollowPlayer>().cameraSpawnRotation = camSpawns[currLevel - 1].rotation;
        
            SpawnLevel();
        }
    }
    public void Restart()
    {
        currLevel = 0;
        tracker = 0;
        lava.SetActive(false);
        foreach(GameObject lever in GameObject.FindGameObjectsWithTag("Lever"))
        {
            lever.GetComponentInChildren<FlipSwitch>().UnFlip();
        }
        foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door"))
        {
            door.GetComponentInChildren<DoorOpen>().CloseDoor();
        }

        foreach (GameObject kspawn in GameObject.FindGameObjectsWithTag("KeySpawn"))
        {
            kspawn.GetComponentInChildren<KeySpawn>().unSpawn();
        }
        nextLevel();
    }
}
