using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public static class HiotaTools
{
    [MenuItem("Tools/Reset Player Prefs")]
    static public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Go Next Checkpoint")]
    static public void GoNextCheckpoint()
    {
        var player = GameObject.FindWithTag("Player");
        if ( player == null )
        {
            Debug.LogWarning("No player found!");
            return;
        }

        foreach ( CheckpointTrigger checkpoint in CheckpointTrigger.instances.OrderBy(i => i.checkpointIndex) )
        {
            if ( !checkpoint.isTriggered )
            {
                player.transform.position = checkpoint.respawnTarget.position;
                return;
            }
        }
    }
}
