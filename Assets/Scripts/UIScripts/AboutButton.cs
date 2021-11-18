//
// Name: Thomas Campbell
// 
// Project: B.E.V.I.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutButton : MonoBehaviour
{
    private string URL = "https://github.com/Choheron/B.E.V.I";

    /// <summary>
    /// Opens a webpage to the github repository.
    /// </summary>
    public void openURL() {
        Debug.Log("User has clicked the ABOUT button, opening github repository.");
        Application.OpenURL(URL);
    }
}
