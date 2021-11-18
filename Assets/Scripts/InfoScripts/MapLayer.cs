using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTable{
    public List<MapLayer> infoTable;
    public MapTable(GridMap gm){
        int i = 1;
        infoTable = new List<MapLayer>();
        foreach (var name in Enum.GetNames(typeof(FeatureType))){//testing with one layer
           MapLayer ml = new MapLayer(name, i, gm);
           infoTable.Add(ml);
           i++;
           //Debug.Log("Made layer for " + name);
        }
       Debug.Log("Map table has finished generating.");
    }

    public Feature CraftFeature(FeatureType type, int x, int y, int x2=-1, int y2=0, bool v=false){
        MapLayer local = infoTable[(int)type - 1];
        Feature m = null;
        switch(type){
            case FeatureType.poi:
                m = new PoI(x,y);
                break;
            case FeatureType.river:
                m = new River(x,y,x2,y2, v);
                break;
            case FeatureType.street:
                m = new Street(x,y,x2,y2, v);
                break;
            case FeatureType.building:
                m = new Building(x,y,x2,y2);
                break;
            case FeatureType.region:
                m = new Region(x,y,x2,y2);
                break;
            case FeatureType.lake:
                m = new Lake(x,y,x2,y2);
                break;
            case FeatureType.stamp:
                m = new Stamp(x,y,(StampType)x2, (MapDirection)y2);
                break;
            default:
                throw new ArgumentOutOfRangeException("An unhandled feature type was detected.");
        }
        if(m != null){
            local.AddFeature(m, type);
            return m;
        }
        else
            return null;
    }
    public void GetMasterTable(out List<List<Feature>> features, out List<string> titles){
        List<List<Feature>> tempfts = new List<List<Feature>>();
        List<string> tempstrings = new List<string>();
        for(int i = 0; i < infoTable.Count; i++){//each layer except stamps (which are first)
            tempfts.Add(infoTable[i].lyrContents);
            tempstrings.Add(infoTable[i].shortname);//grabs titles for each of the lists.
        }
        features = tempfts;
        titles = tempstrings;
    }

    public MapLayer GetLayer(FeatureType ft){
        return infoTable[(int)ft - 1];
    }

    public MapLayer GetLayer(int i){
        return infoTable[i];
    }

    public void Clean(){
        foreach(MapLayer e in infoTable){
            e.Clean();
        }
    }
}

