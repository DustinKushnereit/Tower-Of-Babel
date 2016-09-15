using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public GameObject player;
    public GameObject bar;
    public GameObject abilityCooldownBar;
    private Image healthBar;
    private Image abilityBar;
    private WaveManager waveManager;

    public Text playerScoreText;
    public Text waveText;
    private GameObject[] players;

    private int waveTimerCountdown;
    private bool beginCountdown = true;
    private int currentTreasure = 0;
    private int healThem = 0;
    private int minusTime = 2;
    private bool resetItAll = true;
    private float waveTextX;
    private float waveTextY;
    private int waveTextFontSize;

    GuiManager guiManager;

    void Start()
    {
        healthBar = bar.GetComponent<Image>();
        abilityBar = abilityCooldownBar.GetComponent<Image>();
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        guiManager = GameObject.Find("GuiManager").GetComponent<GuiManager>();

        if (waveText != null)
        {
            waveTextX = waveText.transform.position.x;
            waveTextY = waveText.transform.position.y;
            waveTextFontSize = waveText.fontSize;
        }

        waveTimerCountdown = (waveManager.timeBetweenRounds - minusTime) * 100;

        if (playerScoreText != null)
            playerScoreText.text = "0";

        if (waveText != null)
            waveText.text = "Wave: ";
    }

    void Update()
    {
        if (waveManager.startNextWaveTimer && beginCountdown)
        {
            waveTimerCountdown = (waveManager.timeBetweenRounds - minusTime) * 100;
            beginCountdown = false;
            healThem = 0;
        }

        if (healThem == 0 && waveManager.waveLength > 0)
        {
            healThem++;

            players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject obj in players)
            {
                if (obj.gameObject.GetComponent<PlayerScript>().isActive && obj.gameObject.GetComponent<PlayerScript>().isAlive && obj.gameObject.GetComponent<PlayerScript>().inDungeon == false)
                {
                    obj.GetComponent<PlayerScript>().m_health = obj.GetComponent<PlayerScript>().hero.MAX_HEALTH;
                }
            }
        }

        if (player.tag == "Player" || player.tag == "Skylar")
        {
            if (player != null)
            {
                if (player.GetComponent<PlayerScript>().treasure > currentTreasure)
                {
                    currentTreasure = player.GetComponent<PlayerScript>().treasure;
                    player.gameObject.GetComponent<PlayerScript>().score += 1000;
                }
            }
        }

        if (playerScoreText != null)
            playerScoreText.text = " " + player.gameObject.GetComponent<PlayerScript>().score;

        if (waveText != null && !waveManager.gameOver)
            waveText.text = "Wave: " + waveManager.currWave;

        if (waveText != null && waveManager.currWave != -1 && waveManager.startNextWaveTimer && !waveManager.gameOver)
        {
            waveTimerCountdown -= 5;//Time reduction for waves here
            waveText.text = " " + waveTimerCountdown / 100;
        }

        if (waveTimerCountdown <= 0)
        {
            waveManager.startNextWaveTimer = false;
            waveTimerCountdown = (waveManager.timeBetweenRounds - minusTime) * 100;
            beginCountdown = true;
        }

        if (waveText != null && waveManager.gameOver && resetItAll && !waveManager.playerVictory)
        {
            waveTimerCountdown -= 4;

            waveText.fontSize = 70;
            waveText.transform.position = new Vector3(750, 350, 0);
            GameObject dungeonCam = GameObject.FindWithTag("DungeonCamera");
            dungeonCam.GetComponent<Camera>().enabled = false;

            guiManager.loseScreen.gameObject.GetComponent<Renderer>().enabled = true;

            waveText.text = "\n\n\nNew Game In: " + waveTimerCountdown / 100;
        }

        if (waveText != null && waveManager.playerVictory && resetItAll)
        {
            waveTimerCountdown -= 4;

            waveText.fontSize = 70;
            waveText.transform.position = new Vector3(750, 350, 0);
            GameObject dungeonCam = GameObject.FindWithTag("DungeonCamera");
            dungeonCam.GetComponent<Camera>().enabled = false;

            guiManager.winScreen.gameObject.GetComponent<Renderer>().enabled = true;

            waveText.text = "\n\n\nNew Game In: " + waveTimerCountdown / 100;
        }

        if (resetItAll && waveManager.gameOver && waveTimerCountdown <= 0)
        {
            GameObject.Find("LevelHandler").GetComponent<LevelHandler>().Restart();
            waveManager.gameOver = false;
            waveManager.playerVictory = false;
            resetItAll = true;

            if (waveText != null)
            {
                waveText.fontSize = waveTextFontSize;
                waveText.transform.position = new Vector3(waveTextX, waveTextY, 0);
            }

            GameObject dungeonCam = GameObject.FindWithTag("DungeonCamera");
            dungeonCam.GetComponent<Camera>().enabled = true;

            guiManager.winScreen.gameObject.GetComponent<Renderer>().enabled = false;
            guiManager.loseScreen.gameObject.GetComponent<Renderer>().enabled = false;
            guiManager.scorePlayer1 = 0;
            guiManager.scorePlayer4 = 0;
            guiManager.scorePlayer2 = 0;
            guiManager.scorePlayer3 = 0;

            waveManager.currWave = 0;
            waveManager.startNextWaveTimer = true;
            waveTimerCountdown = (waveManager.timeBetweenRounds - minusTime) * 100;
            beginCountdown = true;
            waveManager.waveEnd = false;
            waveManager.finalBoss = false;
            waveManager.finalBossActivated = false;
            waveManager.spawnOnce = true;

            if (playerScoreText != null)
                playerScoreText.text = " ";

            players = GameObject.FindGameObjectsWithTag("Player");
            int i = 0;

            foreach (GameObject obj in players)
            {
                if (obj != null)
                {
                    obj.gameObject.GetComponent<PlayerScript>().score = 0;
                    obj.gameObject.GetComponent<PlayerScript>().hasKey = false;
                    obj.gameObject.GetComponent<PlayerScript>().inDungeon = false;
                    obj.gameObject.GetComponent<PlayerScript>().Respawn();
                    obj.gameObject.GetComponent<PlayerScript>().LeaveDungeon();
                    obj.gameObject.GetComponent<PlayerScript>().waveDiedOn = -1;
                    obj.gameObject.GetComponent<PlayerScript>().isAlive = true;
                    obj.gameObject.GetComponent<PlayerScript>().isActive = true;
                    //obj.GetComponent<PlayerScript>().m_health = obj.GetComponent<PlayerScript>().hero.MAX_HEALTH;
                    obj.gameObject.GetComponent<PlayerScript>().transform.position = new Vector3(-9 + (i * 5), 1, -6);
                    i++;
                }
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (enemy.gameObject.tag == "Enemy")
                {
                    if (enemy != null)
                    {
                        Destroy(enemy.gameObject);
                        enemy.gameObject.GetComponent<Enemy>().Explode();
                        waveManager.totalEnemies--;
                        waveManager.enemyKilled();
                    }
                }
            }

            GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");

            foreach (GameObject boss in bosses)
            {
                if (boss.gameObject.tag == "Boss")
                {
                    if (boss != null)
                    {
                        Destroy(boss.gameObject);
                        boss.gameObject.GetComponent<Enemy>().Explode();
                        waveManager.totalEnemies--;
                        waveManager.enemyKilled();
                    }
                }
            }

        }

        if (!waveManager.gameOver)
            waveManager.gameOver = waveManager.checkWinLoss();



        //Debug.Log(player.GetComponent<PlayerScript>().m_health + " " + player.GetComponent<PlayerScript>().m_health / player.GetComponent<PlayerScript>().hero.MAX_HEALTH);
        if (player.tag == "Player" || player.tag == "Skylar")
        {
            healthBar.fillAmount = player.GetComponent<PlayerScript>().m_health / player.GetComponent<PlayerScript>().hero.MAX_HEALTH;
            abilityBar.fillAmount = player.GetComponent<PlayerScript>().currentAbilityTimer / player.GetComponent<PlayerScript>().abilityCooldownMaxAmount;
        }
        else
        {
            //Debug.Log(((float)waveManager.waveLength - (float)waveManager.enemiesKilled) / (float)waveManager.waveLength);
            healthBar.fillAmount = ((float)waveManager.waveLength - (float)waveManager.enemiesKilled) / (float)waveManager.waveLength;
        }

    }

}
