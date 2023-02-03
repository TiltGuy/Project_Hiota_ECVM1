using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Scene currentScene;
    private int arenaIndex;
    public int ArenaIndex
    {
        get => arenaIndex;
    }
    public string nextSceneBuildName;

    [Header("-- MAIN MENU --")]

    public string mainMenu;

    [Header("-- DEPENDENCIES --")]

    public GameObject LoadingScreen_GO;
    public Slider LoadingProgress_Bar;

    [Header("-- SCENES SETTINGS --")]

    public string prefix_StandardArena;
    public string prefix_PalierArena;

    public List<string> baseListOfScenes = new List<string>();

    private List<string> currentlistOfScenes = new List<string>();


    public static GameManager instance;

    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)ScenesIndexes.MAINMENU, LoadSceneMode.Additive);
    }

    private void Start()
    {
        currentScene = GetOtherSceneNonActive();
        SceneManager.SetActiveScene(currentScene);
        //Debug.Log(currentScene);
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void StartNewGame()
    {
        #region -- Delete All Saves --

        PlayerPrefs.DeleteAll();
        SaveSystem.ClearData(SaveSystem.MainSaveFileName);
        DataPersistenceManager.instance.TryClearData();

        #endregion

        LoadingScreen_GO.SetActive(true);

        //Debug.Log(GetOtherSceneNonActive().name);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)ScenesIndexes.MAINMENU));
        AsyncOperation newScene = SceneManager.LoadSceneAsync((int)ScenesIndexes.TUTORIAL, LoadSceneMode.Additive);
        scenesLoading.Add(newScene);
        StartCoroutine(GetScenesLoadProgress_Coroutine());
        //Debug.Log("LoadingSceneDone");
    }

    public void ContinueGame()
    {
        if(!File.Exists(SaveSystem.GetPath(SaveSystem.MainSaveFileName)))
        {
            return;
        }
        scenesLoading.Clear();
        scenesLoading = new List<AsyncOperation>();
        LoadingScreen_GO.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)ScenesIndexes.MAINMENU));
        if(DataPersistenceManager.instance.currentDataToApply != null )
        {
            PlayerData playerData = DataPersistenceManager.instance.currentDataToApply;
            if(!playerData.b_HasPassedTutorial)
            {
                scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.TUTORIAL, LoadSceneMode.Additive));
                
                Debug.Log("---- Current Scene Name = " + GetOtherSceneNonActive().name);
                Debug.Log("---- Current Scene Handle = " + GetOtherSceneNonActive().handle);
            }
            else
            {

                scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.HUB, LoadSceneMode.Additive));
                Debug.Log("Launch Hub Scene");
            }
        }
        StartCoroutine(GetScenesLoadProgress_Coroutine());
    }

    public void RespawnPlayer()
    {
        //if(scenesLoading.Count>0)
        //{
        //    scenesLoading.Clear();
        //}
        scenesLoading = new List<AsyncOperation>();
        ResetCurrentListOfScenes();
        LoadingScreen_GO.SetActive(true);
        if ( DataPersistenceManager.instance.currentDataToApply != null )
        {
            PlayerData playerData = DataPersistenceManager.instance.currentDataToApply;
            if ( !playerData.b_HasPassedTutorial )
            {
                //Debug.Log("----- Want to respawn from " + currentScene.name);
                scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));

                scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.TUTORIAL, LoadSceneMode.Additive));
                
                Debug.Log("----- New Scene is  " + currentScene.name);
                Debug.Log("---- Current Scene Handle = " + currentScene.handle);
            }
            else
            {
                //Debug.Log("----- Want to respawn from " + currentScene.name);
                scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
                scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.HUB, LoadSceneMode.Additive));
                LevelManager.currentRoomIndex= 0;
                arenaIndex = 0;
                DataPersistenceManager.instance.saveCurrentMainDataSave();
            }
        }
        StartCoroutine(GetScenesLoadProgress_Coroutine());
    }


    [ContextMenu("Go To Next LVL")]

    public void GoToNextLVL()
    {
        //if(scenesLoading.Count>0)
        //{
        //    scenesLoading.Clear();
        //}
        scenesLoading = new List<AsyncOperation>();
        ResetCurrentListOfScenes();

        LoadingScreen_GO.SetActive(true);

        //Debug.Log(GetCurrentScene().name);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
        arenaIndex++;
        string currentPrefix = prefix_StandardArena;
        if ( arenaIndex % 3 == 0 )
        {
            currentPrefix = prefix_PalierArena;
        }
        string nextScene = PickSceneRandomly(currentPrefix);
        Debug.Log(nextScene);
        currentlistOfScenes.Remove(nextScene);
        AsyncOperation newScene = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        scenesLoading.Add(newScene);
        StartCoroutine(GetScenesLoadProgress_Coroutine());
        //Debug.Log("LoadingSceneDone");
    }

    private string PickSceneRandomly( string currentPrefix )
    {
        string sceneName = currentlistOfScenes.OrderBy(s => Random.Range(0f, 1f)).FirstOrDefault(s => s.StartsWith(currentPrefix));
        Debug.Log("sceneName = " + sceneName);
        if(sceneName== null)
        {
            sceneName = currentlistOfScenes.OrderBy(s=> Random.Range(0f,1f)).First();
        }
        
        return sceneName;
    }

    private void ResetCurrentListOfScenes()
    {
        if ( currentlistOfScenes.Count == 0 )
        {
            currentlistOfScenes = baseListOfScenes.ToList();
        }
    }

    float totalScenesLoadingProgress;

    

    private IEnumerator GetScenesLoadProgress_Coroutine()
    {
        foreach(AsyncOperation loadingScene in scenesLoading)
        {
            while(!loadingScene.isDone)
            {
                totalScenesLoadingProgress = 0;
                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalScenesLoadingProgress += operation.progress;
                }

                totalScenesLoadingProgress = (totalScenesLoadingProgress / scenesLoading.Count) * 100f;
                //Debug.Log("totalSceneLoadingProgress : " + totalScenesLoadingProgress);
                
                if(LoadingProgress_Bar != null)
                {
                    LoadingProgress_Bar.value = Mathf.RoundToInt( totalScenesLoadingProgress);
                }

                yield return null;
            }
        }
        DataPersistenceManager.instance.TryToLoad();
        scenesLoading.Clear();
        LoadingScreen_GO.SetActive(false);
        //Debug.Log("Current Scene Active = " + GetOtherSceneNonActive().name);
        SceneManager.SetActiveScene(GetOtherSceneNonActive());
        //Debug.Log("----- New Scene is  " + currentScene.name);
        //Debug.Log("---- Current Scene Handle = " + currentScene.handle);
    }

    [ContextMenu("GetCurrentSceneHandleNdName")]
    public Scene GetSceneByHandle(int handleToTest)
    {
        if ( SceneManager.sceneCount > 0 )
        {
            for ( int n = 0; n < SceneManager.sceneCount; ++n )
            {
                Scene sceneToExam = SceneManager.GetSceneAt(n);
                if(sceneToExam.handle == handleToTest )
                {
                    //Debug.Log(sceneToExam.name);
                    Debug.Log("SceneManager.sceneCount = " + SceneManager.sceneCount);
                    return sceneToExam;
                }
            }
        }
        return SceneManager.GetActiveScene();
    }


    [ContextMenu("ReturnCurrentScene")]
    public Scene GetOtherSceneNonActive()
    {
        if ( SceneManager.sceneCount > 0 )
        {
            for ( int n = 0; n < SceneManager.sceneCount; ++n )
            {
                Scene sceneToExam = SceneManager.GetSceneAt(n);
                if ( sceneToExam != SceneManager.GetActiveScene() )
                {
                    //Debug.Log(sceneToExam.name);
                    return sceneToExam;
                }
            }
        }
        Debug.Log("Number of Scenes = " + SceneManager.sceneCount);
        return SceneManager.GetActiveScene();
    }
}
