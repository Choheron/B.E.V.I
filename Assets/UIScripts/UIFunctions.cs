using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFunctions : MonoBehaviour
{
    void Start() {

    }

    void Update() {

    }

    public void changeTextElement(Text displayText, string newText) {
        displayText.text = newText;
    }
}
