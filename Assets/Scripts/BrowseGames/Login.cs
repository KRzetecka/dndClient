using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    public void LogIn()
    {
        //Login
        
        GameObject.Find("ButtonLogin").GetComponent<Button>().interactable = false;

        string name = GameObject.Find("AccountNickInputText").GetComponent<Text>().text;
        string password = GameObject.Find("AccountPasswInputText").GetComponent<Text>().text;

        if (name != "" || name != null)
        {
            if (password != "" || password != null)
            {
                if (name.Length > 20 || name.Length < 3)
                {
                    GameObject.Find("LoginError").GetComponent<Text>().text = "Name too long or too short";
                    return;
                }
                if (password.Length > 20 || password.Length < 3)
                {
                    GameObject.Find("LoginError").GetComponent<Text>().text = "Password too long or too short";
                    return;
                }
                ClientSend.instance.loginUser(name, password);
            }
        }

       
    }
}
