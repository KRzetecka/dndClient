using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public Stats Stat1 { get; set; }
    public Stats Stat2 { get; set; }
    public Stats Stat3 { get; set; }
    public Stats Stat4 { get; set; }
    public Stats Stat5 { get; set; }
    public Stats Stat6 { get; set; }
    public Stats Stat7 { get; set; }
    public Stats Stat8 { get; set; }

    public bool statsRedist { get; set; }
    public int statPoints { get; set; }
    public int rollChances { get; set; }

    public CharacterStats()
    {

    }
    public CharacterStats(int _max)
    {
        Stat1 = new Stats(_max);
        Stat2 = new Stats(_max);
        Stat3 = new Stats(_max);
        Stat4 = new Stats(_max);
        Stat5 = new Stats(_max);
        Stat6 = new Stats(_max);
        Stat7 = new Stats(_max);
        Stat8 = new Stats(_max);
    }
}
public class Stats
{
    public string Name { get; set; }
    public string Desc { get; set; }
    public int Value { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public bool TurnedOn { get; set; }


    public BaseStatChosen Base;
    public Base Base1 { get; set; }
    public Base Base2 { get; set; }
    public Stats()
    {
     
    }
    public Stats(int _max)
    {
        Value = 1;
        Min = 1;
        Max = _max;
    }
    public enum BaseStatChosen
    {
        HP,
        DEF,
        ATT,
        NULL
    }
}
