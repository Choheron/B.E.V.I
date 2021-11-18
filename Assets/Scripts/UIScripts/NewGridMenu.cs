/// <summary>
/// Author: Thomas Campbell
/// Project: B.E.V.I.
/// New Map Menu and functionality
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGridMenu : MonoBehaviour
{
    /// <summary>
    /// Self object (Entire menu including background).
    /// </summary>
    private GameObject self;
    /// <summary>
    /// Link to main driver script.
    /// </summary>
    public Driver mainDriver;
    /// <summary>
    /// Script reference to the main camera.
    /// </summary>
    public CameraScript mainCam;
    /// <summary>
    /// Text showing current map parameters.
    /// </summary>
    public Text parameterText;
    /// <summary>
    /// Button for generating a new map.
    /// </summary>
    public Button genNewMapButton;
    /// <summary>
    /// Inputfield corresponding to the width option.
    /// </summary>
    private InputField widthField;
    /// <summary>
    /// Inputfield corresponding to the height option.
    /// </summary>
    private InputField heightField;
    /// <summary>
    /// MaterialType for the currently selected Material.
    /// </summary>
    private MaterialType currMaterial;
    /// <summary>
    /// Info Box driver script.
    /// </summary>
    public InfoBox infoBroxScript;

    void Start() {
        self = this.gameObject;
        widthField = self.transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<InputField>();
        heightField = self.transform.GetChild(0).GetChild(2).GetChild(5).GetComponent<InputField>();
        currMaterial = MaterialType.Grass;

        genNewMapButton.onClick.AddListener(delegate{generateNewMap();});

        updateParamText();
    }

    private void generateNewMap() {
        int width = Int32.Parse(widthField.text);
        int height = Int32.Parse(heightField.text);
        if(width <= 1) {
            width = 2;
        }
        if(height <= 1) {
            height = 2;
        }
        mainDriver.gm.NewMap(width, height, currMaterial);

        mainCam.jumpTo(mainDriver.gm.GetWorldPosition(width/2, height/2));
        if(infoBroxScript.collapseScript.isOpen) {
            infoBroxScript.clearInfoBox();
            infoBroxScript.collapseScript.collapseMenu();
        }
    }

    public void setCurrentMaterial(int matIn) {
        currMaterial = (MaterialType)matIn;
    }

    public void closeMenu() {
        self.SetActive(false);
    }

    public void updateParamText() {
        parameterText.text = ("Material: " + currMaterial + "\n" +
                              "Width: " + widthField.text + "\n" +
                              "Height: " + heightField.text);
    }
}
