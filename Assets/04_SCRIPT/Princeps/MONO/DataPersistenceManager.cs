using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{

    public bool b_IsInTuto;
    [Header("-- SAVES NAMES --")]
    public string mainSaveName;
    public string tutoSaveName;

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

    [ContextMenu("LoadSave")]
    private void LoadCurrentSave()
    {
        if(b_IsInTuto)
        {
            ChosseSaveData(tutoSaveName);
            return;
        }
        ChosseSaveData(mainSaveName);
    }

    private void ChosseSaveData(string nameFile)
    {
        PlayerData CurrentSave = SaveSystem.LoadPlayerData(nameFile);
        if ( CurrentSave != null )
        {
            ApplySaveData(CurrentSave, nameFile);
        }
        else
        {
            Debug.LogWarning("SaveFile cannot be Loaded because it's Empty", this);
        }
    }

    private static void ApplySaveData( PlayerData CurrentSave, string nameFile )
    {
        if(nameFile == DataPersistenceManager.instance.tutoSaveName )
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 newposition = new Vector3(CurrentSave.position[0], CurrentSave.position[1], CurrentSave.position[2]);
            player.transform.position = newposition;
            return;
        }
        DeckManager deckManager = DeckManager.instance;

        deckManager._PlayerDeck = new List<SkillCard_SO>();
        deckManager._PlayerDeck = CurrentSave._PlayerDeck;

        deckManager._HiddenDeck = new List<SkillCard_SO>();
        deckManager._HiddenDeck = CurrentSave._HiddenDeck;
    }
}
