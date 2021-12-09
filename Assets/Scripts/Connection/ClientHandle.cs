using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using Assets.Scripts;
using Assets.Scripts.GameRoom;
using static Stats;

public class ClientHandle : MonoBehaviour
{
    public static ClientHandle instance;
    private ByteBuffer buffer;
    public delegate void Packet(byte[] _data);
    public Dictionary<int, Packet> packets;

    bool CharactersRdy = false, BasicInfoRdy = false, ClassesRdy = false, RacesRdy = false, LvLInfoRdy = false, StatsRdy = false, ItemsRdy = true;
    bool RoomLoaded = false;
    int isPlayerDM = 0;

    public GameObject Panel;
    //public List<string> GameRoomList;

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
    public void InitPackets()
    {
        Debug.Log("Initializing packets...");
        packets = new Dictionary<int, Packet>
        {
            { (int)ServerPackets.welcome, Welcome },
            { (int)ServerPackets.roomlist, RoomList },
            { (int)ServerPackets.roomdesc, RoomDesc },
            { (int)ServerPackets.gameInit, GameInitiation },
            { (int)ServerPackets.passwordCheck, PasswordCheck },
            { (int)ServerPackets.registerError, RegisterMessage},
            { (int)ServerPackets.loginError, LoginMessage},
            { (int)ServerPackets.message, simpleMessage},

            { (int)ServerPackets.RoomData, GetRoomData},
            { (int)ServerPackets.CharacterData, GetCharactersData},
            { (int)ServerPackets.ClassData, GetClassData},
            { (int)ServerPackets.RaceData, GetRaceData},
            { (int)ServerPackets.EquipmentData, GetEquipmentData},
            { (int)ServerPackets.StatsData, GetStatsData},
            { (int)ServerPackets.LevelData, GetLevelInfo},
        };
    }
    private void simpleMessage(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        string msg = _buffer.ReadString();
        _buffer.Dispose();
        if(msg != null)
        {
            SyncContext.RunOnUnityThread(() =>
            {
                GameObject.Find("Text Message").GetComponent<Text>().text = msg;
                WindowOperate.instance.WindowIN("Image Message");
            });
        }      
    }
    public void ClearData()
    {
        CharactersRdy = false;
        BasicInfoRdy = false;
        RacesRdy = false;
        ClassesRdy = false;
        ItemsRdy = false;
        LvLInfoRdy = false;

        RoomLoaded = false;
    }
    public void HandleData(byte[] _data)
    {
        byte[] _tempBuffer = (byte[])_data.Clone();
        int _packetLength = 0;
        if (buffer == null)
        {
            buffer = new ByteBuffer();
        }
        buffer.WriteBytes(_tempBuffer);
        if (buffer.Count() == 0)
        {
            buffer.Clear();
            return;
        }
        if (buffer.Length() >= 4)
        {
            _packetLength = buffer.ReadInt(false);
            if (_packetLength <= 0)
            {
                buffer.Clear();
                return;
            }
        }
        while (_packetLength > 0 && _packetLength <= buffer.Length() - 4)
        {
            if (_packetLength <= buffer.Length() - 4)
            {
                buffer.ReadInt();
                _data = buffer.ReadBytes(_packetLength);
                HandlePackets(_data);
            }
            _packetLength = 0;
            if (buffer.Length() >= 4)
            {
                _packetLength = buffer.ReadInt(false);
                if (_packetLength <= 0)
                {
                    buffer.Clear();
                    return;
                }
            }
        }
        if (_packetLength <= 1)
        {
            buffer.Clear();
        }
    }
    private void HandlePackets(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        int _packetID = _buffer.ReadInt();
        _buffer.Dispose();
        if (packets.TryGetValue(_packetID, out Packet _packet))
        {
            _packet.Invoke(_data);
        }
    }
    private static void Welcome(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        string _msg = _buffer.ReadString();
        int _myPlayerID = _buffer.ReadInt();
        _buffer.Dispose();
        Debug.Log("Message from server: " + _msg);
        ClientTCP.instance.myPlayerID = _myPlayerID;
        ClientSend.instance.WelcomeReceived();

    }

    IEnumerator DisconnectedCoroutine()
    {
        int i = 10;
        while (i > 1){
            SyncContext.RunOnUnityThread(() =>
            {
                GameObject.FindGameObjectWithTag("TextStart").GetComponent<Text>().text = "Server disconnected or internet problem. Next attempt in " + i + " sec.";
            });
            --i;
            yield return new WaitForSeconds(1);
        }
    }

