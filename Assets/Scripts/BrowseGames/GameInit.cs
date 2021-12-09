using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameInit : MonoBehaviour
{
    public static GameInit instance;
    public string SelectedGame;
    public bool isProtected;

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
    public void OnTheClick()
    {
        if(SelectedGame != null || SelectedGame != "")
        {
            GameObject.Find("LoggedOutText").GetComponent<Text>().text = "";
            if (Player.instance.playerNickname != "Guest" || Player.instance.isLogged == false)
            {
                if (isProtected == true)
                {
                    WindowOperate.instance.WindowIN("Image PasswordBackground");
                }
                else
                {
                    ClientSend.instance.isPasswordCorrect("", SelectedGame);
                }
            }
        }
        else
        {
            GameObject.Find("RoomDescPanelText").GetComponent<Text>().text = "Select the game first";
        }           
    }
    public void OnClick2()
    {
        ClientSend.instance.isPasswordCorrect(GameObject.FindGameObjectWithTag("Password").GetComponent<Text>().text, SelectedGame);
    }
}
