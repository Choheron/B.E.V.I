/// <summary>
/// Author: Thomas Campbell
/// Project: B.E.V.I.
/// Initalizer for the button
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vanity;

public class ButtonInitalize : MonoBehaviour
{
    /// <summary>
    /// Link to camera Script for onionskin sprite
    /// </summary>
    public CameraScript mainCam;
    /// <summary>
    /// String to be placed as the caption of the button
    /// </summary>
    public string buttonName;
    /// <summary>
    /// String to be used to search for the sprite
    /// </summary>
    public string spriteName;
    /// <summary>
    /// Int correspoing to atlast needed (backend integration)
    /// 1 - Materials
    /// 2 - Features
    /// 3 - Stamps
    /// </summary>
    [Tooltip("1 = Material, 2 = Feature, 3 = Stamp, 4 = MetaAtlas.")]
    public int atlas;
    /// <summary>
    /// Panel to change image preview
    /// </summary>
    private GameObject imagePanel;
    /// <summary>
    /// Text to show on button
    /// </summary>
    private Text buttonText;


    // Start is called before the first frame update
    void Start()
    {
        buttonText = this.gameObject.transform.Find("Text").GetComponent<Text>();
        buttonText.text = buttonName;
        Image imagePanel = this.gameObject.transform.Find("ButtonImage").GetComponent<Image>();
        Image temp = imagePanel.GetComponent<Image>();
        if(atlas == 1) {
            temp.sprite = Vanity.AtlasHandler.loadImage(AtlasHandler.materials, spriteName);
        } 
        else if(atlas == 2) {
            temp.sprite = Vanity.AtlasHandler.loadImage(AtlasHandler.details, spriteName);
        } 
        else if(atlas == 3) {
            temp.sprite = Vanity.AtlasHandler.loadImage(AtlasHandler.stamps, spriteName);
        }
        else if(atlas == 4) {
            temp.sprite = Vanity.AtlasHandler.loadImage(AtlasHandler.meta, spriteName);
        }
        
    }
}
