using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/AttackStats")]
public class AttackStats_SO : ScriptableObject
{
    public float damages;
    public Transform hitBoxPrefab;
}
