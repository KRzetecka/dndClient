using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewCharacter : MonoBehaviour
{
    public void loadOptions()
    {
        //Load Races
        var RaceScroll = GameObject.Find("Dropdown Race");
        RaceScroll.GetComponent<Dropdown>().ClearOptions();
        List<string> raceOptions = new List<string>();
        foreach (var race in GameRoom.instance.Races)
        {
            raceOptions.Add(race.Name);
        }
        RaceScroll.GetComponent<Dropdown>().AddOptions(raceOptions);
        //Load Classes avaible to race

        //Load Stats

        return;
    }
    public void checkChar()
    {
        Character tmp = new Character();
        tmp.CharName = GameObject.Find("InputField NewName").GetComponent<Text>().text;
        tmp.CharAge = int.Parse(GameObject.Find("InputField NewName").GetComponent<Text>().text);
        tmp.CharRace.Name = GameObject.Find("Dropdown Race").GetComponent<Text>().text;
        tmp.CharClass.ClassName = GameObject.Find("Dropdown Race").GetComponent<Text>().text;
        tmp.CharStory = GameObject.Find("InputField NewDesc").GetComponent<Text>().text;
        switch (GameObject.Find("Dropdown Race").GetComponent<Text>().text)
        {
            case "Male":
                tmp.CharSex = CharacterSex.Male;
                break;
            case "Female":
                tmp.CharSex = CharacterSex.Female;
                break;
            case "Other":
                tmp.CharSex = CharacterSex.Other;
                break;
            case "None":
                tmp.CharSex = CharacterSex.None;
                break;
        }
        if (!IsLenghtOK(4, 30, tmp.CharName, "character name"))
        {
            return;
        }
        if (!IsLenghtOK(0, 500, tmp.CharName, "character story"))
        {
            return;
        }
        CharacterRace tmprace = GameRoom.instance.Races.Find(e => e.Name == tmp.CharRace.Name);
        if (!IsAgeFine(tmprace.MinAge, tmprace.MaxAge, tmp.CharAge))
        {
            return;
        }

    }


    private bool IsLenghtOK(int min, int max, string text, string objectName)
    {
        if (text.Length >= min && text.Length <= max) return true;
        if (text == null || text == "")
        {
            SyncContext.RunOnUnityThread(() =>
            {
                GameObject.Find("Image Message").GetComponentInChildren<Text>().text = "Lenght of " +objectName+ " is empty.";
            });
            WindowOperate.instance.WindowIN("Image Message");
            return false;
        }
        if (text.Length < min)
        {
            SyncContext.RunOnUnityThread(() =>
            {
                GameObject.Find("Image Message").GetComponentInChildren<Text>().text = "Lenght of " + objectName + " is too short. The size should be between " + min + " and " + max + ".";
            });
            WindowOperate.instance.WindowIN("Image Message");
            return false;
        }
        else
        {
            SyncContext.RunOnUnityThread(() =>
            {
                GameObject.Find("Image Message").GetComponentInChildren<Text>().text = "Lenght of " + objectName + " is too long. The size should be between " + min + " and " + max + ".";
            });
            WindowOperate.instance.WindowIN("Image Message");
            return false;
        }
        return false;                                           
    }
    private bool IsAgeFine(int min, int max, int value)
    {
        if (value >= min && value <= max) return true;
        SyncContext.RunOnUnityThread(() =>
        {
            GameObject.Find("Image Message").GetComponentInChildren<Text>().text = "Your age is not correct. Try something between " + min + " and " + max + ".";
        });
        WindowOperate.instance.WindowIN("Image Message");
        return false;
    }
}
