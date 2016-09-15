using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gobble : Enemy
{
    int canAttackTimer = 0;
    float currentDistance;

    //Animator anim;
    //int runHash = Animator.StringToHash("Run");
    //int idleHash = Animator.StringToHash("Idle");

    void Start ()
    {
        base.Init();
        m_health = (waveManager.currWave + 1) * 10;
        m_speed = 3;
        m_damage = 5;
        navComp.speed = m_speed;

        //anim = GetComponent<Animator>();
    }
	
	void Update ()
    {
        GameObject closestPlayer = base.getClosestPlayer();

        if (closestPlayer)
        {
            if (!gotStunned && closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive == true)
            {
                navComp.SetDestination(closestPlayer.transform.position);
                //anim.SetTrigger(runHash);
            }
            else if (gotStunned)
            {
                navComp.SetDestination(transform.position);
                //anim.SetTrigger(idleHash);
            }
        }

        if(closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive && !gotStunned)
            meleeAttack(closestPlayer);

        if (closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive && !gotStunned)
            currentDistance = Vector3.Distance(closestPlayer.transform.position, transform.position);
        else
            currentDistance = Vector3.Distance(transform.position, transform.position);

        if(gotStunned && !activateStunnedParticlesOnce)
        {
            activateStunnedParticlesOnce = true;
            StartStunnedParticles();
            //anim.SetTrigger(idleHash);
        }

        //if (currentDistance <= 4.0f && closestPlayer.gameObject.GetComponent<PlayerScript>().isAlive == false)
        //TakeDamage(1, 1337);
    }

    private void meleeAttack(GameObject closestPlayer)
    {
        if (currentDistance <= 1.5f && canAttackTimer >= 29)
            playerTakesDamage(closestPlayer);

        canAttackTimer++;

        if (canAttackTimer >= 30)
            canAttackTimer = 0;
    }
    
    private void playerTakesDamage(GameObject player)
    {
        player.gameObject.GetComponent<PlayerScript>().TakeDamage(m_damage);
        player.gameObject.GetComponent<PlayerScript>().Rumble();
    }

    void OnCollisionEnter(Collision collider)
    {
        if ((collider.gameObject.tag.Equals("Player") || collider.gameObject.tag.Equals("Skylar")) && !gotStunned)
        {
            playerTakesDamage(collider.gameObject);
        }
    }

}
