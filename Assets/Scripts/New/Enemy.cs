using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour
{
    protected float m_health;
    protected float m_speed;
    protected float m_damage;

    //protected List<GameObject> dungeonPlayers;
    protected NavMeshAgent navComp;

    protected ActivePlayers activePlayers;
    protected WaveManager waveManager;
    protected GuiManager guiManager;

    public GameObject particleSystemExplosion;
    public GameObject stunnedParticleSystem;

    public bool gotStunned = false;
    public bool activateStunnedParticlesOnce = false;

    void Start ()
    {
        Init();
    }

    protected void Init()
    {
        guiManager = GameObject.Find("GuiManager").GetComponent<GuiManager>();
        activePlayers = GameObject.Find("ActivePlayers").GetComponent<ActivePlayers>();
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        navComp = GetComponent<NavMeshAgent>();
    }

    protected GameObject getClosestPlayer()
    {
        if (activePlayers.characters.Count > 0)
        {
            float currentDistance = Vector3.Distance(activePlayers.characters[0].transform.position, transform.position);
            GameObject closestTarget = activePlayers.characters[0];

            //Loop through each player except the first one
            for (int i = 1; i < activePlayers.characters.Count; i++)
            {
                float tempDistance = Vector3.Distance(activePlayers.characters[i].transform.position, transform.position);
                if (tempDistance < currentDistance)
                {
                    currentDistance = tempDistance;
                    closestTarget = activePlayers.characters[i];
                }
            }
            return closestTarget;
        }
        return this.gameObject;
    }

    protected void AddHealth(float amount)
    {
        m_health += amount;
    }

    public void Explode()
    {
        GameObject explosion = (GameObject)Instantiate(particleSystemExplosion, transform.position, transform.rotation);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
    }

    public void StartStunnedParticles()
    {
        GameObject explosion = (GameObject)Instantiate(stunnedParticleSystem, new Vector3(transform.position.x, 3, transform.position.z), stunnedParticleSystem.transform.rotation);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().duration);
    }

    //0 = cosette, 1 = skylar , 2 = leon, 3 = ronan
    public void TakeDamage(float amount, int playerID )
    {
        m_health -= amount;

        if (m_health <= 0)
        {
            if (playerID == 0)
            {
                guiManager.scorePlayer1 += 10;
            }
            else if (playerID == 1)
            {
                guiManager.scorePlayer4 += 10;
            }
            else if (playerID == 2)
            {
                guiManager.scorePlayer2 += 10;
            }
            else if (playerID == 3)
            {
                guiManager.scorePlayer3 += 10;
            }

            Explode();
            Destroy(this.gameObject);

            if (this.gameObject.tag == "Enemy")
            {
                waveManager.totalEnemies--;
                waveManager.enemyKilled();
            }

            if (this.gameObject.tag == "Boss")
            {
                GameObject.Find("BlatherFish").GetComponent<Blatherfish>().setText(5, "Ah yes, good job…!");
                waveManager.totalEnemies--;
                waveManager.enemyKilled();
            }

            if (this.gameObject.tag == "Boss" && waveManager.finalBossActivated)
            {
                waveManager.playerVictory = true;
                waveManager.gameOver = true;
            }
        }
    }
}
