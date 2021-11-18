using UnityEngine;
using Vanity;

public class OverlaySquare
{
    public SpriteRenderer visual;
    public WAF waf = null;
    /// <summary>
    /// Size the square is drawn to.
    /// </summary>
    public float size;
    /// <summary>
    /// A rough worldposition of where the cell is drawn.
    /// </summary>
    public Vector3 position;
    public int lrsrt;
    /// <summary>
    /// Local x and y as to where this grid square is in the grid itself.
    /// </summary>
    public int ox, oy;

    public OverlaySquare(int lx, int ly, Vector3 oPos, float oSize, int layer)
    {
        ox = lx;
        oy = ly;
        size = oSize;
        lrsrt = layer;
        position = oPos +  new Vector3(size, size) / 2.0f;//get to center of square
        visual = null;
    }

    public void AttachVisual(Sprite s){
        if(visual != null) 
        {
            visual.transform.eulerAngles  = Vector3.zero;//resets the transform!
            visual.sprite = s;
        }
        else {
            visual = Utils.CreateWorldSprite(null,"Overlay : " + (FeatureType)lrsrt + " (" + ox + ", " + oy + ")", s, position, new Vector3(size, size),5000 + lrsrt, Color.white).GetComponent<SpriteRenderer>();
        }
    }

    public void AttachFeature(Sprite s, Feature f){
        AttachVisual(s);
        waf = visual.gameObject.AddComponent<WAF>();
        waf.feature = f;
        waf.l = lrsrt;
        visual.gameObject.AddComponent<BoxCollider>();
    }

    public void DetachFeature(){
        if(visual != null) GameObject.DestroyImmediate(visual.gameObject);
    }

    public void Hide(){
        if(visual != null) visual.enabled = false;
    }

    public void Show(){
        if(visual != null) visual.enabled = true;
    }
}

public class GlowSquare : OverlaySquare
{
    public int glowstate = 0;
    private Sprite goodglow = AtlasHandler.loadEditorSprite(EditorType.GlowGoodV1);
    private Sprite badglow = AtlasHandler.loadEditorSprite(EditorType.GlowBadV1);
    public GlowSquare(int lx, int ly, Vector3 oPos, float oSize) : base(lx, ly, oPos, oSize, 9)
    {
        AttachVisual(goodglow);
        visual.enabled = false;
       
    }
    public void GlowOff(){
        glowstate = 0;
        visual.enabled = false;
    }
    public void BadGlow(){
        glowstate = 2;
        AttachVisual(badglow);
        visual.enabled = true;
    }

    public void GoodGlow(){
        glowstate = 1;
        AttachVisual(goodglow);
        visual.enabled = true;
    }
}
