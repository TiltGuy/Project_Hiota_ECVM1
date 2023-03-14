﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTuto : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if(other.CompareTag("Player"))
        {
            DataPersistentManager.instance.saveCurrentTutoDataSave(true);
            DeckManager.Destroy(DeckManager.instance);
            GameManager.instance.GoToNextLVL();
        }
    }
}