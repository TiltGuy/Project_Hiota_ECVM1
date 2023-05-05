using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSoundCard : MonoBehaviour
{
    AudioSource soundFX;

    // Start is called before the first frame update
    void Start()
    {
        soundFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSoundFX()
    {
        soundFX.Play();
    }
}
