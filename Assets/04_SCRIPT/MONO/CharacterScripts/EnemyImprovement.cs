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

}
