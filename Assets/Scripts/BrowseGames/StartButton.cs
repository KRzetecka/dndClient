using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public void Hide()
    {
        GameObject.FindGameObjectWithTag("StartCanvas").SetActive(false);
    }
}
