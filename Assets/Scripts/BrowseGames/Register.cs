using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Register : MonoBehaviour
{
    string LoginName, LoginPassw;
    public void NewAcc()
    {
        try
        {
            LoginName = GameObject.Find("NewAccountNickInputText").GetComponent<Text>().text;
            LoginPassw = GameObject.Find("NewAccountPasswInputText").GetComponent<Text>().text;
        }
        catch
        {
            GameObject.Find("ErrorTextRegister").GetComponent<Text>().text = "How about giving me some data?";
            return;
        }
        if (LoginName == null || LoginPassw == null || LoginPassw == "" || LoginName == "")
        {
            GameObject.Find("ErrorTextRegister").GetComponent<Text>().text = "How about giving me some data?";
            return;
        }
        LoginName = GameObject.Find("NewAccountNickInputText").GetComponent<Text>().text;
        LoginPassw = GameObject.Find("NewAccountPasswInputText").GetComponent<Text>().text;

        if(LoginName.Length > 20 || LoginName.Length < 3)
        {
            GameObject.Find("ErrorTextRegister").GetComponent<Text>().text = "Nick is too long or too short!";
            return;
        }
        if(LoginPassw.Length > 20 || LoginPassw.Length < 5)
        {
            GameObject.Find("ErrorTextRegister").GetComponent<Text>().text = "Password is too long or too short!";
            return;
        }

        ClientSend.instance.registerUser(LoginName, LoginPassw);
    }





}
