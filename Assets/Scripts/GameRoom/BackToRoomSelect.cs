using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToRoomSelect : MonoBehaviour
{
    public void leaveGameRoom()
    {
        GameObject.Find("Canvas RoomSelect").GetComponent<Canvas>().enabled = true;
        ClientSend.instance.leaveRoom();
        ClientHandle.instance.ClearData();
        WindowOperate.instance.WindowOUT("Image BasicSettings");
        WindowOperate.instance.WindowOUT("Image NewCharacter");
    }
}
