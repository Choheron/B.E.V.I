using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanity;

/// <summary>
/// Author: January Nelson, Thomas Campbell
/// Project: B.E.V.I.
/// GridMap Class
/// </summary>
public class GridMap
{
    /// <summary>
    /// Size of a Square in the grid.
    /// </summary>
    public float cellSize;
    public int width, height;
    public MapTable mt;
    public GlowSquare[,] glowRealm;
    /// <summary>
    /// "Top-level controller": basically just a representation of the grid in the hierarchy.
    /// </summary>
    public GameObject tlc;
    /// <summary>
    /// Contains gridline renderers.
    /// </summary>
    public List<LineRenderer> gridlines;
    /// <summary>
    /// Vector position where the grid began drawing.
    /// </summary>
    public Vector3 originPos;
    /// <summary>
    /// Grid data.
    /// </summary>
    public Square[,] data;
    /// <summary>
    /// Constructor for a GridMap
    /// </summary>
    /// <param name="width"> Width of GridMap to make </param>
    /// <param name="height"> Height of GridMap to make </param>
    /// <param name="cs">Cell Size</param>
    /// <param name="origin"> GripMap Origin</param>
    public GridMap(int width, int height, float cs, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cs;
        originPos = origin;
        
        //SetMaterial(2, 1, 56);
        //Debug.Log("Grid generated: w:" + width + " h:" + height);
    }

    public void NewMap(){
        NewMap(width, height);
    }

    /// <summary>
    /// SPECIAL NOTICE: THIS METHOD REMOVES *ALL DATA* FROM A GRID!!<br />
    /// ONLY CALL IT IF THAT'S FINE! -J
    /// </summary>
    public void NewMap(int w, int h, MaterialType materlterp = MaterialType.Grass) {
        if(tlc != null) GameObject.DestroyImmediate(tlc);
        if(mt != null) mt.Clean();
        mt = null;
        data = new Square[w, h];
        width = w;
        height = h;
        glowRealm = new GlowSquare[width, height];
        tlc = new GameObject("GridMap (the constant)");
        gridlines = new List<LineRenderer>();
        ANewDay();
        mt = new MapTable(this);
        for(int x = 0; x < data.GetLength(0); x++){
            for(int y = 0; y < data.GetLength(1); y++){
                SetMaterial(x,y, materlterp);
            }
        }
    }

    /// <summary>
    /// Instantiates grid squares, glow squares, info, gridlines.
    /// </summary>
    public void ANewDay(){
        List<Vector3> pos = new List<Vector3>();
        for(int x = 0; x <= data.GetLength(0); x++)
        {
            GameObject b = new GameObject("X GridLine " + (x), typeof(LineRenderer));
            pos.Clear();
            pos.Add(GetWorldPosition(x,0));
            pos.Add(GetWorldPosition(x, data.GetLength(1)));
            b.GetComponent<LineRenderer>().SetPositions(pos.ToArray());
            b.GetComponent<LineRenderer>().useWorldSpace = true;
            gridlines.Add(b.GetComponent<LineRenderer>());
            if(x <= data.GetUpperBound(0)){
                GameObject rowtop = new GameObject("Column " + (x + 1));
                for(int y = 0; y <= data.GetLength(1); y++)
                {
                    if(y <= data.GetUpperBound(1)){
                        data[x,y] = new Square(x, y, GetWorldPosition(x,y), cellSize);
                        data[x,y].debugVisual.transform.SetParent(rowtop.transform);
                        glowRealm[x,y] = new GlowSquare(x, y, GetWorldPosition(x,y), cellSize);
                        glowRealm[x,y].visual.name = "glow @ " + x + ", " + y;
                        glowRealm[x,y].visual.transform.SetParent(rowtop.transform);
                    }
                    if(x == 0){
                        GameObject a = new GameObject("Y GridLine " + (y), typeof(LineRenderer));
                        pos.Clear();
                        pos.Add(GetWorldPosition(0, y));
                        pos.Add(GetWorldPosition(data.GetLength(0),y));
                        a.GetComponent<LineRenderer>().SetPositions(pos.ToArray());
                        a.GetComponent<LineRenderer>().useWorldSpace = true;
                        gridlines.Add(a.GetComponent<LineRenderer>());
                    }
                }
                rowtop.transform.SetParent(tlc.transform);
                //Debug.Log(glowRealm.GetLowerBound(0));
            }
        }
        GameObject o = new GameObject("Gridlines");
        o.transform.SetParent(tlc.transform);
        GameObject c = new GameObject("X Axis");
        c.transform.SetParent(o.transform);
        GameObject d = new GameObject("Y Axis");
        d.transform.SetParent(o.transform);
        foreach(LineRenderer l in gridlines){
            if(l.name.StartsWith("X"))
                l.transform.SetParent(c.transform);
            else
                l.transform.SetParent(d.transform);
            l.material = new Material(Shader.Find("Sprites/Default"));
            l.startColor = Color.black;
            l.endColor = Color.black;
            l.sortingOrder = 6000;
        }
        ScaleGridlines();
    }

