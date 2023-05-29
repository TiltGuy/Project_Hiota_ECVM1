using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    //static public int debugArenaIndex;

    [Range(0, 5)]
    public int minIndex = 0;
    [Range(0, 5)]
    public int maxIndex = 5;

    public bool b_CanSpawnAfterEndWorld = false;

    static public int debugArenaIndex
    {

        get
        {
            return PlayerPrefs.GetInt("debugArenaIndex");
        }

        set
        {
            PlayerPrefs.SetInt("debugArenaIndex", value );
        }
    }

    static public bool debugPlay
    {
        get
        {
            return PlayerPrefs.GetInt("DebugPlay") == 1;
        }

        set
        {
            PlayerPrefs.SetInt("DebugPlay", value ? 1 : 0);
        }
    }

    [ContextMenu("Debug Play")]
    public void LaunchDebugPlay()
    {
        debugPlay = true;
    }

    public void Start()
    {
        var arenaIndex = debugPlay || GameManager.instance == null ? debugArenaIndex : 
            GameManager.instance.ArenaIndex;
        gameObject.SetActive(arenaIndex >= minIndex && arenaIndex <= maxIndex );
        if(arenaIndex > maxIndex && b_CanSpawnAfterEndWorld)
        {
            gameObject.SetActive(true);
        }
        debugPlay = false;
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
Gizmos.DrawIcon(transform.position, "RoomObject");
//Gizmos.color = Color.blue;
//Gizmos.DrawSphere(transform.position, .5f);
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, minIndex + "-" + maxIndex);
        if(debugPlay)
        {
            UnityEditor.EditorApplication.isPlaying = true;
        }
#endif
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(RoomObject))]
public class RoomObjectEditor : UnityEditor.Editor
{
    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginVertical("box");
        RoomObject.debugArenaIndex = UnityEditor.EditorGUILayout.IntSlider("Debug Index", RoomObject.debugArenaIndex, 0, 5);
        if(GUILayout.Button("DEBUG PLAY", GUILayout.ExpandWidth(false)))
        {
            RoomObject.debugPlay = true;
        }
        GUILayout.EndVertical();
    }
}
#endif
