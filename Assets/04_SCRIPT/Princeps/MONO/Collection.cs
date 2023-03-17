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
    public GameObject prefabCard;
    public GameObject panelDisplay;
    private InputMaster controls;
    public GameObject Cursor;
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;
    private float minCursorPos, maxCursorPos;

    RectTransform rectTransformCursor;

    [SerializeField]
    private Scrollbar bar;
    [SerializeField]
    private float speedScroll = 100f;
    [SerializeField]
    private float speedScrollLerp = 10f;

    [SerializeField]
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

        controls.UI.Gach_Gauche.started += ctx => ScrollUp();
        controls.UI.Gach_Droite.started += ctx => ScrollDown();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay();
        rectTransformCursor = Cursor.GetComponent<RectTransform>();
        minCursorPos = rectTransformCursor.position.y;
        maxCursorPos = minCursorPos + panelDisplay.transform.childCount * gridLayoutGroup.spacing.y;
    }

    private void ScrollCollection(float Axis)
    {
        scrollInput = Axis;
    }

    private void Update()
    {
        //print("RectTransformCursorY = " + rectTransformCursor.position.y);
        //float cursorRelativePos = (rectTransformCursor.position.y - minCursorPos) / maxCursorPos;
        ////print("cursorRelativePos = " + cursorRelativePos);
        //bar.value = Mathf.Lerp(bar.value, cursorRelativePos, Time.deltaTime * speedScrollLerp);
        //bar.value = Mathf.Clamp01(bar.value);

        //float currentCursorPosY = rectTransformCursor.position.y;
        //currentCursorPosY += scrollInput * speedScroll * Time.deltaTime;
        ////currentCursorPosY = Mathf.Clamp(currentCursorPosY, minCursorPos, maxCursorPos);
        //rectTransformCursor.position = new Vector3(rectTransformCursor.position.x,
        //    currentCursorPosY,
        //    rectTransformCursor.position.z);
        //bar.value += Axis * speedScroll * Time.deltaTime;
        //Debug.Log("Axis = " + scrollInput, this);
    }


    public void ScrollUp()
    {
        bar.value += pasScroll;
        bar.value = Mathf.Clamp01(bar.value);
    }

    public void ScrollDown()
    {
        bar.value -= pasScroll;
        bar.value = Mathf.Clamp01(bar.value);
    }

    public void SelectCard(GameObject target)
    {
        Cursor.transform.position = target.transform.position;
        Cursor.transform.parent = target.transform;

        // AJOUTER FEEDBACK SELECTION
    }

    private void UpdateDisplay()
    {
        foreach (SkillCard_SO card in collection.ListCards)
        {
            GameObject clone = Instantiate(prefabCard, panelDisplay.transform);
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
