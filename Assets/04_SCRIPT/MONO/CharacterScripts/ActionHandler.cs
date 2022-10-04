using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    #region INPUT SETTINGS

    [Header(" -- INPUT SETTINGS -- ")]

    private bool b_CursorInvisible = true;

    #endregion

    #region MOVEMENT Settings

    [Header(" -- MOVEMENT SETTINGS -- ")]

    [Tooltip("The sum of the z axis of the controller and the ZQSD")]
    public Vector2 m_InputMoveVector = Vector2.zero;
    public bool b_WantDash = false;

    [Tooltip("If the player is focusing a enemy")]
    [SerializeField] public bool b_IsFocusing = false;

    private bool b_CanChangeFocusCameraTarget = true;
    #endregion

    public delegate void MultiDelegateWithVector2(Vector2 vector);
    public MultiDelegateWithVector2 OnChangeTargetFocus;

    public delegate void MultiDelegate();
    public MultiDelegate OnChangeCurrentPlayerTarget;

    private void OnEnable()
    {
        //OnChangeTargetFocus += UpdateHiotaCurrentTarget;
    }

    private void OnDisable()
    {
        //OnChangeTargetFocus -= UpdateHiotaCurrentTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTargetFocusCamera(Vector2 input)
    {

        if (b_IsFocusing)
        {
            if (b_CanChangeFocusCameraTarget)
            {
                b_CanChangeFocusCameraTarget = false;
                OnChangeTargetFocus(input);
                OnChangeCurrentPlayerTarget();
            }

        }
    }

    //private void UpdateHiotaCurrentTarget(Vector2 input)
    //{
    //    if (targetGatherer.CheckoutNextTargetedEnemy(input) != null)
    //    {
    //        currentHiotaTarget = targetGatherer.CheckoutNextTargetedEnemy(input);
    //        Debug.Log(targetGatherer.CheckoutNextTargetedEnemy(input));
    //    }
    //}
}
