using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EFCT_SO",
    menuName = "SkillCard/SCE_Explosion")]

public class SCE_Explosion : Effect_SO
{
    public GameObject explosionPrefab;

    public override void AddEffect( Controller_FSM controller, CharacterSpecs characterSpecs )
    {
        // Do your stuff
        characterSpecs.onHealthDepleted += SpawnExplosion;
    }

    private void SpawnExplosion( CharacterSpecs characterSpecs )
    {
        Debug.Log("BOUM HEADSHOT!!! ", characterSpecs);
        GameObject gameObject = Instantiate(explosionPrefab,
            characterSpecs.transform.position,
            characterSpecs.transform.rotation);
    }
}
