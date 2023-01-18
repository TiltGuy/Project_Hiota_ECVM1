using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/AttackStats")]
public class AttackStats_SO : ScriptableObject
{
    public bool b_IsAHook = false;
    public float damages;
    public Transform hitBoxPrefab;
    public float AnimID_Anticipation = 0f;
    public float AnticipationAnimSpeed = 1f;
    public float AnimID_Hitframe = 0f;
    public float HitframeAnimSpeed= 1f;
    public float AnimID_Recovery = 0f;
    public float RecoveryAnimSpeed = 1f;

}
