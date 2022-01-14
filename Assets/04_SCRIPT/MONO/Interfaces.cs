using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamages(float damageTaken, Transform Striker);
}

public interface ITouchable
{
    void Touch();
}
