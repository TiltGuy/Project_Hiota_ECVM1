using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCameraPlayer : MonoBehaviour
{
    ActionHandler actionHandler;

    public delegate void MultiDelegate();
    public MultiDelegate OnChangeTargetPlayerPosition;

    private void Awake()
    {
        actionHandler = GetComponent<ActionHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeTargetOfPlayerPosition()
    {
        if (actionHandler.b_IsFocusing)
        {
            if (actionHandler.b_CanChangeFocusTarget)
            {
                OnChangeTargetPlayerPosition();
            }

        }
        
    }
}
