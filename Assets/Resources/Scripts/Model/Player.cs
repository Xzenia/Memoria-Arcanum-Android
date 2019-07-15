using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [HideInInspector]
    public float charge = 0;
    [HideInInspector]
    public float maxCharge = 100;

    public void IncreaseCharge(float amount)
    {
        if (charge < maxCharge)
        {
            charge += amount;
        }
    }

    public void ResetPlayerValues()
    {
        health = maxHealth;
        RevertToDefaultChargeValue();
        RevertToBaseValues();
        
    }

    public void RevertToDefaultChargeValue()
    {
        charge = 0;
    }

    public void RevertToBaseValues()
    {
        attackStat = baseAttackStat;
        defenseStat = baseDefenseStat;
    }
}

public enum Characters
{
    Shou, Emily, Rikko
}
