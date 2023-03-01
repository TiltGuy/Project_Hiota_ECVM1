using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnRoomComplete : MonoBehaviour
{
    static private List<ToggleOnRoomComplete> instances = new List<ToggleOnRoomComplete>();

    static public void ToggleAll()
    {
        foreach ( ToggleOnRoomComplete instance in instances.ToArray() )
        {
            instance.gameObject.SetActive(!instance.isOn);
        }
    }

    public bool isOn;

    private void Awake()
    {
        instances.Add(this);
        gameObject.SetActive(isOn);
    }

    private void OnDestroy()
    {
        instances.Remove(this);
    }
}
