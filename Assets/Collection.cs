using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Collection : MonoBehaviour
{
    public Canvas cardCanvas;
    public ListOfCards collection;
    public GameObject prefabCard;
    public GameObject panelDisplay;

    

    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay();
    }


    private void UpdateDisplay()
    {
        foreach (SkillCard_SO card in collection.ListCards)
        {
            GameObject clone = Instantiate(prefabCard, panelDisplay.transform);
            CardCollection cardScript = clone.GetComponent<CardCollection>();
            cardScript.AssignText(card);
            cardScript.refCollection = this;
            if(DeckManager.instance._PlayerDeck.Contains(card))
            {
                cardScript.SetFeedbackSelected(true);
            }
        }
        cardCanvas.GetComponentsInChildren<Selectable>().First().Select();
    }

    public void ToggleCardToDeck(CardCollection card)
    {
        if(card.b_IsSelected)
        {
            DeckManager.instance._PlayerDeck.Remove(card.currentSkillcard);
            card.SetFeedbackSelected(false);
        }
        else
        {
            if ( DeckManager.instance._PlayerDeck.Count < 24 )
            {
                DeckManager.instance._PlayerDeck.Add(card.currentSkillcard);
                card.SetFeedbackSelected(true);

            }
        }
    }

    
}
