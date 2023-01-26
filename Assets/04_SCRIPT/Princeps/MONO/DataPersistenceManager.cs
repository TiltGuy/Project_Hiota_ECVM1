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
        currentDataToApply = SaveSystem.LoadPlayerData();
        if ( currentDataToApply != null)
        {
            ApplySaveData(currentDataToApply);
        }
    }

    private static void ApplySaveData( PlayerData CurrentSave)
    {
        if(!CurrentSave.b_HasPassedTutorial && GameObject.FindGameObjectWithTag("Respawn") != null)
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
        Debug.Log(asyncOperation.progress);
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
