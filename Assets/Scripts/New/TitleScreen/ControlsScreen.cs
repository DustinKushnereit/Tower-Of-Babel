using UnityEngine;
using System.Collections;

public class ControlsScreen : MonoBehaviour
{ 
    bool canPressStart = false;
    bool startPressed = false;

    Vector3 changeStep = new Vector3(.002f, .002f, .002f);
    bool selectorGrowing = false;
    public GameObject startText;

    void Start()
    {
        startText.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    void Update()
    {
        animateStartText();

        if (canPressStart && GamepadManager.Instance.GetButtonAny("Start") && !startPressed)
        {
            startPressed = true;
            fadeOut();
        }
    }

    void fadeOut()
    {
        GameObject.Find("Mask").GetComponent<ScreenFader>().setFadeTime(0.3f);
        GameObject.Find("Mask").GetComponent<ScreenFader>().fadeIn = false;
        GameObject.Find("CharacterSelect").GetComponent<CharacterSelect>().canSelect = true;
        Invoke("fadeBack", 0.3f);
    }

    void fadeBack()
    {
        GameObject.Find("Mask").GetComponent<ScreenFader>().setFadeTime(0.3f);
        GameObject.Find("Mask").GetComponent<ScreenFader>().fadeIn = true;
        Destroy(this.gameObject);
        Destroy(startText.gameObject);
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
            if (startText.transform.localScale.magnitude <= 1)
            {
                selectorGrowing = true;
            }
        }
    }

    public void startCooldown()
    {
        canPressStart = false;
        startText.SetActive(false);
        Invoke("endCooldown", 2);
    }

    private void endCooldown()
    {
        startText.SetActive(true);
        canPressStart = true;
    }
}
