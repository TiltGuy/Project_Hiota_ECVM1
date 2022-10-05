using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCameraPlayer : MonoBehaviour
{
    ActionHandler actionHandler;
    Player_InputScript player_InputScript;

    public delegate void MultiDelegate();
    public MultiDelegate OnChangeTargetPlayerPosition;

    [SerializeField]
    private TargetGatherer targetGatherer;

    public Transform currentHiotaTarget;

    private void Awake()
    {
        actionHandler = GetComponent<ActionHandler>();
        player_InputScript = GetComponent<Player_InputScript>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTargetOfPlayer(Vector2 input)
    {
        if (actionHandler.b_IsFocusing)
        {
            if (actionHandler.b_CanChangeFocusTarget)
            {
                actionHandler.b_CanChangeFocusTarget = false;
                UpdateHiotaCurrentTarget(input);
                OnChangeTargetPlayerPosition();
            }

        }
        
    }

    private void UpdateHiotaCurrentTarget(Vector2 input)
    {
        if (targetGatherer.CheckoutNextTargetedEnemy(input) != null)
        {
            currentHiotaTarget = targetGatherer.CheckoutNextTargetedEnemy(input);
            actionHandler.currentCharTarget = currentHiotaTarget;
            Debug.Log(targetGatherer.CheckoutNextTargetedEnemy(input));
        }
    }

    public void ResetFocusCameraTargetFactor()
    {
        actionHandler.b_CanChangeFocusCameraTarget = true;
        //print("Reset");
    }
}
