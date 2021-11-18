/// <summary>
/// Author: Aaron Spillman
/// Project: B.E.V.I.
/// Backend Utility for Sprite Loading and Distributing
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Vanity{
public class AtlasHandler
{
    
    /// <summary>
    /// Loads the DetailsAtlas as a local variable.
    /// </summary>
    public static SpriteAtlas details = Resources.Load("DetailsAtlas") as SpriteAtlas;
    
    /// <summary>
    /// Loads the MaterialsAtlas as a local variable.
    /// </summary>
    public static SpriteAtlas materials = Resources.Load("MaterialsAtlas") as SpriteAtlas;
    /// <summary>
    /// Loads the StampAtlas as a local variable.
    /// </summary>
    public static SpriteAtlas stamps = Resources.Load("StampAtlas") as SpriteAtlas;

    public static SpriteAtlas meta = Resources.Load("MetaAtlas") as SpriteAtlas;

    public static SpriteAtlas editor = Resources.Load("EditorAtlas") as SpriteAtlas;
    ///<summary>
    /// Loads a specified Sprite from an Atlas; multi-purpose function
    /// </summary>
    ///<param name= 'atlas'> The atlas to load the Sprite from</param>
    ///<param name= 'sprName'> The name of the Sprite, as a String</param>
    ///<returns> The Sprite with the name sprName in SpriteAtlas atlas</returns>
    public static Sprite loadImage(SpriteAtlas atlas, string sprName){
        //Debug.Log("Started trying to load.");
        Sprite spr = atlas.GetSprite(sprName);
        //if(spr != null) Debug.Log("Yeah, we got this");
        return spr;
    }

    ///<summary>
    /// The specified Square, sqr, loads four Sprites from MaterialsAtlas
    /// with the name sprName, and loads them into its Tiles. sqr saves
    /// the material
    ///</summary>
    public static Sprite loadMatSprite(MaterialType sprName, out int tid){
        tid = (Random.Range(0, 8));
        //Debug.Log("mat" + sprName + "_" + tid);
        return loadImage(materials, "mat" + sprName + "_" + tid);
    }

    public static Sprite loadTestSprite(){
        return loadImage(stamps, "stamp_vendingMachine");
    }

    public static Sprite loadStampSprite(StampType sprName){

        return loadImage(stamps, "stamp_" + sprName);
    }

    public static Sprite loadDetailSprite(DetailType sprName){
        return loadImage(details, "ft" +  sprName);
    }

    public static Sprite loadDetailSprite(DetailType sprName, int id){
        return loadImage(details, "ft" + sprName + "_" + id);
    }
    public static Sprite loadMetaSprite(MetaType sprName){

        return loadImage(meta, "ft" + sprName);
    }

    public static Sprite loadMetaSprite(MetaType sprName, int id){

        return loadImage(meta, "ft" + sprName + "_" + id);
    }

    public static Sprite loadEditorSprite(EditorType sprName){

        return loadImage(editor, "edtr" + sprName);
    }

    
}
}