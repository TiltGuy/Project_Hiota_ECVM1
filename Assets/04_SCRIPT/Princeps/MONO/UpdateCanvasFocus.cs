using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCanvasFocus : MonoBehaviour
{
    public Canvas FocusCanvas;

    public void AddTarget()
    {
        FocusCanvas.gameObject.SetActive(true);
    }

    public void RemoveTarget()
    {
        FocusCanvas.gameObject.SetActive(false);
    }
}
