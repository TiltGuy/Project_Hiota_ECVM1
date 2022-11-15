using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificationScript : MonoBehaviour
{

    #region DEPENDENCIES
    [Header(" -- DEPENDENCIES -- ")]

    //TO REWORK ==> Delocalise this reference to HiotaHealth
    public CharacterStats_SO HiotaStats;

    #endregion


    #region ATTACK Settings

    [Header(" -- ATTACK SETTINGS -- ")]

    [Tooltip("The speedTurn of the player when it attack")]
    [SerializeField] public float m_speedTurnWhenAttack = 5f;

    [Tooltip("the speed of the rotation between the forward of the character and the direction to go when it's in Focus")]
    public float m_turnSpeedWhenFocused = 20;

    [Tooltip("the current Stats of the Basic Attack that will be used for the next or current hit")]
    public AttackStats_SO BasicAttackStats;

    [Tooltip("the current Stats and HitBox of the Side Attack that will be used for the next or current hit")]
    public AttackStats_SO SideAttackStats;

    [Tooltip("the current Stats and HitBox of the Front Attack that will be used for the next or current hit")]
    public AttackStats_SO FrontAttackStats;

    [Tooltip("the current Stats and HitBox of the Back Attack that will be used for the next or current hit")]
    public AttackStats_SO BackAttackStats;

    [Tooltip("the current Stats and HitBox of the Parry Attack that will be used for the next or current hit")]
    public AttackStats_SO ParryAttackStats;


    //[Tooltip("The speed of the player")]
    //public float m_HoldAttackSpeed = 5f;

    //[Tooltip("The time remaining of a Attack")]
    //public float maxAttackTime = .2f;


    #endregion
}
