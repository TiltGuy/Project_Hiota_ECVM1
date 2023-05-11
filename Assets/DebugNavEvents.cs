using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using FMOD;
using FMODUnity;
using Debug = UnityEngine.Debug;



public class DebugNavEvents:MonoBehaviour, ISelectHandler, ISubmitHandler
{


    #region -- SOUND STUFFS --
    [Header("SOUND STUFFS")]
    [SerializeField] private EventReference MoveSound_ER;
    [SerializeField] private EventReference Submit_ER;


    #endregion


    public void OnSelect( BaseEventData eventData )
    {
        RuntimeManager.PlayOneShot(MoveSound_ER, Camera.main.transform.position);
    }

    public void OnSubmit( BaseEventData eventData )
    {
        RuntimeManager.PlayOneShot(Submit_ER, Camera.main.transform.position);
    }
}
