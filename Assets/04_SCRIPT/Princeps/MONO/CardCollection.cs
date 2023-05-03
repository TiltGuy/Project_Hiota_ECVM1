using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardCollection : MonoBehaviour
{
    public TMP_Text Title_Text;
    public TMP_Text Bonus_Texts;
    public TMP_Text Malus_Texts;
    public string bonusPrefix = "Bonus";
    public string malusPrefix = "Malus";
    public string linePrefix = "> ";
    [HideInInspector]
    public SkillCard_SO currentSkillcard;
    [HideInInspector]
    public Collection refCollection;
    public Shop refShop;
    public GameObject feedbackSelectedImage;
    public TMP_Text Cost_Text;

    [HideInInspector]
    public bool b_IsSelected;

    public void AssignText( SkillCard_SO skillCard )
    {
        
        Bonus_Texts.text = "";
        Malus_Texts.text = "";
        currentSkillcard = skillCard;
        UpdateCardMessages(Bonus_Texts, skillCard, true);
        UpdateCardMessages(Malus_Texts, skillCard, false);
        UpdateCardMessageTitle(Title_Text, skillCard);
        if ( Cost_Text != null )
        {
            Cost_Text.text = /*"Cost : " +*/ currentSkillcard.shopCost.ToString();
        }
    }

    private void UpdateCardMessages( TMP_Text textToUpdate, SkillCard_SO skillCard, bool b_BonusDisplay )
    {
        string newLine = System.Environment.NewLine;
        if ( b_BonusDisplay )
        {
            if ( skillCard.Bonus.Count > 0 )
            {
                if ( !string.IsNullOrEmpty(bonusPrefix) )
                    textToUpdate.text = bonusPrefix + "\n";
                for ( int j = 0; j < skillCard.Bonus.Count; j++ )
                {
                    // first Line of Text
                    if ( j == 0 )
                    {
                        textToUpdate.text += linePrefix + skillCard.Bonus[j].cardMessage;
                    }
                    else
                    {
                        textToUpdate.text += newLine + linePrefix + skillCard.Bonus[j].cardMessage;
                    }
                }
            }
        }
        else
        {
            if ( skillCard.Malus.Count > 0 )
            {
                if ( !string.IsNullOrEmpty(malusPrefix) )
                    textToUpdate.text = malusPrefix + "\n";
                for ( int j = 0; j < skillCard.Malus.Count; j++ )
                {
                    // first Line of Text
                    if ( j == 0 )
                    {
                        textToUpdate.text += linePrefix + skillCard.Malus[j].cardMessage;
                    }
                    else
                    {
                        textToUpdate.text += newLine + linePrefix + skillCard.Malus[j].cardMessage;
                    }
                }
            }
        }

    }

    private void UpdateCardMessageTitle( TMP_Text textToUpdate, SkillCard_SO skillCard )
    {
        textToUpdate.text = skillCard.cardName;
    }

    public void SetFeedbackSelected(bool value)
    {
        if(value)
        {
            b_IsSelected = true;
            feedbackSelectedImage.SetActive(true);
        }
        else
        {
            b_IsSelected =false;
            feedbackSelectedImage.SetActive(false);
        }
        
    }

    public void ButtonClick()
    {
        refCollection.ToggleCardToDeck(this);
    }

    public void ShopButtonClick()
    {
        refShop.BuyCard(this);
    }
}
