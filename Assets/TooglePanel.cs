using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TooglePanel: MonoBehaviour
{

    public GameObject panelA;
    public GameObject panelB;

    public void switchPanel() 
    {
        Debug.Log(panelA);
        Debug.Log(panelB);
        if (panelA && panelA.activeSelf) {
            panelA.SetActive(false);
            panelB.SetActive(true);
        } else {
            panelA.SetActive(true);
            panelB.SetActive(false);
        }
    }
}