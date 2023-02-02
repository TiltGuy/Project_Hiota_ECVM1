using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if(other.tag == "Player")
        {
            DataPersistenceManager.instance.saveCurrentMainDataSave();
            GameManager.instance.GoToNextLVL();
        }
    }
}
