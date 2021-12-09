using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;

public class CharacterRace
{
    public string Name { get; set; }
    public int MaxAge { get; set; }
    public int MinAge { get; set; }
    public string Description { get; set; }
    public List<string> AllowedClasses { get; set; }
    public Dictionary<string, int> SEQ { get; set; }

    public CharacterRace()
    {
        AllowedClasses = new List<string>();
        SEQ = new Dictionary<string, int>();
    }
}
