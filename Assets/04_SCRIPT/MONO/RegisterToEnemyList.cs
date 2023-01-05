using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterToEnemyList : MonoBehaviour
{
    [SerializeField]
    ChallengeTrigger challengeTrigger;
    // Start is called before the first frame update
    void Start()
    {
        DeckManager.instance.RegisterEnemy(gameObject);
        if ( ChallengeTrigger.instance.b_ListIsDynamic )
        {
            ChallengeTrigger.instance.RegisterEnemy(GetComponent<CharacterSpecs>());
        }
            
    }
}
