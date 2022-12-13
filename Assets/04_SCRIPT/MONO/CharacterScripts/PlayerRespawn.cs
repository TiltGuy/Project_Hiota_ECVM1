using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    static public bool isRespawning;

    public float fadeDuration = 3f;

    public void Respawn()
    {
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

        // reload level
        var activeScene = SceneManager.GetActiveScene();
        //var asyncOp = SceneManager.LoadSceneAsync(activeScene.name);
        SceneManager.LoadScene(activeScene.name);
        //while (!asyncOp.isDone)
        //{
        //    Debug.Log(asyncOp.progress);
        //    yield return null;
        //}
    }

    private IEnumerator Start()
    {
        isRespawning = true;

        Camera.main.FadeIn(fadeDuration);

        CheckpointTrigger lastCheckpoint = null;

        foreach ( CheckpointTrigger checkpoint in CheckpointTrigger.instances.OrderBy(checkpoint => checkpoint.checkpointIndex) )
        {
            if ( checkpoint.SetCurrentCheckpoint() )
            {
                lastCheckpoint = checkpoint;
                yield return null;
            }
        }

        if ( lastCheckpoint != null )
        {
            //Debug.Log("Reload checkpoint: " + lastCheckpoint.checkpointIndex);
            transform.position = lastCheckpoint.respawnTarget.position;
        }

        yield return new WaitForSeconds(fadeDuration);

        isRespawning = false;
    }
}
