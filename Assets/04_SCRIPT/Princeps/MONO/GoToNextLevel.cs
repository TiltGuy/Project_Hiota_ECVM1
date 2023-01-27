using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{
    public bool b_SaveTutoState = false;
    public bool b_HaveToSave = false;

    private void OnTriggerEnter( Collider other )
    {
        if(other.tag == "Player")
        {
            //LevelManager.instance.LoadNextLevel();
            LevelManager.instance.b_IsPlayerReady = true;
            if( b_HaveToSave == true)
            {
                if(b_SaveTutoState)
                {
                    DataPersistenceManager.instance.saveCurrentTutoDataSave(false);
                }
                else
                {
                    DataPersistenceManager.instance.saveCurrentMainDataSave();
                }
            }
        }
    }
}
