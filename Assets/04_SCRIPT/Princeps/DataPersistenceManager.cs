using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private PlayerData data;

    public static DataPersistenceManager instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Data Persistence Manager already existed");
        }
        instance = this;
        LoadCurrentSave();
    }

    private void LoadCurrentSave()
    {
        PlayerData CurrentSave = SaveSystem.LoadPlayerData();

        DeckManager deckManager = DeckManager.instance;

        deckManager._PlayerDeck = new List<SkillCard_SO>();
        deckManager._PlayerDeck = CurrentSave._PlayerDeck;

        deckManager._HiddenDeck = new List<SkillCard_SO>();
        deckManager._HiddenDeck = CurrentSave._HiddenDeck;
    }
}
