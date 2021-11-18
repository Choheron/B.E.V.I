//
// Author: Thomas Campbell
// Project: B.E.V.I.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerTable : MonoBehaviour
{
    // Menu object
    private GameObject layerViewMenu;
    private bool isOpen;
    // Main Camera Object and Script
    public CameraScript mainCamScript;
    // Driver Script Object
    public Driver mainDriver;
    // Cards for each feature
    private GameObject streetCard;
    private GameObject lakeCard;
    private GameObject riverCard;
    private GameObject stampCard;
    private GameObject buildingCard;
    private GameObject regionCard;
    private GameObject poiCard;

    // Start is called before the first frame update
    void Start() {
        layerViewMenu = this.gameObject;
        registerCards();
        layerViewMenu.SetActive(false);
        isOpen = false;
    }

    private void registerCards() {
        // Register cards to their variables
        streetCard = layerViewMenu.transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        lakeCard = layerViewMenu.transform.GetChild(0).GetChild(2).GetChild(1).gameObject;
        riverCard = layerViewMenu.transform.GetChild(0).GetChild(2).GetChild(2).gameObject;
        stampCard = layerViewMenu.transform.GetChild(0).GetChild(2).GetChild(3).gameObject;
        buildingCard = layerViewMenu.transform.GetChild(0).GetChild(2).GetChild(4).gameObject;
        regionCard = layerViewMenu.transform.GetChild(0).GetChild(2).GetChild(5).gameObject;
        poiCard = layerViewMenu.transform.GetChild(0).GetChild(2).GetChild(6).gameObject;

        // Register each toggle button to toggle the layer being visible
        streetCard.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener(delegate{toggleLayer(FeatureType.street);});
        lakeCard.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener(delegate{toggleLayer(FeatureType.lake);});
        riverCard.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener(delegate{toggleLayer(FeatureType.river);});
        stampCard.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener(delegate{toggleLayer(FeatureType.stamp);});
        buildingCard.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener(delegate{toggleLayer(FeatureType.building);});
        regionCard.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener(delegate{toggleLayer(FeatureType.region);});
        poiCard.transform.GetChild(2).GetComponent<Toggle>().onValueChanged.AddListener(delegate{toggleLayer(FeatureType.poi);});
    }

    private void toggleLayer(FeatureType ft) {
        mainDriver.gm.mt.GetLayer(ft).toggleLayer();
    }

    private void updateCards() {
        streetCard.transform.GetChild(1).GetComponent<Text>().text = mainDriver.gm.mt.GetLayer(FeatureType.street).lyrContents.Count.ToString();
        lakeCard.transform.GetChild(1).GetComponent<Text>().text = mainDriver.gm.mt.GetLayer(FeatureType.lake).lyrContents.Count.ToString();
        riverCard.transform.GetChild(1).GetComponent<Text>().text = mainDriver.gm.mt.GetLayer(FeatureType.river).lyrContents.Count.ToString();
        stampCard.transform.GetChild(1).GetComponent<Text>().text = mainDriver.gm.mt.GetLayer(FeatureType.stamp).lyrContents.Count.ToString();
        buildingCard.transform.GetChild(1).GetComponent<Text>().text = mainDriver.gm.mt.GetLayer(FeatureType.building).lyrContents.Count.ToString();
        regionCard.transform.GetChild(1).GetComponent<Text>().text = mainDriver.gm.mt.GetLayer(FeatureType.region).lyrContents.Count.ToString();
        poiCard.transform.GetChild(1).GetComponent<Text>().text = mainDriver.gm.mt.GetLayer(FeatureType.poi).lyrContents.Count.ToString();
    }

    /// <summary>
    /// Check if the Layer View menu is open
    /// </summary>
    /// <returns>True if menu currently open</returns>
    public bool checkOpen() {
        return isOpen;
    }

    // Open and close methods below
    public void OpenMenu() {
        layerViewMenu.SetActive(true);
        isOpen = true;
        mainCamScript.menuOpen = true;
        updateCards();
    }

    public void CloseMenu() {
        layerViewMenu.SetActive(false);
        isOpen = false;
        mainCamScript.menuOpen = false;
    }

    // Update is called once per frame
    void Update() {
        // Skip all actions if the menu is not open.
        if(!isOpen) {
            return;
        }

        // Check for pressing of escape to leave menu
        if(Input.GetKey(KeyCode.Escape)) {
            CloseMenu();
        }
    }
}