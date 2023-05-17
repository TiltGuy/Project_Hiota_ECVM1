using UnityEngine;
using FMODUnity;
using FMOD;
using Debug = UnityEngine.Debug;

public class PlayerController_Animator:MonoBehaviour
{
    # region DEPENDENCIES

    [Header(" -- DEPENDENCIES -- ")]


    public GameObject Sword_GO;
    public GameObject Spine;
    //public GameObject GuardPoint;
    public GameObject Shield;
    public GameObject Base_Attack;
    public GameObject Dash_Attack_Frwd;
    public GameObject Dash_Attack_L;
    public GameObject Dash_Attack_R;
    public GameObject Counter_Attack;
    public CapsuleCollider swordHitBox;
    public Animator animator;
    public Transform ps;
    public Transform Dash_FX;
    public Transform FXAiles;
    public Controller_FSM controller_FSM;
    public Player_InputScript inputScript;
    [HideInInspector]
    public Transform currentAttackHitboxPrefab;
    public Transform currentAttackHitbox;
    public bool b_IsUsingSwordHB = false;

    [Header(" -- SOUND DEPENDENCIES -- ")]

    [SerializeField]
    private EventReference footSteps_ER;
    [SerializeField]
    private EventReference damage_ER;
    [SerializeField]
    private EventReference heal_ER;
    [SerializeField]
    private EventReference Attack_ER;
    [SerializeField]
    private EventReference AttackLight_ER;
    [SerializeField]
    private EventReference Dash_ER;
    [SerializeField]
    private EventReference GuardNormal_ER;
    [SerializeField]
    private EventReference GuardPerfect_ER;
    [SerializeField]
    private EventReference Respawn_ER;


    //public int nbHitBoxTrue = 0;

    #endregion

    private void Awake()
    {
        inputScript = GetComponent<Player_InputScript>();
    }

    private void Start()
    {
        if ( controller_FSM.CurrentAttackStats )
        {
            currentAttackHitboxPrefab = controller_FSM.CurrentAttackStats.hitBoxPrefab;
        }
        //Debug.Log("Player animator says : " + controller_FSM.BasicAttackStats.hitBoxPrefab, this);

    }

    public void ToggleSwordHitBoxStatut( bool value )
    {
        swordHitBox.enabled = value;
    }

    public void UpdateBasicAttackHitBoxStatutTrue()
    {
        //swordHitBox.enabled = true;
        if ( !b_IsUsingSwordHB )
        {
            if ( currentAttackHitboxPrefab )
            {
                if ( controller_FSM.CurrentAttackStats )
                {
                    currentAttackHitboxPrefab = controller_FSM.CurrentAttackStats.hitBoxPrefab;
                }
                currentAttackHitbox = Instantiate(currentAttackHitboxPrefab, controller_FSM.transform.position, Quaternion.identity);
                Touch currentInstance = currentAttackHitbox.GetComponent<Touch>();
                currentInstance.ControllerFSM = controller_FSM;
                currentInstance.InstigatorAnimator = this;
                currentInstance.AttackStats = controller_FSM.CurrentAttackStats;
                currentAttackHitbox.SetParent(controller_FSM.transform);
                currentAttackHitbox.transform.localPosition = Vector3.zero;
                currentAttackHitbox.transform.localRotation = Quaternion.identity;
                return;
            }
            //Debug.Log(currentInstance.transform.localScale);
        }
        if ( b_IsUsingSwordHB )
        {
            ToggleSwordHitBoxStatut(true);
        }
        //Debug.Log("Basic Attack HitBox is : " + currentAttackHitbox, this);

    }

    public void UpdateBasicAttackHitBoxStatutFalse()
    {
        if ( currentAttackHitbox && !b_IsUsingSwordHB )
        {
            currentAttackHitbox.GetComponent<Touch>().DestroyItSelfAfterUsed();
            return;
        }
        if ( b_IsUsingSwordHB )
        {
            ToggleSwordHitBoxStatut(false);
        }
        //swordHitBox.enabled = false;
    }

    public void RespawnPlayer()
    {
        animator.SetBool("b_Respawn", true);
        Debug.Log("Je lance le respawn", this);
    }

    public void ResetRespawnBool()
    {
        animator.SetBool("b_Respawn", false);
    }

    #region -- FXs Functions --

    public void ShaftSword()
    {
        //Sword_GO.transform.parent = controller_FSM.HolsterSword;
        Sword_GO.transform.localPosition = Vector3.zero;
        Sword_GO.transform.localRotation = Quaternion.identity;
    }

    public void UnShaftSword()
    {
        //Sword_GO.transform.parent = controller_FSM.HandOfSword;
        Sword_GO.transform.localPosition = Vector3.zero;
        Sword_GO.transform.localRotation = Quaternion.identity;
    }

    public void DustCloud()
    {
        if(ps)
        {
                Instantiate(ps, transform.position, transform.rotation);
        }
    }
    public void DashBurst()
    {
        if(Dash_FX)
        {
            Instantiate(Dash_FX, transform.position, transform.rotation);
        }

        if(FXAiles)
        {
            Transform Ailes = Instantiate(FXAiles.transform, Spine.transform.position, Quaternion.identity);
            Ailes.SetParent(Spine.transform);
            Ailes.transform.localRotation = Quaternion.identity;
            Ailes.transform.localPosition = Vector3.zero;
        }
    }


