using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    #region INPUT SETTINGS

    [Header(" -- INPUT SETTINGS -- ")]

    private bool b_CursorInvisible = true;

    #endregion

    //[HideInInspector]
    [Tooltip("The focus of character if he have to")]
    public Transform currentCharacterTarget;
    #region MOVEMENT Settings

    [Header(" -- MOVEMENT SETTINGS -- ")]

    [Tooltip("The sum of the z axis of the controller and the ZQSD")]
    public Vector2 m_InputMoveVector = Vector2.zero;
    public bool b_WantDash = false;

    [Tooltip("If the player is focusing a enemy")]
    [SerializeField] public bool b_IsFocusing = false;

    public bool b_CanChangeFocusTarget = true;
    [HideInInspector]
    public bool b_IsInputParry = false;

    [Tooltip("the Boolean that if the character is stunned")]
    public bool b_Stunned = false;

    [Tooltip("the Boolean that if the character is hooked")]
    public bool b_Hooked = false;

    [Tooltip("the Boolean that check the input")]
    public bool b_AttackInput = false;

    public bool b_IsPerfectlyParrying = false;

    public bool b_CanChangeFocusCameraTarget = true;

    public Animator characterAnimator;

    [Tooltip("the time unitl the input b_AttackInput will become false")]
    public float timeBufferAttackInput = .1f;
    #endregion

    [SerializeField]
    private TargetGatherer targetGatherer;

    public delegate void MultiDelegateWithVector2(Vector2 vector);
    public MultiDelegateWithVector2 OnChangeTargetFocus;
    public delegate void MultiDelegate();
    public MultiDelegate OnTargetNull;

    Coroutine AttackCoroutine;


    public Transform CurrentCharacterTarget
    {
        get => currentCharacterTarget;
        set
        {
            
            if(value == null)
            {
                b_IsFocusing = false;
                characterAnimator.SetBool("Is_Focusing", b_IsFocusing);
                OnTargetNull();
            }
            else
            {
                b_IsFocusing = true;
                characterAnimator.SetBool("Is_Focusing", b_IsFocusing);
            }
            currentCharacterTarget = value;

            if(transform.CompareTag("Player") && currentCharacterTarget != null)
            {
                IABrain currentBrain = currentCharacterTarget.GetComponent<IABrain>();
                currentBrain.DisplayHealthBar(true, true);
            }

            //Debug.Log(" CurrentCharTarget of Hiota = " + currentCharacterTarget);
            //Debug.Log("b_IsFocusing = " + b_IsFocusing);
            //Debug.Log("Anim IsFocusing = " + characterAnimator.GetBool("Is_Focusing"));
        }
    }

    public void ChangeCharTargetFocus(Transform newTarget)
    {

        if (b_IsFocusing)
        {
            if (b_CanChangeFocusTarget)
            {
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
        b_Hooked = b_value;
    }

    public void TakeAttackInputInBuffer()
    {
        if( AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
        }
        b_AttackInput = true;
        AttackCoroutine = StartCoroutine("BufferingAttackInputCoroutine", timeBufferAttackInput);

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
        characterAnimator.SetBool("b_PerfectGuard", b_IsPerfectlyParrying);
        yield return new WaitForSeconds(time);
        b_IsPerfectlyParrying = false;
        characterAnimator.SetBool("b_PerfectGuard", b_IsPerfectlyParrying);
    }

    public void ToggleFocusTarget()
    {
        if (!b_IsFocusing)
        {
            if (targetGatherer && targetGatherer.TargetableEnemies.Count > 0)
            {
                b_IsFocusing = true;
                characterAnimator.SetBool("Is_Focusing", b_IsFocusing);
            }
            //print("AH");
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
