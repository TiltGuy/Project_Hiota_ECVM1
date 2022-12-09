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
    public List<SkillCard_SO> SkillCards = new List<SkillCard_SO>();
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
            TakeDamages(0, null, false);
        }

        UpdateCardMessages(Bonus_Text, SkillCards, true);
        UpdateCardMessages(Malus_Text, SkillCards, false);
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
                    CurrentInstance.CurrentSkillCard = SkillCards[UnityEngine.Random.Range(0, SkillCards.Count)];
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
            _NumberOfUses--;

            if ( saveToPlayerPrefs )
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "." + name + ".t_Activated", 1);
            }
        }
    }

    private void UpdateCardMessages(TMP_Text textToUpdate, List<SkillCard_SO> skillCards, bool b_BonusDisplay)
    {
        for (int i = 0; i < skillCards.Count; i++)
        {
            string newLine = Environment.NewLine;
            if (b_BonusDisplay)
            {
                for ( int j = 0; j < skillCards[i].Bonus.Count; j++ )
                {
                    // first Line of Text
                    if ( i == 0 && j == 0)
                    {
                        textToUpdate.text += skillCards[i].Bonus[j].cardMessage;
                    }
                    else
                    {
                        textToUpdate.text += newLine + skillCards[i].Bonus[j].cardMessage;
                    }
                }
            }
            else
            {
                for ( int j = 0; j < skillCards[i].Malus.Count; j++ )
                {
                    // first Line of Text
                    if ( i == 0 && j == 0 )
                    {
                        textToUpdate.text += skillCards[i].Malus[j].cardMessage;
                    }
                    else
                    {
                        textToUpdate.text += newLine + skillCards[i].Malus[j].cardMessage;
                    }
                }
            }
        }
    }

}
