using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class playSoundCard : MonoBehaviour
{
    StudioEventEmitter soundFX;

    // Start is called before the first frame update
    void Start()
    {
        soundFX = GetComponent<StudioEventEmitter>();
    }

    public void playSoundFX()
    {
        soundFX.Play();
        Debug.LogWarning("Not event on it !!!!!", this);
    }


}
