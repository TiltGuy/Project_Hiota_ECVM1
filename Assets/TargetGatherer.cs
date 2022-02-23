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
                PotentialEnemies.Remove(other.transform);
                TargetableEnemies.Remove(other.transform);
            }
        }
    }

    private void Update()
    {
        if(PotentialEnemies.Count >0)
        {
            foreach (Transform enemies in PotentialEnemies)
            {
                Vector3 dir = (enemies.position - controller.eyes.position);
                if(CheckSightLine(enemies.transform))
                {
                    Debug.DrawRay(controller.eyes.position, dir, Color.yellow);
                    // if True then add to another List of Targetable Enemies
                    if(!TargetableEnemies.Contains(enemies.transform))
                    {
                        TargetableEnemies.Add(enemies.transform);
                    }
                }
                else
                {
                    if (TargetableEnemies.Contains(enemies.transform))
                    {
                        TargetableEnemies.Remove(enemies.transform);
                    }
                    // if Not then remove this element of the List of Targetable Enemies
                }
            }
        }
    }

    private bool CheckSightLine(Transform target)
    {
        RaycastHit hit;
        Vector3 dir = (target.position - controller.eyes.position);
        if (Physics.Raycast(controller.eyes.position, dir, out hit, 1000, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
