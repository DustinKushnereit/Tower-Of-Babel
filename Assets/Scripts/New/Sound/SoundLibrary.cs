using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundLibrary : MonoBehaviour {

    public enum PLAYER_SOUND_INDEX
    {
        SELECTION,
        ANY,
        INJURED,
        BOSS,
        DEATH,
        TEAMMATE_DEATH,
        TM_DEATH1,
        TM_DEATH2,
        TM_DEATH3,
        DUNGEON_ANY,
        FOUND_TREASURE,
        FOUND_LOOT,
        TRAP_TRIGGERED
    }

    public List<AudioClip> cosetteAudio;
    public List<AudioClip> skylarAudio;
    public List<AudioClip> leonAudio;
    public List<AudioClip> ronanAudio;

    public List<AudioClip> betweenRounds;

    public enum BANTER_INDEX
    {
        R_C1,
        R_C2,
        C_S1,
        C_S2,
        L_C1,
        L_C2,
        L_R1,
        L_R2,
        L_R3,
        L_R4,
        L_R5,
        L_S1,
        L_S2,
        R_S1,
        R_S2
    }

    public void playBanter(BANTER_INDEX index)
    {
        //If crashes here the problem might be that all the sounds are not set properly in the inspector
        GameObject.Find("AudioManager").GetComponent<AudioManager>().characterSource.clip = betweenRounds[(int)index];
        GameObject.Find("AudioManager").GetComponent<AudioManager>().characterSource.Play();
    }
   
}
