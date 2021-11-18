using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Thomas Campbell
/// Project: B.E.V.I.
/// Settings menu driver script
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    // Menu object
    private GameObject settingsMenu;
    private bool isOpen;
    [Header("Main Scripts...")]
    // Main Camera Object and Script
    public CameraScript mainCamScript;
    // Main Driver Script
    public Driver mainDriver;
    [Header("Fullscreen Settings Objects...")]
    // Fullscreen Setting Variables
    public GameObject fullscreenToggle;
    [Header("Resolution Settings Objects...")]
    // Resolution Setting Variables
    public Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private int selectedResIndex;
    [Header("Background Color Settings Objects...")]
    // Background Color Setting Variables
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public GameObject colorPrevPanel;
    private Image colorPreview;
    [Header("Rotation Settings Objects...")]
    // Rotation Speed Setting Variables
    public Slider rotationSlider;
    private int rotationSpeed;
    [Header("Gridlines Settings Objects...")]
    // Gridlines Settings Variables
    public GameObject gridLineToggle;

    // Start is called before the first frame update
    void Start() {
        resolutions = Screen.resolutions;
        colorPreview = colorPrevPanel.GetComponent<Image>();
        settingsMenu = this.gameObject;
        settingsMenu.SetActive(false);
        isOpen = false;
    }

    void populateMenuOptions() {
        /* ======================================= RESOLUTION SETTINGS LITERALLY WOULDNT WORK, HAD TO IMPROVISE.
        // Populate Fullscreen Toggle
        fullscreenToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(Screen.fullScreen);

        // Populate the Resolution Dropdowns
        resolutionDropdown.ClearOptions();
        List<string> rOptions = new List<string>();
        int currentResIndex = 0;
        for(int i = 0; i < resolutions.Length; i++) {
            Resolution temp = (resolutions[i]);
            if(!rOptions.Contains(temp.ToString())) {
                rOptions.Add(temp.ToString());
            }
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentResIndex = i;
            }
        }
        resolutionDropdown.AddOptions(rOptions);
        resolutionDropdown.value = currentResIndex;
        */

        // Setup Background Color Text
        redSlider.value  = (Camera.main.backgroundColor.r*100);
        greenSlider.value = (Camera.main.backgroundColor.g*100);
        blueSlider.value = (Camera.main.backgroundColor.b*100);

        // Setup Rotation Speed Text
        rotationSlider.value = (mainCamScript.rotationSpeed);

        // Set GridLines Value
        gridLineToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(mainDriver.getGlVisible());
    }

    /// <summary>
    /// Check if the settings menu is open
    /// </summary>
    /// <returns>True if menu currently open</returns>
    public bool checkOpen() {
        return isOpen;
    }

    // Open and close methods below
    public void OpenMenu() {
        settingsMenu.SetActive(true);
        isOpen = true;
        mainCamScript.menuOpen = true;
        populateMenuOptions();
    }

    public void CloseMenu() {
        settingsMenu.SetActive(false);
        isOpen = false;
        mainCamScript.menuOpen = false;
    }

    // Apply all currently selected settings
    public void Apply() {
        /* =============================== SEE EARLIER COMMENT ABOUT RESOLUTION SETTINGS
        // Apply Fullscreen
        if(fullscreenToggle.GetComponent<Toggle>().isOn) {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        } else {
            Screen.fullScreen = false;
        }
        // Apply Resolution
        Resolution res = resolutions[resolutionDropdown.value];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        */
        // Apply background color
        Camera.main.backgroundColor = new Color((redSlider.value/100),(greenSlider.value/100),(blueSlider.value/100));
        // Apply rotation speed
        mainCamScript.rotationSpeed = (int)rotationSlider.value;
    }

    // Called every frame
    void Update() {
        // Skip all actions if the meny is not open.
        if(!isOpen) {
            return;
        }

        // Update the color preview panel
        colorPreview.color = new Color((redSlider.value/100),(greenSlider.value/100),(blueSlider.value/100));

        // Check for pressing of escape to leave menu
        if(Input.GetKey(KeyCode.Escape)) {
            CloseMenu();
        }
    }
}
