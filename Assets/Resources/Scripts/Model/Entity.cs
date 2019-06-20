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
    [Tooltip("AttackStat has the same initial value as BaseAttackStat")]
    public float attackStat;
    [Tooltip("DefenseStat has the same initial value as BaseDefenseStat")]
    public float defenseStat;

    public void IncreaseHealth(int increase)
    {
        health += increase;
    }

    public void DecreaseHealth(int decrease)
    {
        health -= decrease;
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
