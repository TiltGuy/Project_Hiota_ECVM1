using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IABrain : MonoBehaviour
{

    private Controller_FSM controller_FSM;
    private TargetGatherer targetGatherer;

    

    [HideInInspector] public float factorStrafecrossDirection = 1f;
    private bool b_IsEnemyInFight = false;
    [SerializeField] private float timeToInvertStrafeDirection = 4f;
    [Range(.1f,50f)]
    public float speedOfTurningEnemyWhenFocus = 50f;
    [Range(.1f, 50f)]
    public float speedTurningWhenAttacking = 20f;
    public float SpeedIncreasedWhenEnemyFleeing = 6f;
    public float AntiBennyHillCountdown = 3f;
    public float AntiBennyHillTimer;

    public bool autoStartCombat = true;

    #region ATTACK SETTINGS

    public bool b_WantToAttack;
    public float timerBetweenATK = 6f;
    public float TimeBtwATKRandomize = 1f;
    public float minDistForPerformingAttack = 1f;

    #endregion

    public bool B_IsEnemyInFight
    {
        get { return b_IsEnemyInFight; }
        set 
        { 
            b_IsEnemyInFight = value;
            if (b_IsEnemyInFight)
            {
                StopCoroutine("InvertFactorStrafeDirection_Coroutine");
                StopCoroutine("TimerBetweenAttacks_Coroutine");
                StartCoroutine("InvertFactorStrafeDirection_Coroutine");
                StartCoroutine("TimerBetweenAttacks_Coroutine");
            }
            else
            {
                StopCoroutine("InvertFactorStrafeDirection_Coroutine");
                StopCoroutine("TimerBetweenAttacks_Coroutine");
            }
        }
    }

    private void Awake()
    {
        controller_FSM = GetComponent<Controller_FSM>();
        targetGatherer = GetComponentInChildren<TargetGatherer>();
        if(targetGatherer == null)
        {
            Debug.LogError("WARNING : There is no target gatherer in reference !!!");
        }
        if ( controller_FSM == null )
        {
            Debug.LogError("WARNING : There is no controller_FSM in reference !!!");
        }
    }

    private void OnEnable()
    {
        targetGatherer.AddEnemyToList += AddCurrentControllerTarget;
        //targetGatherer.RemoveEnemyToList += RemoveCurrentControllerTarget;
    }

    private void OnDisable()
    {
        targetGatherer.AddEnemyToList -= AddCurrentControllerTarget;
        //targetGatherer.RemoveEnemyToList -= RemoveCurrentControllerTarget;
    }

    private void AddCurrentControllerTarget(Transform transform)
    {
        controller_FSM.currentCharacterTarget = transform;
        B_IsEnemyInFight = true;
        //Debug.Log(transform, targetGatherer.transform);
    }

    public void AddPlayerToCurrentControllerTarget()
    {
        controller_FSM.currentCharacterTarget = GameObject.FindGameObjectWithTag("Player").transform;
        B_IsEnemyInFight = true;
    }

    private void RemoveCurrentControllerTarget(Transform transform)
    {
        controller_FSM.currentCharacterTarget = null;
        B_IsEnemyInFight = false;
    }

    private IEnumerator InvertFactorStrafeDirection_Coroutine()
    {
        yield return new WaitForSeconds(timeToInvertStrafeDirection);
        factorStrafecrossDirection *= -1;
        StartCoroutine("InvertFactorStrafeDirection_Coroutine");
    }

    private IEnumerator TimerBetweenAttacks_Coroutine()
    {
        float finalTimer;
        TimeBtwATKRandomize = Random.Range(timerBetweenATK - TimeBtwATKRandomize, timerBetweenATK + TimeBtwATKRandomize);
        finalTimer = timerBetweenATK + TimeBtwATKRandomize;
        //print(finalTimer);

        yield return new WaitForSeconds(finalTimer);
        
        b_WantToAttack = true;
        StartCoroutine("TimerBetweenAttacks_Coroutine");
    }

    public void LaunchBennyHillTimer()
    {
        //Launch Coroutine
    }

    private IEnumerator BennyHillTimer_Couroutine()
    {
        yield return new WaitForSeconds(AntiBennyHillTimer);
    }




}
