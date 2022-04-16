using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class StatePrinter : MonoBehaviour
{
    public static bool s_isSit;
    public static bool s_isHeadUp;

    public Text txt;

    void Update()
    {
        if(s_isSit)
        {
            txt.text = "Now: isSit";
        }
        else if(s_isHeadUp)
        {
            txt.text = "Now: isHeadUp";
        }
        else
        {
            txt.text = string.Empty;
        }
    }
}
