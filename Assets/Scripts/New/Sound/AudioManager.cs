using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private AudioClip openingAudio;
    private AudioClip battleMusic;
    public AudioSource musicSource;
    public AudioSource characterSource;
    public AudioClip ronanSelection;
    public AudioClip leonSelection;
    public AudioClip cosetteSelection;
    public AudioClip skylarSelection;

    void Start ()
    {
        musicSource = GetComponents<AudioSource>()[0];
        characterSource = GetComponents<AudioSource>()[1];
        
        battleMusic = Resources.Load("Battle") as AudioClip;
        openingAudio = Resources.Load("Menu") as AudioClip;

        DontDestroyOnLoad(this.gameObject);
    }
	
	void Update ()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            musicSource.loop = true;
            musicSource.clip = openingAudio as AudioClip;

            if (!musicSource.isPlaying)
            {
                musicSource.Stop();
                musicSource.Play();
            }
        }
        else
        {
            musicSource.loop = true;
            musicSource.clip = battleMusic as AudioClip;

            if (!musicSource.isPlaying)
            {
                musicSource.Stop();
                musicSource.Play();
            }
        }

    }

}
