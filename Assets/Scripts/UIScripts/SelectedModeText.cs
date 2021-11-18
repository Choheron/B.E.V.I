//
// Author: Thomas Campbell
// Project: B.E.V.I.
//
using UnityEngine;
using UnityEngine.UI;

public class SelectedModeText : MonoBehaviour
{
    public Text displayText;
    private string currentlySelected;

    void Start() {
        currentlySelected = null;
    }

    public void changeTextElement(string newText) {
        displayText.text = "Currently Selected:\n" + newText;
        currentlySelected = newText;
    }
}
