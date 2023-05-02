using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFade : MonoBehaviour
{
    MainMenu mainMenu;
    private void Awake()
    {
        mainMenu = GetComponent<MainMenu>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Camera.main.FadeIn(3f);
        mainMenu.SetInputMenuActive(false);
        yield return new WaitForSeconds(3f);
        mainMenu.SetInputMenuActive(true);
    }
}
