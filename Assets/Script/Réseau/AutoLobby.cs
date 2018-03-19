using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;
using System;

public class AutoLobby : MonoBehaviour
{

  void Start()
  {
    if (GameObject.FindObjectOfType<LobbyManager>() != null)
      {
        Destroy(this.gameObject);
        return;
      }
    StartCoroutine(ClearErrors());
    SceneManager.LoadScene("Lobby réseau", LoadSceneMode.Single);
  }

  IEnumerator ClearErrors()
  {
    yield return new WaitForSeconds(0.02f);
    Debug.ClearDeveloperConsole();  
  }

}
