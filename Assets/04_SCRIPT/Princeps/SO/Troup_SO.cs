using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Troup of Enemies")]
public class Troup_SO : ScriptableObject
{

    public GameObject[] Enemies;
    public float difficultyModifier;
    public float DifficultyLevel()
    {
        float currentDiff = 0;
        foreach (GameObject item in Enemies)
        {
            currentDiff += item.GetComponent<CharacterSpecs>().CharStats_SO.difficultyNumber;
        }

        return currentDiff + difficultyModifier;
    }
}
