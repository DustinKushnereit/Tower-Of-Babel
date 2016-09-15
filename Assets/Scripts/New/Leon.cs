using UnityEngine;
using System.Collections;
using System;

public class Leon : Character
{
    bool canAttack = true;
    public GameObject bulletInstance;
    bool abilityCanBeUsed = true;
    public float abilityTimerMaxAmount = 35.0f;
    float abilityTimer = 25.0f;

    void Start()
    {
    }

    public override void Init()
    {
        //base.Init();
        player = GetComponent<PlayerScript>();
        gamePad = player.gamePad;
        player.barks = GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().leonAudio;

        abilityTimer = abilityTimerMaxAmount;
        this.gameObject.GetComponent<PlayerScript>().abilityCooldownMaxAmount = abilityTimerMaxAmount;

        m_direction = (new Vector3(Mathf.Atan(1.0f) * 180 / Mathf.PI, 0, Mathf.Atan(1.0f) * 180 / Mathf.PI).normalized);
    }

    void Update()
    {

        if (!abilityCanBeUsed)
        {
            abilityTimer--;

            if (abilityTimer <= 0)
            {
                abilityCanBeUsed = true;
                abilityTimer = abilityTimerMaxAmount;
            }
        }

        this.gameObject.GetComponent<PlayerScript>().currentAbilityTimer = abilityTimer;
    }

    public override void Attack()
    {
        if (canAttack && !player.inDungeon)
        {
            if (gamePad.IsConnected)
            {
                float rightStickX = gamePad.GetStick_R().X;
                float rightStickY = gamePad.GetStick_R().Y;

                if (rightStickX != 0.0f || rightStickY != 0.0f)
                {
                    m_direction = (new Vector3(Mathf.Atan(rightStickX) * 180 / Mathf.PI, 0, Mathf.Atan(rightStickY) * 180 / Mathf.PI).normalized);
                    GameObject temp = Instantiate(bulletInstance, (m_direction + new Vector3(transform.position.x, 1.0f, transform.position.z)), Quaternion.LookRotation(m_direction)) as GameObject;
                    temp.GetComponent<Bullet>().UseTheForce(m_direction, 8.0f);

                    canAttack = false;
                    Invoke("AttackEnable", FIRE_RATE);

                    if (gamePad.GetButton("RB") && abilityCanBeUsed)
                    {
                        for (int i = 0, j = 1; i < 5; i++)
                        {
                            if (i < 3)
                                m_direction = (new Vector3(Mathf.Atan(rightStickX + (0.1f * i)) * 180 / Mathf.PI, 0, Mathf.Atan(rightStickY + (0.1f * i)) * 180 / Mathf.PI).normalized);
                            else
                            {
                                m_direction = (new Vector3(Mathf.Atan(rightStickX - (0.1f * j)) * 180 / Mathf.PI, 0, Mathf.Atan(rightStickY - (0.1f * j)) * 180 / Mathf.PI).normalized);
                                j++;
                            }

                            temp = Instantiate(bulletInstance, (m_direction + new Vector3(transform.position.x, 1.0f, transform.position.z)), Quaternion.LookRotation(m_direction)) as GameObject;
                            temp.transform.localScale += new Vector3(0.6F, 0.6F, 0.6F);
                            temp.GetComponent<Bullet>().UseTheForce(m_direction, 8.0f);
                        }

                        abilityCanBeUsed = false;
                    }
                }
                else if (gamePad.GetButton("RB") && abilityCanBeUsed)
                {
                    for (int i = 0, j = 1; i < 5; i++)
                    {
                        if (i < 3)
                            m_direction = (new Vector3(Mathf.Atan(0 + (0.1f * i)) * 180 / Mathf.PI, 0, Mathf.Atan(1.0f + (0.1f * i)) * 180 / Mathf.PI).normalized);
                        else
                        {
                            m_direction = (new Vector3(Mathf.Atan(0 - (0.1f * j)) * 180 / Mathf.PI, 0, Mathf.Atan(1.0f - (0.1f * j)) * 180 / Mathf.PI).normalized);
                            j++;
                        }

                        GameObject temp = Instantiate(bulletInstance, (m_direction + new Vector3(transform.position.x, 1.0f, transform.position.z)), Quaternion.LookRotation(m_direction)) as GameObject;
                        temp.transform.localScale += new Vector3(0.6F, 0.6F, 0.6F);
                        temp.GetComponent<Bullet>().UseTheForce(m_direction, 8.0f);
                    }

                    abilityCanBeUsed = false;
                }
            }
        }

    }

    public override void EnterDungeon() { }

    public override void LeaveDungeon() { }

    void AttackEnable()
    {
        canAttack = true;
    }
}
