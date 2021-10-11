using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_VersionOnScreen : MonoBehaviour
{
    [SerializeField]
    private Text textVersionGame;

    [SerializeField]
    private string devPhaseGame;

    private void Awake()
    {
        textVersionGame = GameObject.FindGameObjectWithTag("VersionDisplay").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        textVersionGame.text = devPhaseGame + " Version " + Application.version;
        //Debug.Log("Application Version : " + Application.version);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
