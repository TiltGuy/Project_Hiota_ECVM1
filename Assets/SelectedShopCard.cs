using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedShopCard:MonoBehaviour, ISelectHandler
{
    public Shop shopScript;

    public void OnSelect( BaseEventData eventData )
    {
        shopScript.currentCardSelected = transform.GetComponentInParent<CardCollection>().currentSkillcard;
        //Debug.Log(transform.GetComponentInParent<CardCollection>().currentSkillcard, transform.GetComponentInParent<CardCollection>().gameObject);
    }
}
