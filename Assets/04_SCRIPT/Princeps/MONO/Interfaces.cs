using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamagesParriable(float damageTaken, Transform Striker, bool isAHook);

    void TakeDamagesNonParriable(float damageTaken, Transform Striker, float ForceOfProjection);
}

public interface ITouchable
{
    void Touch();
}
