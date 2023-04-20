using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Collection : MonoBehaviour
{

    private InputMaster controls;

    public static Collection instance;

    public Canvas cardCanvas;
    public bool b_UseDeckManagerCollection;
    public ListOfCards collection;
    [SerializeField]
    private float nbOfCardsinLists;
    public GameObject collection_Empty;
    public GameObject prefabCard;
    public SkillCard_SO currentCardSelected;

    [Header("-- PAGINATION SETTINGS --")]
    
    [SerializeField]
    GameObject[] pages;
    int currentPageID = 0;
    public TMP_Text currentPageNumber;
    public TMP_Text totalPageNumber;
    public GameObject cardPage;
    public GameObject Cursor;

    [Header("-- CLOSE UP SETTINGS --")]

    public GameObject CloseUpPanel;
    public GameObject CloseUpCard_GO;
    public TMP_Text CloseUpCard_Description;


    Player_InputScript refPlayerInput;


    private void Awake()
    {
        Time.timeScale = 0;

        if ( instance != null )
        {
            Destroy(this);
        }

        instance = this;

        
        DontDestroyOnLoad(gameObject);
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

        controls.UI.Gach_Gauche.started += ctx => ScrollLeft();
        controls.UI.Gach_Droite.started += ctx => ScrollRight();
        controls.UI.Close_Up.started += ctx => ToggleCloseUp();
    }

    

    private void OnEnable()
    {
        if(refPlayerInput)
        {
            refPlayerInput.B_IsInUI = true;
        }
    }

    private void OnDisable()
    {
        if(refPlayerInput)
        {
            refPlayerInput.B_IsInUI = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        refPlayerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_InputScript>();
        refPlayerInput.B_IsInUI = true;
        if (b_UseDeckManagerCollection && DeckManager.instance != null)
        {
            InitializeDisplay(DeckManager.instance.BaseListOfCards.ListCards);
        }
        else
        {
            InitializeDisplay(collection.ListCards);
        }
    }

    private void Update()
    {
        UpdateIsInUIPlayer();
    }

    private void ToggleCloseUp()
    {
        if(!cardCanvas.gameObject.activeInHierarchy)
        {
            return;
        }
        if(CloseUpPanel.activeInHierarchy)
        {
            CloseUpPanel.SetActive(false);
        }
        else
        {
            // Faire passer le SO au close UP
            CloseUpCard_GO.GetComponent<CardCollection>().AssignText(currentCardSelected);
            CloseUpCard_Description.text = currentCardSelected.description;

            // Afficher Le close UP
            CloseUpPanel.SetActive(true);
        }
    }

    private void UpdateIsInUIPlayer()
    {
        if ( cardCanvas.gameObject.activeInHierarchy )
        {
            refPlayerInput.B_IsInUI = true;
        }
        else
        {
            refPlayerInput.B_IsInUI = false;
        }
    }

    public void ScrollLeft()
    {
        if(cardCanvas.gameObject.activeInHierarchy)
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

    }

    private void UpdateCurrentPageNumberPagination()
    {
        int numberCurrentPage = currentPageID + 1;
        currentPageNumber.text = numberCurrentPage.ToString();
    }

    public void ScrollRight()
    {
        if(cardCanvas.gameObject.activeInHierarchy)
        {
            //print(currentPageID);
            if ( currentPageID + 1 > pages.Length - 1 )
            {
                return;
            }
            currentPageID += 1;
            UpdateCurrentPageNumberPagination();
            SetCurrentPageVisible();
            SetFirstCardSelectable();
        }
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
        currentCardSelected = target.GetComponentInParent<CardCollection>().currentSkillcard;
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
                // mettre à jour le compteur en prenant le count sur 24
            }
        }

    }

    
}
