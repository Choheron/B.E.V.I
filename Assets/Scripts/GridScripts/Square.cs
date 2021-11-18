// Square
// Author: January Nelson
// Represents a single square of information within the grid.
using System;
using System.Collections.Generic;
using UnityEngine;
using Vanity;

public class Tile{
    public LineRenderer debugVisual;
    public SpriteRenderer sprVisual;
    public float tSize;
    public Vector3 tPos;
    public int tid;
    public Tile(float size, Vector3 mypos, int quadrant){
        tSize = size;
        tPos = mypos +  new Vector3(size, size) / 2.0f;//get to center of square
        switch(quadrant){
            case 0://topleft
                tPos += new Vector3(-size, size) / 2.0f;
                break;
            case 1://topright
                tPos += new Vector3(size, size) / 2.0f;
                break;
            case 2://bottomleft
                tPos += new Vector3(-size, -size) / 2.0f;
                break;
            case 3://bottomright
                tPos += new Vector3(size, -size) / 2.0f;
                break;

        }
        //Debug.Log("Created tile");
        sprVisual = Utils.CreateWorldSprite(null,"Tile (" + tPos.x + ", " + tPos.y + ")", null, tPos + new Vector3(tSize, tSize) / 2, new Vector3(tSize, tSize),5000, Color.white).GetComponent<SpriteRenderer>();
        debugVisual = Utils.CreateWorldLine(tPos, tSize, tSize);
        debugVisual.sortingOrder = 4999;
        debugVisual.startColor = Color.gray;
        debugVisual.endColor = Color.gray;
    }

    public void ChangeSprite(Sprite r){
        //if(r == null)
        sprVisual.sprite = r;
    }
}

public class Square{
    /// <summary>
    /// Visualization of the square's exact location.
    /// </summary>
    public LineRenderer debugVisual { get; set;}
    public Tile[,] sqrContents;
    /// <summary>
    /// Value contained inside the square.
    /// </summary>
    public int material;
    /// <summary>
    /// Size the square is drawn to.
    /// </summary>
    public float cellSize;
    /// <summary>
    /// A rough worldposition of where the cell is drawn.
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// Local x and y as to where this grid square is in the grid itself.
    /// </summary>
    public int lx, ly;
    public Square(int lx, int ly, Vector3 mypos, float cellSize){
        this.lx = lx;
        this.ly = ly;
        position = mypos;
        this.cellSize = cellSize;
        debugVisual = Utils.CreateWorldLine(mypos, cellSize , cellSize);
        debugVisual.sortingOrder = 4998;
        debugVisual.startColor = Color.white;
        debugVisual.endColor = Color.white;
        debugVisual.name = "Grid Square (" + (lx + 1) + ", " + (ly + 1) + ")"; //not zero indexing the square names
        SetUpTiles();
    }

    public void SetUpTiles(){
        sqrContents = new Tile[2,2];
        float quarter = cellSize *.5f;
        sqrContents[0,0] = new Tile(quarter, position, 0);
        sqrContents[0,1] = new Tile(quarter, position, 1);
        sqrContents[1,0] = new Tile(quarter, position, 2);
        sqrContents[1,1] = new Tile(quarter, position, 3);
        foreach(Tile t in sqrContents){
            t.debugVisual.transform.SetParent(t.sprVisual.transform);
            t.sprVisual.name = "Tile " + t.tPos.ToString();
            t.sprVisual.transform.SetParent(this.debugVisual.transform);
        }
    }

    public void ChangeMaterial(MaterialType mat)
    {
        int temp = 0;
        this.material = (int)mat;
        foreach(Tile t in sqrContents){
            t.sprVisual.sprite = AtlasHandler.loadMatSprite(mat, out temp);
            t.tid = temp;
        }
    }
    public void Splurge(){
        foreach(Tile t in sqrContents){
            t.sprVisual.sprite = null;
        }
    }

    public void SaveMaterial(out int mat, out int[] tids){
        int[] tidsave = new int[4];
        mat = this.material;
        int temp = 0;
        foreach(Tile t in sqrContents){
            tidsave[temp] = t.tid;
        }
        tids = tidsave;
    }
}