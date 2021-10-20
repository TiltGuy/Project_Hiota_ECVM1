using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamages(float damages);
}

public interface ITouchable
{
    void DoSomething();
}
