using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChallengeTrigger : MonoBehaviour
{
    public List<CharacterSpecs> enemiesToKill = new List<CharacterSpecs>();
    public UnityEvent onAllEnemiesKilled;

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
