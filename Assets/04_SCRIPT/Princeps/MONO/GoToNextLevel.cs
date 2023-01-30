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
            DataPersistenceManager.instance.saveCurrentMainDataSave();
            GameManager.instance.GoToNextLVL(LevelManager.instance.DefineNextFightArena());
        }
    }
}
