using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFade : MonoBehaviour
{

    public CanvasGroup logo, start, controls, quit;


    public MainMenu mainMenu;
    private void Awake()
    {
        mainMenu = GetComponent<MainMenu>();
        logo.alpha = 0;
        start.alpha = 0;
        controls.alpha = 0;
        quit.alpha = 0;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Camera.main.FadeIn(3f);
        mainMenu.SetInputMenuActive(false);
        yield return new WaitForSeconds(2f);

        //lister les boutons
        
        logo.LeanAlpha(1, 1f);
        yield return new WaitForSeconds(1f);

        start.LeanAlpha(1, 1f);
        yield return new WaitForSeconds(.5f);

        controls.LeanAlpha(1, 1f);
        yield return new WaitForSeconds(.5f);

        quit.LeanAlpha(1, 1f);
        yield return new WaitForSeconds(.5f);

        mainMenu.SetInputMenuActive(true);
    }
}
