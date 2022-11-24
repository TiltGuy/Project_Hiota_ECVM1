using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyImprovement : MonoBehaviour, IDamageable
{
    public List<string> TargetTags = new List<string>();
    public string EnemyTag;
    public List<Transform> Enemies = new List<Transform>();
    public UnityEvent OnSelectSkillCard;
    private void Awake()
    {
        foreach ( GameObject enemy in GameObject.FindGameObjectsWithTag(EnemyTag) )
        {
            Enemies.Add(enemy.GetComponent<Transform>());
        }
    }
    private void Start()
    {
        
        foreach ( Transform enemy in Enemies )
        {
            enemy.gameObject.AddComponent<SkillCardScript>();
        }
    }

    public void TakeDamages( float damageTaken, Transform Striker, bool isAHook )
    {
        foreach( GameObject enemy in GameObject.FindGameObjectsWithTag(EnemyTag))
        {
            //Enemies.Add(enemy.GetComponent<Controller_FSM>());
        }
    }
}
