using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("-- MAIN MENU --")]

    public string mainMenu;

    [Header("-- LEVEL NAMES --")]
    public string tutoLevel;
    public string hubLevel;
    public string level01;
    public string level02;

    [Header("-- DEPENDENCIES --")]

    public GameObject LoadingScreen_GO;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;

        SceneManager.LoadSceneAsync(mainMenu, LoadSceneMode.Additive);
    }

    public void StartNewGame()
    {
        #region -- Delete All Saves --

        PlayerPrefs.DeleteAll();
        SaveSystem.ClearData(SaveSystem.MainSaveFileName);

        #endregion

        LoadingScreen_GO.SetActive(true);
        SceneManager.UnloadSceneAsync(mainMenu);
        SceneManager.LoadSceneAsync(tutoLevel, LoadSceneMode.Additive);
    }
}
