using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;
using System;

public class AutoLobby : MonoBehaviour
{

  void Update()
  {
    if (GameObject.FindObjectOfType<LobbyManager>() != null)
      {
        Destroy(this.gameObject);
        return;
      }
          
    SceneManager.LoadScene("Lobby réseau", LoadSceneMode.Single);
  }

}