    private void RoomList(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        string _msg = _buffer.ReadString();
        _buffer.Dispose();
        List<string> RoomNames = new List<string>();
        using (TextReader ts = new StringReader(_msg))
        {
            XmlSerializer xml = new XmlSerializer(RoomNames.GetType());
            RoomNames = (List<string>)xml.Deserialize(ts);
        }

        SyncContext.RunOnUnityThread(() =>
        {
            Panel.GetComponent<RefreshGameList>().AddRooms(RoomNames);
        });
    }
    private void RoomDesc(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        string _msg = _buffer.ReadString();
        int _myPlayerID = _buffer.ReadInt();
        _buffer.Dispose();

        if(_msg.EndsWith("The room is password protected!"))
        {
            GameInit.instance.isProtected = true;
        }else GameInit.instance.isProtected = false;

        SyncContext.RunOnUnityThread(() =>
        {
            GameObject panel = GameObject.Find("RoomDescPanelText");
            panel.GetComponent<Text>().text = _msg;
        });

    }

    private void PasswordCheck(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        string _msg = _buffer.ReadString();
        int _myPlayerID = _buffer.ReadInt();
        _buffer.Dispose();
        if (_msg == "Wrong Password" || _msg == null)
        {
            SyncContext.RunOnUnityThread(() =>
            {
                GameObject ErrorText = GameObject.FindGameObjectWithTag("PasswordError");
                ErrorText.GetComponent<Text>().text = _msg;
            });

        }
        else if(_msg == "Success")
        {
            SyncContext.RunOnUnityThread(() =>
            {
                WindowOperate.instance.WindowOUT("Image PasswordBackground");
            });
        }
    }

    private void RegisterMessage(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        string _msg = _buffer.ReadString();
        int _myPlayerID = _buffer.ReadInt();
        _buffer.Dispose();
        SyncContext.RunOnUnityThread(() =>
        {
            GameObject.Find("ErrorTextRegister").GetComponent<Text>().text = _msg;
        });
    }
    private void LoginMessage(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        string _msg = _buffer.ReadString();
        string _playerName = _buffer.ReadString();
        int _myPlayerID = _buffer.ReadInt();
        _buffer.Dispose();

        if (_msg == "Logged")
        {
            //ClientTCP.instance.logged = true;
            SyncContext.RunOnUnityThread(() =>
            {
                WindowOperate.instance.WindowOUT("Image LoginWindow");
                GameObject.Find("LoggedAsPlaceholder").GetComponent<Text>().text = "Logged as: " + GameObject.Find("AccountNickInputText").GetComponent<Text>().text;
                GameObject.Find("ButtonLogin").GetComponent<Button>().interactable = true;
                Player.instance.PlayerLogin(_myPlayerID, _playerName);
                GameObject.Find("LoggedOutText").GetComponent<Text>().color = new Color(87, 238, 64);
                GameObject.Find("LoggedOutText").GetComponent<Text>().text = "Logged in!";
            });
            return;
        }

        SyncContext.RunOnUnityThread(() =>
        {
            GameObject.Find("LoginError").GetComponent<Text>().text = _msg;
            GameObject.Find("ButtonLogin").GetComponent<Button>().interactable = true;
        });
    }

