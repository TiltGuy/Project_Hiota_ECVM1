using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EventOnMusic : MonoBehaviour
{
    public StudioEventEmitter MusicEmitter;
    private void OnEnable()
    {

        GameObject player_GO = GameObject.FindGameObjectWithTag("Player");
        if(player_GO != null)
        {
            CharacterSpecs refPlayerSpecs = player_GO.GetComponent<CharacterSpecs>();
            refPlayerSpecs.OnLoosingHealth += adjustLifeTrigger;
        }
        // subscribe à l'event du player qui perd sa vie
    }

    private void OnDisable()
    {
        // UNsubscribe à l'event du player qui perd sa vie
        GameObject player_GO = GameObject.FindGameObjectWithTag("Player");
        if ( player_GO != null )
        {
            CharacterSpecs refPlayerSpecs = player_GO.GetComponent<CharacterSpecs>();
            refPlayerSpecs.OnLoosingHealth -= adjustLifeTrigger;
        }
    }

    public void adjustLifeTrigger(float ratioLife)
    {
        MusicEmitter.SetParameter("Health", ratioLife * 100);
        Debug.Log("ratioLife " + ratioLife * 100);
    }
}
