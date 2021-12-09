using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public static Player instance;

        public int playerID;
        public string playerNickname;
        public bool isLogged;

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

        Player()
        {
            playerNickname = "Guest";
            isLogged = false;
            playerID = -1;
        }
        public void PlayerLogin(int _id, string _nickname)
        {
            playerNickname = _nickname;
            playerID = _id;
            isLogged = true;
        }
        public void PlayerLogout()
        {
            playerNickname = "Guest";
            playerID = -1;
            isLogged = false;
        }


    }
}
