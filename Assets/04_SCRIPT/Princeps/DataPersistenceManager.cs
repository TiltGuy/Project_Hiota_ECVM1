using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{

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
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {

        LoadCurrentSave();
    }

    private void LoadCurrentSave()
    {
        ApplySaveData();
    }

    private void ApplySaveData()
    {
        PlayerData CurrentSave = SaveSystem.LoadPlayerData();
        if ( CurrentSave != null )
        {
            DeckManager deckManager = DeckManager.instance;

            deckManager._PlayerDeck_SOList = new List<SkillCard_SO>();
            deckManager._PlayerDeck_SOList = CurrentSave._PlayerDeck;

            deckManager._HiddenDeck_SOList = new List<SkillCard_SO>();
            deckManager._HiddenDeck_SOList = CurrentSave._HiddenDeck;
        }
        else
        {
            Debug.LogWarning("SaveFile cannot be Loaded because it's Empty", this);
        }
    }
}
