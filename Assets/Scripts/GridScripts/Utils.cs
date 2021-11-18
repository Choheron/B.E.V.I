using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/*  Vanity Utils 
 *  Author: January Nelson
 *  Useful methods for rendering/world-space interaction.
 *  
 *  Adapted and customized from CodeMonkey Utils - unitycodemonkey.com
 */ 

namespace Vanity
{
    public class Utils
    {

        public const int sortingOrderDefault = 5000;
        /// <summary>
        /// Generates a new sprite-rendering object.
        /// </summary>
        /// <param name="t">Transform for this object.</param>
        /// <param name="name">Name for the object (in hierarchy).</param>
        /// <param name="spr">Sprite.</param>
        /// <param name="pos">Vector position.</param>
        /// <param name="scale">Vector scale.</param>
        /// <param name="sortingOrder">Sprite sorting order.</param>
        /// <param name="color">Color tint.</param>
        /// <returns>Newly created sprite-rendering GameObject.</returns>
        public static GameObject CreateWorldSprite(Transform t, string name, Sprite spr, Vector3 pos, Vector3 scale, int sortingOrder, Color color)
        {
            GameObject o = new GameObject(name, typeof(SpriteRenderer));
            Transform transform = o.transform;
            transform.SetParent(t, false);
            transform.localPosition = pos;
            transform.localScale = scale;
            SpriteRenderer spriteRenderer = o.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = spr;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color;
            return o;
        }
        /// <summary>
        /// Creates a line renderer object that is used to draw a square.
        /// </summary>
        /// <param name="worldpos">Position of the square</param>
        /// <param name="cellsize">Size of the square.</param>
        /// <param name="width">Size of the square (redundant).</param>
        /// <returns>Newly created line renderer.</returns>
        public static LineRenderer CreateWorldLine(Vector3 worldpos, float cellsize, float width){
            GameObject o = new GameObject("Sqr (" + worldpos.x + ", " + worldpos.y + ")", typeof(LineRenderer));
            List<Vector3> pos = new List<Vector3>();
            pos.Add(worldpos + (new Vector3(cellsize, 0) *.5f));
            pos.Add(worldpos + (new Vector3(cellsize, (cellsize * 2)) * .5f));
            LineRenderer l =  o.GetComponent<LineRenderer>();
            l.startWidth = width;
            l.endWidth = width;
            l.SetPositions(pos.ToArray());
            l.useWorldSpace = true;
            l.material = new Material(Shader.Find("Sprites/Default"));
            l.startColor = Color.white;
            l.endColor = Color.white;
            l.sortingOrder = sortingOrderDefault;
            return l;
        }
        /// <summary>
        /// More condensed form: creates a text-rendering object.
        /// </summary>
        /// <param name="text">Text contained within object.</param>
        /// <param name="t">Transform (Default: null).</param>
        /// <param name="pos">Vector position (Default: Vector3 defaults).</param>
        /// <param name="fontSize">Size of the text (not equal to character size) (Default: 40).</param>
        /// <param name="color">Color of text (Default: white).</param>
        /// <param name="textAnchor">Anchored position of text within text object (Default: Upper Left).</param>
        /// <param name="textAlignment">Alignment of text within text mesh (Default: Left).</param>
        /// <param name="sortingOrder">Sorting order for text object (Default: Sortingorder default).</param>
        /// <returns>New Text Mesh.</returns>
        public static TextMesh CreateWorldText(string text, Transform t = null, Vector3 pos = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
        {
            if (color == null) color = Color.white;
            return CreateWorldText(t, text, pos, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
        /// <summary>
        /// Creates a text-rendering object.
        /// </summary>
        /// <param name="text">Text contained within object.</param>
        /// <param name="t">Transform.</param>
        /// <param name="text">Text contained within.</param>
        /// <param name="pos">Vector position.</param>
        /// <param name="pt">Size of the text (not equal to character size).</param>
        /// <param name="color">Color of text.</param>
        /// <param name="textAnchor">Anchored position of text within text object.</param>
        /// <param name="textAlignment">Alignment of text within text mesh.</param>
        /// <param name="sortingOrder">Sorting order for text object.</param>
        /// <returns>New Text Mesh.</returns>
        public static TextMesh CreateWorldText(Transform t, string text, Vector3 pos, int pt, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(t, false);
            transform.localPosition = pos;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = pt;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;

        }

        //overloaded form

        /// <summary>
        /// Attempts to prevent world interaction when the mouse is over a UI element.
        /// </summary>
        /// <returns>If mouse is over UI.</returns>
        public static bool AcknowledgeUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            else
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }
    }
}
