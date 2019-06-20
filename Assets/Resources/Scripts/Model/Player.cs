using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
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
