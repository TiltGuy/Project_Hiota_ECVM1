using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour
{
#if UNITY_EDITOR
[UnityEditor.MenuItem("Tools/Launch Game")]
static public void LaunchGame()
{
new GameObject("GameLauncher").AddComponent<GameLauncher>();
        UnityEditor.EditorApplication.isPlaying = true;
}
#endif

    public int arenaIndex = -1;

    void Start()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = true;
        hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(0);
#endif
    }
}
