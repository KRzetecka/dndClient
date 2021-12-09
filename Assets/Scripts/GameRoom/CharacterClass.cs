using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class CharacterClass
{
    public string ClassName { get; set; }
    public Dictionary<string, int> SEQ { get; set; } //item+amount
    public CharacterClass()
    {
        SEQ = new Dictionary<string, int>();
    }
    public CharacterClass(string _name)
    {
        SEQ = new Dictionary<string, int>();
        ClassName = _name;
    }
}

