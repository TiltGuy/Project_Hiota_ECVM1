using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/CharacterStats")]
public class CharacterStats_SO : ScriptableObject
{
    [Header("--- HEALTH SETTINGS ---")]
    public float maxHealth;
    public float StartHealth;

    [Header("--- ARMOR SETTINGS ---")]
    public float baseArmor;

    [Header("--- GUARD SETTINGS ---")]
    public float maxGuard;
    public float baseGuard;

    [Header("--- MOVEMENT SETTINGS ---")]
    public float BaseSpeed;
    public float BaseSpeedWhenStrafing;
}
