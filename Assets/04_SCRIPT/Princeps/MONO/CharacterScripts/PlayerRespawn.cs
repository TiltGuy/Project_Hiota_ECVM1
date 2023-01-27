using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public bool isRespawning;

    public float fadeDuration = 3f;

    public string hubSceneName;

    public void Respawn()
    {
        if ( DataPersistenceManager.instance != null )
        {
            DataPersistenceManager.instance.RespawnPlayer();
            return;
        }
        if ( !isRespawning )
        {
            isRespawning = true;
            StartCoroutine("RespawnCoroutine");
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        Camera.main.FadeOut(fadeDuration);
        //GetComponent<Controller_FSM>().gravity = 0;
        yield return new WaitForSecondsRealtime(fadeDuration);

        LevelManager.instance.LoadHub();
    }

    private IEnumerator Start()
    {
        isRespawning = true;

        Camera.main.FadeIn(fadeDuration);

        

        yield return new WaitForSeconds(fadeDuration);

        isRespawning = false;
    }
}
