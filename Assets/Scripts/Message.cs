using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public static Message instance;

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

    public void message(string _msg)
    {
        GameObject.Find("Text Message").GetComponent<Text>().text = _msg;
        WindowOperate.instance.WindowIN("Image Message");
    }



}
