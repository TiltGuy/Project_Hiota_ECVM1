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
    public bool b_UseDeckManagerCollection;
    public ListOfCards collection;
    [SerializeField]
    private float nbOfCardsinLists;
    public GameObject collection_Empty;
    public GameObject prefabCard;
    [SerializeField]
    GameObject[] pages;
    int currentPageID = 0;
    public TMP_Text currentPageNumber;
    public TMP_Text totalPageNumber;
    public GameObject cardPage;
    private InputMaster controls;
    public GameObject Cursor;
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
        if(b_UseDeckManagerCollection && DeckManager.instance != null)
        {
            InitializeDisplay(DeckManager.instance.BaseListOfCards.ListCards);
        }
        else
        {
            InitializeDisplay(collection.ListCards);
        }
        rectTransformCursor = Cursor.GetComponent<RectTransform>();
    }

    private void ScrollCollection(float Axis)
    {
        scrollInput = Axis;
    }


    public void ScrollLeft()
    {
        if ( currentPageID - 1 < 0 )
        {
            return;
        }
        currentPageID -= 1;
        UpdateCurrentPageNumberPagination();
        SetCurrentPageVisible();
        SetFirstCardSelectable();

    }

    private void UpdateCurrentPageNumberPagination()
    {
        int numberCurrentPage = currentPageID + 1;
        currentPageNumber.text = numberCurrentPage.ToString();
    }

    public void ScrollRight()
    {

        print(currentPageID);
        if ( currentPageID + 1 > pages.Length - 1 )
        {
            return;
        }
        currentPageID += 1;
        UpdateCurrentPageNumberPagination();
        SetCurrentPageVisible();
        SetFirstCardSelectable();
    }

    private void SetCurrentPageVisible()
    {
        for ( int i = 0; i < pages.Length; i++ )
        {
            if ( i != currentPageID )
            {
                pages[i].SetActive(false);
            }
            else
            {
                pages[i].SetActive(true);
            }
        }
    }

    public void SelectCard(GameObject target)
    {
        Cursor.transform.position = target.transform.position;
        Cursor.transform.parent = target.transform;

        // AJOUTER FEEDBACK SELECTION
    }

    private void InitializeDisplay(List<SkillCard_SO> CollectionCard)
    {
        int cardCounter = 0;
        int pageCounter = 0;
        int numberOfPages = Mathf.CeilToInt(CollectionCard.Count / nbOfCardsinLists);
        pages = new GameObject[numberOfPages];
        totalPageNumber.text = string.Concat("/ ", pages.Length.ToString());
        UpdateCurrentPageNumberPagination();
        GameObject currentPage;
        currentPage = Instantiate(cardPage, collection_Empty.transform);
        pages[pageCounter] = currentPage;
        foreach ( SkillCard_SO card in CollectionCard )
        {
            if ( cardCounter % nbOfCardsinLists == 0 && cardCounter != 0 )
            {
                cardCounter = 0;
                currentPage = Instantiate(cardPage, collection_Empty.transform);
                pageCounter++;
                pages[pageCounter] = currentPage;
                currentPage.SetActive(false);
                //Debug.Log("NEW PAGE cardCounter = " + cardCounter, currentPage);
            }
            cardCounter++;
            InitializeCard(currentPage, card);
        }
        SetFirstCardSelectable();
    }

    private void SetFirstCardSelectable()
    {
        cardCanvas.GetComponentsInChildren<Selectable>().First().Select();
    }

    private void InitializeCard( GameObject currentPage, SkillCard_SO card )
    {
        GameObject clone = Instantiate(prefabCard, currentPage.transform);
        clone.name = card.name;
        CardCollection cardScript = clone.GetComponent<CardCollection>();
        cardScript.AssignText(card);
        cardScript.refCollection = this;
        if ( DeckManager.instance._PlayerDeck.Contains(card) )
        {
            cardScript.SetFeedbackSelected(true);
        }
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
