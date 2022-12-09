using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ChallengeTrigger : MonoBehaviour
{
    public List<CharacterSpecs> enemiesToKill = new List<CharacterSpecs>();
    public UnityEvent onAllEnemiesKilled;
    public bool saveToPlayerPrefs = true;

    private void Start()
    {
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
        if(enemiesToKill.Count == 0)
        {
            onAllEnemiesKilled?.Invoke();
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
}
