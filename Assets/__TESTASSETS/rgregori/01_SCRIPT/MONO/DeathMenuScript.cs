using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject DeathPanel;

    private InputMaster actions;

    private GameObject refPlayer;

    private void Awake()
    {
        actions = new InputMaster();
        refPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        actions.Enable();
        if(refPlayer != null)
        {
            refPlayer.GetComponent<CharacterSpecs>().OnSomethingKilledMe += setMenu;
        }
    }

    private void OnDisable()
    {
        actions.Disable();
        if(refPlayer != null)
        {
            refPlayer.GetComponent<CharacterSpecs>().OnSomethingKilledMe -= setMenu;
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        print("Go Main Menu");
    }

    public void setMenu()
    {
        DeathPanel.SetActive(true);
    }
}
