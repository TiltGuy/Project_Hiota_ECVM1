using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public string firstLevel;

	private InputMaster action;

	public GameObject pauseMenu, optionsWindow;

    private void Awake()
    {
		action = new InputMaster();
	}

    private void Start()
    {
		action.UI.Cancel.started += ctx => CloseOptions();
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
		SceneManager.LoadScene(firstLevel);
	}

	public void OpenOptions()
	{
		optionsWindow.SetActive(true);
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

	
}
