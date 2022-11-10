using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Animator : MonoBehaviour
{
    # region DEPENDENCIES

    [Header(" -- DEPENDENCIES -- ")]


    [Tooltip("It needs the prefab of CameraBase")]
    public GameObject Sword_GO;
    public CapsuleCollider swordHitBox;
    public Animator animator;
    public Transform ps;
    public Controller_FSM controller_FSM;
    [HideInInspector]
    public Transform currentAttackHitboxPrefab;
    [HideInInspector]
    public Transform currentAttackHitbox;
    //public int nbHitBoxTrue = 0;

    #endregion

    private void Start()
    {
        if(controller_FSM.CurrentAttackStats)
        {
            currentAttackHitboxPrefab = controller_FSM.CurrentAttackStats.hitBoxPrefab;
        }
        //Debug.Log("Player animator says : " + controller_FSM.BasicAttackStats.hitBoxPrefab, this);

    }

    private void Update()
    {
        //print("current attack is : " + currentAttackHitbox);
    }

    public void ToggleSwordHitBoxStatut()
    {
        swordHitBox.enabled = !swordHitBox.enabled;
    }

    public void UpdateBasicAttackHitBoxStatutTrue()
    {
        //swordHitBox.enabled = true;
        if (currentAttackHitboxPrefab)
        {

            currentAttackHitbox = Instantiate(currentAttackHitboxPrefab, controller_FSM.transform.position, Quaternion.identity);
            Touch currentInstance = currentAttackHitbox.GetComponent<Touch>();
            currentInstance.ControllerFSM = controller_FSM;
            currentInstance.InstigatorAnimator = this;
            currentInstance.AttackStats = controller_FSM.CurrentAttackStats;
            currentAttackHitbox.SetParent(controller_FSM.transform);
            currentAttackHitbox.transform.localPosition = Vector3.zero;
            currentAttackHitbox.transform.localRotation = Quaternion.identity;
        }
        //Debug.Log("Basic Attack HitBox is : " + currentAttackHitbox, this);
        
    }

    public void UpdateBasicAttackHitBoxStatutFalse()
    {
        if (currentAttackHitbox)
        {
            currentAttackHitbox.GetComponent<Touch>().DestroyItSelfAfterUsed();
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
        Instantiate(ps,transform.position, Quaternion.identity);
    }

    public void SpawnFX(GameObject TargetFX)
    {
        Instantiate(TargetFX.transform, transform.position, Quaternion.identity);
    }

    public void FinishAttackAnimation()
    {
        Debug.Log("Bob ! Do something ! ",this);
    }

}
