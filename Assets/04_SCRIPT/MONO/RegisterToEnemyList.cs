using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterToEnemyList : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DeckManager.instance.RegisterEnemy(gameObject);
    }
}