public class MapLayer {
    public int layersort;
    public List<Feature> lyrContents;
    public OverlaySquare[,] overlayData;
    public bool showing;
    public string shortname;
    public MapLayer(string s, int lyr, GridMap gm){
        lyrContents = new List<Feature>();
        layersort = lyr;
        shortname = s.ToUpper();
        showing = true;
        //Debug.Log(shortname + ": Created");
        overlayData = new OverlaySquare[gm.data.GetLength(0),gm.data.GetLength(1)];
        for(int x = 0; x < overlayData.GetLength(0); x++){
            for(int y = 0; y < overlayData.GetLength(1); y++){
                overlayData[x,y] = new OverlaySquare(x, y, gm.GetWorldPosition(x,y), gm.cellSize, layersort);
            }
        }
    }
    private Color randomColor(float alpha = 1.0f){
        return new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, alpha);
    } 
    public bool AddFeature(Feature t, FeatureType f){
        switch(f){
            //LINES
            case FeatureType.river:
                River river = (River)t;
                PaintLine(river);
                break;
            case FeatureType.street:
                Street street = (Street)t;
                PaintLine(street);
                break;
            //AREAS
            case FeatureType.lake:
                Lake lake = (Lake)t;
                PaintBox(lake);
                break;
            case FeatureType.region:
                Region region = (Region)t;
                PaintBox(region, randomColor(.5f));
                break;
            case FeatureType.building:
                Building building = (Building)t;
                PaintBox(building, randomColor());
                break;
            //DOTS
            case FeatureType.poi:
                PoI point = (PoI)t;
                PaintDot(point, randomColor());
                break;
            case FeatureType.stamp:
                Stamp stamp = (Stamp)t;
                PaintDot(stamp);
                break;
            default:
                throw new ArgumentOutOfRangeException("An unhandled feature type was detected.");
        }
        Debug.Log(t.GetType());
        return true;
    }

    public bool toggleLayer() {
        if(showing) {
            hideLayer();
        } else {
            showLayer();
        }
        return showing;
    }

    /// <summary>
    /// Turn off visual of layer. - Thomas Campbell
    /// </summary>
    /// <returns>True if completed.</returns>
    public bool hideLayer() {
        foreach(OverlaySquare overlaySquare in overlayData) {
            overlaySquare.Hide();
        }
        showing = false;
        return true;
    }

    /// <summary>
    /// Turn on visual of layer. - Thomas Campbell
    /// </summary>
    /// <returns>True if completed.</returns>
    public bool showLayer() {
        foreach(OverlaySquare overlaySquare in overlayData) {
            overlaySquare.Show();
        }
        showing = true;
        return true;
    }
    public bool TintFeature(Feature f, FeatureType ft){
        switch(ft){
            case FeatureType.river:
                River rvr = (River)f;
                return TintFeature(rvr);
            case FeatureType.street:
                Street skl = (Street)f;
                return TintFeature(skl);
            case FeatureType.stamp:
                Stamp stemp = (Stamp)f;
                return TintFeature(stemp);
            case FeatureType.poi:
                PoI skd = (PoI)f;
                return TintFeature(skd);
            case FeatureType.lake:
                Lake lke = (Lake)f;
                return TintFeature(lke);
            case FeatureType.building:
                Building bld = (Building)f;
                return TintFeature(bld);
            case FeatureType.region:
                Region ska = (Region)f;
                return TintFeature(ska);
        }
        return false;
    }
    public bool TintFeature(FDot fd){
        TintOne(fd.x, fd.y, fd.color);
        return true;
    }
    public bool TintFeature(FLine fl){
        for(int i = fl.x1; i <= fl.x2; i++){
            for(int j = fl.y1; j <= fl.y2; j++){
                TintOne(i,j,fl.color);
            }
        }
        return true;
    }

    public bool TintFeature(FArea fa){
        for(int i = fa.x1; i <= fa.x2; i++){
            for(int j = fa.y1; j <= fa.y2; j++){
                TintOne(i,j,fa.color);
            }
        }
        return true;
    }

    public void ClearSquare(OverlaySquare targ){
        if(lyrContents.Count == 1)
            lyrContents = new List<Feature>();
        else
            lyrContents.Remove(targ.waf.feature);
        targ.DetachFeature();
    }

    public void Clean(){
        lyrContents = new List<Feature>();
        foreach(OverlaySquare t in overlayData){
            if(t != null)
                t.DetachFeature();
        }
    }

    private void PaintOne(Feature f, int x, int y, Sprite sprite, MapDirection rot = MapDirection.down, Color tint = default){
        //INSERT HANDLING CODE FOR INTERNAL CORNERS
        if(overlayData[x,y].waf != null && overlayData[x,y].waf.feature != null){
            Debug.Log("This should be received if an overlap needs to handle.");
        }
        overlayData[x,y].AttachFeature(sprite, f);
        //Debug.Log("We're building an empire, here!");
        if(rot != MapDirection.down){
            overlayData[x, y].visual.transform.eulerAngles = new Vector3(0,0, (int)rot);
        }
        if(tint != default(Color)){
            overlayData[x,y].visual.color = tint;
        }
    }
    private void TintOne(int x, int y, Color tint = default){
        if(overlayData[x,y].visual != null){
            overlayData[x,y].visual.color = tint;
        } 
    }

    private void PaintDot(FDot fd, Color? tint = null){
        if(overlayData[fd.x,fd.y].waf != null && overlayData[fd.x,fd.y].waf.feature != null){
            return;
            // ClearSquare(overlayData[fd.x,fd.y]); --- REMOVED TO TEST INFOBOX
        }
        else{
            lyrContents.Add(fd);
            overlayData[fd.x, fd.y].AttachFeature(fd.sprite, fd);
            if(fd.rot != MapDirection.down){
                overlayData[fd.x, fd.y].visual.transform.eulerAngles = new Vector3(0,0, (int)fd.rot);
            }
            fd.color = (tint != null) ? (Color)tint : Color.white;
            TintFeature(fd);
        }
    }

    private void PaintBox(FArea fa, Color tint = default){
        fa.color = tint;
        int xrange = fa.x2 - fa.x1;
        int yrange = fa.y2 - fa.y1;
         for(int x = fa.x1; x <= fa.x2; x++){
            for(int y = fa.y1; y <= fa.y2; y++){
                PaintOne(fa, x, y, fa.GetContentSprite(), MapDirection.down, tint);
            }
        }
        //paint corners
        PaintOne(fa, fa.x1, fa.y1, fa.cornerSprite, MapDirection.up, tint);
        PaintOne(fa, fa.x2, fa.y1, fa.cornerSprite, MapDirection.right, tint);
        PaintOne(fa, fa.x1, fa.y2, fa.cornerSprite, MapDirection.left, tint);
        PaintOne(fa, fa.x2, fa.y2, fa.cornerSprite, MapDirection.down, tint);
        //try to paint top and bottom walls
        if(xrange > 1){//if at least hori wall will be drawn
            int hwall = xrange - 1;
            for(int i = 1; i <= hwall; i++){
                PaintOne(fa, i + fa.x1, fa.y1, fa.edgeSprite, MapDirection.up, tint);//top wall
                PaintOne(fa, i + fa.x1, fa.y2, fa.edgeSprite, MapDirection.down, tint);//bottom wall
            }
        }
        //try to paint right and left walls
        if(yrange > 1){
            int vwall = yrange - 1;
            for(int i = 1; i <= vwall; i++){
                //x1, i + y1
                PaintOne(fa, fa.x1, i + fa.y1, fa.edgeSprite, MapDirection.left, tint); //left wall
                PaintOne(fa, fa.x2, i + fa.y1, fa.edgeSprite, MapDirection.right, tint);//right wall
            }
        }
        lyrContents.Add(fa);
        Debug.Log("Finished painting area");
    }

    private void PaintLine(FLine fl, Color tint = default)
    {
        fl.color = tint;
        Sprite midline = fl.contentSprites[0];//safer access for this
         for(int x = fl.x1; x <= fl.x2; x++){
            for(int y = fl.y1; y <= fl.y2; y++){
                PaintOne(fl, x, y, fl.GetContentSprite(),MapDirection.down,tint);//paint insides
                //Debug.Log("boink!");
            }
        }
        int xrange = fl.x2 - fl.x1;
        int yrange = fl.y2 - fl.y1;
        bool hasMidline = (fl.vertical) ? ((xrange % 2) != 1) : ((yrange % 2) != 1);
        Debug.Log(hasMidline); 
        //paint left and right lines
        if(fl.vertical){
            for(int i = 0; i <= yrange; i++){
                //x1, i + y1
                PaintOne(fl, fl.x1, i + fl.y1, fl.edgeSprite, MapDirection.left,tint);//left
                if(hasMidline) PaintOne(fl, fl.x1 + Mathf.CeilToInt(xrange / 2), i + fl.y1, midline, MapDirection.left,tint);//paints mid pointed up
                PaintOne(fl, fl.x2, i + fl.y1, fl.edgeSprite, MapDirection.right,tint);//right
            }
        }
        else{//paint top and bottom lines
            for(int i = 0; i <= xrange; i++){
                //i + x1, y1
                PaintOne(fl, i + fl.x1, fl.y1, fl.edgeSprite, MapDirection.up,tint);//top
                if(hasMidline) PaintOne(fl, i + fl.x1, fl.y1 +  Mathf.CeilToInt(yrange / 2), midline, MapDirection.down,tint);//paints mid pointed right
                PaintOne(fl, i + fl.x1, fl.y2, fl.edgeSprite, MapDirection.down,tint);//bottom
            }
        }
        lyrContents.Add(fl);
        Debug.Log("Finished painting line");
    } 
}