    private void GetRoomData(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();

        isPlayerDM = _buffer.ReadInt();
        if (isPlayerDM == 1) GameRoom.instance.isPlayerDM = true;
        else GameRoom.instance.isPlayerDM = false;

        int doIHaveChar = _buffer.ReadInt();
        if (doIHaveChar == 1) GameRoom.instance.doIHaveChar = true;
        else GameRoom.instance.doIHaveChar = false;
        _buffer.Dispose();
        BasicInfoRdy = true;

        GameRoom.instance.enterRoom();
    }
    private void GetCharactersData(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        int num = _buffer.ReadInt();
        string s;
        for (int i = 0; i < num; i++)
        {
            s = "";
            s = _buffer.ReadString();
            Character c = new Character();
            c = XmlTool.DeserializeXmlToObject<Character>(s);
            GameRoom.instance.Characters.Add(c);
        }
        _buffer.Dispose();
        CharactersRdy = true;
    }
    private void GetClassData(byte[] _data)
    {
        GameRoom.instance.Classes.Clear();
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        int numOfClasses = _buffer.ReadInt(); int num2;
        string startItem, name;
        int amountItem;
        Dictionary<string, int> SEQ;
        for (int i = 0; i < numOfClasses; i++)
        {
            SEQ = new Dictionary<string, int>();
            CharacterClass c = new CharacterClass();
            

            name = _buffer.ReadString();            
            num2 = _buffer.ReadInt();
            for (int j = 0; j < num2; j++)
            {
                startItem = _buffer.ReadString();
                amountItem = _buffer.ReadInt();
                SEQ.Add(startItem, amountItem);
            }
            c.ClassName = name;
            c.SEQ = SEQ;
            GameRoom.instance.Classes.Add(c);
        }
        _buffer.Dispose();

        //fill allowed class for race
        List<string> classes = new List<string>();
        foreach (var clas in GameRoom.instance.Classes)
        {
            classes.Add(clas.ClassName);
        }
        SyncContext.RunOnUnityThread(() =>
        {
            ListOfStuff.instance.refreshList(ListOfStuff.instance.DMRaceClassAllowed, ListOfStuff.instance.PrefabRaceClassAllowed, classes);
        });

    }
    private void GetRaceData(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        int numOfRaces = _buffer.ReadInt();
        string itemName;
        int itemAmount;
        int allowedClassesAmount, SEQAmount;
        string allowedClass = "";
        GameRoom.instance.Races.Clear();
        for (int i = 0; i < numOfRaces; i++)
        {
            CharacterRace r = new CharacterRace();
            r.Name = _buffer.ReadString();
            r.MaxAge = _buffer.ReadInt();
            r.MinAge = _buffer.ReadInt();
            r.Description = _buffer.ReadString();

            allowedClassesAmount = _buffer.ReadInt();
            for (int j = 0; j< allowedClassesAmount; j++)
            {
                allowedClass = _buffer.ReadString();
                r.AllowedClasses.Add(allowedClass);
            }
            SEQAmount = _buffer.ReadInt();
            for (int j = 0; j < SEQAmount; j++)
            {
                itemName = _buffer.ReadString();
                itemAmount = _buffer.ReadInt();
                r.SEQ.Add(itemName, itemAmount);
            }
            GameRoom.instance.Races.Add(r);
        }
        _buffer.Dispose();
        RacesRdy = true;
        SyncContext.RunOnUnityThread(() =>
        {
            //DMTools.instance.refreshRaceList();
            ListOfStuff.instance.refreshRaceList();
        });
    }
    private void GetEquipmentData(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        int num = _buffer.ReadInt();
        string s;
        GameRoom.instance.Items.Clear();
        for (int i = 0; i < num; i++)
        {
            s = "";
            s = _buffer.ReadString();
            Equipment e = new Equipment();
            e = XmlTool.DeserializeXmlToObject<Equipment>(s);
            GameRoom.instance.Items.Add(e);
        }
        _buffer.Dispose();
        ItemsRdy = true;

        //refresh seq list for race
        Dictionary<string, int> items = new Dictionary<string, int>();
        foreach(var item in GameRoom.instance.Items)
        {
            items.Add(item.Name, 1);
        }
        SyncContext.RunOnUnityThread(() =>
        {
            ListOfStuff.instance.refreshListWithAmount(ListOfStuff.instance.DMRaceSEQ, ListOfStuff.instance.PrefabRaceSEQ, items);
        });
    }

    public static Base tmpStat;
    private void GetStatsData(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();

        GameRoom.instance.Stats.statsRedist = _buffer.ReadBool();
        GameRoom.instance.Stats.statPoints = _buffer.ReadInt();
        GameRoom.instance.Stats.rollChances = _buffer.ReadInt();

        Stats stat = new Stats();
        _buffer = getStatData(stat, _buffer);
        GameRoom.instance.Stats.Stat1 = stat;
        stat = new Stats();
        _buffer = getStatData(stat, _buffer);
        GameRoom.instance.Stats.Stat2 = stat;
        stat = new Stats();
        _buffer = getStatData(stat, _buffer);
        GameRoom.instance.Stats.Stat3 = stat;
        stat = new Stats();
        _buffer = getStatData(stat, _buffer);
        GameRoom.instance.Stats.Stat4 = stat;
        stat = new Stats();
        _buffer = getStatData(stat, _buffer);
        GameRoom.instance.Stats.Stat5 = stat;
        stat = new Stats();
        _buffer = getStatData(stat, _buffer);
        GameRoom.instance.Stats.Stat6 = stat;
        stat = new Stats();
        _buffer = getStatData(stat, _buffer);
        GameRoom.instance.Stats.Stat7 = stat;
        stat = new Stats();
        _buffer = getStatData(stat, _buffer);
        GameRoom.instance.Stats.Stat8 = stat;
        _buffer.Dispose();
        StatsRdy = true;

        if(isPlayerDM == 1)
        {
            DMTools.instance.refreshAttributeSettings();
        }
    }
    private static ByteBuffer getStatData(Stats s, ByteBuffer _buffer)
    {
        string sBase = _buffer.ReadString();
        if (sBase == "HP") s.Base = BaseStatChosen.HP;
        else if (sBase == "ATT") s.Base = BaseStatChosen.ATT;
        else if (sBase == "DEF") s.Base = BaseStatChosen.DEF;
        else s.Base = BaseStatChosen.NULL;
        DeserializeBaseStats(_buffer);
        s.Base1 = tmpStat;
        DeserializeBaseStats(_buffer);
        s.Base2 = tmpStat;
        s.Desc = _buffer.ReadString();
        s.Name = _buffer.ReadString();
        s.Max = _buffer.ReadInt();
        s.Min = _buffer.ReadInt();
        s.Value = _buffer.ReadInt();
        s.TurnedOn = _buffer.ReadBool();
        return _buffer;
    }
    private static ByteBuffer DeserializeBaseStats(ByteBuffer _buffer)
    {
        Base b = new Base();
        int Count = _buffer.ReadInt();
        if(Count != 0)
        {
            for (int i = 0; i <= Count; i++)
            {
                b.ATT.Add(i, _buffer.ReadInt());
            }
        }
        Count = _buffer.ReadInt();
        if (Count != 0)
        {
            for (int i = 0; i <= Count; i++)
            {
                b.DEF.Add(i, _buffer.ReadInt());
            }
        }
        Count = _buffer.ReadInt();
        if (Count != 0)
        {
            for (int i = 0; i <= Count; i++)
            {
                b.HP.Add(i, _buffer.ReadInt());
            }
        }
        tmpStat = b;
        return _buffer;
    }


