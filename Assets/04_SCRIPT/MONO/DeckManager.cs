using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckManager : MonoBehaviour
{
    public List<SkillCard_SO> _HiddenDeck = new List<SkillCard_SO>();
    public List<SkillCard_SO> _PlayerDeck ;

    public static DeckManager instance;

    private void Awake()
    {
        #region Singleton Instanciation
        if ( instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        #endregion
    }

    public List<SkillCard_SO> DrawCards(float numberCardsToDraw)
    {
        List<SkillCard_SO> cardsDrawn = new List<SkillCard_SO>();

        for (int i = 0; i < numberCardsToDraw; i++)
        {
            int cardIndex = UnityEngine.Random.Range(0, _PlayerDeck.Count );
            cardIndex = Mathf.Clamp(cardIndex, 0, _PlayerDeck.Count - 1);
            Debug.Log(_PlayerDeck.Count, this);
            cardsDrawn[i] = _PlayerDeck[cardIndex];

        }

        return cardsDrawn;
    }

    public SkillCard_SO DrawFuckingCard()
    {
        return _PlayerDeck[UnityEngine.Random.Range(0, _PlayerDeck.Count) -1];
    }
}
