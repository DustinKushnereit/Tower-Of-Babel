using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public List<GameObject> playerHealthBars;
    //private WaveManager waveManager;

    private bool player1HealthBarStatus = false;
    private bool player2HealthBarStatus = false;
    private bool player3HealthBarStatus = false;
    private bool player4HealthBarStatus = false;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;

    private GameObject[] players;

    public GameObject loseScreen;
    public GameObject winScreen;

    public int scorePlayer1 = 0;
    public int scorePlayer2 = 0;
    public int scorePlayer3 = 0;
    public int scorePlayer4 = 0;

    void Start ()
    {
        //setActiveBars();
        //waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        //scorePlayer1 = 0;
    }
	
	void Update ()
    {
        if(player1 != null)
            player1.gameObject.GetComponent<PlayerScript>().score = scorePlayer1;

        if (player2 != null)
            player2.gameObject.GetComponent<PlayerScript>().score = scorePlayer2;

        if (player3 != null)
            player3.gameObject.GetComponent<PlayerScript>().score = scorePlayer3;

        if (player4 != null)
            player4.gameObject.GetComponent<PlayerScript>().score = scorePlayer4;
    }

    public void setActiveBars(GameObject obj)
    {
        //Debug.Log(GameObject.Find("ActivePlayers").GetComponent<ActivePlayers>().characters.Count);
        //players = GameObject.FindGameObjectsWithTag("Player");

        //foreach (GameObject obj in players)
        {
            if(obj != null && obj.GetComponent<PlayerScript>().isActive && obj.GetComponent<PlayerScript>().isAlive)
                getHealthBar(obj).SetActive(true);
        }
    }

    GameObject getHealthBar(GameObject player)
    {
        if (player == GameObject.Find("Player 1"))
        {
            player1HealthBarStatus = true;
            player1 = player;
            //Debug.Log("Player 1");
            return playerHealthBars[0];
        }
        else if (player == GameObject.Find("Player 2"))
        {
            player2HealthBarStatus = true;
            player2 = player;
            //Debug.Log("Player 2");
            return playerHealthBars[1];
        }
        else if (player == GameObject.Find("Player 3"))
        {
            player3HealthBarStatus = true;
            player3 = player;
            //Debug.Log("Player 3");
            return playerHealthBars[2];
        }
        else if (player == GameObject.Find("Player 4"))
        {
            player4HealthBarStatus = true;
            player4 = player;
            //Debug.Log("Player 4");
            return playerHealthBars[3];
        }

        Debug.Log("Healthbar for player " + player + " not found");
        return null;
    }
    
    /*void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle(GUI.skin.label);
        myStyle.fontSize = 30;
        myStyle.normal.textColor = Color.red;

        myStyle.alignment = TextAnchor.UpperLeft;
        string wave = string.Format("Current Wave: {0}\n", waveManager.currWave);
        //Cool Guy Numbers, Please Ignore
        GUI.Label(new Rect(Screen.width * (2f / 10.55f), Screen.height * (0.1f / 16.3f), Screen.width * (4.8f / 6.55f), Screen.height * (0.85f / 6.3f)), wave, myStyle);

        if (player1HealthBarStatus)
        {
            myStyle.fontSize = 30;
            myStyle.normal.textColor = Color.red;
            string P1Lives = string.Format("{0}\n", player1.gameObject.GetComponent<PlayerScript>().m_lives);
            //Cool Guy Numbers, Please Ignore
            GUI.Label(new Rect(Screen.width * (2.2f / 10.55f), (playerHealthBars[0].transform.position.y * 3.6f), 
                Screen.width * (4.8f / 6.55f), Screen.height * (0.85f / 6.3f)), P1Lives, myStyle);
        }

        if (player2HealthBarStatus)
        {
            myStyle.fontSize = 30;
            myStyle.normal.textColor = Color.green;
            string P2Lives = string.Format("{0}\n", player2.gameObject.GetComponent<PlayerScript>().m_lives);
            //Cool Guy Numbers, Please Ignore
            GUI.Label(new Rect(Screen.width * (4.2f / 10.55f), (playerHealthBars[1].transform.position.y * 3.6f),
                Screen.width * (4.8f / 6.55f), Screen.height * (0.85f / 6.3f)), P2Lives, myStyle);
        }

        if (player3HealthBarStatus)
        {
            myStyle.fontSize = 30;
            myStyle.normal.textColor = Color.magenta;
            string P3Lives = string.Format("{0}\n", player3.gameObject.GetComponent<PlayerScript>().m_lives);
            //Cool Guy Numbers, Please Ignore
            GUI.Label(new Rect(Screen.width * (6.2f / 10.55f), (playerHealthBars[2].transform.position.y * 3.6f),
                Screen.width * (4.8f / 6.55f), Screen.height * (0.85f / 6.3f)), P3Lives, myStyle);
        }

        if (player4HealthBarStatus)
        {
            myStyle.fontSize = 30;
            myStyle.normal.textColor = Color.cyan;
            string P4Lives = string.Format("{0}\n", player4.gameObject.GetComponent<PlayerScript>().m_lives);
            //Cool Guy Numbers, Please Ignore
            GUI.Label(new Rect(Screen.width * (8.2f / 10.55f), (playerHealthBars[3].transform.position.y * 3.6f),
                Screen.width * (4.8f / 6.55f), Screen.height * (0.85f / 6.3f)), P4Lives, myStyle);
        }
    }*/
}
