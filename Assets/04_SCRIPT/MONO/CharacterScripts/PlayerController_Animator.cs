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
    [SerializeField]
    private PlayerController_FSM controller_FSM;
    private Transform basicAttackHitBoxPrefab;
    private Transform currentAttackHitbox;
    //public int nbHitBoxTrue = 0;

    #endregion

    private void Start()
    {
        basicAttackHitBoxPrefab = controller_FSM.BasicAttackStats.hitBoxPrefab;
        Debug.Log("Player animator says : " + controller_FSM.BasicAttackStats.hitBoxPrefab, this);

    }

    public void ToggleSwordHitBoxStatut()
    {
        swordHitBox.enabled = !swordHitBox.enabled;
    }

    public void UpdateBasicAttackHitBoxStatutTrue()
    {
        //swordHitBox.enabled = true;
        if (basicAttackHitBoxPrefab)
        {

            currentAttackHitbox = Instantiate(basicAttackHitBoxPrefab, controller_FSM.transform.position, Quaternion.identity);
            currentAttackHitbox.SetParent(controller_FSM.transform);
        }
        Debug.Log("Basic Attack HitBox is : " + basicAttackHitBoxPrefab, this);
        
    }

    public void UpdateBasicAttackHitBoxStatutFalse()
    {
        if (basicAttackHitBoxPrefab)
        {
            
        }
        //swordHitBox.enabled = false;
    }

    

    public void ShaftSword()
    {
        Sword_GO.transform.parent = controller_FSM.HolsterSword;
        Sword_GO.transform.localPosition = Vector3.zero;
        Sword_GO.transform.localRotation = Quaternion.identity;
    }

    public void UnShaftSword()
    {
        Sword_GO.transform.parent = controller_FSM.HandOfSword;
        Sword_GO.transform.localPosition = Vector3.zero;
        Sword_GO.transform.localRotation = Quaternion.identity;
    }

    
}