    /*public void SpawnFX( GameObject TargetFX )
    {
        Instantiate(TargetFX.transform, transform.position, Quaternion.identity);
    }*/

    public void FinishAttackAnimation()
    {
        //Debug.Log("Bob ! Do something ! ", this);
    }

    public void WindUpGlow( GameObject targetFX )
    {
        if(targetFX != null)
        {
            Transform glowingSword = Instantiate(targetFX.transform, Sword_GO.transform.position, Quaternion.identity);
            glowingSword.SetParent(Sword_GO.transform);
            glowingSword.transform.localRotation = Quaternion.identity;
            glowingSword.transform.localPosition = Vector3.zero;
        }
    }

    //public void Sword_Slash( GameObject targetFX )
    //{
      //  if ( targetFX != null )
        //{
          //  Transform Sword_Slash = Instantiate(targetFX.transform, Hip_Localiser.transform.position, Quaternion.identity);
          //  Sword_Slash.SetParent(Hip_Localiser.transform);
          //  Sword_Slash.transform.localRotation = Quaternion.identity;
          //  Sword_Slash.transform.localPosition = Vector3.zero;
        //}
    //}

    public void Slash_Activate_Neutral( GameObject targetFX )
    {
        if ( targetFX != null && Base_Attack != null)
        {
            Transform Slash_Activate_Neutral = Instantiate(targetFX.transform, Base_Attack.transform.position, Quaternion.identity);
            Slash_Activate_Neutral.SetParent(Base_Attack.transform);
            Slash_Activate_Neutral.transform.localRotation = Quaternion.identity;
            Slash_Activate_Neutral.transform.localPosition = Vector3.zero;
        }
    }

    public void Slash_Activate_Dash_Frwd( GameObject targetFX )
    {
        if ( targetFX != null  && Dash_Attack_Frwd )
        {
            Transform Slash_Activate_Dash_Frwd = Instantiate(targetFX.transform, Dash_Attack_Frwd.transform.position, Quaternion.identity);
            Slash_Activate_Dash_Frwd.SetParent(Base_Attack.transform);
            Slash_Activate_Dash_Frwd.transform.localRotation = Quaternion.identity;
            Slash_Activate_Dash_Frwd.transform.localPosition = Vector3.zero;
        }
    }

    public void Slash_Activate_Dash_L( GameObject targetFX )
    {
        if ( targetFX != null && Dash_Attack_L )
        {
            Transform Slash_Activate_Dash_L = Instantiate(targetFX.transform, Dash_Attack_L.transform.position, Quaternion.identity);
            Slash_Activate_Dash_L.SetParent(Base_Attack.transform);
            Slash_Activate_Dash_L.transform.localRotation = Quaternion.identity;
            Slash_Activate_Dash_L.transform.localPosition = Vector3.zero;
        }
    }

    public void Slash_Activate_Dash_R( GameObject targetFX )
    {
        if ( targetFX != null && Dash_Attack_R )
        {
            Transform Slash_Activate_Dash_R = Instantiate(targetFX.transform, Dash_Attack_R.transform.position, Quaternion.identity);
            Slash_Activate_Dash_R.SetParent(Base_Attack.transform);
            Slash_Activate_Dash_R.transform.localRotation = Quaternion.identity;
            Slash_Activate_Dash_R.transform.localPosition = Vector3.zero;
        }
    }

    public void Slash_Activate_Counter( GameObject targetFX )
    {
        if ( targetFX != null && Counter_Attack )
        {
            Transform Slash_Activate_Counter = Instantiate(targetFX.transform, Counter_Attack.transform.position, Quaternion.identity);
            Slash_Activate_Counter.SetParent(Base_Attack.transform);
            Slash_Activate_Counter.transform.localRotation = Quaternion.identity;
            Slash_Activate_Counter.transform.localPosition = Vector3.zero;
        }
    }
    #endregion

    public void UpdatePlayerControllableStatus_False()
    {
        inputScript.B_IsControllable = false;
    }

    public void UpdatePlayerControllableStatus_True()
    {
        inputScript.B_IsControllable = true;
        animator.SetBool("b_Respawn", false);
    }

    #region -- SOUNDS METHOD --

    public void PlayFootStep_Sound()
    {
        RuntimeManager.PlayOneShot(footSteps_ER, transform.position);
        
    }

    public void LaunchAttack()
    {
        RuntimeManager.PlayOneShot(Attack_ER, transform.position);
    }

    public void LaunchAttackLight()
    {
        RuntimeManager.PlayOneShot(AttackLight_ER, transform.position);
    }

    public void TakeHit_Sound()
    {
        RuntimeManager.PlayOneShot(damage_ER,transform.position);
    }

    public void HealYourself()
    {
        RuntimeManager.PlayOneShotAttached(heal_ER, gameObject);
    }

    public void Dash_Sound()
    {
        RuntimeManager.PlayOneShotAttached(Dash_ER, gameObject);
    }

    public void GuardNormal_SoundEvent()
    {
        RuntimeManager.PlayOneShot(GuardNormal_ER, transform.position);
    }

    public void GuardPerfect_SoundEvent()
    {
        RuntimeManager.PlayOneShot(GuardPerfect_ER, transform.position);
    }

    public void Respawn_SoundEvent()
    {
        RuntimeManager.PlayOneShotAttached(Respawn_ER, gameObject);
    }

    #endregion

}
