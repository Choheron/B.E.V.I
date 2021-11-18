using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vanity;


public struct IntPair{
    public int x, y;

    public IntPair(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

/// <summary>
/// Author: Thomas Campbell, January Nelson
/// Project: B.E.V.I.
/// System Driver
/// </summary>
public class Driver : MonoBehaviour
{
    public GridMap gm;
    /// <summary>
    /// Reference to main camera script.
    /// </summary>
    public CameraScript mainCam;
    /// <summary>
    /// Reference to the script of the currently selected mode text.
    /// </summary>
    public SelectedModeText currSelTextScript;
    /// <summary>
    /// Build Menu Function Script.
    /// </summary>
    public BuildMenuFunctions buildMenScript;
    // Variables for in editor changes to grid - Thomas
    public int width = 15;
    public int height = 15;
    public float cs = 5f;
    private Vector3 origin = new Vector3(0f,0f);
    public MaterialType selectedBrush = 0;
    private FeatureType selectedFeature = 0;
    private StampType selectedStamp = 0;
    private MapDirection placementRotation = MapDirection.down;
    public int editorMode;
    private IntPair s1, s2, sf, sb;
    private bool glVisible = true;
    private bool[,] inOneClick;
    [Header("DO NOT EDIT")]
    public bool postToggle = false;
    private int linesRelevance = 0;
    public Image spriteArrow;

    // Start is called before the first frame update
    void Start()
    {
        gm = new GridMap(width, height, cs, origin);
        glVisible = true;
        inOneClick = new bool[width,height];
        s1 = s2 = sf = sb = new IntPair(-1, -1);
        gm.NewMap();

        mainCam.jumpTo(gm.GetWorldPosition(width/2, height/2));
    }

    public void setEdMode(int edMode){
        selectedBrush = 0;
        selectedStamp = 0;
        if(edMode != 3){
            s1 = s2 = sf = sb = new IntPair(-1, -1);
        }
        editorMode = edMode;
        //Debug.Log("Just changed editor mode!");
    }
    
    public void Save(){
        SaveSystem.SaveGrid(gm);
    }

    public void Load(){
        GD_OVERSEER gridData = SaveSystem.LoadGrid();
        gm.KittenCaboodle(gridData);
        
    }
    public void setBrushType(int materialInt) {
        setEdMode(0);
        selectedBrush = (MaterialType)materialInt;
        
    }

    public void setSelectedStamp(int selStamp) {
        setEdMode(4);
        selectedStamp = (StampType)selStamp;
        
    }

    public void setFeature(int selFeature) {
        setEdMode(2);
        selectedFeature = (FeatureType)selFeature;
        if(selectedFeature == FeatureType.river || selectedFeature == FeatureType.street){
            linesRelevance = 1;
        }
        
    }

    /// <summary>
    /// Current status of grid line display
    /// </summary>
    /// <returns>Boolean true if grid lines are being displayed, false if not</returns>
    public bool getGlVisible() {
        return glVisible;
    }

    public void StmpRotL(){
        //Debug.Log("Test left");
        MapDirection temp = placementRotation;
        switch(temp){
            case MapDirection.down:
                placementRotation = MapDirection.left; 
                break;
            case MapDirection.left:
                placementRotation = MapDirection.up;
                break;
            case MapDirection.up:
                placementRotation = MapDirection.right;
                break;
            case MapDirection.right:
                placementRotation = MapDirection.down;
                break;    
        }
        spriteArrow.transform.eulerAngles = new Vector3(0, 0, (int)placementRotation);
        //Debug.Log(placementRotation);
    }

    public void StmpRotR(){
        //Debug.Log("Test right");
        switch(placementRotation){
            case MapDirection.down:
                placementRotation = MapDirection.right;
                break;
            case MapDirection.left:
                placementRotation = MapDirection.down;
                break;
            case MapDirection.up:
                placementRotation = MapDirection.left;
                break;
            case MapDirection.right:
                placementRotation = MapDirection.up;
                break;    
        }
        spriteArrow.transform.eulerAngles = new Vector3(0, 0, (int)placementRotation);
        Debug.Log(placementRotation);
    }

    /// <summary>
    /// Toggle map gridlines
    /// </summary>
    public void toggleGridLines() {
        if(glVisible) {
            glVisible = false;
            gm.HideGridlines();
        }
        else {
            glVisible = true;
            gm.ShowGridlines();
        }
    }

    void Update() {
        //REGARDLESS OF INPUT
        if(editorMode == 3){
            gm.GetXY(Camera.main.ScreenToWorldPoint(Input.mousePosition), out s2.x, out s2.y);//s2 is repeatedly set
            gm.GlowYaSquares(s1, s2, EditorType.GlowGoodV1, out sf, out sb); //saves SF, which allows the top right to jump higher.
        }
        //NO INPUT
        if(!Input.anyKey){
            // If G is released, stop moving the camera
            postToggle = false;
            if(Input.GetMouseButtonUp(0) && editorMode == 0){
                for(int x = 0; x < inOneClick.GetLength(0); x++){
                    for(int y = 0; y < inOneClick.GetLength(1); y++){
                        inOneClick[x,y] = false;
                    }
                }
               
            }
            return;//Speeds update quite significantly.
        }
        //EXIT CURRENT MODE / CLEAR BRUSH
        else{
            // Toggle Gridlines with Ctrl+G
            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
                if(Input.GetKey(KeyCode.G)) {
                    if(!postToggle) {
                        toggleGridLines();
                        postToggle = true;
                        return;
                    }
                }
            }
            if(Input.GetKey(KeyCode.Escape)){
                setEdMode(0);
                currSelTextScript.changeTextElement("Selection Mode");
                buildMenScript.closeAllMenus();
            }
            switch(editorMode){
                case 0://BRUSH / DEFAULT MODE
                    //paint material
                    if(Input.GetMouseButton(0)) {
                        if(!Utils.AcknowledgeUI()) {
                            if(!(selectedBrush == 0)) {
                                int tempx = 0;
                                int tempy = 0;
                                gm.GetXY(Camera.main.ScreenToWorldPoint(Input.mousePosition), out tempx, out tempy);
                                if(gm.ValidCoords(tempx, tempy)){
                                    if(!inOneClick[tempx, tempy]){
                                        inOneClick[tempx, tempy] = true;
                                        gm.SetMaterial(tempx, tempy, selectedBrush);
                                    }
                                }
                            }
                        }
                    }
                    //EDITABILITY RESTORE     
                    return;//END DEFAULT ACCESIBLE FEATURES
                case 1: //PLACING DOT MODE
                    if(Input.GetMouseButtonDown(0)) {
                        if(!Utils.AcknowledgeUI()) {
                            int tempx = 0;
                            int tempy = 0;
                            gm.GetXY(Camera.main.ScreenToWorldPoint(Input.mousePosition), out tempx, out tempy);
                            if(gm.ValidCoords(tempx, tempy)){
                                gm.PutDot(tempx, tempy, FeatureType.poi);
                                return;
                            }
                        }
                    }
                    return;//END DOT MODE
                case 2://CREATE LINE/AREA STEP ONE
                    if(!postToggle){
                        if(Input.GetMouseButtonDown(0)) {
                            if(!Utils.AcknowledgeUI()) {
                                int tempx, tempy;
                                gm.GetXY(Camera.main.ScreenToWorldPoint(Input.mousePosition), out tempx, out tempy);
                                if(gm.ValidCoords(tempx, tempy)){
                                    s1 = new IntPair(tempx, tempy);
                                    //glow the square
                                    postToggle = true;
                                    setEdMode(3);
                                    return;
                                }
                            }
                        }
                    }
                    return;
                case 3://CREATE LINE/AREA STEP TWO
                    if(!postToggle){
                        if(Input.GetMouseButtonDown(0) && !Utils.AcknowledgeUI()){
                            //sf is our first, s2 is our second
                            postToggle = true;
                            gm.UnGlowAll();
                            if(linesRelevance > 0){
                                //1 is horizontal, 2 is vertical
                                gm.PutLine(sf.x, sf.y, sb.x, sb.y, selectedFeature);
                            }
                            else gm.PutArea(sf.x, sf.y, sb.x, sb.y, selectedFeature);
                            Debug.Log("I tried to make a box between (" + sf.x + ", " + sf.y + ") and (" + sb.x + ", " + sb.y + ")");
                            setEdMode(2); //once the area is created, return to step one
                        }
                    }
                    return;
                case 4: //PLACING STAMP MODE
                    if(!postToggle){
                        //ROTATION OF STAMP!
                        if(Input.GetKey(KeyCode.LeftArrow)){
                            StmpRotL();
                            postToggle = true;
                            return;
                        } 
                        if(Input.GetKey(KeyCode.RightArrow)){
                            StmpRotR();
                            postToggle = true;
                            return;
                        }
                    }
                    if(Input.GetMouseButtonDown(0)) {
                        if(!Utils.AcknowledgeUI()) {
                            if(!(selectedStamp == 0)) {
                                int tempx = 0;
                                int tempy = 0;
                                gm.GetXY(Camera.main.ScreenToWorldPoint(Input.mousePosition), out tempx, out tempy);
                                if(gm.ValidCoords(tempx, tempy)){
                                    gm.PutDot(tempx, tempy, FeatureType.stamp, selectedStamp, placementRotation);
                                }
                            }
                        }
                    }
                    return;
            }
        }
    }
}
