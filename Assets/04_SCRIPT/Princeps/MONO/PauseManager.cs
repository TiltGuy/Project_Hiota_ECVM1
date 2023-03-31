using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class PauseManager : MonoBehaviour
{
    public static bool b_IsPaused = false;

    private InputMaster action;

    [SerializeField]
    private Transform Menu;
    [SerializeField]
    private Transform SettingsMenu;

    /*public GameObject pausePanel;
    public GameObject shortcutPanel;
    private bool leftShortcut = true;
    private bool rightShortcut = true;

    public GameObject keyboard, controller;
    private bool leftWindowSwitched = true;
    private bool rightWindowSwitched = true;*/
    public bool b_IsChoosingCard = false;

    private void Awake()
    {
        b_IsPaused = false;
        action = new InputMaster();
    }

    private void Start()
    {
        action.UI.Cancel.started += ctx => HideSettings();
        action.UI.Pause.performed += _ => DetermineGamePauseStatut();
        //Debug.Log(b_IsPaused);

        //action.UI.SwitchWindow.started += ctx => SwitchLeftWindow();
        //action.UI.SwitchWindow.started += ctx => SwitchRightWindow();

        //action.UI.SwitchShortcut.started += ctx => SwitchLeftShortcut();
        //action.UI.SwitchShortcut.started += ctx => SwitchRightShortcut();
        ResumeGame();
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
        if(b_IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        if ( !b_IsChoosingCard )
        {
            Time.timeScale = 0;
        }
        Menu.gameObject.SetActive(true);
        HideSettings();
        b_IsPaused = true;
        GetComponentsInChildren<Selectable>().First().Select();
    }

    public void ResumeGame()
    {
        if(!b_IsChoosingCard)
        {
            Time.timeScale = 1;
        }
        b_IsPaused = false;
        HideSettings();
        Menu.gameObject.SetActive(false);
        print("ResumeGame");
    }

    public void ShowSettings()
    {
        GameObject Menu = SettingsMenu.gameObject;
        if (!Menu.activeInHierarchy)
        {
            Menu.SetActive(true);
        }
    }

    public void HideSettings()
    {
        GameObject Menu = SettingsMenu.gameObject;
        if (Menu.activeInHierarchy)
        {
            Menu.SetActive(false);
        }
    }

    /*private void SwitchLeftWindow()
    {

        if ( leftWindowSwitched )
        {
            pausePanel.SetActive(false);
            shortcutPanel.SetActive(true);

            leftWindowSwitched = !leftWindowSwitched;
        }
        else if ( !leftWindowSwitched )
        {
            pausePanel.SetActive(true);
            shortcutPanel.SetActive(false);

            leftWindowSwitched = !leftWindowSwitched;
        }
    }
    private void SwitchRightWindow()
    {
        if ( rightWindowSwitched )
        {
            pausePanel.SetActive(false);
            shortcutPanel.SetActive(true);

            rightWindowSwitched = !rightWindowSwitched;
        }
        else if ( !rightWindowSwitched )
        {
            pausePanel.SetActive(true);
            shortcutPanel.SetActive(false);

            rightWindowSwitched = !rightWindowSwitched;
        }

    }

    private void SwitchLeftShortcut()
    {
        if ( leftShortcut )
        {
            controller.SetActive(false);
            keyboard.SetActive(true);

            leftShortcut = !leftShortcut;
        }
        else if ( !leftShortcut )
        {
            controller.SetActive(true);
            keyboard.SetActive(false);

            leftShortcut = !leftShortcut;
        }
    }

    private void SwitchRightShortcut()
    {
        if ( rightShortcut )
        {
            controller.SetActive(false);
            keyboard.SetActive(true);

            rightShortcut = !rightShortcut;
        }
        else if ( !rightShortcut )
        {
            controller.SetActive(true);
            keyboard.SetActive(false);

            rightShortcut = !rightShortcut;
        }
    }*/

    public void LoadMenu()
    {
        ResumeGame();
        if(GameManager.instance !=  null)
        {
            GameManager.instance.GoToMainMenu();
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
