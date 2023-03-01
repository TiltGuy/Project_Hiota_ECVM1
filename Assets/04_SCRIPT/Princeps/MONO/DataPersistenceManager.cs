using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    public bool isRespawning;

    public float fadeDuration = 5f;

    [SerializeField]
    public Canvas LoadingScreen;
    private Canvas loda;

    AsyncOperation preloadingScene = null;

    public bool b_IsInTuto;
    [Header("-- SAVES NAMES --")]
    public string mainSaveName;
    public string tutoSaveName;

    internal PlayerData currentDataToApply;


    public static DataPersistenceManager instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Data Persistence Manager already existed");
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        currentDataToApply = SaveSystem.LoadPlayerData();
        //Debug.Log(currentDataToApply);
        //LoadCurrentSave();
    }

    [ContextMenu("LoadSave")]
    private void LoadCurrentSave()
    {
        currentDataToApply = SaveSystem.LoadPlayerData();
        if ( currentDataToApply != null)
        {
            ApplySaveData(currentDataToApply);
            Debug.Log("Lance le applyData");
        }
        else
        {
            Debug.Log("No Save File Found... ");
        }
    }

    internal void TryToLoad()
    {
        LoadCurrentSave();
    }

    internal void saveCurrentMainDataSave()
    {
        if ( currentDataToApply == null )
        {
            CharacterSpecs player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSpecs>();
            currentDataToApply = new PlayerData(player, DeckManager.instance);
        }
        currentDataToApply._DeckManagerState = DeckManager.instance;
        SaveSystem.SavePlayerData(currentDataToApply);
        currentDataToApply._PlayerDeck = currentDataToApply._DeckManagerState._PlayerDeck.ToList();
        currentDataToApply._HiddenDeck = currentDataToApply._DeckManagerState._HiddenDeck.ToList();
    }

    internal void saveCurrentTutoDataSave( bool TutoStatus, Vector3 tutoPosition )
    {
        if ( currentDataToApply == null )
        {
            CharacterSpecs player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSpecs>();
            currentDataToApply = new PlayerData(player, DeckManager.instance);
        }
        currentDataToApply.b_HasPassedTutorial = TutoStatus;
        currentDataToApply.tutoPosition = tutoPosition;
        Debug.Log("tutoPosition" + currentDataToApply.tutoPosition);
        SaveSystem.SavePlayerData(currentDataToApply);
    }

    internal void saveCurrentTutoDataSave( bool TutoStatus )
    {
        if ( currentDataToApply == null )
        {
            CharacterSpecs player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSpecs>();
            currentDataToApply = new PlayerData(player, DeckManager.instance);
        }
        currentDataToApply.b_HasPassedTutorial = TutoStatus;
        SaveSystem.SavePlayerData(currentDataToApply);
    }

    private static void ApplySaveData( PlayerData CurrentSave)
    {
        if(!CurrentSave.b_HasPassedTutorial && GameObject.FindGameObjectWithTag("Respawn") != null)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 newposition = CurrentSave.tutoPosition;
            player.transform.position = newposition;
            int securityNumber = 0;
            while (player.transform.position != newposition && securityNumber < 1000)
            {
                player.transform.position = newposition;
                Debug.Log("Trying to put player at the good position");
                Debug.Log("tutoPosition" + newposition);
                Debug.Log("playerPosition" + player.transform.position);
                securityNumber++;
            }
            return;
        }
        DeckManager deckManager = DeckManager.instance;

        deckManager._PlayerDeck = new List<SkillCard_SO>();
        deckManager._PlayerDeck = CurrentSave._PlayerDeck;

        deckManager._HiddenDeck = new List<SkillCard_SO>();
        deckManager._HiddenDeck = CurrentSave._HiddenDeck;
    }

    public void TryClearData()
    {
        if ( currentDataToApply == null )
            return; 
        currentDataToApply.b_HasPassedTutorial = false;
        currentDataToApply._PlayerDeck?.Clear();
        currentDataToApply._HiddenDeck?.Clear();


    }
    

    
}
