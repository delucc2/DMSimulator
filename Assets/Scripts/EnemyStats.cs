using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {
    [SerializeField]
    public int HEALTH;
    public int DAMAGE;
    public int COST;

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

    public void TakeDamage(int damage)
    {
        HEALTH -= damage;
    }
}
