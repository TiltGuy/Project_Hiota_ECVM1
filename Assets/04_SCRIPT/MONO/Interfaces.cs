using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamages(float damageTaken);
}

public interface ITouchable
{
    void Touch();
}
