using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class Collection : MonoBehaviour
{

    private InputMaster controls;

    public static Collection instance;

    public Canvas cardCanvas;
    public GameObject collectionEmpty;
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
        //Time.timeScale = 0;

        if ( instance != null )
        {
            Debug.Log("Je m'autodétruis!!!!");
            Destroy(gameObject);
        }

        //Debug.Log(instance,instance);
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

    public void RefreshCollection()
    {
        if ( b_UseDeckManagerCollection && DeckManager.instance != null )
        {
            InitializeDisplay(DeckManager.instance._PlayerDeck);
        }
        else
        {
            InitializeDisplay(collection.ListCards);
        }
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
        //instance = this;

        refPlayerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_InputScript>();
        refPlayerInput.B_IsInUI = true;
        RefreshCollection();
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
        refPlayerInput.B_IsInUI = cardCanvas.gameObject.activeInHierarchy;
    }

    public void ScrollLeft()
    {
        if ( cardCanvas == null )
            return;
        if (cardCanvas.gameObject.activeInHierarchy)
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
        if ( cardCanvas == null )
            return;
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
        //Debug.Log(Cursor.name, Cursor);
        Debug.Log(target.name, target.transform);
        //Cursor.transform.position = target.transform.position;
        //Cursor.transform.parent = target.transform;
        currentCardSelected = target.GetComponentInParent<CardCollection>().currentSkillcard;
    }

    private void InitializeDisplay(List<SkillCard_SO> CollectionCard)
    {
        int cardCounter = 0;
        int pageCounter = 0;
        int numberOfPages = Mathf.CeilToInt(CollectionCard.Count / nbOfCardsinLists);
        for(int i = 0; i < pages.Length; i++)
        {
            Destroy(pages[i]);
        }
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

    public void SetFirstCardSelectable()
    {
        //List<SelectedScroll> list = new List<SelectedScroll>();
        //list = cardCanvas.GetComponentsInChildren<SelectedScroll>().ToList();
        //foreach(SelectedScroll scroll in list)
        //{
        //    Debug.Log(scroll.name, scroll.transform);
        //}
        //Debug.Log(list.Count);
        //Debug.Log(cardCanvas.GetComponentsInChildren<SelectedScroll>().First());
        //cardCanvas.GetComponentsInChildren<SelectedScroll>().First().GetComponent<Button>().Select();
        StartCoroutine("SelectContinueButtonLater_Coroutine");
    }

    IEnumerator SelectContinueButtonLater_Coroutine()
    {
        yield return null;
        GameObject eventSchose = GameObject.Find("EventSystem");
        if(eventSchose != null)
        {
            EventSystem es = eventSchose.GetComponent<EventSystem>();
            //es.SetSelectedGameObject(null);
            es.SetSelectedGameObject(cardCanvas.GetComponentsInChildren<SelectedScroll>().First().gameObject);
        }
    }

    private void InitializeCard( GameObject currentPage, SkillCard_SO card )
    {
        GameObject clone = Instantiate(prefabCard, currentPage.transform);
        clone.name = card.name;
        CardCollection cardScript = clone.GetComponent<CardCollection>();
        cardScript.AssignText(card);
        cardScript.refCollection = this;
        if ( DeckManager.instance._CurrentDeck.Contains(card) )
        {
            cardScript.SetFeedbackSelected(true);
        }
    }

    public void ToggleCardToDeck(CardCollection card)
    {
        if(card.b_IsSelected)
        {
            DeckManager.instance._CurrentDeck.Remove(card.currentSkillcard);
            card.SetFeedbackSelected(false);
        }
        else
        {
            if ( DeckManager.instance._CurrentDeck.Count < 24 )
            {
                DeckManager.instance._CurrentDeck.Add(card.currentSkillcard);
                card.SetFeedbackSelected(true);
                // mettre à jour le compteur en prenant le count sur 24
            }
        }

    }

    
}
