using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedScroll : MonoBehaviour, ISelectHandler
{
    public void OnSelect( BaseEventData eventData )
    {
        Debug.Log(this.gameObject.name + " was selected");
    }
}
