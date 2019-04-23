using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour {

    private AudioSource audio;
    public AudioClip combat_noises;
    public AudioClip level_complete;
    public static AudioClip ambient_convo;
    public static AudioClip cheering;
    public static AudioClip coin_clinking;
    public static AudioClip evil_laugh;
    public static AudioClip female_grunt;
    public static AudioClip grumbling;
    public static AudioClip holy_sound;
    public static AudioClip huh;
    public static AudioClip male_grunt;
    public static AudioClip papers_rustling;
    public static AudioClip sigh;
    public static AudioClip magic_circle;
    public static AudioClip wizard_attack_magic_missile;
    public static AudioClip paladin_attack_magic_strike;
    public static AudioClip warlock_sound;
    public static AudioClip monster_attack;
    public static AudioClip monster_death;
    public static AudioClip swordfighting1;
    public static AudioClip swordfighting2;
    public static AudioClip horrorambience;
    public static AudioClip bard_attack;
    public static AudioClip rogue_attack;

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