    public void ScaleGridlines(){
        if(gridlines != null){
            if(gridlines[0].enabled == true){
                float pseudoPixelWidth = (Camera.main.orthographicSize * (-Mathf.Log10(Screen.width / Screen.height) + 2.0f) * Screen.width / Screen.height) * (cellSize / Screen.width);
                foreach(LineRenderer l in gridlines){
                    l.startWidth = pseudoPixelWidth;
                    l.endWidth = pseudoPixelWidth;
                }
            }
        }
    }

    public void HideGridlines(){
        foreach(LineRenderer l in gridlines){
            l.enabled = false;
        }
    }

    public void ShowGridlines(){
        foreach(LineRenderer l in gridlines){
            l.enabled = true;
        }
    }
    /// <summary>
    /// Using a world position coordinate, gets the X/Y coordinates that are being moused over.
    /// </summary>
    /// <param name="worldpos">Vector position.</param>
    /// <param name="x">An outside variable representing the X coordinate of a grid-square.</param>
    /// <param name="y">An outside variable representing the Y coordinate of a grid-square.</param>
    public void GetXY(Vector3 worldpos, out int x, out int y){
        x = Mathf.FloorToInt((worldpos - originPos).x / cellSize);
        y = Mathf.FloorToInt((worldpos - originPos).y / cellSize);
    }

