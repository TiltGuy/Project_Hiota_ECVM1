using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    public ListOfCards BaseListOfCards;
    public List<SkillCard_SO> _HiddenDeck = new List<SkillCard_SO>();
    public List<SkillCard_SO> _PlayerDeck = new List<SkillCard_SO>();
    public List<SkillCard_SO> _RunDeck = new List<SkillCard_SO>();
    public List<SkillCard_SO> _EnemiesDeck = new List<SkillCard_SO>();

    [System.Serializable]
    public struct EnemyHolder
    {
        public CharacterSpecs characterSpecs;
        public Controller_FSM  controllerFSM;
    }

    [SerializeField]
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
        //for ( int i = 0; i < _PlayerDeck.Count; i++ )
        //{
        //    _RunDeck.Add(_PlayerDeck[i]);
        //}
        if(DataPersistentManager.instance)
        {
            TakeSavedDecksFromDPManager();
        }
        else
        {
            _PlayerDeck.Clear();
            _PlayerDeck = BaseListOfCards.ListCards.ToList();
        }
        _RunDeck = _PlayerDeck.ToList();
    }

    #region Save STuffs

    private void TakeSavedDecksFromDPManager()
    {
        if ( DataPersistentManager.instance.currentDataToApply != null
                    && DataPersistentManager.instance.currentDataToApply._PlayerDeck != null
                    && DataPersistentManager.instance.currentDataToApply._HiddenDeck != null
                    && DataPersistentManager.instance.currentDataToApply.b_HasPassedTutorial )
        {
            _PlayerDeck = DataPersistentManager.instance.currentDataToApply._PlayerDeck.ToList();
            _HiddenDeck = DataPersistentManager.instance.currentDataToApply._HiddenDeck.ToList();

        }
    }

    #endregion

    #region Dealer Stuffs

    public int CalculateCurrentCardsBet()
    {
        int moneyBet = 0;

        foreach(SkillCard_SO skillCard in _EnemiesDeck)
        {
            moneyBet += skillCard.moneyBet;
        }
        return moneyBet;
    }

    #endregion

    #region Draw Card STUFFS
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
        
        if ( _RunDeck.Count == 0 )
        {
            _RunDeck = _PlayerDeck;
        }
        SkillCard_SO newCard = _RunDeck[UnityEngine.Random.Range(0, _RunDeck.Count)];
        //_RunDeck.Remove(newCard);
        return newCard;
    }

    #endregion

    public void AddCardToEnnemyDeck(SkillCard_SO newCard)
    {
        _EnemiesDeck.Add(newCard);
        //ApplyCardEffectForAllEnemies(newCard);
    }


    public void RemoveCardFromRunDeck( SkillCard_SO newCard )
    {
        if ( _RunDeck.Contains(newCard) )
        {
            _RunDeck.Remove(newCard);
        }
    }

    public void ApplyCardEffectsToEnemy(Controller_FSM targetController, CharacterSpecs targetSpecs)
    {
        if(_EnemiesDeck.Count > 0)
        {
            foreach ( SkillCard_SO targetCard in _EnemiesDeck )
            {
                targetCard.ApplyEffects(targetController, targetSpecs);
            }
        }
    }

    #region Registering Enemy Stuffs

    public void RegisterEnemy(GameObject enemy_go)
    {
        EnemyHolder currentEnemy = new EnemyHolder();
        currentEnemy.characterSpecs = enemy_go.GetComponent<CharacterSpecs>();
        currentEnemy.controllerFSM = enemy_go.GetComponent<Controller_FSM>();
        _EnemyList.Add(currentEnemy);
    }

    public void UnRegisterEnemy(GameObject enemy_go)
    {
        for(int i = 0; i < _EnemyList.Count; i++)
        {
            if ( _EnemyList[i].controllerFSM == enemy_go.GetComponent<Controller_FSM>() )
            {
                _EnemyList.Remove(_EnemyList[i]);
            }
        }

    }

    #endregion
}
