using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Policy;
using UnityEngine.Networking;
using System;

public class EventManager : NetworkBehaviour {

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
  public void RpcHoverEvent (string hoveredCaseString, string hoveredPersonnageString, string hoveredBallonString) 
    {
      PersoData hoveredPersonnage = null;
  BallonData hoveredBallon = null;

    CaseData hoveredCase = GameObject.Find(hoveredCaseString).GetComponent<CaseData>();
      if (hoveredPersonnageString != "null")
      hoveredPersonnage = GameObject.Find(hoveredPersonnageString).GetComponent<PersoData>();

      if (hoveredBallonString != "null")
      hoveredBallon = GameObject.Find(hoveredBallonString).GetComponent<BallonData>();

      newHoverEvent (this, new HoverArgs (hoveredCase, hoveredPersonnage, hoveredBallon));
    }

    [ClientRpc]
    public void RpcClickEvent ()
    {
      newClickEvent();
    }

  [ClientRpc]
  public void RpcMenuContextuelClick (string buttonName)
    {
        switch (buttonName) {
        case ("MenuContextuelReplacer"):
              if (SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData> ().actualPointMovement < 1) {
                return;

                MenuContextuel.Instance.gameObject.SetActive (false);
            }
            SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData> ().actualPointMovement--;
            ReplacerBalleBehaviour.Instance.ReplacerBalle ();
            MenuContextuel.Instance.gameObject.SetActive (false);

        break;
        case ("MenuContextuelTirer"):
            SelectionManager.Instance.selectedBallon.GetComponent<BallonData> ().StartCoroutine("Move");
            TurnManager.Instance.StartCoroutine("EnableFinishTurn");
            MenuContextuel.Instance.gameObject.SetActive (false);
        break;
        case ("MenuContextuelNothing"):
            TurnManager.Instance.StartCoroutine("EnableFinishTurn");
            MenuContextuel.Instance.gameObject.SetActive (false);
        break;
      case ("MenuContextuelRetourner"):
            ReplacerBalleBehaviour.Instance.ReplacerBalle ();
            MenuContextuel.Instance.gameObject.SetActive (false);
       // TurnManager.Instance.StartCoroutine("EnableFinishTurn");
        break;
    }
    }
}
