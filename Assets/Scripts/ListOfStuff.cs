using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ListOfStuff : MonoBehaviour
{
    public static ListOfStuff instance;
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
    //RACE
    public List<GameObject> DMRaceList = new List<GameObject>();
    public Button btnPrefabRaceList;
    public Button DMRaceListFirst;
    
    public List<GameObject> DMRaceClassAllowed = new List<GameObject>();
    public GameObject PrefabRaceClassAllowed;
    
    public List<GameObject> DMRaceSEQ = new List<GameObject>();
    public GameObject PrefabRaceSEQ;

    public void refreshRaceList()
    {
        for(int i = 1; i<DMRaceList.Count; i++)
        {
            Destroy(DMRaceList[i].gameObject);
        }
        DMRaceList.Clear();
        DMRaceList.Add(DMRaceListFirst.gameObject); //NewRace Button
        foreach (var race in GameRoom.instance.Races)
        {
            Button btn = Instantiate(btnPrefabRaceList, GameObject.Find("Content RaceList").transform);
            btn.name = "btn_" + race.Name;

            btn.transform.gameObject.SetActive(true);
            btn.GetComponentInChildren<Text>().text = race.Name;
            btn.GetComponentInChildren<Text>().name = "btn_text_"+race.Name;
            btn.onClick.AddListener(() =>
            {
                DMTools.instance.listEditRace(race.Name);
                DMTools.instance.lastPickedRace = race.Name;
                refreshRaceClassAllowed();
                refreshRaceSeq();
            });
            DMRaceList.Add(btn.gameObject);
        }
    }
    public void refreshRaceClassAllowed()
    {
        List<string> allowed = new List<string>();
        allowed = GameRoom.instance.Races.Find(e => e.Name == DMTools.instance.lastPickedRace).AllowedClasses;
        foreach (var element in DMRaceClassAllowed)
        {
            if (allowed.Contains(element.GetComponentInChildren<Text>().text))
            {
                element.GetComponentInChildren<Toggle>().isOn = true;
            }
            else
            {
                element.GetComponentInChildren<Toggle>().isOn = false;
            }          
        }
    }
    public void refreshRaceSeq()
    {
        Dictionary<string, int> Allowed = new Dictionary<string, int>();
        Allowed = GameRoom.instance.Races.Find(e => e.Name == DMTools.instance.lastPickedRace).SEQ;
        foreach(var element in DMRaceSEQ)
        {
            if (Allowed.ContainsKey(element.GetComponentInChildren<Text>().text))
            {
                int value;
                Allowed.TryGetValue(element.GetComponentInChildren<Text>().text, out value);
                element.GetComponentInChildren<Toggle>().isOn = true;
                element.GetComponentInChildren<InputField>().text = value.ToString();
            }
            else
            {
                int value = 1;
                element.GetComponentInChildren<Toggle>().isOn = false;
                element.GetComponentInChildren<InputField>().text = value.ToString();

            }
        }
    }

    // ATTRIBUTES/STATS
    public void refreshStats()
    {
        GameObject Stat1, Stat2, Stat3, Stat4, Stat5, Stat6, Stat7, Stat8;
        Stat1 = GameObject.Find("Stat1").transform.GetChild(1).gameObject;
        Stat2 = GameObject.Find("Stat2").transform.GetChild(1).gameObject;
        Stat3 = GameObject.Find("Stat3").transform.GetChild(1).gameObject;
        Stat4 = GameObject.Find("Stat4").transform.GetChild(1).gameObject;
        Stat5 = GameObject.Find("Stat5").transform.GetChild(1).gameObject;
        Stat6 = GameObject.Find("Stat6").transform.GetChild(1).gameObject;
        Stat7 = GameObject.Find("Stat7").transform.GetChild(1).gameObject;
        Stat8 = GameObject.Find("Stat8").transform.GetChild(1).gameObject;

        Stat1.GetComponent<InputField>().text = GameRoom.instance.Stats.Stat1.Name;
        Stat2.GetComponent<InputField>().text = GameRoom.instance.Stats.Stat2.Name;
        Stat3.GetComponent<InputField>().text = GameRoom.instance.Stats.Stat3.Name;
        Stat4.GetComponent<InputField>().text = GameRoom.instance.Stats.Stat4.Name;
        Stat5.GetComponent<InputField>().text = GameRoom.instance.Stats.Stat5.Name;
        Stat6.GetComponent<InputField>().text = GameRoom.instance.Stats.Stat6.Name;
        Stat7.GetComponent<InputField>().text = GameRoom.instance.Stats.Stat7.Name;
        Stat8.GetComponent<InputField>().text = GameRoom.instance.Stats.Stat8.Name;

    }


    // LISTS
    public void refreshList(List<GameObject> List, GameObject prefab, List<string> fillList)
    {
        for (int i = 0; i < List.Count; i++)
        {
            Destroy(List[i].gameObject);
        }
        List.Clear();
        foreach (var _object in fillList)
        {
            GameObject tmp = Instantiate(prefab, prefab.transform.parent);
            tmp.name = "object_" + _object;
            tmp.transform.gameObject.SetActive(true);
            tmp.tag = "Untagged";
            tmp.GetComponentInChildren<Text>().text = _object;
            List.Add(tmp);
        }
    }
    public void refreshListWithAmount(List<GameObject> List, GameObject prefab, Dictionary<string, int> fillList)
    {
        for (int i = 0; i < List.Count; i++)
        {
            Destroy(List[i].gameObject);
        }
        List.Clear();
        foreach (var _object in fillList)
        {
            GameObject tmp = Instantiate(prefab, prefab.transform.parent);
            tmp.name = "object_" + _object;
            tmp.transform.gameObject.SetActive(true);
            tmp.tag = "Untagged";
            tmp.GetComponentInChildren<Text>().text = _object.Key;
            tmp.GetComponentInChildren<InputField>().text = _object.Value.ToString();
            tmp.GetComponentInChildren<Toggle>().isOn = false;

            List.Add(tmp);
        }
    }

}
