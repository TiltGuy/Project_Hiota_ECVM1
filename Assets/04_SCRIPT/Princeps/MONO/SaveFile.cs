using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    public void SaveOnExit()
    {
        CharacterSpecs charSpecs = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSpecs>();
        SaveSystem.SavePlayerData(
            charSpecs, 
            DeckManager.instance, 
            SaveSystem.MainSaveFileName);
    }
}
