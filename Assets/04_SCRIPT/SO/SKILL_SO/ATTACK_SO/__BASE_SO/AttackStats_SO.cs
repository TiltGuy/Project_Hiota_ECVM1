using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/AttackStats")]
public class AttackStats_SO : ScriptableObject
{
    public float damages;
    public Transform hitBoxPrefab;
    public float AnimID_Anticipation = 0f;
    public float AnimID_Hitframe = 0f;
    public float AnimID_Recovery = 0f;
}
