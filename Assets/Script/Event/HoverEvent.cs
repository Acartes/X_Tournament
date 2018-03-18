using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class HoverEvent : NetworkBehaviour
{

  void Awake()
  {
    GetComponent<PolygonCollider2D>().enabled = false;
  }

  public override void OnStartClient()
  {
    GetComponent<PolygonCollider2D>().enabled = false;
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    while (!LoadingManager.Instance.isGameReady())
      yield return new WaitForEndOfFrame();

    yield return new WaitForSeconds(0.01f);
    Init();
  }

  private void Init()
  {
    this.enabled = true;
    GetComponent<PolygonCollider2D>().enabled = true;
  }

  void OnMouseOver()
  {
    if (!GameManager.Instance.isSoloGame)
      {
        if ((RpcFunctions.Instance.localId == 0 && TurnManager.Instance.currentPlayer == Player.Blue)
          || (RpcFunctions.Instance.localId == 1 && TurnManager.Instance.currentPlayer == Player.Red))
          return;
      }

    if (!enabled || !LoadingManager.Instance.isGameReady())
      return;

    string hoveredCase = "null";
    string hoveredPersonnage = "null";
    string hoveredBallon = "null";

    hoveredCase = this.GetComponent<CaseData>().name;

    if (GetComponent<CaseData>().personnageData != null)
      hoveredPersonnage = GetComponent<CaseData>().personnageData.name;

    if (GetComponent<CaseData>().ballon != null)
      hoveredBallon = GetComponent<CaseData>().ballon.name;

    RpcFunctions.Instance.CmdSendHoverEvent(hoveredCase, hoveredPersonnage, hoveredBallon);
  }

  void OnMouseExit()
  {
    if (!enabled || !LoadingManager.Instance.isGameReady())
      {
        RpcFunctions.Instance.CmdSendHoverEvent("null", "null", "null");
      }
    return;
  }
}
