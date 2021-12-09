using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WindowOperate : MonoBehaviour
{
    public static WindowOperate instance;
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
    public void WindowIN(string w)
    {
        try
        {    
        GameObject Window = GameObject.Find(w);
        RectTransform MainRect = GameObject.Find("Image SelectBackground").GetComponent<RectTransform>();
        Vector2 newPos = Vector2.zero;

        newPos.x = Window.GetComponent<RectTransform>().rect.width / 2;
        newPos.x -= MainRect.GetComponent<RectTransform>().rect.width / 5;

        newPos.y = Window.GetComponent<RectTransform>().rect.height / 2;
        newPos.y -= MainRect.GetComponent<RectTransform>().rect.height / 5;

        //        Window.transform.localPosition = newPos;
        Window.transform.localPosition = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void WindowOUT(string w)
    {
        GameObject OffScrPoint = GameObject.Find("ButtonOffWorld");
        GameObject Window = GameObject.Find(w);
        Vector2 newPos = Vector2.zero;

        newPos.x = OffScrPoint.GetComponent<RectTransform>().localPosition.x;
        newPos.y = OffScrPoint.GetComponent<RectTransform>().localPosition.y;
        Window.transform.localPosition = newPos;
    }
}
