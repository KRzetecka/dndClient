using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRoomList : MonoBehaviour
{
    public void getList()
    {
        ClientSend.instance.GetRoomsList();       
    }
}
