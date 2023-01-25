using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAllCheckpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if(other.CompareTag("Player"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
