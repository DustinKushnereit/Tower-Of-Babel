using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivePlayers : MonoBehaviour
{
    public List<GameObject> characters;

    public enum CHARACTERS
    {
        COSETTE,
        SKYLAR,
        LEON,
        RONAN
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        characters = new List<GameObject>();
    }
	
    //Takes in the number of the player that dies and chooses a random active player to play a sound
    //If the player chooses itself, it plays its own death noise
    public void playerDeathSound(int playerDead)
    {
        //Tell blatherfish to say something
        GameObject.Find("BlatherFish").GetComponent<Blatherfish>().setText(5, "One down!");

        int rand = Random.Range(0, characters.Count);
        int randID = characters[rand].GetComponent<PlayerScript>().playerID;

        //Self died
        if(randID == playerDead)
        {
            characters[rand].GetComponent<PlayerScript>().playSound(SoundLibrary.PLAYER_SOUND_INDEX.DEATH);
        }
        else //Other player died
        {
            if (Random.Range(0, 1) == 0)// 1 in 2 chance to play character specific death
            {
                //Checks dead player and uses enum math to play proper death sound
                if (randID > playerDead)
                {
                    characters[rand].GetComponent<PlayerScript>().playSound(SoundLibrary.PLAYER_SOUND_INDEX.TEAMMATE_DEATH + 1 + playerDead);
                }
                else
                {
                    characters[rand].GetComponent<PlayerScript>().playSound(SoundLibrary.PLAYER_SOUND_INDEX.TEAMMATE_DEATH + playerDead);
                }
            }
            else//Play generic teammate death
            {
                characters[rand].GetComponent<PlayerScript>().playSound(SoundLibrary.PLAYER_SOUND_INDEX.TEAMMATE_DEATH);
            }
        }

    }

    //Just going to be a bunch of case checks
    public void playBanter()
    {
        //Don't talk with someone if there is only one character
        if(characters.Count > 1)
        {
            //Choose two characters to start banter
            int firstChar = characters[Random.Range(0, characters.Count)].GetComponent<PlayerScript>().playerID;
            int secondChar;
            
            do//Makes sure doesnt choose the same character
            {
                secondChar = characters[Random.Range(0, characters.Count)].GetComponent<PlayerScript>().playerID;
            }while(secondChar == firstChar);

            //0 = cosette, 1 = skylar , 2 = leon, 3 = ronan
            //Make sure it is different every time
            switch (Random.Range(0, 5))
            {
                case 0:
                    if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR))
                        CosetteAndSkylar();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        CosetteAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        CosetteAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        SkylarAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        SkylarAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.LEON) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        LeonAndRonan();
                    break;
                case 1:
                    if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        CosetteAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        CosetteAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        SkylarAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        SkylarAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.LEON) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        LeonAndRonan();
                    else if(calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR))
                        CosetteAndSkylar();
                    break;
                case 2:
                    if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        CosetteAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        SkylarAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        SkylarAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.LEON) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        LeonAndRonan();
                    else if(calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR))
                        CosetteAndSkylar();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        CosetteAndLeon();
                    break;
                case 3:
                    if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        SkylarAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        SkylarAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.LEON) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        LeonAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR))
                        CosetteAndSkylar();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        CosetteAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        CosetteAndRonan();
                    break;
                case 4:
                    if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        SkylarAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.LEON) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        LeonAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR))
                        CosetteAndSkylar();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        CosetteAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        CosetteAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        SkylarAndLeon();
                    break;
                case 5:
                    if (calcPlayers(firstChar, secondChar, CHARACTERS.LEON) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        LeonAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR))
                        CosetteAndSkylar();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        CosetteAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.COSETTE) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        CosetteAndRonan();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.LEON))
                        SkylarAndLeon();
                    else if (calcPlayers(firstChar, secondChar, CHARACTERS.SKYLAR) && calcPlayers(firstChar, secondChar, CHARACTERS.RONAN))
                        SkylarAndRonan();
                    break;
            }
        }
    }

    public bool characterExists(CHARACTERS character)
    {
        foreach(GameObject obj in characters)
        {
            if (obj.GetComponent<PlayerScript>().playerID == (int)character)
                return true;
        }

        return false;
    }

    //Short check for finding if either player is result player
    private bool calcPlayers(int char1, int char2, CHARACTERS result)
    {
        return (char1 == (int)result || char2 == (int)result);
    }

    //These functions just make the banter loop smaller and allows for easier randomization

    private void CosetteAndSkylar()
    {
        //Add cases for each new banter sound
        switch(Random.Range(0,1))
        {
            case 0:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.C_S1);
                break;
            case 1:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.C_S2);
                break;
        }
    }

    private void CosetteAndLeon()
    {
        switch (Random.Range(0, 1))
        {
            case 0:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.L_C1);
                break;
            case 1:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.L_C2);
                break;
        }
    }

    private void CosetteAndRonan()
    {
        switch (Random.Range(0, 1))
        {
            case 0:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.R_C1);
                break;
            case 1:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.R_C2);
                break;
        }
    }

    private void SkylarAndLeon()
    {
        switch (Random.Range(0, 1))
        {
            case 0:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.L_S1);
                break;
            case 1:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.L_S2);
                break;
        }
    }

    private void SkylarAndRonan()
    {
        switch (Random.Range(0, 1))
        {
            case 0:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.R_S1);
                break;
            case 1:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.R_S2);
                break;
        }
    }

    private void LeonAndRonan()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.L_R1);
                break;
            case 1:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.L_R2);
                break;
            case 2:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.L_R3);
                break;
            case 3:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.L_R4);
                break;
            case 4:
                GameObject.Find("SoundLibrary").GetComponent<SoundLibrary>().playBanter(SoundLibrary.BANTER_INDEX.L_R5);
                break;
        }
    }
}
