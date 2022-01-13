using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject Dialogue01;
    // Start is called before the first frame update
    void Start()
    {
        Dialogue01.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider player)
	{
        if(player.tag == "Player")
		{
            Dialogue01.SetActive(true);
            Debug.Log("Dialogue ON");
            
        }
        
	}

	private void OnTriggerExit(Collider other)
	{
        Destroy(Dialogue01);
        Debug.Log("Dialogue OFF");
        
	}


}
