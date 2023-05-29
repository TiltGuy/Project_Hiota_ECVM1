using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class GameLauncher : MonoBehaviour
{
    static public bool launchGame
    {
        get {
            return PlayerPrefs.GetInt("LaunchGame") == 1;
        }
        set
        {
            PlayerPrefs.SetInt("LaunchGame", value ? 1 : 0);
        }
    }

#if UNITY_EDITOR
[UnityEditor.MenuItem("Tools/Launch Game")]
static public void LaunchGame()
{
launchGame = true;
new GameObject("GameLauncher").AddComponent<GameLauncher>();
            UnityEditor.EditorApplication.isPlaying = true;
}
#endif

    void OnDrawGizmosSelected()
    {
        DestroyImmediate(gameObject);
    }

    void Update()
    {
        gameObject.hideFlags = HideFlags.DontSave;

        if ( Application.isPlaying )
        {
#if UNITY_EDITOR
            if(launchGame)
            {
                launchGame = false;
                SceneManager.LoadScene(0);
            }
#endif
        }

    }
}
