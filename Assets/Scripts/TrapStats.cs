using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapStats : MonoBehaviour {
    [SerializeField]
    public int notice_check;
    public int avoid_check;
    public int damage;
    public int cost;
    public AudioClip failure_sound;
    public AudioClip success_sound;

    public int GetNoticeCheck(){
        return notice_check;
    }

    public int GetAvoidCheck() {
        return avoid_check;
    }

    public int GetDamage() {
        return damage;
    }

    public AudioClip GetFailureSound()
    {
        return failure_sound;
    }

    public AudioClip GetSuccessSound()
    {
        return success_sound;
    }
}
