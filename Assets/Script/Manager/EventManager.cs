﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Policy;
using UnityEngine.Networking;
using System;

public class EventManager : NetworkBehaviour
{

  public static EventManager Instance;

  public static EventHandler<HoverArgs> newHoverEvent;
  public static System.Action newClickEvent;

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;

    Debug.Log("EventManager is Instanced");
  }

    [ClientRpc]
    public void RpcReceiveHoverEvent(string hoveredCaseString, string hoveredPersonnageString, string hoveredBallonString)
    {
        if ((RpcFunctions.Instance.localId == 0 && TurnManager.Instance.currentPlayer == Player.Red)
            || (RpcFunctions.Instance.localId == 1 && TurnManager.Instance.currentPlayer == Player.Blue))
            return;

        CaseData hoveredCase = null;
        PersoData hoveredPersonnage = null;
        BallonData hoveredBallon = null;

        if (hoveredCaseString != "null")
            hoveredCase = GameObject.Find(hoveredCaseString).GetComponent<CaseData>();
        if (hoveredPersonnageString != "null")
            hoveredPersonnage = GameObject.Find(hoveredPersonnageString).GetComponent<PersoData>();
        if (hoveredBallonString != "null")
            hoveredBallon = GameObject.Find(hoveredBallonString).GetComponent<BallonData>();

        newHoverEvent(this, new HoverArgs(hoveredCase, hoveredPersonnage, hoveredBallon));

        RpcFunctions.Instance.CmdValidateHoverEvent(hoveredCaseString, hoveredPersonnageString, hoveredBallonString);
    }

    [ClientRpc]
    public void RpcValidateHoverEvent(string hoveredCaseString, string hoveredPersonnageString, string hoveredBallonString)
    {
        if ((RpcFunctions.Instance.localId == 0 && TurnManager.Instance.currentPlayer == Player.Blue)
            || (RpcFunctions.Instance.localId == 1 && TurnManager.Instance.currentPlayer == Player.Red))
            return;

        // le sender a bien reçu la validation que la fonction a été effectuée chez le receiver
        RpcFunctions.Instance.validatedCommand = true;
        Debug.Log("event validated");

        CaseData hoveredCase = null;
        PersoData hoveredPersonnage = null;
        BallonData hoveredBallon = null;

        if (hoveredCaseString != "null")
            hoveredCase = GameObject.Find(hoveredCaseString).GetComponent<CaseData>();
        if (hoveredPersonnageString != "null")
            hoveredPersonnage = GameObject.Find(hoveredPersonnageString).GetComponent<PersoData>();
        if (hoveredBallonString != "null")
            hoveredBallon = GameObject.Find(hoveredBallonString).GetComponent<BallonData>();

        newHoverEvent(this, new HoverArgs(hoveredCase, hoveredPersonnage, hoveredBallon));


    }

    [ClientRpc]
  public void RpcClickEvent()
  {
    newClickEvent();
    // issou(newClickEvent);
  }

  [ClientRpc]
  public void RpcMenuContextuelClick(string buttonName)
  {
    switch (buttonName)
      {
      case ("MenuContextuelReplacer"):
        if (SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>().actualPointMovement < 1)
          return;
            
        SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>().actualPointMovement--;
        ReplacerBalleBehaviour.Instance.ReplacerBalle();
        MenuContextuel.Instance.gameObject.SetActive(false);

        break;
      case ("MenuContextuelTirer"):
        SelectionManager.Instance.selectedBallon.GetComponent<BallonData>().StartCoroutine("Move");
        TurnManager.Instance.StartCoroutine("EnableFinishTurn");
        MenuContextuel.Instance.gameObject.SetActive(false);
        break;
      case ("MenuContextuelNothing"):
        TurnManager.Instance.StartCoroutine("EnableFinishTurn");
        MenuContextuel.Instance.gameObject.SetActive(false);
        break;
      }
  }
  /*
     
    void issou(Action newAction)
    {
        functionsToRun.Add(newAction);
        functionsToRun[0].Invoke();
    }
*/
}
