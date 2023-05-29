using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnInput : MonoBehaviour
{
    public UnityEvent eventOnInput;

    public string axisName;
    public KeyCode keyCode;

    private InputMaster controls;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
  //  void Update()
  //  {
  //      if(axisName != "" && Input.GetButtonDown(axisName))
		//{
  //          eventOnInput.Invoke();
  //      }
  //      if (keyCode != KeyCode.None && Input.GetKeyDown(keyCode))
  //      {
  //          eventOnInput.Invoke();
  //      }

  //  }

    
}
