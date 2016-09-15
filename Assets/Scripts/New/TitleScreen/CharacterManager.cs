using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public List<int> characters;
    
	void Start ()
    {
        DontDestroyOnLoad(this.gameObject);

        characters = new List<int>();
	}
	
	void Update ()
    {
	}
}
