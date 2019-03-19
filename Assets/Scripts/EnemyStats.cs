using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
    [SerializeField]
    public int HEALTH;
    public int DAMAGE;
    public int COST;
    public int EXP;

    public int GetHealth()
    {
        return HEALTH;
    }

    public int GetDamage()
    {
        return DAMAGE;
    }

    public int GetCost()
    {
        return COST;
    }

    public int GetEXP()
    {
        return EXP;
    }

    public void TakeDamage(int damage)
    {
        HEALTH -= damage;
    }
}
