/// <summary>
/// Author: Thomas Campbell
/// Project: B.E.V.I.
/// Info Panel Driver and Functionality
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    // Reference to self gameobject
    private GameObject self;
    // InfoBox Content Canvas
    public GameObject canvas;
    // Script to close this specific menu
    public UIMenuCollapse collapseScript;
    // Array of all info components
    private List<GameObject> components;
    // Driver script
    public Driver mainDriver;
    // Prefab Types Below
    public GameObject topCard;
    public GameObject infoSingleLine;
    public GameObject infoMultiLine;
    public GameObject infoAddressLine;
    public GameObject infoMultiLineItalic;
    // Currently selected Feature - Updated when WAF is updated
    private Feature currFeature;
    // Currently selected WAF
    private WAF currWAF;

    // Start is called before the first frame update
    void Start()
    {
        self = this.gameObject;
        components = new List<GameObject>();
        currFeature = null;
        currWAF = null;
        collapseScript.collapseMenu();
    }

    public bool setCurrentWAF(WAF input) {
        if(input == null) {
            return false;
        } else if(input == currWAF && collapseScript.isOpen) {
            // Return if the user has clicked on the same feature
            return true;
        }
        clearInfoBox();
        currWAF = input;
        setCurrentFeature(currWAF.feature);

        return true;
    }

    /// <summary>
    /// Set the currently activated feature.
    /// </summary>
    /// <param name="input">Feature to be set as main feature.</param>
    /// <returns>True if completed.</returns>
    private bool setCurrentFeature(Feature input) {
        currFeature = input;
        populateInfoBox();
        return true;
    }

    /// <summary>
    /// Populates the information box using the feature passed in.
    /// </summary>
    /// <param name="inFeature">Passed in feature with populated InfoWrapper.</param>
    /// <returns>True upon completion.</returns>
    private bool populateInfoBox() {
        buildTopLevel(currFeature.contents, canvas.transform, currFeature.contents.topName, currFeature.contents.jx, currFeature.contents.jy, currFeature.color);

        foreach(InfoField field in currFeature.contents.fields) {
            switch(field.displayStyle) {
                case 0:
                    buildInfoMultiLine(field, canvas.transform, field.fieldName, field.input.ToString());
                    break;
                case 1:
                    buildInfoSingleLine(field, canvas.transform, field.fieldName, field.input.ToString());
                    break;
                case 2:
                    buildInfoAddressLine(field, canvas.transform, field.fieldName, field.input.ToString());
                    break;
                case 3:
                    buildInfoMultiLineItalic(field, canvas.transform, field.fieldName, field.input.ToString());
                    break;
                default:
                    buildInfoMultiLine(field, canvas.transform, field.fieldName, field.input.ToString());
                    break;
            }
        }
        
        return true;
    }

    public bool clearInfoBox() {
        foreach(GameObject card in components) {
            GameObject.Destroy(card);
        }
        return true;
    }

    public bool testEqualWAF(WAF input) {
        return currWAF.equals(input);
    }

    void Update() {
        if(collapseScript.isOpen) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                collapseScript.collapseMenu();
            }
        }
    }

    // ================================================================
    //          Menu Component Building Methods Below
    // ================================================================

    /// <summary>
    /// Builds the toplevel info card for the feature clicked, feature names are limited to 250 words, and adds it to the component list.
    /// </summary>
    /// <param name="position">Transform position [NOTE: Not needed for information menu, as it has an auto sorter].</param>
    /// <param name="featureName">String to replace the feature name.</param>
    /// <param name="x">Grid X position of the feature.</param>
    /// <param name="y">Grid Y position of the feature.</param>
    /// <param name="tint">Color current tint of the feature.</param>
    /// <returns></returns>
    private bool buildTopLevel(InfoWrapper wrapper, Transform position, string featureName, int x, int y, Color tint) {
        GameObject outObject = Instantiate(topCard, position, false);

        InputField featureField = outObject.transform.GetChild(0).GetChild(1).GetComponent<InputField>();
        Image colorPanel = outObject.transform.GetChild(1).GetChild(3).GetComponent<Image>();
        Slider redSlider = outObject.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        Slider greenSlider = outObject.transform.GetChild(1).GetChild(1).GetComponent<Slider>();
        Slider blueSlider = outObject.transform.GetChild(1).GetChild(2).GetComponent<Slider>();
        Text coordText = outObject.transform.GetChild(2).GetChild(1).GetComponent<Text>();

        featureField.text = featureName;
        redSlider.value = (tint.r*100);
        greenSlider.value = (tint.g*100);
        blueSlider.value = (tint.b*100);
        colorPanel.color = tint;
        coordText.text = "X: " + x + "\nY: " + y;

        if((FeatureType)(currWAF.l) == FeatureType.river || (FeatureType)(currWAF.l) == FeatureType.street || (FeatureType)(currWAF.l) == FeatureType.lake) {
            redSlider.interactable = false;
            greenSlider.interactable = false;
            blueSlider.interactable = false;
        } else {
            redSlider.onValueChanged.AddListener(delegate{updateTint(colorPanel, redSlider, greenSlider, blueSlider);});
            greenSlider.onValueChanged.AddListener(delegate{updateTint(colorPanel, redSlider, greenSlider, blueSlider);});
            blueSlider.onValueChanged.AddListener(delegate{updateTint(colorPanel, redSlider, greenSlider, blueSlider);});
        }
        featureField.onEndEdit.AddListener(delegate{applyTopLevel(wrapper, featureField);});

        components.Add(outObject);
        return true;
    }

    /// <summary>
    /// Apply changes to top level wrapper on completion of edit.
    /// </summary>
    /// <param name="wrapper">The InfoWrapper to be edited.</param>
    /// <param name="saveString">The inputfild holding user edited string.</param>
    private void applyTopLevel(InfoWrapper wrapper, InputField saveString) {
        wrapper.topName = saveString.text;
    }

    /// <summary>
    /// Helper Method for building a toplevel card.
    /// </summary>
    private void updateTint(Image img, Slider redSlider, Slider greenSlider, Slider blueSlider) {
        //if(!Input.anyKey) {
            Color temp = new Color((redSlider.value/100),(greenSlider.value/100),(blueSlider.value/100), currFeature.color.a);
            img.color = temp;
            currFeature.color = temp;
            mainDriver.gm.mt.GetLayer(currWAF.l - 1).TintFeature(currFeature, (FeatureType)(currWAF.l));
        //}
    }

    /// <summary>
    /// Builds a single line string information panel ("[Title]: [Info]"), with a character limit of 250, and adds it to the component list.
    /// </summary>
    /// <param name="position"> Transform position [NOTE: Not needed for information menu, as it has an auto sorter].</param>
    /// <param name="titleString">String to replace the component title.</param>
    /// <param name="infoString">String to replace the information.</param>
    /// <returns>Boolean True if completed.</returns>
    private bool buildInfoSingleLine(InfoField field, Transform position, string titleString, string infoString) {
        GameObject outObject = Instantiate(infoSingleLine, position, false);
        Text titleText = outObject.transform.GetChild(0).GetComponent<Text>();
        InputField infoText = outObject.transform.GetChild(1).GetComponent<InputField>();

        titleText.text = titleString;
        infoText.text = infoString;

        infoText.onEndEdit.AddListener(delegate{saveEdit(field, infoText);});

        components.Add(outObject);
        return true;
    }

    /// <summary>
    /// Builds a single line string information panel ("[Title]: [Info]"), with a character limit of 50, and adds it to the component list.
    /// </summary>
    /// <param name="position"> Transform position [NOTE: Not needed for information menu, as it has an auto sorter].</param>
    /// <param name="titleString">String to replace the component title.</param>
    /// <param name="infoString">String to replace the information.</param>
    /// <returns>Boolean True if completed.</returns>
    private bool buildInfoAddressLine(InfoField field, Transform position, string titleString, string infoString) {
        GameObject outObject = Instantiate(infoAddressLine, position, false);
        Text titleText = outObject.transform.GetChild(0).GetComponent<Text>();
        InputField infoText = outObject.transform.GetChild(1).GetComponent<InputField>();

        titleText.text = titleString;
        infoText.text = infoString;

        infoText.onEndEdit.AddListener(delegate{saveEdit(field, infoText);});

        components.Add(outObject);
        return true;
    }

    /// <summary>
    /// Builds a multi line information panel ("[Title]:\n [Multi line text]"), with a character limit of 5000, and adds it to the component list.
    /// </summary>
    /// <param name="position">Transform position [NOTE: Not needed for information menu, as it has an auto sorter].</param>
    /// <param name="titleString">String to replace the component title.</param>
    /// <param name="infoString">String to replace the information field.</param>
    /// <returns>Boolean True if completed.</returns>
    private bool buildInfoMultiLine(InfoField field, Transform position, string titleString, string infoString) {
        GameObject outObject = Instantiate(infoMultiLine, position, false);
        Text titleText = outObject.transform.GetChild(0).GetComponent<Text>();
        InputField infoText = outObject.transform.GetChild(1).GetChild(0).GetComponent<InputField>();

        titleText.text = titleString;
        infoText.text = infoString;

        infoText.onEndEdit.AddListener(delegate{saveEdit(field, infoText);});

        components.Add(outObject);
        return true;
    }

    /// <summary>
    /// Builds a multi line italic information panel ("[Title]:\n [Multi line text]"), with a character limit of 5000, and adds it to the component list.
    /// </summary>
    /// <param name="position">Transform position [NOTE: Not needed for information menu, as it has an auto sorter].</param>
    /// <param name="titleString">String to replace the component title.</param>
    /// <param name="infoString">String to replace the information field.</param>
    /// <returns>Boolean True if completed.</returns>
    private bool buildInfoMultiLineItalic(InfoField field, Transform position, string titleString, string infoString) {
        GameObject outObject = Instantiate(infoMultiLineItalic, position, false);
        Text titleText = outObject.transform.GetChild(0).GetComponent<Text>();
        InputField infoText = outObject.transform.GetChild(1).GetChild(0).GetComponent<InputField>();

        titleText.text = titleString;
        infoText.text = infoString;

        infoText.onEndEdit.AddListener(delegate{saveEdit(field, infoText);});

        components.Add(outObject);
        return true;
    }

    /// <summary>
    /// Save the changes to the field that the user had made.
    /// </summary>
    /// <param name="field">InfoField containing feature field information.</param>
    /// <param name="saveString">InputField containing data to be saved.</param>
    /// <returns></returns>
    private bool saveEdit(InfoField field, InputField saveString) {
        field.input.Clear();
        field.input.Append(saveString.text);
        return true;
    }
}
