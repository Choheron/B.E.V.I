using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class atlasTesting : MonoBehaviour
{
    public SpriteAtlas spa;

    // Start is called before the first frame update
    void Start()
    {
        spa = Resources.Load("MaterialsAtlas") as SpriteAtlas;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
