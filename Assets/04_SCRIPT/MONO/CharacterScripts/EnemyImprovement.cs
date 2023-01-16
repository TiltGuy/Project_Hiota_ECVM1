using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using TMPro;


public class EnemyImprovement : MonoBehaviour, IDamageable
{
    private Animator _animator;
    public float _NumberOfUses;
    public List<string> TargetTags = new List<string>();
    //public List<SkillCard_SO> SkillCards = new List<SkillCard_SO>();
    public SkillCard_SO SkillCard;
    public string EnemyTag;
    public List<Transform> Enemies = new List<Transform>();
    public UnityEvent OnSelectSkillCard;
    public bool saveToPlayerPrefs = false;
    //put the ref in the editor
    public TMP_Text Bonus_Text;
    //put the ref in the editor
    public TMP_Text Malus_Text;

    private void Awake()
    {
        UpdateListOfEnemies();
        _animator = GetComponent<Animator>();
    }

    private void UpdateListOfEnemies()
    {
        foreach ( GameObject enemy in GameObject.FindGameObjectsWithTag(EnemyTag) )
        {
            Enemies.Add(enemy.GetComponent<Transform>());
        }
    }

    

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        //AssignNewSkillCard();

        if ( saveToPlayerPrefs && PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "." + name + ".t_Activated") == 1 )
        {
            TakeDamagesParriable(0, null, false);
        }

        if(SkillCard != null)
        {
            UpdateCardMessages(Bonus_Text, SkillCard, true);
            UpdateCardMessages(Malus_Text, SkillCard, false);
        }
    }

    private void AddCurrentSkillCardToEnemyDeck()
    {

        if ( SkillCard != null )
        {
            //SkillCardScript CurrentInstance = enemy.gameObject.AddComponent<SkillCardScript>();
            //CurrentInstance.CurrentSkillCard = SkillCard;
            DeckManager.instance.AddCardToEnnemyDeck(SkillCard);
        }
        OnSelectSkillCard?.Invoke();
    }

    public void TakeDamagesParriable( float damageTaken, Transform Striker, bool isAHook )
    {
        if(_NumberOfUses>0)
        {
            //print("Assign");
            AddCurrentSkillCardToEnemyDeck();
            _animator.SetTrigger("t_Activated");
            _NumberOfUses--;

            SaveSteleToPlayerPref();
        }
    }

    public void TakeDamagesNonParriable( float damageTaken, Transform Striker, float ForceOfProjection )
    {

    }

    private void SaveSteleToPlayerPref()
    {
        if ( saveToPlayerPrefs )
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "." + name + ".t_Activated", 1);
        }
    }

    private void UpdateCardMessages(TMP_Text textToUpdate, SkillCard_SO skillCard, bool b_BonusDisplay)
    {
        string newLine = Environment.NewLine;
        if (b_BonusDisplay)
        {
            if( skillCard.Bonus.Count > 0)
            {
                for ( int j = 0; j < skillCard.Bonus.Count; j++ )
                {
                    // first Line of Text
                    if ( j == 0 )
                    {
                        textToUpdate.text += skillCard.Bonus[j].cardMessage;
                    }
                    else
                    {
                        textToUpdate.text += newLine + skillCard.Bonus[j].cardMessage;
                    }
                }
            }
        }
        else
        {
            if( skillCard.Malus.Count > 0 )
            {
                for ( int j = 0; j < skillCard.Malus.Count; j++ )
                {
                    // first Line of Text
                    if ( j == 0 )
                    {
                        textToUpdate.text += skillCard.Malus[j].cardMessage;
                    }
                    else
                    {
                        textToUpdate.text += newLine + skillCard.Malus[j].cardMessage;
                    }
                }
            }
        }
        
    }

    
}
