using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshGameList : MonoBehaviour
{
    public Button buttonPrefab;
    public GameObject PanelList;
    GameObject tmp;
    public List<string> GameList;
    int refreshDelay = 0;


    private void Update()
    {
        if(refreshDelay == 20)
        {
            float x = GameObject.Find("ListContent").GetComponent<RectTransform>().rect.width - 10;
            float y = 30;
            GameObject.Find("ListContent").GetComponent<GridLayoutGroup>().cellSize = new Vector2(x, y);
            refreshDelay = 0;
        }
        refreshDelay++;
    }

    public void AddRooms(List<string> names)
    { 
        foreach (var room in GameList)
        {
            tmp = GameObject.Find(room);
            Destroy(tmp.gameObject);
        }
        GameList.Clear();
        int i = 0;
        float startPt = buttonPrefab.GetComponent<RectTransform>().localPosition.y;
        foreach (string name in names)
        {
            string tmp = name;
            Button newButton = Instantiate(buttonPrefab);

            newButton.transform.gameObject.SetActive(true);
            newButton.GetComponentInChildren<Text>().text = tmp;
            newButton.GetComponentInChildren<Text>().name = "Text (" + tmp + ")";
            newButton.name = "Button (" + tmp + ")";
            newButton.transform.SetParent(PanelList.transform, false);

            newButton.onClick.AddListener(() => {
                generatedButtonOnClick(tmp);
            });

            //float height = buttonPrefab.GetComponent<RectTransform>().rect.height, width = buttonPrefab.GetComponent<RectTransform>().rect.width;
            //Set_Size2(buttonPrefab, newButton);
            //newButton.transform.localPosition = new Vector2(0, startPt - i * height);

            GameList.Add(newButton.name);
            i++;
        }
        Debug.Log("Refreshed");
    }

    public void generatedButtonOnClick(string name)
    {
        ClientSend.instance.GetRoomDesc(name);
        GameInit.instance.SelectedGame = name;
        if (GameObject.FindGameObjectWithTag("GetInButton").GetComponent<Button>().IsInteractable() == false)
            GameObject.FindGameObjectWithTag("GetInButton").GetComponent<Button>().interactable = true;
    }

    private static void Set_Size(Button gameObject, float width, float height)
    {
        if (gameObject != null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(width, height);
            }
        }
    }
    private static void Set_Size2(Button prefab, Button dest)
    {
        dest.GetComponent<RectTransform>().sizeDelta = prefab.GetComponent<RectTransform>().sizeDelta;
    }

}
