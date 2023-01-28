using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Scene currentScene;
    public int currentHandle;
    public string nextSceneBuildName;

    [Header("-- MAIN MENU --")]

    public string mainMenu;

    [Header("-- LEVEL NAMES --")]
    public string tutoLevel;
    public string hubLevel;
    public string level01;
    public string level02;

    [Header("-- DEPENDENCIES --")]

    public GameObject LoadingScreen_GO;
    public Slider LoadingProgress_Bar;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)ScenesIndexes.MAINMENU, LoadSceneMode.Additive);
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void StartNewGame()
    {
        #region -- Delete All Saves --

        PlayerPrefs.DeleteAll();
        SaveSystem.ClearData(SaveSystem.MainSaveFileName);

        #endregion

        LoadingScreen_GO.SetActive(true);

        Debug.Log(GetCurrentScene().name);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(GetCurrentScene()));
        AsyncOperation newScene = SceneManager.LoadSceneAsync((int)ScenesIndexes.TUTORIAL, LoadSceneMode.Additive);
        scenesLoading.Add(newScene);
        currentHandle = SceneManager.GetSceneByBuildIndex((int)ScenesIndexes.TUTORIAL).handle;
        StartCoroutine(GetScenesLoadProgress_Coroutine());
        Debug.Log("LoadingSceneDone");
    }

    public void ContinueGame()
    {

        LoadingScreen_GO.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(GetCurrentScene()));
        if(DataPersistenceManager.instance.currentDataToApply != null)
        {
            PlayerData playerData = DataPersistenceManager.instance.currentDataToApply;
            if(!playerData.b_HasPassedTutorial)
            {
                scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.TUTORIAL, LoadSceneMode.Additive));
                
                Debug.Log("---- Current Scene Name = " + currentScene.name);
                Debug.Log("---- Current Scene Handle = " + currentScene.handle);
            }
            else
            {
                scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.HUB, LoadSceneMode.Additive));
                Debug.Log("---- Current Scene Name = " + currentScene.name);
                Debug.Log("---- Current Scene Handle = " + currentScene.handle);
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

        LoadingScreen_GO.SetActive(true);
        if ( DataPersistenceManager.instance.currentDataToApply != null )
        {
            PlayerData playerData = DataPersistenceManager.instance.currentDataToApply;
            if ( !playerData.b_HasPassedTutorial )
            {
                //Debug.Log("----- Want to respawn from " + currentScene.name);
                scenesLoading.Add(SceneManager.UnloadSceneAsync(GetCurrentScene()));

                scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.TUTORIAL, LoadSceneMode.Additive));
                
                Debug.Log("----- New Scene is  " + currentScene.name);
                Debug.Log("---- Current Scene Handle = " + currentScene.handle);
            }
            else
            {
                //Debug.Log("----- Want to respawn from " + currentScene.name);
                Scene sceneToUnload;
                scenesLoading.Add(SceneManager.UnloadSceneAsync(GetCurrentScene()));
                scenesLoading.Add(SceneManager.LoadSceneAsync((int)ScenesIndexes.HUB, LoadSceneMode.Additive));
                Debug.Log("----- New Scene is  " + currentScene.name);
                Debug.Log("---- Current Scene Handle = " + currentScene.handle);
                DataPersistenceManager.instance.saveCurrentMainDataSave();
            }
        }
        StartCoroutine(GetScenesLoadProgress_Coroutine());
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
                Debug.Log("totalSceneLoadingProgress : " + totalScenesLoadingProgress);
                
                if(LoadingProgress_Bar != null)
                {
                    LoadingProgress_Bar.value = Mathf.RoundToInt( totalScenesLoadingProgress);
                }

                yield return null;
            }
        }
        DataPersistenceManager.instance.TryToLoad();
        scenesLoading.Clear();
        Debug.Log(SceneManager.GetActiveScene().name);
        LoadingScreen_GO.SetActive(false);
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
    public Scene GetCurrentScene()
    {
        if ( SceneManager.sceneCount > 0 )
        {
            for ( int n = 0; n < SceneManager.sceneCount; ++n )
            {
                Scene sceneToExam = SceneManager.GetSceneAt(n);
                if ( sceneToExam != SceneManager.GetActiveScene() )
                {
                    //Debug.Log(sceneToExam.name);
                    Debug.Log(sceneToExam.name);
                    return sceneToExam;
                }
            }
        }
        return SceneManager.GetActiveScene();
    }
}
