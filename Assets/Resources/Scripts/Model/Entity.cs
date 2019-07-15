using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public new string name;

    public float health;

    public float maxHealth;

    [Tooltip("AttackStat bases its value on this one.")]
    public float baseAttackStat;
    [Tooltip("DefenseStat bases its value on this one.")]
    public float baseDefenseStat;
    [HideInInspector]
    public float attackStat;
    [HideInInspector]
    public float defenseStat;

    void Start()
    {
        attackStat = baseAttackStat;
        defenseStat = baseDefenseStat;
    }

    public void IncreaseHealth(int increase)
    {
        if (health < maxHealth)
        {
            health += increase;
        }
    }

    public void DecreaseHealth(int decrease)
    {
        if (health > 0)
        {
            health -= decrease;
        }
    }

    public void IncreaseAttackStat(int increase)
    {
        attackStat += increase;
    }

    public void DecreaseAttackStat(int decrease)
    {
        attackStat -= decrease;
    }

    public void IncreaseDefenseStat(int increase)
    {
        defenseStat += increase;
    }

    public void DecreaseDefenseStat(int decrease)
    {
        defenseStat -= decrease;
    }
}
