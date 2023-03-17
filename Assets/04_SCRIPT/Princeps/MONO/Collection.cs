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
    private InputMaster controls;

    [SerializeField]
    private Scrollbar bar;
    [SerializeField]
    private float speedScroll = 1f;

    private void Awake()
    {
        //Initialisation of ALL the Bindings with InputMaster
        if ( InputManager.inputMaster != null )
        {
            controls = InputManager.inputMaster;
            controls.UI.Enable();
        }
        else
        {
            controls = new InputMaster();
            controls.UI.Enable();
        }

        controls.UI.ScrollController.performed += ctx => ScrollCollection(ctx.ReadValue<float>());
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay();
    }

    private void ScrollCollection(float Axis)
    {
        bar.value += Axis * speedScroll * Time.deltaTime;
        bar.value = Mathf.Clamp01(bar.value);
        Debug.Log("Axis = " + bar.value, this);
    }

    private void Update()
    {

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
