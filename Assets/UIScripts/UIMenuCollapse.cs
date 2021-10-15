using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuCollapse : MonoBehaviour
{
    private GameObject menu;
    public GameObject minimisedMenu;

    // Start is called before the first frame update
    void Start()
    {
        menu = this.gameObject;
    }

    public void collapseMenu() {
        menu.SetActive(false);
        minimisedMenu.SetActive(true);
    }

    public void expandMenu() {
        menu.SetActive(true);
        minimisedMenu.SetActive(false);
    }
}
