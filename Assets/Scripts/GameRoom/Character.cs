using Assets.Scripts.GameRoom;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using UnityEngine;


internal class Character : MonoBehaviour
{
    public Character MyChar;
    public Character SbChar;


    public string CharName { get; set; }
    public string CharStory { get; set; }
    public int CharAge { get; set; }      
    public CharacterSex CharSex { get; set; }
    public int LvL { get; set; }
    public int LvLPts { get; set; } //CURRENT PTS
    public int LvLGap { get; set; } //MAX PTS
    public int HP { get; set; }
    public int DEF { get; set; }
    public int ATT { get; set; }

    public CharacterClass CharClass { get; set; }
    public CharacterRace CharRace { get; set; }
    public CharacterStats CharStats { get; set; }
    public Dictionary<Equipment, int> CharEq { get; set; } //item + amount

    public Character()
    {

    }

}
enum CharacterSex
{
    Male,
    Female,
    None,
    Other
}

