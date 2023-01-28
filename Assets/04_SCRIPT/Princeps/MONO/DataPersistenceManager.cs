using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {

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
    }

    internal void TryToLoad()
    {
        LoadCurrentSave();
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

    public void TryToContinue()
    {
        if(currentDataToApply != null)
        {

        }
    }

    public void RespawnPlayer()
    {
        if(currentDataToApply == null) 
        {
            CharacterSpecs specs = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterSpecs>();
            currentDataToApply = new PlayerData(specs, DeckManager.instance);
        }
        if(!currentDataToApply.b_HasPassedTutorial)
        {
            // Lancer la coroutine respawn (nom de la scène)
            if(preloadingScene== null)
            {
                StartCoroutine(Respawn_Coroutine(tutoLevel));
            }
        }
        else
        {
            if(preloadingScene== null)
            {
                StartCoroutine(Respawn_Coroutine(hubLevel));
            }
        }
    }

    private IEnumerator Respawn_Coroutine(string nameTargetScene)
    {
        AsyncOperation preloadingScene = SceneManager.LoadSceneAsync(nameTargetScene);
        preloadingScene.allowSceneActivation = false;
        Debug.Log("reloading the scene : " + preloadingScene.progress);
        Camera.main.FadeOut(fadeDuration);
        //GetComponent<Controller_FSM>().gravity = 0;
        yield return new WaitForSecondsRealtime(fadeDuration);
        if ( loda == null )
        {
            loda = Instantiate(LoadingScreen);
            DontDestroyOnLoad(loda);
        }
        else
        {
            loda.gameObject.SetActive(true);
        }
        preloadingScene.allowSceneActivation = true;
        while ( !preloadingScene.isDone )
        {
            yield return null;
        }
        if ( loda != null )
        {
            loda.gameObject.SetActive(false);
        }
        LoadCurrentSave();
    }

    public IEnumerator PreLoadNextRandomRoom_Coroutine( string nextPalierRoomName )
    {
        AsyncOperation preloadingScene = SceneManager.LoadSceneAsync(nextPalierRoomName);
        bool b_IsPlayerReady = LevelManager.instance.b_IsPlayerReady;
        preloadingScene.allowSceneActivation = false;
        Debug.Log("Progress : " + preloadingScene.progress);
        while ( !preloadingScene.isDone )
        {
            //Debug.Log("Progress : " + preloadingScene.progress);

            if ( preloadingScene.progress >= .9f )
            {
                if ( b_IsPlayerReady )
                {
                    if ( loda == null )
                    {
                        loda = Instantiate(LoadingScreen);
                        DontDestroyOnLoad(loda);
                    }
                    else
                    {
                        loda.gameObject.SetActive(true);
                    }
                    preloadingScene.allowSceneActivation = true;
                    b_IsPlayerReady = false;
                }
            }
            yield return null;
        }
        if(loda != null)
        {
            loda.gameObject.SetActive(false);
        }
    }
    
    public void TryPreloadNextRandomScene( string nextPalierRoomName )
    {
        if(preloadingScene == null)
        {
            StartCoroutine(PreLoadNextRandomRoom_Coroutine(nextPalierRoomName));
        }
        else
        {
            Debug.Log("Preloading Scene failed cuz not empty" + preloadingScene.progress);
        }
    }

    
}
