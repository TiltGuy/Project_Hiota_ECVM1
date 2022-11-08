using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IABrain : MonoBehaviour
{

    private Controller_FSM controller_FSM;
    private TargetGatherer targetGatherer;

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
        targetGatherer.RemoveEnemyToList += RemoveCurrentControllerTarget;
    }

    private void OnDisable()
    {
        targetGatherer.AddEnemyToList -= AddCurrentControllerTarget;
        targetGatherer.RemoveEnemyToList -= RemoveCurrentControllerTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddCurrentControllerTarget(Transform transform)
    {
        controller_FSM.currentCharacterTarget = transform;
        Debug.Log(transform, targetGatherer.transform);
    }

    private void RemoveCurrentControllerTarget(Transform transform)
    {
        controller_FSM.currentCharacterTarget = null;
    }
}