    private void GetLevelInfo(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        GameRoom.instance.LevelInfo.LvlGaps.Clear();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();
        int num = _buffer.ReadInt();
        GameRoom.instance.LevelInfo.MaxLvl = num;
        string s;
        for (int i = 0; i <= num; i++)
        {
            int v =_buffer.ReadInt();
            GameRoom.instance.LevelInfo.LvlGaps.Add(i, v);
        }
        _buffer.Dispose();
        LvLInfoRdy = true;

        DMTools.instance.refreshLvlList();
        DMTools.instance.lvlListOnRoomStart();
    }
    private void GameInitiation(byte[] _data)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteBytes(_data);
        _buffer.ReadInt();


        int num = _buffer.ReadInt();
        string s;
        for (int i = 0; i<num; i++)
        {
            s = "";
            s = _buffer.ReadString();
            CharacterRace r = new CharacterRace();
            r = XmlTool.DeserializeXmlToObject<CharacterRace>(s);
            GameRoom.instance.Races.Add(r);
        }
        //GameRoom.instance.Races = XmlTool.SharpXMLStringToObject<List<CharacterRace>>(_buffer.ReadString());

        _buffer.Dispose();
        /*
        int isDM = _buffer.ReadInt();
        int charExists = _buffer.ReadInt();

        bool isPlayerDM;
        if (isDM == 1) isPlayerDM = true;
        else isPlayerDM = false;
        //
        int _myPlayerID = _buffer.ReadInt();
        


        //send data
        if(isPlayerDM == true)
        {
            SyncContext.RunOnUnityThread(() =>
            {
                //Get addonal info from server

                //Prepare View
                GameObject.Find("Canvas DM").GetComponent<Canvas>().enabled = true;
                GameObject.Find("Canvas Player").GetComponent<Canvas>().enabled = false;
                //Hide Room select on finish
                GameObject.Find("Canvas RoomSelect").GetComponent<Canvas>().enabled =false;
            });
        }
        else
        {
            //Get addonal info from server
            
            if (charExists == 1) // I have char
            {
                SyncContext.RunOnUnityThread(() =>
                {
                string characterData = _buffer.ReadString();
                GameObject.Find("MyChar").GetComponent<Character>().MyChar = XmlTool.SharpXMLStringToObject<Character>(characterData);
                GameObject.Find("NewChar Button").GetComponent<Button>().enabled = false;
                GameObject.Find("EQ Button").GetComponent<Button>().interactable = false;
                GameObject.Find("AB Button").GetComponent<Button>().interactable = false;
                GameObject.Find("AT Button").GetComponent<Button>().interactable = false;
                //Debug.Log(characterData);
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
            //roominfo
            //GameRoom.instance = XmlTool.SharpXMLStringToObject<GameRoom>(_buffer.ReadString());

            SyncContext.RunOnUnityThread(() =>
            {
            //Prepare View
            GameObject.Find("Canvas DM").GetComponent<Canvas>().enabled=false;
            GameObject.Find("Canvas Player").GetComponent<Canvas>().enabled=true;
                //Hide Room select on finish
            GameObject.Find("Canvas RoomSelect").GetComponent<Canvas>().enabled = false;
            });
        }
        _buffer.Dispose();
        */
        //Debug.Log(roomInfo);
    }

}
