using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public string firstLevel;

	public GameObject pauseMenu, optionsWindow;


	/*private void Update()
	{
		if (Input.GetButtonDown("Fire3"))
		{
			PauseUnpause();
		}
	}

	public void PauseUnpause()
	{
		if (!pauseMenu.activeInHierarchy)
		{
			pauseMenu.SetActive(true);
			Time.timeScale = 0f;
		}
		else
		{
			pauseMenu.SetActive(false);
			Time.timeScale = 1f;
			optionsWindow.SetActive(false);
		}
	}*/
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
		optionsWindow.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
		Debug.Log("Quit");
	}

	
}
