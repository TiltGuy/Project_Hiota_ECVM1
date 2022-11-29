using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyImprovement : MonoBehaviour, IDamageable
{
    private Animator _animator;
    public float _NumberOfUses;
    public List<string> TargetTags = new List<string>();
    public List<SkillCard_SO> SkillCards = new List<SkillCard_SO>();
    public string EnemyTag;
    public List<Transform> Enemies = new List<Transform>();
    public UnityEvent OnSelectSkillCard;
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
            _NumberOfUses--;
        }
    }

}
