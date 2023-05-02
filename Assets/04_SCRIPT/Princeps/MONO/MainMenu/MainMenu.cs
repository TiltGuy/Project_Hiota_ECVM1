using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [Header("-- SAVES NAMES --")]
    public string mainSaveName;
    public string tutoSaveName;

    [Header("-- LEVEL NAMES --")]
    public string tutoLevel;
	public string hubLevel;

    public string artMap;

    [Header("-- OTHER --")]

    private InputMaster action;

	public GameObject pauseMenu, optionsWindow;

    public GameObject keyboard, controller;

	private bool b_CursorInvisible = true;

    private bool leftWindowSwitched = true;

    private bool rightWindowSwitched = true;

    [SerializeField]
    private float timeForLaunchingGame = 1f;

    [SerializeField]
    private Animator Hiota_Animator;


    private void Awake()
    {
		action = new InputMaster();
	}

    private void Start()
    {
		action.UI.Cancel.started += ctx => CloseOptions();
		action.Player.DebugCursorBinding.started += ctx => HideCursor();


		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

        optionsWindow.SetActive(false);

        action.UI.SwitchShortcut.started += ctx => SwitchLeftWindow();
        action.UI.SwitchShortcut.started += ctx => SwitchRightWindow();
    }

    private void SwitchLeftWindow()
    {  

        if (leftWindowSwitched)
        {
            controller.SetActive(false);
            keyboard.SetActive(true);

            leftWindowSwitched = !leftWindowSwitched;
        }
        else if (!leftWindowSwitched)
        {
            controller.SetActive(true);
            keyboard.SetActive(false);

            leftWindowSwitched = !leftWindowSwitched;
        }
    }
    private void SwitchRightWindow()
    {

        if ( rightWindowSwitched )
        {
            controller.SetActive(false);
            keyboard.SetActive(true);

            rightWindowSwitched = !rightWindowSwitched;
        }
        else if ( !rightWindowSwitched )
        {
            controller.SetActive(true);
            keyboard.SetActive(false);

            rightWindowSwitched = !rightWindowSwitched;
        }
    }

    private void OnEnable()
    {
		action.Enable();
	}

    private void OnDisable()
    {
		action.Disable();
	}

	public void StartGame()
	{
        StartCoroutine(LaunchGame());
	}

    private IEnumerator LaunchGame()
    {
        Hiota_Animator.SetBool("b_LaunchAnimation", true);

        yield return new WaitForSeconds(timeForLaunchingGame);

        if(GameManager.instance != null)
        {
            GameManager.instance.StartNewGame();
        }
        else
        {
            Debug.Log("There isn't a Game Manager", this);
        }
    }

    public void Continue()
    {
        GameManager.instance.ContinueGame();
        
    }

    public void ArtMap()
    {
        SceneManager.LoadScene(artMap);
    }

	public void OpenOptions()
	{
        if(!optionsWindow)
        {
            optionsWindow.SetActive(true);
            Debug.Log("OptionsWindow open");
        }
	}

	public void CloseOptions()
	{
		if (optionsWindow)
		{
			optionsWindow.SetActive(false);
		}
		
	}

	public void QuitGame()
	{
		Application.Quit();
		Debug.Log("Quit");
	}

	private void HideCursor()
    {
		if (!b_CursorInvisible)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			b_CursorInvisible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			b_CursorInvisible = false;
		}
	}

	
}
