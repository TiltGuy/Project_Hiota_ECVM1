using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{

    public bool isRespawning;

    public float fadeDuration = 3f;

    [SerializeField]
    private Canvas LoadingScreen;

    

    public bool b_IsInTuto;
    [Header("-- SAVES NAMES --")]
    public string mainSaveName;
    public string tutoSaveName;

    [Header("-- LEVEL NAMES --")]
    public string tutoLevel;
    public string hubLevel;

    private PlayerData currentDataToApply;

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
            return;
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
        currentDataToApply = SaveSystem.LoadPlayerData();
        if ( currentDataToApply != null)
        {
            ApplySaveData(currentDataToApply);
        }
    }

    internal void saveCurrentMainDataSave()
    {
        currentDataToApply._DeckManagerState = DeckManager.instance;
        SaveSystem.SavePlayerData(currentDataToApply);
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
            Debug.Log("Tamere je change le layer d place");
            return;
        }
        DeckManager deckManager = DeckManager.instance;

        deckManager._PlayerDeck = new List<SkillCard_SO>();
        deckManager._PlayerDeck = CurrentSave._PlayerDeck;

        deckManager._HiddenDeck = new List<SkillCard_SO>();
        deckManager._HiddenDeck = CurrentSave._HiddenDeck;
    }

    public void RespawnPlayer()
    {
        if(!currentDataToApply.b_HasPassedTutorial)
        {
            // Lancer la coroutine respawn (nom de la scène)
            StartCoroutine(RespawnCoroutine(tutoLevel));
        }
        else
        {
            StartCoroutine(RespawnCoroutine(hubLevel));
        }
    }

    private IEnumerator RespawnCoroutine(string nameTargetScene)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nameTargetScene);
        asyncOperation.allowSceneActivation = false;
        Debug.Log("reloading the scene : " + asyncOperation.progress);
        Camera.main.FadeOut(fadeDuration);
        //GetComponent<Controller_FSM>().gravity = 0;
        yield return new WaitForSecondsRealtime(fadeDuration);
        Instantiate(LoadingScreen);
        asyncOperation.allowSceneActivation = true;
        while ( !asyncOperation.isDone )
        {
            yield return null;
        }
    }
}
