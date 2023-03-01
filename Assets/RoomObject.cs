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

    public void Start()
    {
        if ( GameManager.instance == null )
            return;
        var arenaIndex = GameManager.instance.ArenaIndex;
        gameObject.SetActive(arenaIndex >= minIndex && arenaIndex <= maxIndex);
    }

    void OnDrawGizmos()
    {
#if UNITY_EDITOR
//Gizmos.color = Color.blue;
//Gizmos.DrawSphere(transform.position, .5f);
        UnityEditor.Handles.Label(transform.position, minIndex + "-" + maxIndex);
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