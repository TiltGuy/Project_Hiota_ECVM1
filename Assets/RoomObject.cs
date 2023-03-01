using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    //static public int debugArenaIndex;

    [Range(0, 12)]
    public int minIndex = 0;
    [Range(0, 12)]
    public int maxIndex = 12;
    [Range(0, 12)]
    public int debugArenaIndex = 0;

    [ContextMenu("Debug Play")]
    public void LaunchDebugPlay()
    {
        debugPlay = true;
    }

    public bool debugPlay
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

    public void Start()
    {
        var arenaIndex = debugPlay || GameManager.instance == null ? debugArenaIndex : GameManager.instance.ArenaIndex;
        gameObject.SetActive(arenaIndex >= minIndex && arenaIndex <= maxIndex);
        debugPlay = false;
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
//Gizmos.color = Color.blue;
//Gizmos.DrawSphere(transform.position, .5f);
        UnityEditor.Handles.Label(transform.position, minIndex + "-" + maxIndex);
        if(debugPlay)
        {
            UnityEditor.EditorApplication.isPlaying = true;
        }
#endif
    }
}
/*
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(RoomObject))]
public class RoomObjectEditor : UnityEditor.Editor
{
    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();

       // RoomObject.debugArenaIndex = UnityEditor.EditorGUILayout.IntField("Debug",RoomObject.debugArenaIndex);
       // (target as RoomObject).Update();
    }
}
#endif
*/