using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if(other.CompareTag("Player"))
        {
            CharacterSpecs tempSpecs = other.GetComponent<CharacterSpecs>();
            tempSpecs.OnSomethingKilledMe();
            //Scene scene = SceneManager.GetActiveScene();
            //SceneManager.LoadScene(scene.name);
        }
    }
}
