﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

<<<<<<< Updated upstream
public class EnemyImprovement : MonoBehaviour, IDamageable
{
    private Animator _animator;
    public float _NumberOfUses;
    public List<string> TargetTags = new List<string>();
    public List<SkillCard_SO> SkillCards = new List<SkillCard_SO>();
    public string EnemyTag;
    public List<Transform> Enemies = new List<Transform>();
=======

public class EnemyImprovement : MonoBehaviour, IDamageable
{
    private Animator _animator;
    public float _NumberOfUses;
    public List<string> TargetTags = new List<string>();
    //public List<SkillCard_SO> SkillCards = new List<SkillCard_SO>();
    public SkillCard_SO SkillCard;
    public string EnemyTag;
    public List<Transform> Enemies = new List<Transform>();
>>>>>>> Stashed changes
    public UnityEvent OnSelectSkillCard;
    public bool saveToPlayerPrefs = false;

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

    private void Start()
    {
        //AssignNewSkillCard();

        if ( saveToPlayerPrefs && PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "." + name + ".t_Activated") == 1 )
        {
            TakeDamages(0, null, false);
<<<<<<< Updated upstream
        }
    }

    private void AssignNewSkillCard()
    {
        foreach ( Transform enemy in Enemies )
        {
            if(enemy.gameObject.activeInHierarchy)
            {
                if(SkillCards.Count > 0)
                {
                    SkillCardScript CurrentInstance = enemy.gameObject.AddComponent<SkillCardScript>();
                    CurrentInstance.CurrentSkillCard = SkillCards[Random.Range(0, SkillCards.Count)];
                }
            }
        }
        OnSelectSkillCard?.Invoke();
    }

    public void TakeDamages( float damageTaken, Transform Striker, bool isAHook )
    {
        if(_NumberOfUses>0)
        {
            print("Assign");
            AssignNewSkillCard();
            _animator.SetTrigger("t_Activated");
=======
        }

        UpdateCardMessages(Bonus_Text, SkillCard, true);
        UpdateCardMessages(Malus_Text, SkillCard, false);
    }

    private void AssignNewSkillCard()
    {
        //foreach ( Transform enemy in Enemies )
        //{
        //    if(enemy.gameObject.activeInHierarchy)
        //    {
        //        if(SkillCard != null)
        //        {
        //           //SkillCardScript CurrentInstance = enemy.gameObject.AddComponent<SkillCardScript>();
        //            //CurrentInstance.CurrentSkillCard = SkillCard;
        //            DeckManager.instance.AddCardToEnnemyDeck(SkillCard);
        //        }
        //    }
        //}

        if ( SkillCard != null )
        {
            //SkillCardScript CurrentInstance = enemy.gameObject.AddComponent<SkillCardScript>();
            //CurrentInstance.CurrentSkillCard = SkillCard;
            DeckManager.instance.AddCardToEnnemyDeck(SkillCard);
        }
        OnSelectSkillCard?.Invoke();
    }

    public void TakeDamages( float damageTaken, Transform Striker, bool isAHook )
    {
        if(_NumberOfUses>0)
        {
            print("Assign");
            AssignNewSkillCard();
            _animator.SetTrigger("t_Activated");
>>>>>>> Stashed changes
            _NumberOfUses--;

            if ( saveToPlayerPrefs )
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "." + name + ".t_Activated", 1);
<<<<<<< Updated upstream
            }
        }
    }

}
=======
            }
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
>>>>>>> Stashed changes
