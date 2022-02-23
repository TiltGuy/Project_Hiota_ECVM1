using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGatherer : MonoBehaviour
{
    public List<Transform> PotentialEnemies;

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
            }
        }
    }
}
