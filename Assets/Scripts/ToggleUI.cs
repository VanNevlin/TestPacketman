using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject TargetCanvas;
    
    private void Start()
    {
        TargetCanvas.SetActive(false); 
    }

    public void UIControl()
    {
        if (TargetCanvas.activeSelf)
        {
            TargetCanvas.SetActive(false);
        }

        else
        {
            TargetCanvas.SetActive(true);
        }
    }
}
