/// 
/// Feature Types
/// Author: January Nelson
/// Map feature types that can be created, inherited from Feature.cs
/// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanity;

/// <summary>
/// Point of interest.
/// A pin in the map that marks a specific square.
/// </summary>
public class PoI : FDot
{
    public PoI(int x, int y) : base("New Point of Interest", x,y)
    {
        this.sprite = AtlasHandler.loadMetaSprite(MetaType.Pin);
    }
    public override List<InfoField> PopulateFields()
    {
        List<InfoField> temp = new List<InfoField>();
        temp.Add(new InfoField("Type / Category", 1));
        temp.Add(new InfoField("Hours of Operation", 4));
        temp.Add(new InfoField("Address",2));
        temp.Add(new InfoField("Phone Number", 2));
        temp.Add(new InfoField("Information", 0));
        temp.Add(new InfoField("Creator Notes", 3));
        return temp;
    }
}

/// <summary>
/// River.
/// A flowing body of water that passes over squares.
/// </summary>
public class River : FLine
{
    public River(int x1, int y1, int x2, int y2, bool v) : base(NameFactory.genRiver(), x1, y1, x2, y2, v)
    {
        if((x2 - x1) == 1 || (y2 - y1) == 1)
            RenameFeature(NameFactory.genCreek());
        edgeSprite = AtlasHandler.loadDetailSprite(DetailType.WtrEdge);
        for(int i = 0; i < 8; i++){
            contentSprites[i] = AtlasHandler.loadDetailSprite(DetailType.WtrArea, i);
        }
        Debug.Log("River loaded sprites");
    }

    public override List<InfoField> PopulateFields()
    {
        List<InfoField> temp = new List<InfoField>();
        temp.Add(new InfoField("Other Names", 1));
        temp.Add(new InfoField("History / Information", 0));
        temp.Add(new InfoField("Wildlife", 0));
        temp.Add(new InfoField("Creator Notes", 3));
        return temp;
    }
}

/// <summary>
/// Street.
/// A man-made pathway that passes over squares.
/// </summary>
public class Street : FLine
{
    public Street(int x1, int y1, int x2, int y2, bool v) : base(NameFactory.genRoad(), x1, y1, x2, y2, v)
    {
        edgeSprite = AtlasHandler.loadDetailSprite(DetailType.StrtEdge);
        for(int i = 0; i < 8; i++){
            contentSprites[i] = AtlasHandler.loadDetailSprite(DetailType.StrtArea, i);
        }
        Debug.Log("Street loaded sprites");
    }

    public override List<InfoField> PopulateFields()
    {
        List<InfoField> temp = new List<InfoField>();
        temp.Add(new InfoField("Other Names", 1));
        temp.Add(new InfoField("Speed Limit",2));
        temp.Add(new InfoField("History / Information", 0));
        temp.Add(new InfoField("Creator Notes", 3));
        return temp;
    }
}

public class Building : FArea
{
    public Building(int x1, int y1, int x2, int y2) : base("Unnamed Building", x1, y1, x2, y2)
    {
        contentSprite = AtlasHandler.loadDetailSprite(DetailType.Building, 0);
        edgeSprite = AtlasHandler.loadDetailSprite(DetailType.Building, 1);
        cornerSprite = AtlasHandler.loadDetailSprite(DetailType.Building, 2);
    }

    public override List<InfoField> PopulateFields()
    {
        List<InfoField> temp = new List<InfoField>();
        temp.Add(new InfoField("Type / Category", 1));
        temp.Add(new InfoField("Hours of Operation", 4));
        temp.Add(new InfoField("Address",2));
        temp.Add(new InfoField("Phone Number", 2));
        temp.Add(new InfoField("Information", 0));
        temp.Add(new InfoField("Creator Notes", 3));
        return temp;
    }
}

/// <summary>
/// Region.
/// A multi-purpose sub-division of a map.
/// </summary>
public class Region : FArea
{
    public Region(int x1, int y1, int x2, int y2) : base("New Region", x1, y1, x2, y2)
    {
        contentSprite = AtlasHandler.loadMetaSprite(MetaType.Region, 0);
        edgeSprite = AtlasHandler.loadMetaSprite(MetaType.Region, 1);
        cornerSprite = AtlasHandler.loadMetaSprite(MetaType.Region, 2);
    }

    public override List<InfoField> PopulateFields()
    {
        List<InfoField> temp = new List<InfoField>();
        temp.Add(new InfoField("Type / Category", 1));
        temp.Add(new InfoField("Population",2));
        temp.Add(new InfoField("History", 0));
        temp.Add(new InfoField("Culture", 0));
        temp.Add(new InfoField("Creator Notes", 3));
        return temp;
    }
}

public class Lake : FArea
{
    private Sprite[] cSprs;
    public Lake(int x1, int y1, int x2, int y2) : base("Unnamed Lake", x1, y1, x2, y2)
    {
        RenameFeature(NameFactory.genLake());
        edgeSprite = AtlasHandler.loadDetailSprite(DetailType.WtrEdge);
        cSprs = new Sprite[7];
        for(int i = 1; i < 8; i++){
            cSprs[i - 1] = AtlasHandler.loadDetailSprite(DetailType.WtrArea, i);
        }
        //NEED CORNER SPRITE
    }

    public override List<InfoField> PopulateFields()
    {
        List<InfoField> temp = new List<InfoField>();
        temp.Add(new InfoField("Other Names", 1));
        temp.Add(new InfoField("History / Information", 0));
        temp.Add(new InfoField("Wildlife", 0));
        temp.Add(new InfoField("Creator Notes", 3));
        return temp;
    }
    public override Sprite GetContentSprite()
    {
        return cSprs[Random.Range(0,6)];
    }
}
public class Stamp : FDot
{
    public StampType stType;
    public Stamp(int x, int y, StampType a, MapDirection r = MapDirection.down) : base(a + "", x,y)
    {
        stType = a;
        rot = r;
        this.sprite = AtlasHandler.loadStampSprite(a);
    }

    public override List<InfoField> PopulateFields()
    {
        List<InfoField> temp = new List<InfoField>();
        //no fields for a stamp
        return temp;
    }
}