using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Thomas Campbell
/// Project: B.E.V.I.
/// Basic open Close Menu Script
/// </summary>
public class BasicMenu : MonoBehaviour
{
    /// <summary>
    /// Gameobject reference to menu being opened and closed
    /// </summary>
    public GameObject menu;
    /// <summary>
    /// Track if menu is open or closed
    /// </summary>
    private bool isOpen;

    void Start() {
        isOpen = false;
    }

    public void closeMenu() {
        menu.SetActive(false);
        isOpen = false;
    }

    public void openMenu() {
        menu.SetActive(true);
        isOpen = true;
    }

    public void toggleMenu() {
        if(isOpen) {
            closeMenu();
        } else {
            openMenu();
        }
    }
}
