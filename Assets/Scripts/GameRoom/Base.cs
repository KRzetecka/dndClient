using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
[XmlType(TypeName = "Base")]
public class Base : MonoBehaviour
{
    //                 lvl, base
    [XmlArray]
    public Dictionary<int, int> HP { get; set; }
    [XmlArray]
    public Dictionary<int, int> DEF { get; set; }
    [XmlArray]
    public Dictionary<int, int> ATT { get; set; }

    public Base()
    {
        HP = new Dictionary<int, int>();
        DEF = new Dictionary<int, int>();
        ATT = new Dictionary<int, int>();
    }


}
