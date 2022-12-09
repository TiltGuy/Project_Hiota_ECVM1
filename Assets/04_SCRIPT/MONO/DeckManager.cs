﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckManager : MonoBehaviour
{
    public List<SkillCard_SO> _HiddenDeck = new List<SkillCard_SO>();
    public List<SkillCard_SO> _PlayerDeck = new List<SkillCard_SO>();
    public List<SkillCard_SO> _RunDeck = new List<SkillCard_SO>();
    public List<SkillCard_SO> _EnemiesDeck = new List<SkillCard_SO>();

    public struct EnemyHolder
    {
        public CharacterSpecs characterSpecs;
        public Controller_FSM  controllerFSM;
    }

    public List<EnemyHolder> _EnemyList = new List<EnemyHolder>();

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

    private void Start()
    {
        _RunDeck = _PlayerDeck;
    }

    public List<SkillCard_SO> DrawCards(float numberCardsToDraw)
    {
        List<SkillCard_SO> cardsDrawnToReturn = new List<SkillCard_SO>();

        for ( int i = 0; i < numberCardsToDraw; i++ )
        {
            //int cardIndex = UnityEngine.Random.Range(0, _PlayerDeck.Count);
            //cardIndex = Mathf.Clamp(cardIndex, 0, _PlayerDeck.Count - 1);
            //Debug.Log(_PlayerDeck.Count, this);
            //cardsDrawn[i] = _PlayerDeck[cardIndex];
            SkillCard_SO newCard = DrawOneCard();

            //Debug.Log(newCard);
            cardsDrawnToReturn.Add(newCard);
        }
        return cardsDrawnToReturn;
    }

    public SkillCard_SO DrawOneCard()
    {
        if(UnityEngine.Random.value < .5f && _HiddenDeck.Count > 0)
        {
            SkillCard_SO newCard = _HiddenDeck[UnityEngine.Random.Range(0, _HiddenDeck.Count)];
            _HiddenDeck.Remove(newCard);
            _PlayerDeck.Add(newCard);
            return newCard;
        }
        else
        {
            if ( _RunDeck.Count == 0 )
            {
                _RunDeck = _PlayerDeck;
            }
            SkillCard_SO newCard = _RunDeck[UnityEngine.Random.Range(0, _RunDeck.Count)];
            _RunDeck.Remove(newCard);
            return newCard;
        }
    }

    public void AddCardToEnnemyDeck(SkillCard_SO newCard)
    {
        _EnemiesDeck.Add(newCard);
        foreach (EnemyHolder enemy in _EnemyList)
        {
            newCard.ApplyEffects(enemy.controllerFSM, enemy.characterSpecs);
        }
    }

    public void RegisterEnemy(GameObject enemy_go)
    {
        EnemyHolder currentEnemy = new EnemyHolder();
        currentEnemy.characterSpecs = enemy_go.GetComponent<CharacterSpecs>();
        currentEnemy.controllerFSM = enemy_go.GetComponent<Controller_FSM>();
        _EnemyList.Add(currentEnemy);
    }
}