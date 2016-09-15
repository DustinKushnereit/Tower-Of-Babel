using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public int currWave;
    public int enemyCount;
    public bool bossWave = false;

    public bool waveEnd = false;
    public bool waveStart;
    public float enemySpawnDelay;
    public float waveSpawnDelay;
    public int waveLength;
    public int timeBetweenRounds;
    public float totalEnemies;
    public int enemiesKilled;
    public bool finalBoss = false;
    public bool finalBossActivated = false;

    public bool startTimer = false;

    public bool startNextWaveTimer = false;

    public List<GameObject> enemyTypes;
    public List<GameObject> bossTypes;
    public GameObject portal;
    public bool spawnOnce = true;

    public GameObject bossCaution;

    private float spawnRadius = 25.0f;

    private int incrementer = 0;

    GameObject[] players;

    public bool gameOver = false;
    public bool playerVictory = false;

    public Blatherfish blatherfish;
    Vector3 spawnPosition;

    public GameObject bossSpawnIndicator;

    GameObject explosion;

    public bool littleBossSpawn = false;
    public bool littleBossSpawnOnce = false;

    void Start()
    {
        float tempX = Random.insideUnitCircle.x;
        float tempY = Random.insideUnitCircle.y;

        spawnPosition = new Vector3(0, 0, 11);
        //
        //MOST OF THESE ARE SET IN THE INSPECTOR NOW
        //
        //currWave = 0; //resest to zero to play game from beginning

        portal = GameObject.Find("PortalTo");
        portal.SetActive(false);
        //enemySpawnDelay = 1;
        //waveSpawnDelay = 3;
        //waveLength = 3; //change this to alter number of enemies per wave
        //timeBetweenRounds = 15;
        totalEnemies = 0;
        enemiesKilled = 0;
        Invoke("StartWave", waveSpawnDelay);
        blatherfish.setText(5, "Fight my pretties!");
    }

    void Update()
    {
        if (!finalBoss)
        {
            if (!waveEnd)//If wave is still going on check to see if it should still be spawning
            {
                if (incrementer >= waveLength)
                    StopSpawning();

            }
            else if (waveStart && CheckCount())//If spawning stopped, check to see if all enemies are dead
            {
                EndWave();
            }

            if (GameObject.Find("DungeonEntrance").GetComponent<DungeonEntranceLogic>().DungeonCheck())
            {
                if (currWave != 0)
                {
                    portal.SetActive(true);
                }
                startTimer = true;
            }
            else
            {
                portal.SetActive(false);
                startTimer = false;
            }
        }

        if (!finalBossActivated)
        {
            if (GameObject.Find("LevelHandler").GetComponent<LevelHandler>().currLevel >= 4)
            {
                waveLength = 0;
                waveEnd = true;
                finalBoss = true;
                finalBossActivated = true;

                spawnOnce = true;

                spawnPosition = new Vector3(0, 0, 11);
                explosion = (GameObject)Instantiate(bossSpawnIndicator, spawnPosition, bossSpawnIndicator.transform.rotation);
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
            }

        }

        if (finalBoss && spawnOnce)
        {
            if (explosion == null)
            {
                spawnPosition = new Vector3(0, 0, 11);
                Instantiate(bossTypes[2], spawnPosition, bossTypes[2].transform.rotation);
                blatherfish.setText(5, "Hehe. Yes! Run away!");
                spawnOnce = false;
            }
        }

        if (littleBossSpawn)
        {
            spawnPosition = new Vector3(0, 0, 11);
            explosion = (GameObject)Instantiate(bossSpawnIndicator, spawnPosition, bossSpawnIndicator.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);

            littleBossSpawn = false;
            littleBossSpawnOnce = true;
        }

        if (littleBossSpawnOnce)
        {
            if (explosion == null)
            {
                spawnPosition = new Vector3(0, 0, 11);
                blatherfish.setText(5, "Hehe. Yes! Run away!");
                Instantiate(bossTypes[0], spawnPosition, bossTypes[0].transform.rotation);
                littleBossSpawnOnce = false;
            }
        }
    }

    public bool checkWinLoss()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        bool lossStatus = false;

        foreach (GameObject obj in players)
        {
            if (obj.gameObject.GetComponent<PlayerScript>().isActive && obj.gameObject.GetComponent<PlayerScript>().isAlive)
            {
                return false;
            }
            else
                lossStatus = true;
        }

        Debug.Log(lossStatus);

        return lossStatus;
    }

    void StartWave()
    {

        enemiesKilled = 0;

        //Increase difficulty a bit (temp)
        waveLength = ((currWave + 1) * 2) + 1;

        waveEnd = false;
        currWave++;
        totalEnemies = 0;
        incrementer = 0;
        waveStart = true;

        if (currWave % 5 == 0)
        {
            bossWave = true;
            blatherfish.setText(5, "Hehe. Yes! Run away!");
        }
        else
        {
            if (Random.Range(0, 10) == 0)
            {
                blatherfish.setText(5, "Fear my giant hat!");
            }

            bossWave = false;
        }

        //Debug.Log("Starting Wave " + currWave);  
        if (!bossWave)
        {
            SpawnUnit();
        }
        else
        {
            Invoke("SpawnBoss", 5);
            totalEnemies = 1;
        }
        //Invoke("StopSpawning", waveLength);

    }

    void SpawnBoss()
    {
        littleBossSpawn = true;
        incrementer = waveLength; //Tells spawner it is done spawing
        totalEnemies++;
    }

    void SpawnUnit()
    {
        if (!waveEnd)
        {
            spawnPosition = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y).normalized * spawnRadius;

            if (incrementer % 3 == 0)
                Instantiate(enemyTypes[1], spawnPosition, enemyTypes[1].transform.rotation);
            else
                Instantiate(enemyTypes[0], spawnPosition, enemyTypes[0].transform.rotation);

            Invoke("SpawnUnit", enemySpawnDelay);
            incrementer++;
            totalEnemies++;
        }
    }

    bool CheckCount()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && GameObject.FindGameObjectsWithTag("Boss").Length <= 0)
        {
            startNextWaveTimer = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    //Added Second function so that there would be time in between waves (banter, entering/leaving dundegon etc...)
    void StopSpawning()
    {
        waveEnd = true;
    }

    void EndWave()
    {
        waveStart = false;
        waveEnd = false;

        //Tells players that the wave has ended and it is time to banter
        GameObject.Find("ActivePlayers").GetComponent<ActivePlayers>().playBanter();
        Invoke("StartWave", timeBetweenRounds);
    }

    //Maybe temp keeps track of amount of enemies killed per wave
    //Can also be expanded upon to make a stat system/kills for each person per round
    public void enemyKilled()
    {
        enemiesKilled++;
    }

}
