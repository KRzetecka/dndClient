using System;
using UnityEngine;
using System.Xml.Serialization;

namespace Assets.Scripts.GameRoom
{
    public class Equipment
    {
        public string Name { get; set; }
        internal EquipmentCategory Category;
        internal WearableType WearableType;
        internal WeaponType WeaponType;
        public int Damage { get; set; }
        public int Defence { get; set; }
        public int HPRecovery { get; set; }
        public int ManaRecovery { get; set; }
        public int Condition { get; set; }
        public int BaseValue { get; set; }
        public int PictureID { get; set; }
        public string Description { get; set; }

        public Equipment() {
        }

    }

    enum EquipmentCategory
    {
        Wearable,
        Money,
        Food,
        Misc,
        Liquid,
        Ingredient,
        Herb,
        Loot
    }
    enum WearableType
    {
        NotWearable,
        Torso,
        Legs,
        Feets,
        Head,
        Ears,
        Makeup,
        Face,
        Weapon,
        Necklese,
        Ring,
    }
    enum WeaponType
    {
        NotWeapon,

        //General
        Malee,
        Ranged,
        Magic,
        //Malee
        Sword,
        Axe,
        Club,
        Spear,
        Shield,
        Hammer,
        Flexible, //na sznurkach, np. nunczako
        Gloves,
        //Ranged
        Bow,
        Quiver,
        Gun,
        Boomerang,
        ThrowableExplosive,
        Throwable,
        Crossbow,
        //Magic
        Wand,
        Elemental,
        Summoning,
        Psychic,
        //Other
        Explosive,
        MaritalArt,
        Instrument,

        Other,
    }

}
