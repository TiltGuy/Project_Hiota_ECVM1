using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if(other.tag == "Player")
        {
            if(DataPersistenceManager.instance!=null && GameManager.instance!=null)
            {
                DataPersistenceManager.instance.saveCurrentMainDataSave();
                GameManager.instance.GoToNextLVL();
            }
            else
            {
                Debug.Log("TELEPORTAION!!!");
            }
        }
    }
}
