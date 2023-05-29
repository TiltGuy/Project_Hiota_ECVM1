using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionCameraPlayer))]
public class ActualizationCurrentTargetShader : MonoBehaviour
{
    [HideInInspector]
    public ActionCameraPlayer actionCameraPlayer;

    private void Awake()
    {
        actionCameraPlayer = GetComponent<ActionCameraPlayer>();
    }

    private void OnEnable()
    {
        actionCameraPlayer.OnNewCameraTarget += NewTarget;
        actionCameraPlayer.OnRemoveCameraTarget += RemoveTarget;
    }

    private void OnDisable()
    {
        actionCameraPlayer.OnNewCameraTarget -= NewTarget;
        actionCameraPlayer.OnRemoveCameraTarget -= RemoveTarget;
    }

    private void NewTarget(GameObject NewTarget_go)
    {
        if(NewTarget_go == null)
        {
            //Debug.LogError("ICI problème", this);
            return;
        }
        NewTarget_go.GetComponent<UpdateCanvasFocus>().AddTarget();
        // Change le shader 
        // Change la flèche de position
        //Debug.Log("Assign New Target = " + NewTarget_go.name);
    }

    private void RemoveTarget( GameObject LastTarget_go )
    {
        if ( LastTarget_go == null )
        {
            Debug.LogError("ICI problème", this);
            return;
        }
        LastTarget_go.GetComponent<UpdateCanvasFocus>().RemoveTarget();
        // Reset le shader de la target
        // Reset la position de la flèche et son alpha
        //Debug.Log("Remove Last Target = " + LastTarget_go.name);
    }
}
