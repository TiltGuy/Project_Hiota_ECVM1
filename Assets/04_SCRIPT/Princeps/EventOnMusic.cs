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
        GameObject pauseManager = GameObject.Find("PauseManager");
        if(player_GO != null)
        {
            CharacterSpecs refPlayerSpecs = player_GO.GetComponent<CharacterSpecs>();
            refPlayerSpecs.OnLoosingHealth += adjustLifeTrigger;
        }
        // subscribe à l'event du player qui perd sa vie
        if ( pauseManager != null )
        {
            PauseManager pauseManagerScript = pauseManager.GetComponent<PauseManager>();
            pauseManagerScript.OnPaused += ChangePausedParameter;
        }
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


        GameObject pauseManager = GameObject.Find("PauseManager");
        if ( pauseManager != null )
        {
            PauseManager pauseManagerScript = pauseManager.GetComponent<PauseManager>();
            pauseManagerScript.OnPaused -= ChangePausedParameter;
        }
    }

    public void adjustLifeTrigger(float ratioLife)
    {
        MusicEmitter.SetParameter("Health", ratioLife * 100);
    }

    public void ChangePausedParameter(bool b_GamePaused)
    {
        if(b_GamePaused)
        {
            MusicEmitter.SetParameter("Pause", 1);
        }
        else
        {
            MusicEmitter.SetParameter("Pause", 0);
        }
    }
}
