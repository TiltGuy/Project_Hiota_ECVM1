﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using FMODUnity;

public class ChallengeTrigger : MonoBehaviour
{

    public List<CharacterSpecs> enemiesToKill = new List<CharacterSpecs>();
    public UnityEvent onPlayerEnterCombatZone;
    public delegate void OnMultiDelegate();
    public OnMultiDelegate OnStartCombatDelegate;
    public UnityEvent onAllEnemiesKilled;
    public bool saveToPlayerPrefs = true;
    public bool b_ListIsDynamic = false;
    public bool b_CanCheckWinCondition = true;
    public List<GameObject> toggleObjects = new List<GameObject>();
    private bool b_CombatDone = false;
    public static ChallengeTrigger instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        toggleObjects.ForEach(o => o.SetActive(false));



        if ( saveToPlayerPrefs )
        {
            if ( PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "." + name + ".completed") == 1 )
            {
                //Debug.Log("CompleteChallenge => " + this);
                enemiesToKill.ForEach(e => e.gameObject.SetActive(false));
                enemiesToKill.Clear();
            }
        }
    }

    private void OnEnable()
    {
        enemiesToKill.ForEach(e =>
        {
            e.onHealthDepleted += OnEnemyHealthDepleted;
        });
    }

    private void OnDisable()
    {
        enemiesToKill.ForEach(e =>
        {
            e.onHealthDepleted -= OnEnemyHealthDepleted;
        });
    }

    private void OnEnemyHealthDepleted( CharacterSpecs obj )
    {
        obj.onHealthDepleted -= OnEnemyHealthDepleted;
        enemiesToKill.Remove(obj);
    }

    private void Update()
    {
        if(enemiesToKill.Count == 0 && b_CanCheckWinCondition)
        {
            toggleObjects.ForEach(o => o.SetActive(true));
            onAllEnemiesKilled?.Invoke();
            RuntimeManager.StudioSystem.setParameterByName("Fight", 0);
            if (LevelManager.instance != null)
            {
                LevelManager.instance.DefineNextTroopIndex();
            }
            enabled = false;

            if ( saveToPlayerPrefs )
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "." + name + ".completed", 1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        enemiesToKill.ForEach(e =>
        {
            if(e != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, e.transform.position);
            }
        });
    }

    public void RegisterEnemy(CharacterSpecs enemy)
    {
        if(b_ListIsDynamic)
        {
            enemiesToKill.Add(enemy);
            b_CanCheckWinCondition = true;
            enemy.onHealthDepleted += OnEnemyHealthDepleted;

            var aiBrain = enemy.GetComponent<IABrain>();
            if ( aiBrain != null && aiBrain.autoStartCombat)
            {
                OnStartCombatDelegate += aiBrain.AddPlayerToCurrentControllerTarget;
            }
        }
    }

    private void OnTriggerEnter( Collider other )
    {
        if(other.CompareTag("Player") && !b_CombatDone )
        {
            ApplyCardEffectsToAllEnemiesInCombat();
            onPlayerEnterCombatZone?.Invoke();
            OnStartCombatDelegate?.Invoke();
            b_CombatDone = true;
            //RuntimeManager.StudioSystem.setParameterByNameWithLabel("Fight", "Value B");
            RuntimeManager.StudioSystem.setParameterByName("Fight", 1);
        }

    }

    private void ApplyCardEffectsToAllEnemiesInCombat()
    {
        foreach ( CharacterSpecs enemy in enemiesToKill )
        {
            DeckManager.instance.ApplyCardEffectsToEnemy(
                enemy.GetComponent<Controller_FSM>(),
                enemy
                );
            //Debug.Log("Power ++ " + enemy.MaxHealth, enemy);
        }
    }
}
