﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MenuContextuel : NetworkBehaviour
{

  public static MenuContextuel Instance;

  GameObject ballon;


  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log(this.GetType() + " is Instanced");
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    while (!LoadingManager.Instance.isGameReady())
      yield return new WaitForSeconds(0.2f);
    Init();
  }

  private void Init()
  {
    ballon = GameObject.Find("Ballon");
    UIManager.Instance.menuContextuel = this.gameObject;
    TurnManager.Instance.changeTurnEvent += OnChangeTurn;
    this.gameObject.transform.position = new Vector3(999, 999, 999);
  }

  void OnEnable()
  {
        /* 
        if (LoadingManager.Instance.isGameReady()) 
          { 
            ballon = GameObject.Find("Ballon"); 
            Debug.Log(ballon.transform.position); 
            transform.position = ballon.transform.position; 
            CaseManager.Instance.DisableAllColliders(); 
 
          }*/
        if (LoadingManager.Instance != null && LoadingManager.Instance.isGameReady())
        {
            CaseManager.Instance.DisableAllColliders();
        }
}

  void OnDisable()
  {
    if (LoadingManager.Instance != null && LoadingManager.Instance.isGameReady())
      {
        CaseManager.Instance.EnableAllColliders();
      }
  }

  void OnChangeTurn(object sender, PlayerArgs e)
  { // Lorsqu'un joueur termine son tour

    if (MenuContextuel.Instance != null)
      {
        MenuContextuel.Instance.gameObject.transform.position = new Vector3(999, 999, 999);
      }
  }

}
