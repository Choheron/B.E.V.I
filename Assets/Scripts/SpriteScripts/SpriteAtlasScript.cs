//Author: Aaron Spillman
//Project: B.E.V.I.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteAtlasScript : MonoBehaviour
{
    ///<summary>
    /// The literal Sprite Atlas being called
    ///<summary>
    [SerializeField] private SpriteAtlas sprAtlas;
    ///<summary>
    ///The name of the sprite
    ///<summary>
    [SerializeField] private string sprName;
    ///<summary>
    /// Start loads the specified Sprite thats stored in the Sprite Atlas
    ///<summary>
    void Start()
    {
        GetComponent<Image>().sprite = sprAtlas.GetSprite(sprName);
    }

    

}
