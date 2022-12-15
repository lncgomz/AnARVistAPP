using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeSC : MonoBehaviour
{
     public void onTakeSC()
    {
        ScreenCapture.CaptureScreenshot("screenshot.png");
    }
}