    /// <summary>
    /// Using a grid coordinate, gets the world position coordinate of that grid square.
    /// </summary>
    /// <param name="x">Coordinate for X in the grid.</param>
    /// <param name="y">Coordinate for Y in the grid.</param>
    /// <returns>Vector representation of the grid-square in world coordinates.</returns>
    public Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x, y) * cellSize + originPos;
    }
    
    /// <summary>
    /// Helper method that checks to see if a coordinate pair is valid.
    /// </summary>
    /// <param name="x">Coordinate for X in the grid.</param>
    /// <param name="y">Coordinate for Y in the grid.</param>
    /// <returns>True if grid coordinate is in-bounds of the grid, false otherwise.</returns>
    public bool ValidCoords(int x, int y){
        return(x >= 0 && y >= 0 && x < data.GetLength(0) && y < data.GetLength(1));
    }
    

    /// <summary>
    /// Sets the value of a grid square. NEEDS REFACTORING.
    /// </summary>
    /// <param name="x">Coordinate for X in the grid.</param>
    /// <param name="y">Coordinate for Y in the grid.</param>
    /// <param name="value">Value to set to.</param>
    public void SetMaterial(int x, int y, MaterialType mat){
        if(ValidCoords(x,y)){
            GetCell(x,y).ChangeMaterial(mat);
        }
    }

    /// <summary>
    /// Takes a world position and sets the value of the grid square on that position.
    /// </summary>
    /// <param name="worldpos">Vector position.</param>
    /// <param name="value"></param>
    public void SetMaterial(Vector3 worldpos, MaterialType value){

        int x,y;
        GetXY(worldpos, out x, out y);
        SetMaterial(x, y, value);
    }
    /// <summary>
    /// Gets the value of a specific square.
    /// </summary>
    /// <param name="x">Coordinate for X in the grid.</param>
    /// <param name="y">Coordinate for Y in the grid.</param>
    /// <returns>If valid, returns the grid-square's value. Else, returns -1.</returns>
    public int GetMaterial(int x, int y){
        if(ValidCoords(x,y))
            return data[x,y].material;
        else
            return -1;
    }
    /// <summary>
    /// Gets the value of a specific square given world coordinates.
    /// </summary>
    /// <param name="worldpos">Vector position.</param>
    /// <returns>If valid, returns the grid-square's value. Else, returns -1.</returns>
    public int GetMaterial(Vector3 worldpos){
        int x, y;
        GetXY(worldpos, out x, out y);
        return GetMaterial(x, y);
    }

    /// <summary>
    /// Getter for the grid's data.
    /// </summary>
    /// <returns>Data table (in Square[,]).</returns>
    public Square[,] GetData(){ return data;}
    /// <summary>
    /// Creates a list of valid squares which surround a specific coordinate.
    /// </summary>
    /// <param name="x">Coordinate for X in the grid.</param>
    /// <param name="y">Coordinate for Y in the grid.</param>
    /// <returns>List of Squares surrounding a specific coordinate</returns>
    public List<Square> GetNeighbors(int x, int y){
        List<Square> t = new List<Square>();
        t.Add(ValidCoords(x-1, y-1) ?   data[x-1, y-1]  : null);//1
        t.Add(ValidCoords(x, y-1)   ?   data[x, y-1]    : null);//2
        t.Add(ValidCoords(x+1, y-1) ?   data[x+1, y-1]  : null);//3
        t.Add(ValidCoords(x-1, y)   ?   data[x-1, y]    : null);//4
        t.Add(ValidCoords(x+1, y)   ?   data[x+1, y]    : null);//6
        t.Add(ValidCoords(x-1, y+1) ?   data[x-1, y+1]  : null);//7
        t.Add(ValidCoords(x, y+1)   ?   data[x, y+1]    : null);//8
        t.Add(ValidCoords(x+1, y+1) ?   data[x+1, y+1]  : null);//9
        t.RemoveAll(item => item == null);//clears nulls from the list
        return t;
    }
    /// <summary>
    /// Returns true if the given second coordinate is a neighbor of the first coordinate.<br/>
    /// See also <seealso cref="GridMap.GetNeighbors(int, int)"/>.
    /// </summary>
    /// <param name="x1">First coordinate for X in the grid.</param>
    /// <param name="y1">First coordinate for Y in the grid.</param>
    /// <param name="x2">Second coordinate for X in the grid.</param>
    /// <param name="y2">Second coordinate for Y in the grid.</param>
    /// <returns>True if the second grid-square is a neighbor of the first. False on bad input or if not neighbors.</returns>
    public bool IsNeighbors(int x1, int y1, int x2, int y2){
        if(ValidCoords(x1,y1) && ValidCoords(x2,y2)){
            List<Square> t = GetNeighbors(data[x1,y1]);
            foreach(Square c in t){
                if(c.lx == x2 && c.ly == y2)
                    return true;
            }
        }
        return false;
    }
    /// <summary>
    /// <inheritdoc cref="GridMap.GetNeighbors(int,int)"/>
    /// </summary>
    /// <param name="s">Square input.</param>
    /// <returns><inheritdoc cref="GridMap.GetNeighbors(int,int)"/></returns>
    public List<Square> GetNeighbors(Square s){
        return GetNeighbors(s.lx, s.ly);
    }
    
    /// <summary>
    /// Getter for a cell existing at a given coordinate point.
    /// </summary>
    /// <param name="x">Coordinate for X in the grid.</param>
    /// <param name="y">Coordinate for Y in the grid.</param>
    /// <returns></returns>
    public Square GetCell(int x, int y){
        if(ValidCoords(x,y))
            return data[x, y];
        return null;
    }
    public Feature PutDot(int x, int y, FeatureType typing,  StampType ft = StampType.streetSign, MapDirection r = MapDirection.down){
        return mt.CraftFeature(typing, x, y,(int)ft, (int)r);
    }

    public Feature PutLine(int x1, int y1, int x2, int y2, FeatureType typing){
        if((x2 - x1) < 1 || (y2 - y1) < 1){
            Debug.Log("Tried to make a line, but invalid limits. [min. size: 2x2]");
            return null; //cannot make a road where the width will be one or less
        }
        return mt.CraftFeature(typing, x1,y1,x2,y2, (y2 - y1) >= (x2 - x1));
    }

    public Feature PutArea(int x1, int y1, int x2, int y2, FeatureType typing){
        if((x2 - x1) < 1 || (y2 - y1) < 1){
            Debug.Log("Tried to make an area, but invalid limits. [min. size: 2x2]");
           return null;
        }
        return mt.CraftFeature(typing, x1, y1, x2, y2);
    }

    public IntPair InBounds(IntPair p){
        if(p.x < 0){
            p.x = 0;
        }
        if(p.y < 0){
            p.y = 0;
        }
        if(p.x > data.GetUpperBound(0)){
            p.x =  data.GetUpperBound(0);
        }
        if(p.y > data.GetUpperBound(1)){
            p.y = data.GetUpperBound(1);
        }
        return p;
    }

    public void GlowYaSquares(IntPair start, IntPair end, EditorType edt, out IntPair extStart, out IntPair extEnd){
        int temp = 0;
        //Debug.Log(glowRealm.GetLength(0));
        start = InBounds(start);
        end = InBounds(end);
        if(start.x > end.x){
            temp = end.x;
            end.x = start.x;
            start.x = temp;
        }
        if(start.y > end.y){
            temp = end.y;
            end.y = start.y;
            start.y = temp;
        }
        //Debug.Log(start.x + " " + start.y);
        //Debug.Log(end.x + " " + end.y);
        extStart = start;
        extEnd = end;
        for(int x = 0; x < glowRealm.GetLength(0); x++){
            for(int y = 0; y < glowRealm.GetLength(1); y++){
                if(x >= start.x && y >= start.y && x <= end.x && y <= end.y){
                    //Debug.Log("run!");
                    switch(edt){
                        case EditorType.GlowGoodV1:
                            glowRealm[x,y].GoodGlow();
                            break;
                        case EditorType.GlowBadV1:
                            glowRealm[x,y].BadGlow();
                            break;
                        default:
                            glowRealm[x,y].GlowOff();
                            break;
                    }
                }
                else
                    glowRealm[x,y].GlowOff();
            }
        }
    }

    public void UnGlowAll(){
        foreach(GlowSquare gs in glowRealm) gs.GlowOff();
    }

    public void KittenCaboodle(GD_OVERSEER overseer){
        NewMap(overseer.width, overseer.height);
        int i = 0;
        for(int x = 0; x < data.GetLength(0); x++){
            for(int y = 0; y < data.GetLength(1);y++){
                GD_SD temp = overseer.squareData[i];
                //temp will operate on data[x,y]
                data[x,y].ChangeMaterial((MaterialType)temp.mtrl);
                i++;
            }
        }
        foreach(GD_SF featproto in overseer.featureData){
            Feature flombo = null;
            switch((FeatureType)featproto.ftt){
                case FeatureType.river:
                case FeatureType.street:
                    flombo = PutLine(featproto.coords[0],featproto.coords[1], featproto.coords[2], featproto.coords[3], (FeatureType)featproto.ftt);
                    break;
                case FeatureType.lake:
                case FeatureType.region:
                case FeatureType.building:
                    flombo = PutArea(featproto.coords[0],featproto.coords[1], featproto.coords[2], featproto.coords[3], (FeatureType)featproto.ftt);
                    break; 
                case FeatureType.poi:
                case FeatureType.stamp:
                    flombo = PutDot(featproto.coords[0],featproto.coords[1], (FeatureType)featproto.ftt, (StampType)featproto.coords[2], (MapDirection)featproto.coords[3]);
                    break;
            }
            flombo.contents = new InfoWrapper(featproto.tn);
            List<InfoField> ghoul = new List<InfoField>(); 
            for(int e = 0; e < featproto.ifk.Length; e++){
                InfoField sora = new InfoField(featproto.ifk[e], featproto.dispsterl[e],featproto.ifi[e]);
                ghoul.Add(sora);  
            }
            flombo.contents.fields = ghoul;
        }
        Debug.Log("Load complete");
    }
}
