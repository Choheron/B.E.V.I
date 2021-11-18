using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Align element with the passed in element's z axis rotation (Useful for rotating UI elements with the camera)
/// </summary>
public class AlignUiItem : MonoBehaviour
{
    /// <summary>
    /// Game Object Item to pull alignment from.
    /// </summary>
    public GameObject alignItem;
    /// <summary>
    /// Game Object that the script is applied to.
    /// </summary>
    private GameObject self;
    /// <summary>
    /// Rotation of the alignItem's z axis.
    /// </summary>
    private float rotation;

    // Start is called before the first frame update
    void Start()
    {
        self = this.gameObject;
        rotation = alignItem.transform.rotation.eulerAngles.z;
        self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, self.transform.eulerAngles.y, -rotation);
    }

    // Update is called once per frame
    void Update()
    {
        rotation = alignItem.transform.rotation.eulerAngles.z;
        self.transform.eulerAngles = new Vector3(self.transform.eulerAngles.x, self.transform.eulerAngles.y, -rotation);
    }
}
