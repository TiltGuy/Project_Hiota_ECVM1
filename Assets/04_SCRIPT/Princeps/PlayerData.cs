﻿using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    #region -- PLAYER VARIABLES --
    public int currentSceneBuildIndex;
    public float currentHealthOfHiota;
    public bool b_HasPassedTutorial;
    public Vector3 tutoPosition;
    public float[] position;
    #endregion

    #region -- GAME MANAGER VARIABLES --
    public List<SkillCard_SO> _PlayerDeck;
    public List<SkillCard_SO> _HiddenDeck;
    public DeckManager _DeckManagerState;
    #endregion

    public PlayerData( CharacterSpecs playerSpecs, DeckManager deckManager )
    {
        currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        currentHealthOfHiota = playerSpecs.Health;

        b_HasPassedTutorial = false;

        tutoPosition = playerSpecs.transform.position;

        position = new float[3];
        position[0] = playerSpecs.transform.position.x;
        position[1] = playerSpecs.transform.position.y;
        position[2] = playerSpecs.transform.position.z;

        _PlayerDeck = new List<SkillCard_SO>();
        _HiddenDeck = new List<SkillCard_SO>();

        _PlayerDeck = deckManager._PlayerDeck;
        _HiddenDeck = deckManager._HiddenDeck;
        _DeckManagerState = deckManager;
    }

    public PlayerData( CharacterSpecs playerSpecs)
    {
        currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        currentHealthOfHiota = playerSpecs.Health;

        b_HasPassedTutorial = false;

        tutoPosition = playerSpecs.transform.position;

        position = new float[3];
        position[0] = playerSpecs.transform.position.x;
        position[1] = playerSpecs.transform.position.y;
        position[2] = playerSpecs.transform.position.z;
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
