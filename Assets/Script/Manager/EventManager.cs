using System.Collections;
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
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }
        HoverEvent(hoveredCaseString, hoveredPersonnageString, hoveredBallonString);

    SynchroManager.Instance.CmdValidateHoverEvent(hoveredCaseString, hoveredPersonnageString, hoveredBallonString);
  }

  [ClientRpc]
  public void RpcValidateHoverEvent(string hoveredCaseString, string hoveredPersonnageString, string hoveredBallonString)
  {
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }
        // le sender a bien reçu la validation que la fonction a été effectuée chez le receiver
        SynchroManager.Instance.validatedCommand = true;
    Debug.Log("event validated");

    HoverEvent(hoveredCaseString, hoveredPersonnageString, hoveredBallonString);
  }

  private void HoverEvent(string hoveredCaseString, string hoveredPersonnageString, string hoveredBallonString)
  {
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
  public void RpcReceiveClickEvent()
  {
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }

        newClickEvent();

    SynchroManager.Instance.CmdValidateClickEvent();
  }

  [ClientRpc]
  public void RpcValidateClickEvent()
  {
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }
        // le sender a bien reçu la validation que la fonction a été effectuée chez le receiver
        SynchroManager.Instance.validatedCommand = true;
    newClickEvent();
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
        MenuContextuel.Instance.gameObject.transform.position = new Vector3(999, 999, 999);

        break;
      case ("MenuContextuelTirer"):
        SelectionManager.Instance.selectedBallon.GetComponent<BallonData>().StartCoroutine("Move");
        TurnManager.Instance.StartCoroutine("EnableFinishTurn");
        MenuContextuel.Instance.gameObject.transform.position = new Vector3(999, 999, 999); 
        break;
      case ("MenuContextuelNothing"):
        TurnManager.Instance.StartCoroutine("EnableFinishTurn");
        MenuContextuel.Instance.gameObject.transform.position = new Vector3(999, 999, 999);
        break;
      }
  }

}
