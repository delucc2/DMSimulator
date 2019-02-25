using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapStats : MonoBehaviour {
    [SerializeField]
    public int notice_check;
    public int avoid_check;
    public int damage;

    public int GetNoticeCheck(){
        return notice_check;
    }

    public int GetAvoidCheck() {
        return avoid_check;
    }

    public int GetDamage() {
        return damage;
    }
}
