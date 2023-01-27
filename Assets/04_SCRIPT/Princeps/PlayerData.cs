using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    #region -- PLAYER VARIABLES --
    public bool b_HasPassedTutorial;
    public Vector3 tutoPosition;
    #endregion

    #region -- GAME MANAGER VARIABLES --
    public List<SkillCard_SO> _PlayerDeck;
    public List<SkillCard_SO> _HiddenDeck;
    public DeckManager _DeckManagerState;
    #endregion

    public PlayerData( CharacterSpecs playerSpecs, DeckManager deckManager )
    {

        b_HasPassedTutorial = false;

        tutoPosition = playerSpecs.transform.position;


        _PlayerDeck = new List<SkillCard_SO>();
        _HiddenDeck = new List<SkillCard_SO>();

        _PlayerDeck = deckManager._PlayerDeck;
        _HiddenDeck = deckManager._HiddenDeck;
        _DeckManagerState = deckManager;
    }

    public PlayerData( Transform playerPos, bool tutoState)
    {

        b_HasPassedTutorial = tutoState;

        tutoPosition = playerPos.position;
    }

    public PlayerData(DeckManager deckManager )
    {

        _PlayerDeck = new List<SkillCard_SO>();
        _HiddenDeck = new List<SkillCard_SO>();

        _PlayerDeck = deckManager._PlayerDeck;
        _HiddenDeck = deckManager._HiddenDeck;
        _DeckManagerState = deckManager;
    }


}
