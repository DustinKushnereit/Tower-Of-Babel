using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class PlayerScript : MonoBehaviour
{
    public int playerID; //0 = cosette, 1 = skylar , 2 = leon, 3 = ronan
    public bool inDungeon;
    public bool hasKey = false;
    public int treasure = 0;
    public int score = 0;

    public float m_health;
    public float m_speed;
    public bool isActive = false;
    public bool isAlive = true;
    public bool cosetteAttacked = false;
    public bool lavaShouldDisappear = false;
    public float abilityCooldownMaxAmount;
    public float currentAbilityTimer;

    public bool isSkylar = false;

    public int waveDiedOn = 0;

    //movement stuff
    public x360_Gamepad gamePad;
    public Movement move;
    public Character hero;

    //sound stuff
    public List<AudioClip> barks;
    private AudioSource audioSource;

    public GameObject particleSystemExplosion;
    public GameObject dungeonHurtParticles;
    public GameObject generalizedDamageParticles;

    private WaveManager waveManager;

    private Vector3 playerSize;

    void Start()
    {
        m_health = hero.MAX_HEALTH;
        m_speed = hero.MAX_SPEED;

        playerSize = transform.localScale;

        audioSource = GameObject.Find("AudioManager").GetComponent<AudioManager>().characterSource;
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        GameObject temp = GameObject.Find("CharacterManager");
        //Debug.Log(temp);

        for (int i = 0; i < temp.GetComponent<CharacterManager>().characters.Count; i++)
        {
            if (temp.GetComponent<CharacterManager>().characters[i] == playerID)
            {
                gamePad = GamepadManager.Instance.GetGamepad(i + 1);
                isActive = true;
                GameObject.Find("ActivePlayers").GetComponent<ActivePlayers>().characters.Add(this.gameObject);
                GuiManager guiManager = GameObject.Find("GuiManager").GetComponent<GuiManager>();
                guiManager.setActiveBars(this.gameObject);
            }
        }

        if (!isActive)
        {
            this.gameObject.SetActive(false);
        }
        else if (!inDungeon)
        {
            move = this.gameObject.AddComponent<DefenseMovement>();
            hero.Init();
        }
        else if (inDungeon)
        {
            move = this.gameObject.AddComponent<DungeonMovement>();
            hero.Init();
        }


    }

    void Update()
    {
        if (isAlive && cosetteAttacked == false)
        {
            move.Controls();
            hero.Attack();
        }

        if (!isAlive)
        {
            if (waveManager.currWave > waveDiedOn)
                Invoke("Respawn", 0);//Invoke of 0 seconds?
        }

        if (transform.position.y <= -4)
            transform.position = new Vector3(transform.position.x, 2, transform.position.z);

    }

    public void TakeDamage(float amount)
    {
        m_health -= amount;

        if (inDungeon)
        {
            GameObject explosion = (GameObject)Instantiate(dungeonHurtParticles, transform.position, transform.rotation);
            explosion.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            explosion.transform.position = explosion.transform.position + new Vector3(0, 1, 0);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
        }
        else
        {
            GameObject explosion = (GameObject)Instantiate(generalizedDamageParticles, transform.position, generalizedDamageParticles.transform.rotation);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
        }


        if (m_health <= 0)
        {
            Die();
            /*if (m_lives > 0)
            {
                Invoke("Respawn", m_respawnTime);
            }
            else
            {*/
            //this.gameObject.SetActive(false);
            gameObject.GetComponent<Renderer>().enabled = false; //if we use the SetActive code, the boss movement breaks
            isActive = false;
            waveDiedOn = waveManager.currWave;
            //}

        }
        else
        {
            //chance to play Injured Sound
            float rand = Random.Range(1, 20);
            if (rand == 1)
            {
                playSound(SoundLibrary.PLAYER_SOUND_INDEX.INJURED);
            }
        }
    }

    protected void Explode()
    {
        GameObject explosion = (GameObject)Instantiate(particleSystemExplosion, transform.position, particleSystemExplosion.transform.rotation);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().startLifetime);
    }

    public void Die()
    {
        GameObject.Find("ActivePlayers").GetComponent<ActivePlayers>().playerDeathSound(playerID);
        Explode();
        //m_lives--;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        isAlive = false;
    }

    public void Respawn()
    {
        m_health = hero.MAX_HEALTH;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        isAlive = true;
    }

    public void Rumble()
    {
        //                timer            power         fade
        gamePad.AddRumble(0.2f, new Vector2(1.0f, 1.0f), 0.5f);
    }

    public void EnterDungeon()
    {
        inDungeon = true;
        Destroy(gameObject.GetComponent<DefenseMovement>());
        gameObject.AddComponent<DungeonMovement>();
        move = GetComponent<DungeonMovement>();
        hero.EnterDungeon();
        transform.position = GameObject.Find("DungeonSpawn").transform.position;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        GameObject.Find("DungeonCamera").GetComponent<CameraFollowPlayer>().AssignPlayer(this.gameObject);
    }

    public void LeaveDungeon()
    {
        inDungeon = false;

        GameObject.Find("DungeonCamera").GetComponent<CameraFollowPlayer>().UnassignPlayer();

        Destroy(gameObject.GetComponent<DungeonMovement>());
        gameObject.AddComponent<DefenseMovement>();
        move = GetComponent<DefenseMovement>();
        hero.LeaveDungeon();
        transform.rotation = Quaternion.identity;
        transform.position = GameObject.Find("DefenseSpawn").transform.position;
        //transform.rotation = Quaternion.identity;
        transform.localScale = playerSize;
    }

    public float getMaxHealth()
    {
        return hero.MAX_HEALTH;
    }

    public void playSound(SoundLibrary.PLAYER_SOUND_INDEX soundEnum)//Takes an enum
    {
        //Only play a sound if one isn't alreay playing
        if (!audioSource.isPlaying)
        {
            audioSource.clip = barks[(int)soundEnum];
            audioSource.Play();
            //Put logic to put their sprite up here
        }
    }

}
