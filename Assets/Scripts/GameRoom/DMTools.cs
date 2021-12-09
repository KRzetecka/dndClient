using Assets.Scripts.GameRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DMTools : MonoBehaviour
{
    public static DMTools instance;
    public Button btnPrefabRaceList;

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



    //====================================================== LVL

    public void setNewMaxLvL()
    {
        int newLvL = 1;
        try
        {
            newLvL = int.Parse(GameObject.Find("InputField LvlMax").GetComponent<InputField>().text);
        }
        catch
        {
            Message.instance.message("Something is wrong with max lvl value.");
            return;
        }
        if (newLvL <= 1)
        {
            Message.instance.message("Level cannot be lower than 1.");
            return;
        }
        if (newLvL >= 99999)
        {
            Message.instance.message("99999 Level is the limit.");
            return;
        }
        ClientSend.instance.changeSettings("MaxLevel", newLvL);
        GameRoom.instance.LevelInfo.MaxLvl = newLvL;
        refreshLvlList();
    }
    public void lvlListOnRoomStart() //set first element on view (1lvl)
    {
        TMPro.TMP_Dropdown DropDown;
        SyncContext.RunOnUnityThread(() => {
            DropDown = GameObject.Find("Dropdown XPGap").GetComponent<TMPro.TMP_Dropdown>();
            DropDown.RefreshShownValue();
            LvlUpGapRefresh();
        });
    }
    public void refreshLvlList() //Dropdown fill 1-x
    {
        SyncContext.RunOnUnityThread(() => {
            GameObject.Find("InputField LvlMax").GetComponent<InputField>().text = GameRoom.instance.LevelInfo.MaxLvl.ToString();
            TMPro.TMP_Dropdown DropDown = GameObject.Find("Dropdown XPGap").GetComponent<TMPro.TMP_Dropdown>();
            TMPro.TMP_Dropdown.OptionData od;
            DropDown.ClearOptions();
            for (int i = 1; i <= GameRoom.instance.LevelInfo.MaxLvl; i++)
            {
                od = new TMPro.TMP_Dropdown.OptionData();
                od.text = i.ToString();
                DropDown.options.Add(od);
            }
            DropDown.RefreshShownValue();
        });
        LvlUpGapRefresh();
    }
    public void LvlUpGapRefresh() //Gap for selected lvl
    {
        SyncContext.RunOnUnityThread(() => {
            int lvl = GameObject.Find("Dropdown XPGap").GetComponent<TMPro.TMP_Dropdown>().value+1;
            int value = GameRoom.instance.LevelInfo.LvlGaps[lvl];
            GameObject a = GameObject.Find("InputField XPGap");
            a.GetComponent<InputField>().text = value.ToString();
        });
    }
    public void changeLvlGap()
    {
        SyncContext.RunOnUnityThread(() =>
        {
            int value = int.Parse(GameObject.Find("InputField XPGap").GetComponent<InputField>().text);
            int lvl = GameObject.Find("Dropdown XPGap").GetComponent<TMPro.TMP_Dropdown>().value + 1; 
            ClientSend.instance.changeSettings("NewGap." + lvl, value);
            //GameRoom.instance.LevelInfo.LvlGaps[lvl] = value;
        });
    }
    //====================================================== ATTRIBUTES
    public void changeAttributeName(string _statNum)
    {
        string name = GameObject.Find("Inputfield DMStat" + _statNum).GetComponent<InputField>().text;
        if (name.Length > 4 && name.Length < 30)
        {
            if (CharTest.instance.isTextFine(name))
            {
                ClientSend.instance.changeSettings("AttributeName" + _statNum, name);
            }
            else
            {
                Message.instance.message("Attribute name contains forbidden character.");
            }
        }
        else
        {
            GameObject.Find("Image Message").GetComponent<Text>().text = "Attribute name is too long or too short. Proper lenght is between 4 and 30.";
            WindowOperate.instance.WindowIN("Message");
        }
    }
    public void refreshAttributeSettings()
    {
        SyncContext.RunOnUnityThread(() =>
        {
            GameObject.Find("InputField DMStat1").GetComponent<InputField>().text = GameRoom.instance.Stats.Stat1.Name;
            GameObject.Find("InputField DMStat2").GetComponent<InputField>().text = GameRoom.instance.Stats.Stat2.Name;
            GameObject.Find("InputField DMStat3").GetComponent<InputField>().text = GameRoom.instance.Stats.Stat3.Name;
            GameObject.Find("InputField DMStat4").GetComponent<InputField>().text = GameRoom.instance.Stats.Stat4.Name;
            GameObject.Find("InputField DMStat5").GetComponent<InputField>().text = GameRoom.instance.Stats.Stat5.Name;
            GameObject.Find("InputField DMStat6").GetComponent<InputField>().text = GameRoom.instance.Stats.Stat6.Name;
            GameObject.Find("InputField DMStat7").GetComponent<InputField>().text = GameRoom.instance.Stats.Stat7.Name;
            GameObject.Find("InputField DMStat8").GetComponent<InputField>().text = GameRoom.instance.Stats.Stat8.Name;
        });
    }
    //====================================================== RACES
    public string lastPickedRace = "";
    public void refreshRaceList()
    {
        GameObject a = GameObject.Find("Content RaceList");
        GameObject b = GameObject.Find("Scroll View RaceList");
        foreach(var obj in GameObject.FindGameObjectsWithTag("TEMPLATE"))
        {
            if(obj.active == true)
            {
                GameObject tmp = GameObject.Find(obj.name).gameObject;
                DestroyImmediate(tmp.gameObject);
            }
        }
        foreach(var race in GameRoom.instance.Races)
        {
            Button btn = Instantiate(btnPrefabRaceList, GameObject.Find("Content RaceList").transform);
            btn.name = "btn_" + race.Name;
            btn.onClick.AddListener(() =>
            {
                listEditRace(race.Name);
            });
            btn.transform.gameObject.SetActive(true);
            btn.GetComponentInChildren<Text>().text = race.Name;
            btn.transform.SetParent(GameObject.Find("Content RaceList").transform);
        }
        b.GetComponent<ScrollRect>().CalculateLayoutInputVertical();
    }

    public void listNewRace()
    {
        GameObject.Find("Button NRaceDelete").GetComponent<Button>().interactable = false;
        GameObject.Find("Button NRaceEdit").GetComponent<Button>().interactable = false;
        GameObject.Find("Button NRaceNew").GetComponent<Button>().interactable = true;

        GameObject.Find("InputField NRaceName").GetComponent<InputField>().text = "";
        GameObject.Find("InputField NRaceMinAge").GetComponent<InputField>().text = "";
        GameObject.Find("InputField NRaceMaxAge").GetComponent<InputField>().text = "";
        GameObject.Find("InputFieldtmp NRaceDesc").GetComponent<TMPro.TMP_InputField>().text = "";

        Dictionary<string, int> empty = new Dictionary<string, int>();
        foreach (var item in GameRoom.instance.Items)
        {
            empty.Add(item.Name, 1);
        }
        SyncContext.RunOnUnityThread(() =>
        {
            ListOfStuff.instance.refreshListWithAmount(ListOfStuff.instance.DMRaceSEQ, ListOfStuff.instance.PrefabRaceSEQ, empty);
        });
    }
    public void listEditRace(string name)
    {
        GameObject.Find("Button NRaceDelete").GetComponent<Button>().interactable = true;
        GameObject.Find("Button NRaceEdit").GetComponent<Button>().interactable = true;
        GameObject.Find("Button NRaceNew").GetComponent<Button>().interactable = false;

        string race = GameObject.Find("btn_" + name).GetComponentInChildren<Text>().text;
        CharacterRace CRace = GameRoom.instance.Races.Find(e => e.Name == race);
        GameObject.Find("InputField NRaceName").GetComponent<InputField>().text = CRace.Name;
        GameObject.Find("InputField NRaceMinAge").GetComponent<InputField>().text = CRace.MinAge.ToString();
        GameObject.Find("InputField NRaceMaxAge").GetComponent<InputField>().text = CRace.MaxAge.ToString();
        GameObject.Find("InputFieldtmp NRaceDesc").GetComponent<TMPro.TMP_InputField>().text = CRace.Description;
    }
    public void editRace()
    {
        string Name, Desc;
        int Min, Max;
        List<string> Classes;
        Dictionary<string, int> Seq;
        GetRaceData(out Name, out Min, out Max, out Desc, out Classes, out Seq);
        ClientSend.instance.raceSettings(false, Name, Min, Max, Desc, Classes, Seq);
    }
    public void newRace()
    {
        string Name, Desc;
        int Min , Max;
        List<string> Classes;
        Dictionary<string, int> Seq;
        GetRaceData(out Name, out Min, out Max, out Desc, out Classes, out Seq);
        ClientSend.instance.raceSettings(true, Name, Min, Max, Desc, Classes, Seq);
    }
    private void GetRaceData(out string _name, out int _min, out int _max, out string _desc, out List<string> _classes, out Dictionary<string, int> _seq)
    {
        _name = GameObject.Find("InputField NRaceName").GetComponent<InputField>().text;
        _min = int.Parse(GameObject.Find("InputField NRaceMinAge").GetComponent<InputField>().text);
        _max = int.Parse(GameObject.Find("InputField NRaceMaxAge").GetComponent<InputField>().text);
        _desc = GameObject.Find("InputFieldtmp NRaceDesc").GetComponent<TMPro.TMP_InputField>().text;

        _classes = new List<string>();
        _seq = new Dictionary<string, int>();

        GameObject classObj = GameObject.Find("Content DMRaceClasses");
        GameObject seqObj = GameObject.Find("Content DMRaceSEQ");

        for(int i = 0; i < classObj.transform.childCount; i++)
        {
            if(classObj.transform.GetChild(i).tag != "TEMPLATE")
            {
                if (classObj.transform.GetChild(i).GetComponentInChildren<Toggle>().isOn) {
                    _classes.Add(classObj.transform.GetChild(i).GetComponentInChildren<Text>().text);
                }
            }
        }
        for (int i = 0; i < seqObj.transform.childCount; i++)
        {
            if (seqObj.transform.GetChild(i).tag != "TEMPLATE")
            {
                if (seqObj.transform.GetChild(i).GetComponentInChildren<Toggle>().isOn)
                {
                    _seq.Add(seqObj.transform.GetChild(i).GetComponentInChildren<Text>().text, int.Parse(seqObj.transform.GetChild(i).GetComponentInChildren<InputField>().text));
                }
            }
        }
    }
    public void removeRace()
    {
        List<string> Classes = new List<string>();
        Dictionary<string, int> Seq = new Dictionary<string, int>();
        Debug.Log(lastPickedRace);
        ClientSend.instance.raceSettings(false, lastPickedRace, 0, 0, "ToDelete", Classes, Seq);
        listNewRace();
    }
    //====================================================== CLASSES
    public void refreshClassList()
    {
        GameObject a = GameObject.Find("Content ClassList");
        GameObject b = GameObject.Find("Scroll View ClassList");
        foreach (var obj in GameObject.FindGameObjectsWithTag("TEMPLATE"))
        {
            if (obj.active == true)
            {
                GameObject tmp = GameObject.Find(obj.name).gameObject;
                DestroyImmediate(tmp.gameObject);
            }
        }
        foreach (var clas in GameRoom.instance.Classes)
        {
            Button btn = Instantiate(btnPrefabRaceList, GameObject.Find("Content ClassList").transform);
            btn.name = "btn_" + clas.ClassName;
            btn.onClick.AddListener(() =>
            {
                listEditRace(clas.ClassName);
            });
            btn.transform.gameObject.SetActive(true);
            btn.GetComponentInChildren<Text>().text = clas.ClassName;
            btn.transform.SetParent(GameObject.Find("Content ClassList").transform);
        }
        b.GetComponent<ScrollRect>().CalculateLayoutInputVertical();
    }


}
