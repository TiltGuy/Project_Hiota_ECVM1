using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyImprovement : MonoBehaviour, IDamageable
{
    private Animator _animator;
    public List<string> TargetTags = new List<string>();
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
                enemy.gameObject.AddComponent<SkillCardScript>();
            }
        }
    }

    public void TakeDamages( float damageTaken, Transform Striker, bool isAHook )
    {
        AssignNewSkillCard();
        _animator.SetTrigger("t_Activated");
        OnSelectSkillCard?.Invoke();
    }

}
