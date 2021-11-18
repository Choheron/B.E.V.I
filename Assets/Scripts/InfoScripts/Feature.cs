/// 
/// Feature
/// Author: January Nelson
/// Back-end classes and code for the framework of adding Map Features.
/// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanity;

/// <summary>
/// Generic feature.
/// </summary>
public abstract class Feature
{

    /// <summary>
    /// Wrapper class which is used to contain the info about a feature.
    /// See InfoWrapper's def for more info.
    /// </summary>
    public InfoWrapper contents;
    public Color color = Color.white;

     public Feature(){
       contents = new InfoWrapper("GenericFeatureName");
       contents.fields = PopulateFields();
    }

    
    public void RenameFeature(string s){
        contents.topName = s;
    }
    /// <summary>
    /// Create the default fields for the InfoWrapper upon generation.
    /// Will vary wildly depending on the type of feature created.
    /// </summary>
    /// <returns>Fields list.</returns>
    public abstract List<InfoField> PopulateFields();
}

public abstract class FDot : Feature
{
    public MapDirection rot;
    public Sprite sprite;
    public int x,y;

    public FDot(string name, int x, int y) : base()
    {
        RenameFeature(name);
        this.x = x;
        this.y = y;
        contents.jx = x;
        contents.jy = y;
    }
}

public abstract class FLine : Feature
{
    /// <summary>
    /// Designates the material type of the line.
    /// Will interact with texture loader to determine what kind of sprites to load.
    /// </summary>
    public Sprite edgeSprite;
    public Sprite[] contentSprites;
    public int x1, y1, x2, y2;
    public bool vertical;
    public FLine(string name, int x1, int y1, int x2, int y2, bool v) : base()
    {
        RenameFeature(name);
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        vertical = v;
        contents.jx = (int)((x1 + x2) / 2);
        contents.jy = (int)((y1 + y2) / 2);

        contentSprites = new Sprite[8];
    }

    public Sprite GetContentSprite(){
        return contentSprites[Random.Range(1,7)];
    }
}

public abstract class FArea : Feature
{
    public int x1, y1, x2, y2;
    public Sprite contentSprite;
    public Sprite edgeSprite;
    public Sprite cornerSprite;
    
    public FArea(string name, int x1, int y1, int x2, int y2) : base() {
        RenameFeature(name);
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        contents.jx = (int)((x1 + x2) / 2);
        contents.jy = (int)((y1 + y2) / 2);
    }

    public virtual Sprite GetContentSprite(){
        return contentSprite;
    }
}