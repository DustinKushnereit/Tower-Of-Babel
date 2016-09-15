using UnityEngine;
using System.Collections;
using System;

public class DungeonMovement : Movement
{

    public float m_moveSpeed = 4.0f;
    public float m_rotationalSpeed = 5.0f;

    private float yLookSensitivity = 0.03f;
    private bool autoLookCenter = true;

    x360_Gamepad gamePad;
    PlayerScript player;
    Rigidbody rgb;

    Animator anim;
    int runHash = Animator.StringToHash("Run");
    int idleHash = Animator.StringToHash("Idle");
    float leftStickX;
    float leftStickY;
    float rightStickX;
    float rightStickY;

    void Start ()
    {
        rgb = GetComponent<Rigidbody>();
        player = GetComponent<PlayerScript>();
        gamePad = player.gamePad;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (leftStickX >= 0.01f || leftStickY >= 0.01f || rightStickX >= 0.01f || rightStickY >= 0.01f)
        {
            anim.SetTrigger(runHash);
        }
        else
            anim.SetTrigger(idleHash);
    }

    public override void Controls()
    {
        try
        {
            if (gamePad.IsConnected)
            {
                rightStickX = gamePad.GetStick_R().X;
                rightStickY = gamePad.GetStick_R().Y;
                leftStickX = gamePad.GetStick_L().X;
                leftStickY = gamePad.GetStick_L().Y;
                
                
                Vector3 moveDirection = new Vector3(leftStickX, 0, leftStickY);
                transform.Translate(moveDirection * m_moveSpeed * Time.deltaTime);
                
                transform.Rotate(0, rightStickX * m_rotationalSpeed, 0);

                if (leftStickX >= 0.01f || leftStickY >= 0.01f || rightStickX >= 0.01f || rightStickY >= 0.01f)
                {
                    anim.SetTrigger(runHash);
                }

                //Camera tends towards center
                if (autoLookCenter && Math.Abs(gamePad.GetStick_R().Y) <= .05)
                {
                    if (Math.Abs(player.transform.GetChild(0).transform.localPosition.y - 2) > .1)
                    {
                        if (player.transform.GetChild(0).transform.localPosition.y > 2)
                            player.transform.GetChild(0).transform.position += new Vector3(0, -yLookSensitivity / 3, 0);
                        else
                            player.transform.GetChild(0).transform.position += new Vector3(0, yLookSensitivity / 3, 0);
                    }
                    else
                        player.transform.GetChild(0).transform.localPosition = new Vector3(player.transform.GetChild(0).transform.localPosition.x, 2, player.transform.GetChild(0).transform.localPosition.z);
                }
                else if (player.transform.GetChild(0).transform.localPosition.y + (0.1 * gamePad.GetStick_R().Y) < 3 && player.transform.GetChild(0).transform.localPosition.y + (0.1 * gamePad.GetStick_R().Y) > 1)
                {
                    player.transform.GetChild(0).transform.position += (new Vector3(0, yLookSensitivity, 0) * gamePad.GetStick_R().Y);
                }
             

            }
        }
        catch
        {
            Debug.Log("Caught a crash? (gamepad probably)");
        }
    }
}
