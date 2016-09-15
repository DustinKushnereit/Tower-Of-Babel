using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : Enemy
{
    public float m_healAmount = 40;

    public float moveSpeed = 5.0f;
    private Vector3 moveDirection;

    private bool sendToBottomRight =  false;
    private bool sendToBottomLeft =   false;
    private bool sendToTopLeft =      false;
    private bool sendToTopRight =     true;

    private GameObject[] players;

    private Vector3 topRight = new Vector3(15, 1, 11);
    private Vector3 bottomRight = new Vector3(15, 1, -8);
    private Vector3 bottomLeft = new Vector3(-17, 1, -8);
    private Vector3 topLeft = new Vector3(-16, 1, 12);
    private Vector3[] corners;
    private Vector3 startingPoint;

    private float distanceToTopRight;
    private float distanceToBottomRight;
    private float distanceToBottomLeft;
    private float distanceToTopLeft;

    public bool finalBoss = false;
    int canAttackTimer = 0;
    float currentDistance;
    private Vector3 m_direction;
    public GameObject rangedEnemyBullet;
    public float bulletSpeed = 8.0f;

    void Start()
    {
        corners = new[] { topRight, bottomRight, bottomLeft, topLeft };
        startingPoint = closestPoint();

        distanceToTopRight = Vector3.Distance(topRight, transform.position);
        distanceToBottomRight = Vector3.Distance(bottomRight, transform.position);
        distanceToBottomLeft = Vector3.Distance(bottomLeft, transform.position);
        distanceToTopLeft = Vector3.Distance(topLeft, transform.position);

        float distanceToStartPoint = Vector3.Distance(startingPoint, transform.position);

        if (distanceToStartPoint == distanceToTopRight)
        {
            sendToTopRight = true;
            sendToBottomRight = false;
            sendToBottomLeft = false;
            sendToTopLeft = false;
        }
        else if (distanceToStartPoint == distanceToBottomRight)
        {
            sendToTopRight = false;
            sendToBottomRight = true;
            sendToBottomLeft = false;
            sendToTopLeft = false;
        }
        else if (distanceToStartPoint == distanceToBottomLeft)
        {
            sendToTopRight = false;
            sendToBottomRight = false;
            sendToBottomLeft = true;
            sendToTopLeft = false;
        }
        else if (distanceToStartPoint == distanceToTopLeft)
        {
            sendToTopRight = false;
            sendToBottomRight = false;
            sendToBottomLeft = false;
            sendToTopLeft = true;
        }

        base.Init();

        if (finalBoss)
        {
            m_health = ((waveManager.currWave + 1) / 5) * 400 + 200;
            m_speed = 6;
            m_damage = 40;
        }
        else
        {
            m_health = ((waveManager.currWave + 1) / 5) * 100 + 100;
            m_speed = 5;
            m_damage = 40;
        }

        navComp.speed = m_speed;
    }
    
    void Update()
    {
        WaveManager waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        if (waveManager.finalBossActivated == true)
            finalBoss = true;

        if (!gotStunned)
            movement();

        GameObject closestPlayer = base.getClosestPlayer();

        if (closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive && !gotStunned)
            rangedAttack(closestPlayer);

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (gotStunned && !activateStunnedParticlesOnce)
        {
            activateStunnedParticlesOnce = true;
            StartStunnedParticles();
        }
    }

    private void rangedAttack(GameObject closestPlayer)
    {
        if (finalBoss)
        {
            if (canAttackTimer >= 19)
            {
                Vector3 direction = (closestPlayer.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.0f);

                for (int i = 0; i < 5; i++)
                {
                    GameObject temp = Instantiate(rangedEnemyBullet, (direction + new Vector3(transform.position.x + (i * 0.5f), 1, transform.position.z + (i * 0.5f))), Quaternion.LookRotation(direction)) as GameObject;
                    temp.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
                }
            }

            canAttackTimer++;

            if (canAttackTimer >= 20)
                canAttackTimer = 0;
        }
        else
        {
            if (canAttackTimer >= 39)
            {
                Vector3 direction = (closestPlayer.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.0f);

                GameObject temp = Instantiate(rangedEnemyBullet, (direction + new Vector3(transform.position.x, 1, transform.position.z)), Quaternion.LookRotation(direction)) as GameObject;
                temp.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
            }

            canAttackTimer++;

            if (canAttackTimer >= 40)
                canAttackTimer = 0;
        }
    }

    Vector3 closestPoint()
    {
        float currentDistance = Vector3.Distance(topRight, transform.position);
        int vectorClosest = 0;

        for (int i = 0; i < 4; i++)
        {
            float tempDistance = Vector3.Distance(corners[i], transform.position);

            if (tempDistance < currentDistance)
            {
                currentDistance = tempDistance;
                vectorClosest = i;
            }
        }

        return corners[vectorClosest];
    }

    void movement()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        
        float distanceToPlayer = 10000000000;
        GameObject player = players[0];

        foreach(GameObject obj in players)
        {
            if (Vector3.Distance(transform.position, obj.transform.position) < distanceToPlayer)
            {
                distanceToPlayer = Vector3.Distance(transform.position, obj.transform.position);
                player = obj;
            }
        }

        

        if (player.gameObject.GetComponent<PlayerScript>().isActive && player.gameObject.GetComponent<PlayerScript>().isAlive)
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        distanceToTopRight = Vector3.Distance(topRight, transform.position);
        distanceToBottomRight = Vector3.Distance(bottomRight, transform.position);
        distanceToBottomLeft = Vector3.Distance(bottomLeft, transform.position);
        distanceToTopLeft = Vector3.Distance(topLeft, transform.position);

        if (distanceToPlayer <= 3.0f && player.gameObject.GetComponent<PlayerScript>().isActive && player.gameObject.GetComponent<PlayerScript>().isAlive)
        {
            player.gameObject.GetComponent<PlayerScript>().TakeDamage(m_damage);
            player.gameObject.GetComponent<PlayerScript>().Rumble();
            AddHealth(m_healAmount);
        }

        if (sendToTopRight)
        {
            if (distanceToTopRight <= 2.8f)
            {
                sendToTopRight = false;
                sendToBottomRight = true;
                sendToBottomLeft = false;
                sendToTopLeft = false;
            }
            else
            {
                moveDirection = (topRight - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
        else if (sendToBottomRight)
        {
            if (distanceToBottomRight <= 2.8f)
            {
                sendToTopRight = false;
                sendToBottomRight = false;
                sendToBottomLeft = true;
                sendToTopLeft = false;
            }
            else
            {
                moveDirection = (bottomRight - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
        else if (sendToBottomLeft)
        {
            if (distanceToBottomLeft <= 2.8f)
            {
                sendToTopRight = false;
                sendToBottomRight = false;
                sendToBottomLeft = false;
                sendToTopLeft = true;
            }
            else
            {
                moveDirection = (bottomLeft - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
        else if (sendToTopLeft)
        {
            if (distanceToTopLeft <= 2.8f)
            {
                sendToTopRight = true;
                sendToBottomRight = false;
                sendToBottomLeft = false;
                sendToTopLeft = false;
            }
            else
            {
                moveDirection = (topLeft - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
    }
}
