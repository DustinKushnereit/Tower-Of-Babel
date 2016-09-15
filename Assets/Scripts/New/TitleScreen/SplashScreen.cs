using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class SplashScreen : MonoBehaviour
{

    bool startPressed = false;

    Vector3 changeStep = new Vector3(.002f, .002f, .002f);
    bool selectorGrowing = false;
    public GameObject startText;
    public ControlsScreen controlsScreen;

	void Start ()
    {
        startText.transform.localScale = new Vector3(0.5f, 0.2f, 1.0f);
	}
	
	void Update ()
    {
        animateStartText();

        if (GamepadManager.Instance.GetButtonAny("Start") && !startPressed)
        {
            startPressed = true;
            fadeOut();
        }
	}

    void fadeOut()
    {
        GameObject.Find("Mask").GetComponent<ScreenFader>().setFadeTime(0.5f);
        GameObject.Find("Mask").GetComponent<ScreenFader>().fadeIn = false;
        Invoke("fadeBack", 0.4f);
    }

    void fadeBack()
    {
        GameObject.Find("Mask").GetComponent<ScreenFader>().setFadeTime(0.2f);
        GameObject.Find("Mask").GetComponent<ScreenFader>().fadeIn = true;
        controlsScreen.gameObject.GetComponent<ControlsScreen>().startText.gameObject.GetComponent<Renderer>().enabled = true;
        controlsScreen.startCooldown();
        Destroy(this.gameObject);
    }

    private void animateStartText()
    {
        if (selectorGrowing)
        {
            //Debug.Log("Growing" + selectors[currChoosing].transform.localScale.magnitude + " " + minScale.magnitude);
            startText.transform.localScale += changeStep;
            if (startText.transform.localScale.magnitude >= 1.2)
            {
                selectorGrowing = false;
            }
        }
        else
        {
            //Debug.Log("Shrinking" + selectors[currChoosing].transform.localScale.magnitude + " " + maxScale.magnitude);
            startText.transform.localScale -= changeStep;
            if (startText.transform.localScale.magnitude <= 1.1)
            {
                selectorGrowing = true;
            }
        }
    }
}
