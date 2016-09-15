using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lava : Enemy
{
    int canAttackTimer = 0;
    float currentDistance;
    public bool lavaBoolOn = false;
    private List<PlayerScript> playersInLava;

    void Start()
    {
        base.Init();
        m_health = 999999999999999;
        m_damage = 2.5f;

        playersInLava = new List<PlayerScript>();
    }

    public void spawnLava()
    {
        transform.position = new Vector3(transform.position.x, -0.6f, transform.position.z);
        lavaBoolOn = true;
    }

    public void deSpawnLava()
    {
        transform.position = new Vector3(transform.position.x, -100, transform.position.z);
        lavaBoolOn = false;
    }

    private void playerTakesDamage(PlayerScript player)
    {
        player.TakeDamage(m_damage);
        player.Rumble();
    }

    void Update()
    {
        canAttackTimer++;

        if (canAttackTimer > 10 && playersInLava.Count > 0) {
            foreach (PlayerScript player in playersInLava)
            {
                if(player.isAlive)
                {
                    playerTakesDamage(player);
                }
            }

            canAttackTimer = 0;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player") || collider.gameObject.tag.Equals("Skylar"))
        {
            playersInLava.Add(collider.GetComponent<PlayerScript>());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player") || collider.gameObject.tag.Equals("Skylar"))
        {
            playersInLava.Remove(collider.GetComponent<PlayerScript>());
        }
    }
}
