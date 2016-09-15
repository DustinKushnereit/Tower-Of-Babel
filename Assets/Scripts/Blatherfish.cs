using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Blatherfish : MonoBehaviour {

    public GameObject textBox;
    public GameObject panel;

	// Use this for initialization
	void Start () {
        hideBlatherfish();
        setText(5, "Welcome! Welcome to… Hmm. Where are we?");
	}

    public void setText(float time, string text)
    {
        //Show Blatherfish
        GetComponent<Image>().enabled = true;
        panel.GetComponent<Image>().enabled = true;
        textBox.GetComponent<Text>().enabled = true;

        //Set his text
        textBox.GetComponent<Text>().text = text;

        //Hide him after specified time
        Invoke("hideBlatherfish", time);
    }

    void Update()
    {
 
    }

    void hideBlatherfish()
    {
        //Hide Blatherfish
        GetComponent<Image>().enabled = false;
        panel.GetComponent<Image>().enabled = false;
        textBox.GetComponent<Text>().enabled = false;
    }
}
