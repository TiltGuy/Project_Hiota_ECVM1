using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{

    public SaveFile saveFile;

    private void OnTriggerEnter( Collider other )
    {
        if(other.tag == "Player")
        {
            //LevelManager.instance.LoadNextLevel();
            LevelManager.instance.b_IsPlayerReady = true;
            saveFile.SaveOnExit();
        }
    }
}
