using UnityEngine;
using System.Collections;

public class Cosette : Character
{
    bool canAttack = true;
    public GameObject bulletInstance;
    public GameObject swordInstance;
    public GameObject sword;
    bool abilityCanBeUsed = true;
    public float abilityTimerMaxAmount = 200.0f;
    float abilityTimer = 25.0f;

    bool swordTimerStart = false;
    int swordTimer = 20;

    public GameObject stunParticles;
    public GameObject swingSwordParticles;

    Vector3 relativePos;
    Quaternion prevRotation;
    Quaternion newRotation;

    void Start()
    {
        
    }

    public override void Init()
    {
        //base.Init();
        player = GetComponent<PlayerScript>();
        gamePad = player.gamePad;

        abilityTimer = abilityTimerMaxAmount;
        this.gameObject.GetComponent<PlayerScript>().abilityCooldownMaxAmount = abilityTimerMaxAmount;

        if (player.inDungeon)
        {
            canAttack = false;
        }


        player.barks = GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().cosetteAudio;

        m_direction = (new Vector3(Mathf.Atan(1.0f) * 180 / Mathf.PI, 0, Mathf.Atan(1.0f) * 180 / Mathf.PI).normalized);
        prevRotation = Quaternion.LookRotation(relativePos);
        newRotation = Quaternion.LookRotation(relativePos);

    }


    void Update()
    {

        if (sword != null)
        {
            sword.transform.RotateAround(transform.position, Vector3.up, 350 * Time.deltaTime);

            player.cosetteAttacked = true;
            swordTimerStart = true;
        }

        if (swordTimerStart)
        {
            swordTimer -= 4;
        }

        if (swordTimer <= 0)
        {
            swordTimerStart = false;
            swordTimer = 20;
            Destroy(sword);
            player.cosetteAttacked = false;
        }

        if (!abilityCanBeUsed)
        {
            abilityTimer--;

            if (abilityTimer <= abilityTimerMaxAmount / 2)
                StunDuration();

            if (abilityTimer <= 0)
            {
                abilityCanBeUsed = true;
                abilityTimer = abilityTimerMaxAmount;
            }
        }

        this.gameObject.GetComponent<PlayerScript>().currentAbilityTimer = abilityTimer;
    }

    public override void EnterDungeon()
    {
        Destroy(sword);
    }

    public override void LeaveDungeon()
    {
        m_direction = (new Vector3(Mathf.Atan(1) * 180 / Mathf.PI, 0, Mathf.Atan(1) * 180 / Mathf.PI).normalized);
    }

    public override void Attack()
    {
        if (canAttack && !player.inDungeon)
        {
            if (gamePad.IsConnected)
            {
                float rightStickX = gamePad.GetStick_R().X;
                float rightStickY = gamePad.GetStick_R().Y;

                if (gamePad.GetButton("LB") && abilityCanBeUsed)
                    GameObject.Find("LevelHandler").GetComponent<LevelHandler>().currLevel++;

                if (rightStickX != 0.0f || rightStickY != 0.0f)
                {
                    m_direction = (new Vector3(Mathf.Atan(rightStickX) * 180 / Mathf.PI, 0, Mathf.Atan(rightStickY) * 180 / Mathf.PI).normalized);
                    sword = Instantiate(bulletInstance, (m_direction + new Vector3(transform.position.x, 0.5f, transform.position.z)), Quaternion.LookRotation(m_direction)) as GameObject;
                    sword.transform.localScale += new Vector3(2.0F, 8.0f, 2.0f);

                    GameObject swingSword = (GameObject)Instantiate(swingSwordParticles, new Vector3(sword.transform.position.x, 3, sword.transform.position.z), Quaternion.LookRotation(m_direction));

                    swingSword.transform.Rotate(new Vector3(90, 0, 20));
                    swingSword.transform.position = m_direction * 2 + transform.position + transform.forward * 1.0f + new Vector3(0, 1.0f, 0);
                    Destroy(swingSword, swingSword.GetComponent<ParticleSystem>().startLifetime);

                    sword.transform.position = m_direction * 2 + transform.position + sword.transform.forward * 1.1f + new Vector3(0, 1.5f, 0);

                    canAttack = false;
                    Invoke("AttackEnable", FIRE_RATE);

                    if (gamePad.GetButton("RB") && abilityCanBeUsed)
                    {
                        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
                        GameObject tempEnemy = null;
                        foreach (GameObject enemy in enemyArray)
                        {
                            tempEnemy = enemy;

                            if (Vector3.Distance(tempEnemy.transform.position, transform.position) < 15.0f)
                                tempEnemy.GetComponent<Enemy>().gotStunned = true;
                        }

                        GameObject[] bossArray = GameObject.FindGameObjectsWithTag("Boss");
                        tempEnemy = null;
                        foreach (GameObject boss in bossArray)
                        {
                            tempEnemy = boss;

                            if (Vector3.Distance(tempEnemy.transform.position, transform.position) < 15.0f)
                                tempEnemy.GetComponent<Enemy>().gotStunned = true;
                        }

                        GameObject explosion = (GameObject)Instantiate(stunParticles, new Vector3(stunParticles.transform.position.x + transform.position.x, 3, stunParticles.transform.position.z + transform.position.z), stunParticles.transform.rotation);
                        Destroy(explosion, explosion.GetComponent<ParticleSystem>().duration);

                        abilityCanBeUsed = false;
                    }
                }
                else if (gamePad.GetButton("RB") && abilityCanBeUsed)
                {
                    GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
                    GameObject tempEnemy = null;
                    foreach (GameObject enemy in enemyArray)
                    {
                        tempEnemy = enemy;

                        if (Vector3.Distance(tempEnemy.transform.position, transform.position) < 15.0f)
                            tempEnemy.GetComponent<Enemy>().gotStunned = true;
                    }

                    GameObject[] bossArray = GameObject.FindGameObjectsWithTag("Boss");
                    tempEnemy = null;
                    foreach (GameObject boss in bossArray)
                    {
                        tempEnemy = boss;

                        if (Vector3.Distance(tempEnemy.transform.position, transform.position) < 15.0f)
                            tempEnemy.GetComponent<Enemy>().gotStunned = true;
                    }

                    GameObject explosion = (GameObject)Instantiate(stunParticles, new Vector3(stunParticles.transform.position.x + transform.position.x, 3, stunParticles.transform.position.z + transform.position.z), stunParticles.transform.rotation);
                    Destroy(explosion, explosion.GetComponent<ParticleSystem>().duration);

                    abilityCanBeUsed = false;
                }
            }
        }
    }

    void AttackEnable()
    {
        canAttack = true;
    }

    void StunDuration()
    {
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject tempEnemy = null;
        foreach (GameObject enemy in enemyArray)
        {
            tempEnemy = enemy;
            tempEnemy.GetComponent<Enemy>().gotStunned = false;
            tempEnemy.GetComponent<Enemy>().activateStunnedParticlesOnce = false;
        }

        GameObject[] bossArray = GameObject.FindGameObjectsWithTag("Boss");
        tempEnemy = null;
        foreach (GameObject boss in bossArray)
        {
            tempEnemy = boss;
            tempEnemy.GetComponent<Enemy>().gotStunned = false;
            tempEnemy.GetComponent<Enemy>().activateStunnedParticlesOnce = false;
        }
    }
}
