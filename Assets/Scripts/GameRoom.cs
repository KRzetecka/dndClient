using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Assets.Scripts.GameRoom;
using UnityEngine.UI;

[XmlRoot("GameRoom")]
[XmlType("GameRoom")]
public class GameRoom : MonoBehaviour
{
    
    public static GameRoom instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public bool isPlayerDM = false;
    public bool doIHaveChar = false;


    public int ID { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Password { get; set; }
    public string Owner { get; set; }
    public string OwnerPassword { get; set; }
    public bool isRoomLocked { get; set; }

    public int PlayersOn { get; set; }

    public bool IsTurnedOn { get; set; }
    public bool IsProtected { get; set; }

    public List<string> Players { get; set; }

    internal List<Equipment> Items { get; set; }

    internal List<Character> Characters { get; set; }

    internal List<CharacterClass> Classes { get; set; }

    public List<CharacterRace> Races { get; set; }

    public CharacterStats Stats { get; set; }

    public Level LevelInfo { get; set; }


    //tmp's for new char
    public Stats n_Stats;

    public GameRoom()
    {
        Players = new List<string>();
        Items = new List<Equipment>();
        Characters = new List<Character>();
        Races = new List<CharacterRace>();
        Classes = new List<CharacterClass>();
        Stats = new CharacterStats();
        LevelInfo = new Level();
    }

    public void enterRoom()
    {
        if (isPlayerDM == true)
        {
            SyncContext.RunOnUnityThread(() =>
            {
                //Get addonal info from server

                //Prepare View
                GameObject.Find("Canvas RoomDM").GetComponent<Canvas>().enabled = true;
                GameObject.Find("Canvas RoomPlayer").GetComponent<Canvas>().enabled = false;
                
                //prepare BasicSettings

               // GameObject.Find("InputField XPGap").GetComponent<InputField>().text = LevelInfo.LvlGaps[1].ToString();
                //prepare RaceSettings
                //ListOfStuff.instance.refreshRaceList();

                //Hide Room select on finish
                GameObject.Find("Canvas RoomSelect").GetComponent<Canvas>().enabled = false;


            });
            
        }
        else
        {
            //Get addonal info from server
            if (doIHaveChar == true) // I have char
            {
                SyncContext.RunOnUnityThread(() =>
                {
                    GameObject.Find("NewChar Button").GetComponent<Button>().enabled = false;
                    GameObject.Find("EQ Button").GetComponent<Button>().interactable = false;
                    GameObject.Find("AB Button").GetComponent<Button>().interactable = false;
                    GameObject.Find("AT Button").GetComponent<Button>().interactable = false;
                });
            }
            else // I dont have char
            {
                SyncContext.RunOnUnityThread(() =>
                {
                    GameObject.Find("NewChar Button").GetComponent<Button>().enabled = true;
                    GameObject.Find("EQ Button").GetComponent<Button>().interactable = false;
                    GameObject.Find("AB Button").GetComponent<Button>().interactable = false;
                    GameObject.Find("AT Button").GetComponent<Button>().interactable = false;
                });
            }
            SyncContext.RunOnUnityThread(() =>
            {
                //Prepare View
                GameObject.Find("Canvas RoomDM").GetComponent<Canvas>().enabled = false;
                GameObject.Find("Canvas RoomPlayer").GetComponent<Canvas>().enabled = true;
                //Hide Room select on finish
                GameObject.Find("Canvas RoomSelect").GetComponent<Canvas>().enabled = false;

                //New Character Window info fill
                GameObject.Find("Dropdown Race").GetComponent<Dropdown>().options.Clear();
                Dropdown.OptionData dropdownData;
                foreach (var race in Races)
                {
                    dropdownData = new Dropdown.OptionData();
                    dropdownData.text = race.Name;
                    GameObject.Find("Dropdown Race").GetComponent<Dropdown>().options.Add(dropdownData);
                }
            });
            SyncContext.RunOnUnityThread(() =>
            {
                //TO-DO CLASES ONLY RELATED TO RACE
                //GameObject.Find("Dropdown Class");
                //=====
            });
            SyncContext.RunOnUnityThread(() =>
            {
                GameObject.Find("Text Stat1").GetComponent<Text>().text = Stats.Stat1.Name;
                GameObject.Find("Text Stat2").GetComponent<Text>().text = Stats.Stat2.Name;
                GameObject.Find("Text Stat3").GetComponent<Text>().text = Stats.Stat3.Name;
                GameObject.Find("Text Stat4").GetComponent<Text>().text = Stats.Stat4.Name;
                GameObject.Find("Text Stat5").GetComponent<Text>().text = Stats.Stat5.Name;
                GameObject.Find("Text Stat6").GetComponent<Text>().text = Stats.Stat6.Name;
                GameObject.Find("Text Stat7").GetComponent<Text>().text = Stats.Stat7.Name;
                GameObject.Find("Text Stat8").GetComponent<Text>().text = Stats.Stat8.Name;
                GameObject.Find("Placeholder Stat1").GetComponent<Text>().text = "1/" + Stats.Stat1.Max;
                GameObject.Find("Placeholder Stat2").GetComponent<Text>().text = "1/" + Stats.Stat2.Max;
                GameObject.Find("Placeholder Stat3").GetComponent<Text>().text = "1/" + Stats.Stat3.Max;
                GameObject.Find("Placeholder Stat4").GetComponent<Text>().text = "1/" + Stats.Stat4.Max;
                GameObject.Find("Placeholder Stat5").GetComponent<Text>().text = "1/" + Stats.Stat5.Max;
                GameObject.Find("Placeholder Stat6").GetComponent<Text>().text = "1/" + Stats.Stat6.Max;
                GameObject.Find("Placeholder Stat7").GetComponent<Text>().text = "1/" + Stats.Stat7.Max;
                GameObject.Find("Placeholder Stat8").GetComponent<Text>().text = "1/" + Stats.Stat8.Max;

                n_Stats = new Stats(Stats.Stat1.Max);
            });



        }
    }

    public void decreaseButton(int stat)
    {
        string value = GameObject.Find("Placeholder Stat" + stat).GetComponent<Text>().text;
        string[] values = value.Split('/');
        int oldValue = int.Parse(values[0]);
        string newValue;
        if (oldValue <= 1) newValue = "1";
        else newValue = (oldValue - 1).ToString();
        GameObject.Find("Placeholder Stat" + stat).GetComponent<Text>().text = newValue + "/" + values[1];
    }
    public void increaseButton(int stat)
    {
        string value = GameObject.Find("Placeholder Stat" + stat).GetComponent<Text>().text;
        string[] values = value.Split('/');
        int oldValue = int.Parse(values[0]);
        int max = int.Parse(values[1]);
        string newValue;
        if (oldValue >= max) newValue = max.ToString();
        else newValue = (oldValue + 1).ToString();
        GameObject.Find("Placeholder Stat" + stat).GetComponent<Text>().text = newValue + "/" + values[1];
    }



}

