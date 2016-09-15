using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RangedEnemy : Enemy
{
    int canAttackTimer = 0;
    float currentDistance;

    public float moveSpeed = 5.0f;
    private Vector3 moveDirection;
    private Vector3 m_direction;
    public GameObject rangedEnemyBullet;

    private bool sendToBottomRight = false;
    private bool sendToBottomLeft = false;
    private bool sendToTopLeft = false;
    private bool sendToTopRight = true;

    private Vector3 topRight = new Vector3(14, 1, 10);
    private Vector3 bottomRight = new Vector3(14, 1, -7);
    private Vector3 bottomLeft = new Vector3(-15, 1, -6);
    private Vector3 topLeft = new Vector3(-15, 1, 10);
    private Vector3[] corners;
    private Vector3 startingPoint;

    private float distanceToTopRight;
    private float distanceToBottomRight;
    private float distanceToBottomLeft;
    private float distanceToTopLeft;

    //Animator anim;
    //int runHash = Animator.StringToHash("Run");
    //int idleHash = Animator.StringToHash("Idle");

    void Start()
    {
        //anim = GetComponent<Animator>();

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
        m_health = (waveManager.currWave + 1) * 10;
        m_speed = 3;
        m_damage = 5;
        navComp.speed = m_speed;
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

    void Update()
    {
        GameObject closestPlayer = base.getClosestPlayer();

        if ((transform.position.x < -18 || transform.position.x > 18) && !gotStunned && closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive == true)
        { 
            movement();
            //anim.SetTrigger(runHash);
        }
        else if ((transform.position.z < -8 || transform.position.z > 12) && !gotStunned && closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive == true)
        { 
            movement();
            //anim.SetTrigger(runHash);
        }
        else
        {
            if (!gotStunned && closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive == true)
            {
                followPlayer(closestPlayer);
                //anim.SetTrigger(runHash);
            }
            else if (gotStunned)
            {
                navComp.SetDestination(transform.position);
                //anim.SetTrigger(idleHash);
            }
        }

        if (closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive && !gotStunned)
            rangedAttack(closestPlayer);

        if (closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive && !gotStunned)
            currentDistance = Vector3.Distance(closestPlayer.transform.position, transform.position);
        else
            currentDistance = Vector3.Distance(transform.position, transform.position);

        if (gotStunned && !activateStunnedParticlesOnce)
        {
            activateStunnedParticlesOnce = true;
            StartStunnedParticles();
            //anim.SetTrigger(idleHash);
        }

        //if (currentDistance <= 4.0f && closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive == false)
        //TakeDamage(1, 1337);
    }

    void followPlayer(GameObject closestPlayer)
    {
        if(Vector3.Distance(closestPlayer.transform.position, transform.position) > 10.0f)
            navComp.SetDestination(closestPlayer.transform.position);
        else if (Vector3.Distance(closestPlayer.transform.position, transform.position) < 10.0f)
            navComp.SetDestination(topRight);
    }

    void movement()
    {
        distanceToTopRight = Vector3.Distance(topRight, transform.position);
        distanceToBottomRight = Vector3.Distance(bottomRight, transform.position);
        distanceToBottomLeft = Vector3.Distance(bottomLeft, transform.position);
        distanceToTopLeft = Vector3.Distance(topLeft, transform.position);

        if (sendToTopRight)
        {
            if (distanceToTopRight <= 0.8f)
            {
                sendToTopRight = false;
                sendToBottomRight = false;
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
            if (distanceToBottomRight <= 0.8f)
            {
                sendToTopRight = false;
                sendToBottomRight = false;
                sendToBottomLeft = false;
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
            if (distanceToBottomLeft <= 0.8f)
            {
                sendToTopRight = false;
                sendToBottomRight = false;
                sendToBottomLeft = false;
                sendToTopLeft = false;
            }
            else
            {
                moveDirection = (bottomLeft - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
        else if (sendToTopLeft)
        {
            if (distanceToTopLeft <= 0.8f)
            {
                sendToTopRight = false;
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

    private void rangedAttack(GameObject closestPlayer)
    {
        if (canAttackTimer >= 89)
        {
            Vector3 direction = (closestPlayer.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.0f);

            //m_direction = (transform.eulerAngles = new Vector3(Mathf.Atan(0) * 180 / Mathf.PI, 0, Mathf.Atan(1) * 180 / Mathf.PI).normalized);

            GameObject temp = Instantiate(rangedEnemyBullet, (direction + new Vector3(transform.position.x, 1, transform.position.z)), Quaternion.LookRotation(direction)) as GameObject;
            temp.GetComponent<Rigidbody>().AddForce(direction * 5.0f, ForceMode.VelocityChange);
        }

        canAttackTimer++;

        if (canAttackTimer >= 90)
            canAttackTimer = 0;
    }

    private void playerTakesDamage(GameObject player)
    {
        player.gameObject.GetComponent<PlayerScript>().TakeDamage(m_damage);
        player.gameObject.GetComponent<PlayerScript>().Rumble();
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag.Equals("Player") || collider.gameObject.tag.Equals("Skylar"))
        {
            playerTakesDamage(collider.gameObject);
        }
    }

}
