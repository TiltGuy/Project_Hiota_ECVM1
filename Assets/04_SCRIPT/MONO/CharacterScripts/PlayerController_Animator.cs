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
    public int nbHitBoxTrue = 0;

    #endregion
    

    public void ToggleSwordHitBoxStatut()
    {
        swordHitBox.enabled = !swordHitBox.enabled;
    }

    public void UpdateSwordHitBoxStatutFalse()
    {
        swordHitBox.enabled = false;
    }

    public void UpdateSwordHitBoxStatutTrue()
    {
        swordHitBox.enabled = true;
        nbHitBoxTrue++;
        Debug.Log(nbHitBoxTrue);
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
