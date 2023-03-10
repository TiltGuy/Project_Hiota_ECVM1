using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadSaveFileOnStart : MonoBehaviour
{
    private DataPersistentManager currentInstance;

    void Start()
    {
        if(DataPersistentManager.instance!= null)
        {
            currentInstance = DataPersistentManager.instance;
            currentInstance.TryToLoad();
        }
    }
}
