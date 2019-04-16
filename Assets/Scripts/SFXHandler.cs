using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour {

    private AudioSource audio;
    public AudioClip combat_noises;
    public AudioClip level_complete;

    public void Start()
    {
        audio = this.gameObject.GetComponent<AudioSource>();
    }

    public void playCombat()
    {
        audio.clip = combat_noises;
        audio.loop = true;
        audio.Play();
    }

    public void stopCombat()
    { 
        audio.loop = false;
        audio.Stop();
    }

    public void playSound(AudioClip sound)
    {
        audio.clip = sound;
        audio.Play();
    }

    public void playLevelComplete()
    {
        audio.clip = level_complete;
        audio.Play();
    }
}
