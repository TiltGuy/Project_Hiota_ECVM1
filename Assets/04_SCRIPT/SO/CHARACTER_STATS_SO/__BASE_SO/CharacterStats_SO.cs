using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/CharacterStats")]
public class CharacterStats_SO : ScriptableObject
{
    public float maxHealth;
    public float baseHealth;

    public float baseArmor;

    public float maxGuard;
    public float baseGuard;
}
