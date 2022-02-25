using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGatherer : MonoBehaviour
{
    [SerializeField]
    private Controller_FSM controller;
    [SerializeField]
    private LayerMask layerMask;
    public List<Transform> PotentialEnemies;
    public List<Transform> TargetableEnemies;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(!PotentialEnemies.Contains(other.transform))
            {
                PotentialEnemies.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(PotentialEnemies.Contains(other.transform))
            {
                TargetableEnemies.Remove(other.transform);
                PotentialEnemies.Remove(other.transform);
            }
        }
    }

    private void Update()
    {
        if (PotentialEnemies.Count > 0)
        {
            foreach (Transform enemies in PotentialEnemies)
            {
                Vector3 dir = (enemies.position - transform.position);
                Debug.Log("check : " + CheckSightLine(enemies.transform));
                if (CheckSightLine(enemies.transform))
                {
                    
                    // if True then add to another List of Targetable Enemies
                    if (!TargetableEnemies.Contains(enemies.transform))
                    {
                        TargetableEnemies.Add(enemies.transform);
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, dir, Color.red);
                    if (TargetableEnemies.Contains(enemies.transform))
                    {
                        TargetableEnemies.Remove(enemies.transform);
                    }
                    // if Not then remove this element of the List of Targetable Enemies
                }
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    private bool CheckSightLine(Transform target)
    {
        RaycastHit hit;
        Vector3 dir = (target.position - transform.position);
        if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.DrawRay(transform.position, dir, Color.yellow);
                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }

    }
}
