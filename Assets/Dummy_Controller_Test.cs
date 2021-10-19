using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Controller_Test : MonoBehaviour, IDamageable, ITouchable
{
    public void DoSomething()
    {
        Debug.Log("ARGH!!!");
    }

    public void TakeDamages()
    {
        Debug.Log("ARGH!!!");
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
