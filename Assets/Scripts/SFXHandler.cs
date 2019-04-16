using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour {

    private AudioSource audio;
    public AudioClip combat_noises;
    public AudioClip level_complete;
    public AudioClip ambient_convo;
    public AudioClip cheering;
    public AudioClip coin_clinking;
    public AudioClip evil_laugh;
    public AudioClip female_grunt;
    public AudioClip grumbling;
    public AudioClip holy_sound;
    public AudioClip huh;
    public AudioClip male_grunt;
    public AudioClip papers_rustling;
    public AudioClip sigh;
    public AudioClip magic_circle;
    public AudioClip wizard_attack_magic_missile;
    public AudioClip paladin_attack_magic_strike;
    public AudioClip warlock_sound;
    public AudioClip monster_attack;
    public AudioClip monster_death;
    public AudioClip swordfighting1;
    public AudioClip swordfighting2;
    public AudioClip horrorambience;
    public AudioClip bard_attack;
    public AudioClip rogue_attack;

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
