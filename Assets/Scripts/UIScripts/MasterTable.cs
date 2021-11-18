//
// Author: Thomas Campbell
// Project: B.E.V.I.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterTable : MonoBehaviour
{
    // Menu object
    private GameObject masterTableMenu;
    private bool isOpen;
    // Main Camera Object and Script
    public CameraScript mainCamScript;
    // Layer List Canvas
    public GameObject canvas;
    // Driver Script Object
    public Driver mainDriver;
    // List corresponding to all features in table
    private List<List<Feature>> featureList;
    // List corresponding to names of lists in featureList
    private List<string> featureNames;
    // Prefab for a feature card
    public GameObject featureCardPrefab;
    // List of feature cards for post menu cleanup
    private List<GameObject> cardList;

    // Start is called before the first frame update
    void Start() {
        cardList = new List<GameObject>();
        masterTableMenu = this.gameObject;
        masterTableMenu.SetActive(false);
        isOpen = false;
        updateLists();
    }

    private void updateLists(){
        mainDriver.gm.mt.GetMasterTable(out featureList, out featureNames);
    }

    private void populateMenu() {
        for(int i = 0; i < featureNames.Count; i++) {
            if(featureNames[i] == "STAMP") {
                continue;
            }
            for(int j = 0; j < featureList[i].Count; j++) {
                buildFeatureCard(canvas.transform, featureNames[i], featureList[i][j].contents.topName, featureList[i][j].contents.jx, featureList[i][j].contents.jy);
            }
        }
    }

    private bool buildFeatureCard(Transform position, string typeString, string featureName, int xPos, int yPos) {
        GameObject newCard = Instantiate(featureCardPrefab, position, false);
        Text typeText = newCard.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        Text featNameText = newCard.transform.GetChild(1).GetComponent<Text>();
        Button jumpToButton = newCard.transform.GetChild(2).GetComponent<Button>();
        Text buttonText = newCard.transform.GetChild(2).GetChild(0).GetComponent<Text>();

        typeText.text = typeString;
        featNameText.text = featureName;
        buttonText.text = "Jump to " + typeString;

        jumpToButton.onClick.AddListener(delegate{buttonOnClick(xPos, yPos);});

        cardList.Add(newCard);
        return true;
    }

    void buttonOnClick(int x, int y) {
        mainCamScript.jumpTo(mainDriver.gm.GetWorldPosition(x, y));
        CloseMenu();
    }

    private bool clearMenu() {
        foreach (GameObject card in cardList){
            GameObject.Destroy(card);
        }

        return true;
    }

    /// <summary>
    /// Check if the Master Table menu is open
    /// </summary>
    /// <returns>True if menu currently open</returns>
    public bool checkOpen() {
        return isOpen;
    }

    // Open and close methods below
    public void OpenMenu() {
        masterTableMenu.SetActive(true);
        isOpen = true;
        mainCamScript.menuOpen = true;
        updateLists();
        populateMenu();
    }

    public void CloseMenu() {
        masterTableMenu.SetActive(false);
        isOpen = false;
        mainCamScript.menuOpen = false;
        clearMenu();
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
