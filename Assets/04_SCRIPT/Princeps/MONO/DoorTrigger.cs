using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator doorAnimator;
    public string triggerName;

    public void DoDoorOpening()
    {
        doorAnimator.SetTrigger(triggerName);
    }
}
