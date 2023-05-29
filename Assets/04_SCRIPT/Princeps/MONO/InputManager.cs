using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputMaster inputMaster;

    private void Awake()
    {
        inputMaster = new InputMaster();
    }
}
