using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public static bool b_IsPaused = false;

    private InputMaster action;

    [SerializeField]
    private Transform Menu;
    [SerializeField]
    private Transform SettingsMenu;

    private void Awake()
    {
        action = new InputMaster();
    }

    private void Start()
    {
        action.UI.Cancel.started += ctx => HideSettings();
        action.UI.Pause.performed += _ => DetermineGamePauseStatut();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    void DetermineGamePauseStatut()
    {
        if ( !b_IsPaused )
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        Menu.gameObject.SetActive(true);
        HideSettings();
        b_IsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        b_IsPaused = false;
        HideSettings();
        Menu.gameObject.SetActive(false);
        print("ResumeGame");
    }

    public void ShowSettings()
    {
        GameObject Menu = SettingsMenu.gameObject;
        if ( !Menu.activeInHierarchy )
        {
            Menu.SetActive(true);
        }
    }

    public void HideSettings()
    {
        GameObject Menu = SettingsMenu.gameObject;
        if ( Menu.activeInHierarchy )
        {
            Menu.SetActive(false);
        }
    }

    public void LoadMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(0);
        print("Go Main Menu");
    }
}
