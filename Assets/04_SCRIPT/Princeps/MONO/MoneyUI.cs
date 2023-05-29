using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text playerMoney_Text;
    [SerializeField]
    private TMP_Text playerBetMoney_Text;


    private void Start()
    {
        if ( DeckManager.instance != null )
        {
            DeckManager.instance.OnMoneyChanged += UpdatePlayerMoney;
            DeckManager.instance.OnMoneyBetChanged += UpdatePlayerBet;

            UpdatePlayerBet();
            UpdatePlayerMoney();
        }
    }

    private void UpdatePlayerMoney()
    {
        playerMoney_Text.text = DeckManager.instance.PlayerMoney.ToString();
        //print(DeckManager.instance.PlayerMoney.ToString());
    }

    private void UpdatePlayerBet()
    {
        playerBetMoney_Text.text = DeckManager.instance.CalculateCurrentCardsBet().ToString();
    }


}
