using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
    [SerializeField]
    public int HEALTH;
    public int MELEE_DAMAGE;
    public int RANGED_DAMAGE;
    public int COST;
    public int EXP;
    public bool ranged;
    public float HITRATE;

    public int GetHealth()
    {
        return HEALTH;
    }

    public int GetMeleeDamage()
    {
        return MELEE_DAMAGE;
    }

    public int GetRangedDamage()
    {
        return RANGED_DAMAGE;
    }

    public int GetCost()
    {
        return COST;
    }

    public int GetEXP()
    {
        return EXP;
    }

    public float GetHitrate()
    {
        return HITRATE;
    }

    public void TakeDamage(int damage)
    {
        HEALTH -= damage;
        if (HEALTH < 0)
        {
            HEALTH = 0;
        }
    }

    public bool GetRanged()
    {
        return ranged;
    }
}
