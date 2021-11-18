/// <summary>
/// Author: Thomas Campbell
/// Project: B.E.V.I.
/// Functions and Functionality for Build Menu
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuFunctions : MonoBehaviour
{

    // Menu Objects Below
    /// <summary>
    /// Material submenu GameObject
    /// </summary>
    public GameObject materialMenu;
    /// <summary>
    /// Script to collapse this menu.
    /// </summary>
    public UIMenuCollapse collapseScript;
    /// <summary>
    /// Feature submenu GameObject
    /// </summary>
    public GameObject featureMenu;
    /// <summary>
    /// Stamp submenu GameObject
    /// </summary>
    public GameObject stampMenu;
    /// <summary>
    /// Private GameObject reference to the current open menu
    /// </summary>
    private GameObject currentOpenMenu = null;
    /// <summary>
    /// True if a submenu of the build menu is open
    /// </summary>
    public bool subMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        closeAllMenus();
        subMenuOpen = false;
        collapseScript.collapseMenu();
    }

    public GameObject getCurrentMenu() {
        return currentOpenMenu;
    }

    public void closeAllMenus() {
        closeMenu(materialMenu);
        closeMenu(featureMenu);
        closeMenu(stampMenu);
        subMenuOpen = false;
    }

    public void openMaterials() {
        if(currentOpenMenu == materialMenu) {
            closeAllMenus();
            return;
        }
        closeAllMenus();
        materialMenu.SetActive(true);
        currentOpenMenu = materialMenu;
        subMenuOpen = true;
    }

    public void openFeatures() {
        if(currentOpenMenu == featureMenu) {
            closeAllMenus();
            return;
        }
        closeAllMenus();
        featureMenu.SetActive(true);
        currentOpenMenu = featureMenu;
        subMenuOpen = true;
    }

    public void openStamps() {
        if(currentOpenMenu == stampMenu) {
            closeAllMenus();
            return;
        }
        closeAllMenus();
        stampMenu.SetActive(true);
        currentOpenMenu = stampMenu;
        subMenuOpen = true;
    }

    private void closeMenu(GameObject menu) {
        menu.SetActive(false);
        currentOpenMenu = null;
    }

    void Update() {
        if(currentOpenMenu == null) {
            subMenuOpen = false;
            return;
        }

        if(Input.GetKey(KeyCode.Escape)) {
            closeAllMenus();
        }
    }
}
