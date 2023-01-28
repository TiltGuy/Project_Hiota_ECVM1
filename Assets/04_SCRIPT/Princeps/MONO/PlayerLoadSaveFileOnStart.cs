using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadSaveFileOnStart : MonoBehaviour
{
    private DataPersistenceManager currentInstance;

    void Start()
    {
        if(DataPersistenceManager.instance!= null)
        {
            currentInstance = DataPersistenceManager.instance;
            currentInstance.TryToLoad();
        }
    }
}
