using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamages(float damageTaken, Transform Striker, bool isAHook);
    void TakeDamages( float damageTaken, Transform striker );
}

public interface ITouchable
{
    void Touch();
}
