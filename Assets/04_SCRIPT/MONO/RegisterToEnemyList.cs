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
        CharacterSpecs enemy = GetComponent<CharacterSpecs>();
        DeckManager.instance.RegisterEnemy(gameObject);
        if (challengeTrigger != null)
        {
            challengeTrigger.RegisterEnemy(enemy);
        }
        if ( ChallengeTrigger.instance.b_ListIsDynamic )
        {
            ChallengeTrigger.instance.RegisterEnemy(enemy);
        }
            
    }
}
