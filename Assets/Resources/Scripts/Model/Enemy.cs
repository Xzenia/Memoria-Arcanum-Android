using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : Entity
{
    public AttackType attackType;

    public static List<GameObject> SetupEnemyList(GameObject[] enemies)
    {
        List<GameObject> enemyList = new List<GameObject>(enemies);
        List<GameObject> finalEnemyList = new List<GameObject>();

        if (enemyList.Count > 0)
        {
            enemyList = Extensions.Shuffle(enemyList);

            int numberOfEnemies = Random.Range(3, 11);

            for (int counter = 0; counter < numberOfEnemies; counter++)
            {
                finalEnemyList.Add(enemyList[counter]);
            }
        }
        else
        {
            Debug.LogError("SetupEnemyList(): enemyList is empty!");
        }

        finalEnemyList = Extensions.SortEnemies(finalEnemyList);
        return finalEnemyList;
    }
}
public enum AttackType
{
    Teeth = 0,
    Claws = 1,
    Crystal = 2,
    Projectile = 3
}