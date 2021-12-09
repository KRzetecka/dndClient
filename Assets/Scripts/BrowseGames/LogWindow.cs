using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogWindow : MonoBehaviour
{
    public void window()
    {
        if (Player.instance.isLogged == false)
        {
            WindowOperate.instance.WindowIN("Image LoginWindow");

        }
        else
        {
            Player.instance.PlayerLogout();
            ClientSend.instance.logoutUser();
            Color c = new Color(239, 91, 64);
            GameObject.Find("LoggedOutText").GetComponent<Text>().color = c;
            GameObject.Find("LoggedOutText").GetComponent<Text>().text = "Logged out!";
            GameObject.Find("LoggedAsPlaceholder").GetComponent<Text>().text = "Logged as: Guest";
        }
    }
}
