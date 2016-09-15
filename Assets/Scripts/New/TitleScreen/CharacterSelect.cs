using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class CharacterSelect : MonoBehaviour
{
    List<x360_Gamepad> gamePads;
    public bool canSelect = false;

    public List<GameObject> selectionSpots;
    public List<GameObject> selectors;
    public List<GameObject> splashes;

    int currChoosing;
    int currSelect;

    Vector3 changeStep = new Vector3(.005f,.005f,.005f);
    bool selectorGrowing = false;

    private bool finishedChoosing = false;
    private bool stickCooldown = false;

    void Start()
    {
        gamePads = new List<x360_Gamepad>();

        currChoosing = 0;
        currSelect = 0;
        selectors[currChoosing].transform.position = selectionSpots[currSelect].transform.position;
    }

    private void refreshControls()
    {
        if(GamepadManager.Instance.ConnectedTotal() < gamePads.Count || gamePads.Count == 0)
        {
            gamePads.Clear();
            Debug.Log("Connecting Controllers...");
            for (int i = 0; i < GamepadManager.Instance.ConnectedTotal(); i++)
            {
                gamePads.Add(GamepadManager.Instance.GetGamepad(i + 1));
            }
        }
    }
    
	void Update ()
    {
        refreshControls();

        if (canSelect)
        {
            move();
            animateSelector();
        }
    }

    private void move()
    {
        float leftStickX = gamePads[currChoosing].GetStick_L().X;

        if(gamePads[currChoosing].IsConnected && !finishedChoosing)
        {

            if (gamePads[currChoosing].GetButtonDown("DPad_Left") || (leftStickX < 0 && !stickCooldown))
            {
                stickCooldown = true;
                if(currSelect == 0)
                {
                    currSelect = 3;
                    selectors[currChoosing].transform.position = selectionSpots[currSelect].transform.position;
                }
                else
                {
                    currSelect--;
                    selectors[currChoosing].transform.position = selectionSpots[currSelect].transform.position;
                }
            }
            else if (gamePads[currChoosing].GetButtonDown("DPad_Right") || (leftStickX > 0 && !stickCooldown))
            {
                stickCooldown = true;
                if (currSelect == 3)
                {
                    currSelect = 0;
                    selectors[currChoosing].transform.position = selectionSpots[currSelect].transform.position;
                }
                else
                {
                    currSelect++;
                    selectors[currChoosing].transform.position = selectionSpots[currSelect].transform.position;
                }
            }

            if (leftStickX == 0)
            {
                stickCooldown = false;
            }

            if(gamePads[currChoosing].GetButtonDown("A"))
            {
                select();
            }

        }
    }

    private void select()
    {
        if (!selectionSpots[currSelect].GetComponent<Select>().selected)
        {
            selectionSpots[currSelect].GetComponent<Select>().selected = true;
            GameObject.Find("CharacterManager").GetComponent<CharacterManager>().characters.Add(currSelect);
            //Debug.Log(currSelect);
            splashes[currSelect].GetComponent<SpriteRenderer>().color = Color.black;
            selectors[currChoosing].transform.localScale = new Vector3(1,1,1);

            switch (currSelect)
            {
                case 0://Cosette
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().characterSource.clip = GameObject.Find("AudioManager").GetComponent<AudioManager>().cosetteSelection;
                    break;
                case 1://Mute thing
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().characterSource.clip = GameObject.Find("AudioManager").GetComponent<AudioManager>().skylarSelection;
                    break;
                case 2://Leon
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().characterSource.clip = GameObject.Find("AudioManager").GetComponent<AudioManager>().leonSelection;
                    break;
                case 3://Ronan
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().characterSource.clip = GameObject.Find("AudioManager").GetComponent<AudioManager>().ronanSelection;
                    break;
            }

            GameObject.Find("AudioManager").GetComponent<AudioManager>().characterSource.Play();

            if (currChoosing != 4 && currChoosing + 1 < GamepadManager.Instance.ConnectedTotal())
            {
                //selectors[currChoosing].GetComponent<Renderer>().enabled = false;
                Destroy(selectors[currChoosing].gameObject);
                currChoosing++;
            }
            else
            {
                //start the game pass in character list 
                //Debug.Log("Start Game with " + (currChoosing+1) + " characters");
                GameObject.Find("Mask").GetComponent<ScreenFader>().setFadeTime(2.0f);
                GameObject.Find("Mask").GetComponent<ScreenFader>().fadeIn = false;
                finishedChoosing = true;
                Invoke("advanceScene", 3.0f);
            }

            selectors[currChoosing].transform.position = selectionSpots[currSelect].transform.position;
        }
    }

    private void animateSelector()
    {
        if (selectorGrowing)
        {
            //Debug.Log("Growing" + selectors[currChoosing].transform.localScale.magnitude + " " + minScale.magnitude);
            selectors[currChoosing].transform.localScale += changeStep;
            if (selectors[currChoosing].transform.localScale.magnitude >= 1.5)
            {
                selectorGrowing = false;
            }
        }
        else
        {
            //Debug.Log("Shrinking" + selectors[currChoosing].transform.localScale.magnitude + " " + maxScale.magnitude);
            selectors[currChoosing].transform.localScale -= changeStep;
            if (selectors[currChoosing].transform.localScale.magnitude <= 1)
            {
                selectorGrowing = true;
            }
        }
    }

    private void advanceScene()
    {
        GameObject.Find("Mask").GetComponent<ScreenFader>().fadeIn = true;
        SceneManager.LoadScene(1);
    }

}
