using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static List<GameObject> Shuffle (List<GameObject> list)
    {
        //Implements the Sattolo Algorithm to shuffle the list.
        int length = list.Count;
       
        while (length > 1)
        {
            length -= 1;
            int pickedItem = Random.Range(0, length);

            GameObject temp = list[length];
            list[length] = list[pickedItem];
            list[pickedItem] = temp;
        }

        return list;
    }

    public static List<GameObject> SortEnemies(List<GameObject> list)
    {
        var enemyList = list;

        int beginningIndex = 0;
        int endingIndex = enemyList.Count - 1;

        /*
         *  Sorting algorithm based on paper "An Approach to Improve the Performance of Insertion Sort Algorithm" by Partha Sarathi Dutta
         *  Link to paper: http://ijcset.com/docs/IJCSET13-04-05-068.pdf
         */
        while (beginningIndex < endingIndex)
        {
            var beginningIndexItem = enemyList[beginningIndex].GetComponent<Enemy>();
            var endingIndexItem = enemyList[endingIndex].GetComponent<Enemy>();
            if ((beginningIndexItem.baseAttackStat + beginningIndexItem.baseDefenseStat) > (endingIndexItem.baseAttackStat + endingIndexItem.baseDefenseStat))
            {
                var temp = enemyList[beginningIndex];
                enemyList[beginningIndex] = enemyList[endingIndex];
                enemyList[endingIndex] = temp;
            }

            beginningIndex++;
            endingIndex--;
        }
        //Insertion Sort
        for (int counter = 1; counter < enemyList.Count - 1; counter++)
        {
            var key = enemyList[counter];
            int j = counter - 1;

            var jItem = enemyList[j].GetComponent<Enemy>();
            var keyItem = key.GetComponent<Enemy>();
            while (j >= 0 && (jItem.baseAttackStat + jItem.baseDefenseStat) > (keyItem.baseAttackStat + keyItem.baseDefenseStat))
            {
                enemyList[j + 1] = enemyList[j];
                j--;
            }
            enemyList[j + 1] = key;
        }
        Debug.Log("SortEnemies(): Number of Elements in enemyList (after Insertion Sort): " + enemyList.Count);
        return enemyList;
    }
}
