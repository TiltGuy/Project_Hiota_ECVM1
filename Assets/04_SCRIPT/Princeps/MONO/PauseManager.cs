using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using FMODUnity;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public static bool b_IsPaused = false;

    private InputMaster action;

    public Collection collection;

    public Shop shop;

    [SerializeField]
    private OpenMenu openMenuScript;

    public Transform Menu;
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

    public delegate void OnCastDelegateBool(bool b);
    public OnCastDelegateBool OnPaused;

    private void Awake()
    {
        b_IsPaused = false;
        action = new InputMaster();
    }

    private void Start()
    {
        //Debug.Log(b_IsPaused);

        //action.UI.SwitchWindow.started += ctx => SwitchLeftWindow();
        //action.UI.SwitchWindow.started += ctx => SwitchRightWindow();

        //action.UI.SwitchShortcut.started += ctx => SwitchLeftShortcut();
        //action.UI.SwitchShortcut.started += ctx => SwitchRightShortcut();
        ResumeGame();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController_Animator animator = player.GetComponent<PlayerController_Animator>();
        animator.RespawnPlayer();
    }

    private void OnEnable()
    {
        action.Enable();
        action.UI.Cancel.started += ctx => HideSettings();
        action.UI.Pause.performed += _ => DetermineGamePauseStatut();
    }

    private void OnDisable()
    {
        action.Disable();
        action.UI.Pause.performed -= _ => DetermineGamePauseStatut();
        action.UI.Cancel.started -= ctx => HideSettings();
    }

    void DetermineGamePauseStatut()
    {
        if(collection.cardCanvas.gameObject.activeInHierarchy)
        {
            collection.cardCanvas.gameObject.SetActive(false);
            openMenuScript.b_IsOpen = false;
            return;
        }
        if(shop != null)
        {
            if ( shop.ShopPanel_GO.gameObject.activeInHierarchy )
            {
                shop.ToggleShopCanvas();
                return;
            }
        }
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
        OnPaused?.Invoke(b_IsPaused);

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
        OnPaused?.Invoke(b_IsPaused);
        //print("ResumeGame");
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

    public void LoadMenu()
    {
        ResumeGame();
        if(GameManager.instance !=  null)
        {
            GameManager.instance.GoToMainMenu();
            Destroy(this.gameObject);
        }
    }

    public void DestroyMySelf()
    {
        Destroy(gameObject);
    }
}
