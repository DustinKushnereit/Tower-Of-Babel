using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure;

public class DefenseMovement : Movement
{
    //gamepad stuff
    x360_Gamepad gamePad;
    PlayerScript player;

    Animator anim;
    int runHash = Animator.StringToHash("Run");
    int idleHash = Animator.StringToHash("Idle");
    int throwHash = Animator.StringToHash("Throw");

    float leftStickX;
    float leftStickY;
    float rightStickX;
    float rightStickY;

    Vector3 lastKnownPosition;

    void Start ()
    {
        player = GetComponent<PlayerScript>();
        gamePad = player.gamePad;
        Debug.Log(gamePad.IsConnected);//LEAVE IT

        anim = GetComponent<Animator>();
        lastKnownPosition = transform.position;
    }

    void Update()
    {
        if (leftStickX >= 0.01f || leftStickY >= 0.01f)
        {
            anim.SetTrigger(runHash);
        }
        else if (rightStickX >= 0.01f || rightStickY >= 0.01f)
        {
            anim.SetTrigger(throwHash);
        }
        else
            anim.SetTrigger(idleHash);

        if(transform.position == lastKnownPosition)
            anim.SetTrigger(idleHash);
    }

    public override void Controls()
    {
        try
        {
            if (gamePad.IsConnected)
            {

                leftStickX = gamePad.GetStick_L().X;
                leftStickY = gamePad.GetStick_L().Y;
                rightStickX = gamePad.GetStick_R().X;
                rightStickY = gamePad.GetStick_R().Y;

                //float horizontal = -leftStickX * Time.deltaTime * player.m_speed;
                //float vertical = leftStickY * Time.deltaTime * player.m_speed;

                
                if (rightStickX != 0.0f || rightStickY != 0.0f)
                {
                    anim.SetTrigger(throwHash);
                    transform.rotation = Quaternion.LookRotation(new Vector3(Mathf.Atan(rightStickX) * 180 / Mathf.PI, 0, Mathf.Atan(rightStickY) * 180 / Mathf.PI).normalized);
                }
                else if (leftStickX != 0.0f || leftStickY != 0.0f)
                {
                    anim.SetTrigger(runHash);
                    transform.rotation = Quaternion.LookRotation(new Vector3(Mathf.Atan(leftStickX) * 180 / Mathf.PI, 0, Mathf.Atan(leftStickY) * 180 / Mathf.PI).normalized);
                }


                Vector3 moveDirection = new Vector3(leftStickX, 0, leftStickY);
                transform.position += moveDirection * player.m_speed * Time.deltaTime;

                lastKnownPosition = transform.position;
            }
        }
        catch
        {
            Debug.Log("Error with controller");
        }
    }
}
