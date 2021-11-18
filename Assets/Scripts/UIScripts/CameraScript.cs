using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vanity;

/// <summary>
/// Author: Thomas Campbell
/// Project: B.E.V.I.
/// Main Camera Driver Script
/// </summary>
public class CameraScript : MonoBehaviour {
    [Header("Main Driver Script...")]
    /// <summary>
    /// Script link to main driver
    /// </summary>
    public Driver mainDriver;
    [Header("Camera Movement Variables...")]
    // Camera Movement Public Variables
    public int maxZoom = 15;
    public int minZoom = 3;
    [Header("Camera Home Variables...")]
    // Camera Home Public Variables
    public float homeX = 0.0f;
    public float homeY = 0.0f;
    [Header("Camera Panning Variables...")]
    // Camera Panning Private Variables
    private Vector2 mouseClickPos;
    private Vector2 mouseCurrentPos;
    [Header("Camera Rotation Variables...")]
    // Camera Rotation Private Variables
    private Vector2 rotMouseClickPos;
    private Vector2 rotMouseCurrentPos;
    [Header("Menu Open Variables (DO NOT TOUCH)...")]
    /// <summary>
    /// Menu Variable - Boolean. True if user is in menu (thus stopping any camera movement) false if user is not in menu. Can be set using setters or calling the variable as it is public.
    /// </summary>
    public bool menuOpen = false;
    /// <summary>
    /// Boolean value to lock rotation until the rotate key is released.
    /// </summary>
    private bool isRotating;
    [Header("Rotation Speed...")]
    /// <summary>
    /// Rotation Variable - Float. Speed with which to rotate the camera.
    /// </summary>
    public int rotationSpeed = 40;
    /// <summary>
    /// Boolean showing sprite being placed.
    /// </summary>
    private bool placingSprite = false;
    /// <summary>
    /// Current Sprite being placed.
    /// </summary>
    public Sprite currSprite = null;
    [Header("InfoBox Variable Scripts...")]
    /// <summary>
    /// Script controlling the infobox and its maximizing.
    /// </summary>
    public UIMenuCollapse infoBoxMaster;
    /// <summary>
    /// Script for main infobox.
    /// </summary>
    public InfoBox infoBox;

    

    // Called on launch
    void Start() {
        // If editor indicates no limit on zooming out (by inputting 0), then set limit to 1000.
        if(maxZoom == 0) {
            maxZoom = int.MaxValue;
        }

        //mouseFollower.sprite = Vanity.AtlasHandler.loadImage(AtlasHandler.editor, "edtrClear");
    }

    /// <summary>
    /// Resets the Z rotation of the camera.
    /// </summary>
    public void resetRotation() {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    public void togglePlacing() {
        placingSprite = !placingSprite;
    }

    public void jumpTo(Vector3 input) {
        transform.position = input + new Vector3(0, 0, -10);
    }
    
    void Update() {
        if(mainDriver.editorMode == 0 && mainDriver.selectedBrush == 0 && Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            WAF temp;
            if(Physics.Raycast(ray, out hit)){
                // hit.collider.gameObject.TryGetComponent<WAF>(out temp);
                if(hit.collider.gameObject.TryGetComponent<WAF>(out temp)) {
                    if(!infoBoxMaster.isOpen) {
                        infoBoxMaster.expandMenu();
                    } else if(!infoBox.testEqualWAF(temp)){
                        infoBox.clearInfoBox();
                    }
                    if(temp.feature != null)
                        infoBox.setCurrentWAF(temp);
                }
                return;
            }
        }
    }

    // Update is called once at the end of each frame - Current update function allows the user to click and drag with middle mouse click.
    void LateUpdate() {
        
        // Camera cannot change anything while a menu is open
        if(menuOpen || EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        // Zoom In and Out Code - Auto resize gridlines
            if(Input.GetAxis("Mouse ScrollWheel") < 0f ) { // Zoom Out 
                if(Camera.main.orthographicSize + 1 > maxZoom) {
                    return;
                }
                Camera.main.orthographicSize++;
                mainDriver.gm.ScaleGridlines();
            }
            else if(Input.GetAxis("Mouse ScrollWheel") > 0f ) { // Zoom In
                if(Camera.main.orthographicSize - 1 < minZoom) {
                    return;
                }
                Camera.main.orthographicSize--;
                mainDriver.gm.ScaleGridlines();
            }
        if(!Input.anyKey){//NO INPUT?
            if(mouseClickPos != default){
                if(Input.GetKeyUp(KeyCode.Mouse2)) {
                    mouseClickPos = default;
                }
            }
            return;
        }
        else{
            // Rotation Code (Placed here to make it so that the user can still rotate if they are moused over a UI element)
            if(Input.GetKey(KeyCode.LeftBracket)) {
                transform.RotateAround(transform.position, -Vector3.forward, rotationSpeed * Time.deltaTime);
                return;
            }
            if(Input.GetKey(KeyCode.RightBracket)) {
                transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
                return;
            }

            // When MMB clicked get mouse click position and start panning
            if(Input.GetKey(KeyCode.Mouse2))
            {
                if (mouseClickPos == default)
                {
                    mouseClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
    
                mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 distance = mouseCurrentPos - mouseClickPos;
                transform.position += new Vector3(-distance.x, -distance.y, 0);
            }

            // Camera Home Position Code
            // Return Camera to Home
            if(Input.GetKey(KeyCode.Home)) {
                transform.position = new Vector3(homeX, homeY, -10);
            }

            // Set new Camera Home
            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
                if(Input.GetKey(KeyCode.H)) {
                    homeX = transform.position.x;
                    homeY = transform.position.y;
                }
            }
        }
    }
}
