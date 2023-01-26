﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.TerrainAPI;

public class CheckpointTrigger : MonoBehaviour
{
	static public List<CheckpointTrigger> instances = new List<CheckpointTrigger>();
	
	public bool isTriggered = false;
	public int checkpointIndex = 1;
    public Transform respawnTarget = null;
    public GameObject triggerParticle = null;
    public Animator triggerAnimator = null;
	
	public string tagFilter = "Player";
	public string playerPrefsKey = "LastCheckpoint";
	
	public UnityEvent onEntered;

    [ContextMenu("Set Last Checkpoint")]
    public void SetLastCheckpoint()
    {
        PlayerPrefs.SetInt(playerPrefsKey, checkpointIndex);
    }

    public bool SetCurrentCheckpoint()
    {
        var activeIndex = PlayerPrefs.GetInt(playerPrefsKey);
        if ( activeIndex >= checkpointIndex )
        {
            // checkpoint reloaded from Player Respawn
            if ( triggerAnimator != null )
            {
                triggerAnimator.SetBool("Triggered", true);
            }

            onEntered?.Invoke();

            return true;
        }
        else
        {
            return false;
        }
    }
	
	private void OnTriggerEnter(Collider other)
	{
		if(isTriggered)
		{
			return;
		}
		
		if(other.tag == tagFilter)
		{
            // checkpoint entered
            string nameFile = DataPersistenceManager.instance.tutoSaveName;
            CharacterSpecs playerSpecs = other.gameObject.GetComponent<CharacterSpecs>();
            SaveSystem.SavePlayerData(playerSpecs, DeckManager.instance, nameFile);
            //PlayerPrefs.SetInt(playerPrefsKey, checkpointIndex);
            Debug.Log(playerPrefsKey + " => " + checkpointIndex);

            if ( triggerParticle != null )
            {
                Instantiate(triggerParticle, other.transform.position, other.transform.rotation);
            }

            SetCurrentCheckpoint();

            isTriggered = true;
        }
	}
	
	private void OnEnable()
	{
        if ( respawnTarget == null )
            respawnTarget = transform;

		instances.Add(this);
	}
	
	private void OnDisable()
	{
		instances.Remove(this);
	}
}