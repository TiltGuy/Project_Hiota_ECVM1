using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if(other.tag == "Player")
        {
            //LevelManager.instance.LoadNextLevel();
            LevelManager.instance.b_IsPlayerReady = true;
        }
    }
}
