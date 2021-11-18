//
// Author: Thomas Campbell
// Project: B.E.V.I.
//
using UnityEngine;

public class UIMenuCollapse : MonoBehaviour
{
    private GameObject menu;
    public GameObject minimizedMenu;

    public bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        menu = this.gameObject;
    }

    public void collapseMenu() {
        menu.SetActive(false);
        minimizedMenu.SetActive(true);
        isOpen = false;
    }

    public void expandMenu() {
        menu.SetActive(true);
        minimizedMenu.SetActive(false);
        isOpen = true;
    }
}
