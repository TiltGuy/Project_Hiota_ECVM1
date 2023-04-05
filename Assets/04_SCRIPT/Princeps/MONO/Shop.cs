using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Shop : MonoBehaviour
{
    private InputMaster controls;

    public List<SkillCard_SO> currentListOfSkillCardSOToChoose = new List<SkillCard_SO>();
    public List<CardCollection> ListOfCardSlots = new List<CardCollection>();

    public GameObject CloseUpPanel;
    public GameObject CloseUpCard_GO;
    public TMP_Text CloseUpCard_Description;
    public GameObject ConfirmationPanel;

    public GameObject ShopGrid;
    public SkillCard_SO currentCardSelected;
    public CardCollection cardToBeBought;

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

        controls.UI.Close_Up.started += ctx => ToggleCloseUp();
    }

    // Start is called before the first frame update
    void Start()
    {
        SortCardsSO();
        ShopGrid.GetComponentsInChildren<Selectable>().First().Select();
    }


    private void SortCardsSO()
    {
        // Dupliquer cela deux fois pour les Communes, Rares et Légendaires
        currentListOfSkillCardSOToChoose = DeckManager.instance.BaseListOfCards.ListCards.ToList();
        List<SkillCard_SO> newListSorted = new List<SkillCard_SO>();
        foreach(SkillCard_SO skillCard in currentListOfSkillCardSOToChoose)
        {
            if(!DeckManager.instance._PlayerDeck.Contains(skillCard))
            {
                newListSorted.Add(skillCard);
            }
        }

        // boucle carte commune
        for(int i = 0; i < 6; i++)
        {
            int j = Random.Range(0, newListSorted.Count);
            if(newListSorted.Count > 0)
            {
                ListOfCardSlots[i].AssignText(newListSorted[j]);

                newListSorted.RemoveAt(j);
            }
            else
            {
                //Afficher un truc SOLDOUT
            }
        }
    }

    private void ToggleCloseUp()
    {
        if ( CloseUpPanel.activeInHierarchy )
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

    public void BuyCard(CardCollection cardCollection)
    {
        cardToBeBought = cardCollection;
        ConfirmationPanel.SetActive(true);
        foreach(CardCollection cardUI in ListOfCardSlots)
        {
            cardUI.GetComponentInChildren<Button>().interactable = false;
        }
        ConfirmationPanel.GetComponentsInChildren<Selectable>().First().Select();

    }

    public void Confirm()
    {
        if(DeckManager.instance.PlayerMoney >= cardToBeBought.currentSkillcard.shopCost)
        {
            DeckManager.instance._PlayerDeck.Add(cardToBeBought.currentSkillcard);

            Debug.Log("You have the MONEY BRO");
        }
        else
        {
            Debug.Log("You haven't the MONEY BRO");
        }
        ConfirmationPanel.SetActive(false);

        foreach ( CardCollection cardUI in ListOfCardSlots )
        {
            cardUI.GetComponentInChildren<Button>().interactable = true;
        }
        cardToBeBought.gameObject.SetActive(false);
        // Rajouter le SOLD OUT
        if( ShopGrid.GetComponentsInChildren<Selectable>().Length > 0 )
        {
            ShopGrid.GetComponentsInChildren<Selectable>().First().Select();
        }
    }

    public void Cancel()
    {
        ConfirmationPanel.SetActive(false);

        foreach ( CardCollection cardUI in ListOfCardSlots )
        {
            cardUI.GetComponentInChildren<Button>().interactable = true;
        }
        if ( ShopGrid.GetComponentsInChildren<Selectable>().Length > 0 )
        {
            ShopGrid.GetComponentsInChildren<Selectable>().First().Select();
        }
    }
}
