/// <summary>
/// Author: Thomas Campbell, January Nelson
/// Project: B.E.V.I.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// We access feat - :)
/// </summary>
public class WAF : MonoBehaviour
{
    public Feature feature;
    public int l;
    public WAF(Feature waf, int layer) {
        feature = waf;
        l = layer;
    }

    public bool equals(WAF other) {
        if(feature != other.feature) {
            return false;
        }
        return true;
    }
}
