using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Controller_Test : MonoBehaviour, IDamageable, ITouchable
{
    public void DoSomething()
    {
        Debug.Log("ARGH!!!");
    }

    public void TakeDamages(float damages)
    {
        Debug.Log("ARGH!!! j'ai pris : " + damages + " points de Dommages", this);
    }
}
