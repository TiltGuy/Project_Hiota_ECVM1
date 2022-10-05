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

    public bool b_CanChangeFocusTarget = true;
    [HideInInspector]
    public bool b_IsParrying = false;

    [Tooltip("the Boolean that if the player is stunned")]
    public bool b_Stunned = false;

    [Tooltip("the Boolean that check the input")]
    public bool b_AttackInput = false;

    public bool b_IsPerfectlyParrying = false;

    public bool b_CanChangeFocusCameraTarget = true;

    public Animator characterAnimator;

    [Tooltip("the time unitl the input b_AttackInput will become false")]
    public float timeBufferAttackInput = .5f;
    #endregion

    [SerializeField]
    private TargetGatherer targetGatherer;

    public delegate void MultiDelegateWithVector2(Vector2 vector);
    public MultiDelegateWithVector2 OnChangeTargetFocus;
    [HideInInspector]
    public Transform currentCharTarget;
    [HideInInspector]
    [Tooltip("The focus of hiota if he have to")]
    public Transform currentCharacterTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void ChangeCharTargetFocus(Transform newTarget)
    {

        if (b_IsFocusing)
        {
            if (b_CanChangeFocusTarget)
            {
                currentCharTarget = newTarget;
                currentCharacterTarget = newTarget;
                OnChangeTargetFocus(newTarget.position);
            }

        }
    }

    public void DebugAction(bool b_Value)
    {
        toggleStunnedValue(b_Value);
    }

    private void toggleStunnedValue(bool b_value)
    {
        b_Stunned = b_value;
    }

    public void TakeAttackInputInBuffer()
    {
        StopCoroutine(BufferingAttackInputCoroutine(timeBufferAttackInput));
        b_AttackInput = true;
        StartCoroutine(BufferingAttackInputCoroutine(timeBufferAttackInput));
    }
    private IEnumerator BufferingAttackInputCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        b_AttackInput = false;
    }

    public IEnumerator SetIsPerfectlyParryingCoroutine(float time)
    {
        StopCoroutine("SetIsPerfectlyParryingCoroutine");
        b_IsPerfectlyParrying = true;
        yield return new WaitForSeconds(time);
        b_IsPerfectlyParrying = false;
    }

    public void ToggleFocusTarget()
    {
        if (!b_IsFocusing)
        {
            if (targetGatherer.TargetableEnemies.Count > 0)
            {
                b_IsFocusing = true;
                characterAnimator.SetBool("Is_Focusing", b_IsFocusing);
            }
            print("AH");
        }
        else
        {
            b_IsFocusing = false;
            characterAnimator.SetBool("Is_Focusing", b_IsFocusing);
        }
    }

    public void HideCursor()
    {
        if (!b_CursorInvisible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            b_CursorInvisible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            b_CursorInvisible = false;
        }
    }


}
