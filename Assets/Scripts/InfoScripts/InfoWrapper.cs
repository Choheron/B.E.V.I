//
// Author: January Nelson
// Project: B.E.V.I.
//
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



/// <summary>
/// Master-class for all info contained within a Feature.
/// Accesible by the player through UI elements.
/// </summary>
public class InfoWrapper
{
    public int jx, jy;
    public string topName;
    public List<InfoField> fields;
    public InfoWrapper(string name){
        topName = name;
        fields = new List<InfoField>(200);
    }

    public InfoField findField(string fieldName) {
        foreach(InfoField field in fields) {
            if(fieldName == field.fieldName) {
                return field;
            }
        }
        return null;
    }

}

/// <summary>
/// Data element that will be editable 
/// and accessible by the player through UI elements.
/// 
/// Limited to 200 entries per Feature.
/// </summary>
public class InfoField
{
    
    /// <summary>
    /// The name of a field.
    /// Can also be edited by the 
    /// </summary>
    public string fieldName;
    /// <summary>
    /// Container for user input in this field.
    /// </summary>
    public StringBuilder input;
    /// <summary>
    /// Used to determine how this field displays to the user.<br/>
    /// 0 = Big box. 5000 char limit.<br/>
    /// 1 = "One-line" box. 250 char limit.<br/>
    /// 2 = "Address" (phone or postal) box. 50 char limit.<br/>
    /// 3 = "Out-of-character" box. 5000 char limit. Should be formatted italicized and gray, 
    ///         could be used by an author to make notes about a location.<br/>
    /// 4 = placeholder for potential future updates ("location hours"). 
    ///         defaults to making a big box for now.<br/>
    /// 5> = other placeholders. defaults to a big box for now.<br/>
    /// </summary>
    public int displayStyle;
    const int NUMFIELDTYPES = 4;
    public InfoField(string s = "New Info Field", int style = 0){
        if(style < NUMFIELDTYPES && style > 0){
            displayStyle = style;
        }
        else displayStyle = 0;
        fieldName = s;
        input = new StringBuilder();
        input.Append("No information.");
    }

    public InfoField(string s, int style, string helpme) :this(s, style){
        input.Clear();
        input.Append(helpme);
    }
}
