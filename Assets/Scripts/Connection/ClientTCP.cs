using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System;

public class ClientTCP : MonoBehaviour
{
    public static ClientTCP instance;
    public string ip = "127.0.0.1";
    public string ip2 = "25.88.180.180";
    public int port = 9423;
    public int myPlayerID = 0;
    //public bool logged = false;

    public TcpClient socket;
    public NetworkStream stream;
    private byte[] receiveBuffer;

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

    private void Start()
    {
        GameObject.FindGameObjectWithTag("TextStart").GetComponent<Text>().text = "Connecting...";
        ConnectToServer();
        
    }

    public void ConnectToServer()
    {
        ClientHandle.instance.InitPackets();

        socket = new TcpClient
        {
            ReceiveBufferSize = 8192,
            SendBufferSize = 8192,
            NoDelay = true,
        };
 
        receiveBuffer = new byte[socket.ReceiveBufferSize];
        socket.BeginConnect(ip2, port, ConnectCallback, socket);
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
    private void ConnectCallback(IAsyncResult _result)
    {

        try
        {
            socket.EndConnect(_result);
            SyncContext.RunOnUnityThread(() =>
            {
                GameObject.FindGameObjectWithTag("StartCanvas").GetComponentInChildren<Button>().interactable = true;
                GameObject.FindGameObjectWithTag("TextStart").GetComponent<Text>().text = "All good! Lets go!";
            });
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            for (int i = 10; i > 1; i--)
            {
                SyncContext.RunOnUnityThread(() =>
                {
                    GameObject.FindGameObjectWithTag("TextStart").GetComponent<Text>().text = "Cannot connect. Server is offline or there is no internet connection.\n Try restarting your game.";
                });
            }
        }
        

        if (!socket.Connected)
        {
            return;
        }
        else
        {
            socket.NoDelay = true;
            stream = socket.GetStream();
            stream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, ReceivedData, null);
        }
        
    }
    private void ReceivedData(IAsyncResult _result)
    {
        try
        {
            int _byteLength = stream.EndRead(_result);
            if (_byteLength <= 0)
            {
                CloseConnection();
                return;
            }
            byte[] _tempBuffer = new byte[_byteLength];
            Array.Copy(receiveBuffer, _tempBuffer, _byteLength);
            ClientHandle.instance.HandleData(_tempBuffer);
            stream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, ReceivedData, null);
        }
        catch (Exception _ex)
        {
            Debug.Log("Error while receiving data: " + _ex);
            //CloseConnection();          
            return;
        }
    }
    private void CloseConnection()
    {
        SyncContext.RunOnUnityThread(() => {
            WindowOperate.instance.WindowIN("Image Disconnected");
        });
        socket.Close();
    }

}
