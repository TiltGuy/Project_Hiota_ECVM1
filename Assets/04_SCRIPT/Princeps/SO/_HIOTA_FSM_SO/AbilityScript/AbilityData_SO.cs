using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityData_SO : ScriptableObject
{
    public float Duration;

    public abstract void UpdateAbility();
}
