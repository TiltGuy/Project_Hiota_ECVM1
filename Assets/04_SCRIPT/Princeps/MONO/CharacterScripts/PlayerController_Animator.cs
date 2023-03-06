using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Animator:MonoBehaviour
{
    # region DEPENDENCIES

    [Header(" -- DEPENDENCIES -- ")]


    [Tooltip("It needs the prefab of CameraBase")]
    public GameObject Sword_GO;
    public GameObject Hip_Localiser;
    public GameObject GuardPoint;
    public GameObject Shield;
    public CapsuleCollider swordHitBox;
    public Animator animator;
    public Transform ps;
    public Controller_FSM controller_FSM;
    [HideInInspector]
    public Transform currentAttackHitboxPrefab;
    public Transform currentAttackHitbox;
    public bool b_IsUsingSwordHB = false;


    //public int nbHitBoxTrue = 0;

    #endregion

    private void Start()
    {
        if ( controller_FSM.CurrentAttackStats )
        {
            currentAttackHitboxPrefab = controller_FSM.CurrentAttackStats.hitBoxPrefab;
        }
        //Debug.Log("Player animator says : " + controller_FSM.BasicAttackStats.hitBoxPrefab, this);

    }

    private void Update()
    {
        //print("current attack is : " + currentAttackHitbox);
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
        Instantiate(ps, transform.position, Quaternion.identity);
    }

    public void SpawnFX( GameObject TargetFX )
    {
        Instantiate(TargetFX.transform, transform.position, Quaternion.identity);
    }

    public void FinishAttackAnimation()
    {
        Debug.Log("Bob ! Do something ! ", this);
    }

    public void WindUpGlow( GameObject targetFX )
    {
        Transform glowingSword = Instantiate(targetFX.transform, Sword_GO.transform.position, Quaternion.identity);
        glowingSword.SetParent(Sword_GO.transform);
        glowingSword.transform.localRotation = Quaternion.identity;
        glowingSword.transform.localPosition = Vector3.zero;
    }

    public void Sword_Slash( GameObject targetFX )
    {
        Transform Sword_Slash = Instantiate(targetFX.transform, Hip_Localiser.transform.position, Quaternion.identity);
        Sword_Slash.SetParent(Hip_Localiser.transform);
        Sword_Slash.transform.localRotation = Quaternion.identity;
        Sword_Slash.transform.localPosition = Vector3.zero;
    }

    //*public void ShieldSpawn( GameObject targetFX )
    //{
    //Transform ShieldSpawn = Instantiate(targetFX.transform, GuardPoint.transform.position, Quaternion.identity);
    //ShieldSpawn.SetParent(GuardPoint.transform);
    //ShieldSpawn.transform.localRotation = Quaternion.identity;
    //  ShieldSpawn.transform.localPosition = Vector3.zero;
    //}

    //public void ShieldDown( GameObject targetFX )
    //{
    //    Destroy(ShieldSpawn);
    //}



    //if(isParrying == true)
      //  {

        //    Shield.SetActive(true);
          //  animator.SetBool("",true);

        //}
    //else
//{
  //  Shield.SetActive(false);
    //animator.SetBool("", false);
//}

}
