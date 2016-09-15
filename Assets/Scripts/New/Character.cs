using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public abstract class Character : MonoBehaviour
{
    public float MAX_HEALTH;
    public float MAX_SPEED;
    public float FIRE_RATE;

    protected x360_Gamepad gamePad;
    protected PlayerScript player;
    protected Vector3 m_direction;
    
    void Start ()
    {
        Init();
	}

    public abstract void Init();

    public abstract void Attack();

    public abstract void EnterDungeon();
    public abstract void LeaveDungeon();
}
