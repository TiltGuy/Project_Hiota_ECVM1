using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Collection : MonoBehaviour
{

    public static Collection instance;

    public Canvas cardCanvas;
    public ListOfCards collection;
    [SerializeField]
    private float nbOfCardsinLists;
    public GameObject collection_Empty;
    public GameObject prefabCard;
    public GameObject cardPage;
    private InputMaster controls;
    public GameObject Cursor;
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;
    private float minCursorPos, maxCursorPos;

    RectTransform rectTransformCursor;

    private Scrollbar bar;
    private float speedScroll = 100f;
    private float speedScrollLerp = 10f;

    float pasScroll = .2f;

    float scrollInput;

    private void Awake()
    {
        Time.timeScale = 0;

        instance = this;

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

        controls.UI.Gach_Gauche.started += ctx => ScrollLeft();
        controls.UI.Gach_Droite.started += ctx => ScrollRight();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeDisplay();
        rectTransformCursor = Cursor.GetComponent<RectTransform>();
    }

    private void ScrollCollection(float Axis)
    {
        scrollInput = Axis;
    }


    public void ScrollLeft()
    {
        
    }

    public void ScrollRight()
    {

    }

    public void SelectCard(GameObject target)
    {
        Cursor.transform.position = target.transform.position;
        Cursor.transform.parent = target.transform;

        // AJOUTER FEEDBACK SELECTION
    }

    private void InitializeDisplay()
    {
        int cardCounter = 0;
        GameObject currentPage = Instantiate(cardPage, collection_Empty.transform);

        foreach (SkillCard_SO card in collection.ListCards)
        {
            cardCounter++;
            if (cardCounter % nbOfCardsinLists == 0)
            {
                Debug.Log("NEW PAGE cardCounter = " + cardCounter, currentPage);
                cardCounter = 0;
            }
            GameObject clone = Instantiate(prefabCard, currentPage.transform);
            clone.name = card.name;
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
