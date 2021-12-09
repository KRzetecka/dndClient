using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public class Level
{
    public int MaxLvl { get; set; }
    public Dictionary<int, int> LvlGaps { get; set; }  // id|value
    
    public Level()
    {
        LvlGaps = new Dictionary<int, int>();
    }
}
