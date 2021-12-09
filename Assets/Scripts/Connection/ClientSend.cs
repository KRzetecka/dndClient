using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.GameRoom;

public class ClientSend : MonoBehaviour
{
    public static ClientSend instance;

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

    public void SendDataToServer(byte[] _data)
    {
        try
        {
            if (ClientTCP.instance.socket != null)
            {
                ByteBuffer _buffer = new ByteBuffer();
                _buffer.WriteInt(_data.GetUpperBound(0) - _data.GetLowerBound(0) + 1);
                _buffer.WriteBytes(_data);
                ClientTCP.instance.stream.BeginWrite(_buffer.ToArray(), 0, _buffer.ToArray().Length, null, null);
                _buffer.Dispose();
            }
        }
        catch (Exception _ex)
        {
            Debug.Log("Error sending data: " + _ex);
        }
    }

    public void WelcomeReceived()
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.welcomeReceived);
        _buffer.WriteString("Guest");
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    public void GetRoomsList()
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.getRoomList);
        _buffer.WriteString("Test");
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }

    public void GetRoomDesc(string name)
    {
        if (name == null || name == "") return;
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.getRoomDesc);
        _buffer.WriteString(name);
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    public void isPasswordCorrect(string passw, string room)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.isPasswordCorrect);
        _buffer.WriteString(passw);
        _buffer.WriteString(room);
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    public void registerUser(string name, string passw)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.userRegister);
        _buffer.WriteString(name);
        _buffer.WriteString(passw);
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    public void loginUser(string name, string passw)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.userLogin);
        _buffer.WriteString(name);
        _buffer.WriteString(passw);
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    public void logoutUser()
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.userLogout);
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    public void loadRoom(string name)
    {

    }
    public void leaveRoom()
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.leaveRoom);
        //_buffer.WriteString(name);
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    internal void newCharacter(Character _char)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.newCharacter);
        _buffer.WriteString(XmlTool.SharpObjectToXMLString(_char));
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }

    public void refreshData(string _option)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.refreshRoomData);
        _buffer.WriteString(_option);
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    public void changeSettings(string _option, string _setting)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.changeSettings);
        _buffer.WriteString(_option);
        _buffer.WriteInt(0);
        _buffer.WriteString(_setting);
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    public void changeSettings(string _option, int _setting)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.changeSettings);
        _buffer.WriteString(_option);
        _buffer.WriteInt(1);
        _buffer.WriteInt(_setting);
        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
    public void raceSettings(bool _isRaceNew, string _name, int _min, int _max, string _desc,List<string> _classes, Dictionary<string, int> _seq)
    {
        ByteBuffer _buffer = new ByteBuffer();
        _buffer.WriteInt((int)ClientPackets.editRace);
        _buffer.WriteBool(_isRaceNew);
        _buffer.WriteString(_name);
        _buffer.WriteInt(_min);
        _buffer.WriteInt(_max);
        _buffer.WriteString(_desc);

        _buffer.WriteInt(_classes.Count);
        foreach(var item in _classes)
        {
            _buffer.WriteString(item);
        }
        _buffer.WriteInt(_seq.Count);
        foreach (var item in _seq)
        {
            _buffer.WriteString(item.Key);
            _buffer.WriteInt(item.Value);
        }

        SendDataToServer(_buffer.ToArray());
        _buffer.Dispose();
    }
}
