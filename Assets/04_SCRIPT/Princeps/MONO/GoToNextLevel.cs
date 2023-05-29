﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextLevel : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if(other.tag == "Player")
        {
            if(GameManager.instance!=null)
            {
                DataPersistentManager.instance.saveCurrentMainDataSave();
                GameManager.instance.GoToNextLVL();
            }
            else
            {
                Debug.Log("TELEPORTAION!!!");
                //GameManager.instance.GoToNextLVL();
            }
        }
    }
}
