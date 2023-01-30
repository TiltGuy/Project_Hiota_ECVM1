using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTuto : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if(other.CompareTag("Player"))
        {
            DataPersistenceManager.instance.saveCurrentTutoDataSave(true);
            GameManager.instance.GoToNextLVL(LevelManager.instance.DefineNextFightArena());
        }
    }
}
