using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[InitializeOnLoad]
public class MoveSelectionToCursor
{
    static private List<Object> moveSelection = new List<Object>();

    [MenuItem("Tools/Move selection under mouse #3")]
    static public void DoMoveSelection()
    {
        if(moveSelection.Count > 0)
        {
            moveSelection.Clear();
        }
        else
        {
            moveSelection.AddRange(Selection.gameObjects);
        }
    }

    static MoveSelectionToCursor()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static private void OnSceneGUI(SceneView sceneView)
    {
        if(moveSelection.Count > 0)
        {
            var sceneCam = sceneView.camera;
            var mousePosition = Event.current.mousePosition;
            mousePosition.y = sceneView.position.height - mousePosition.y;
            var mouseRay = sceneCam.ScreenPointToRay(mousePosition);
            var hits = Physics.RaycastAll(mouseRay, Mathf.Infinity);
            foreach(RaycastHit hit in hits)
            {
                if(!moveSelection.Contains(hit.collider.gameObject))
                {
                    foreach (GameObject go in Selection.gameObjects)
                    {
                        go.transform.position = hit.point;
                    }

                    break;
                }
            }

            moveSelection.Clear();
        }
    }
}