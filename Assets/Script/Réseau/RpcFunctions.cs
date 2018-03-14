using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class RpcFunctions : NetworkBehaviour
{

  public bool validatedCommand = true;

  public static RpcFunctions Instance;

  public int localId;

  public override void OnStartLocalPlayer()
  {
    Instance = this;
    Debug.Log(this.GetType() + " is Instanced");
    if (isServer)
      localId = 0;
    else
      localId = 1;
  }

  [Command]
  public void CmdCastSpell(int IDSpell)
  {
    SpellManager.Instance.RpcSpellCast(IDSpell);
  }

  [Command]
  public void CmdChangeTurn()
  {
    TurnManager.Instance.RpcChangeTurn();
  }

  [Command]
  public void CmdFirstTurn()
  {
    TurnManager.Instance.RpcFirstTurn();
  }

  [Command]
  public void CmdIsAllGameReady()
  {
    LoadingManager.Instance.RpcIsAllGameReady();
  }

  [Command]
  public void CmdSpawnPlayers()
  {
    RosterManager.Instance.RpcSpawnPlayers();
  }

  [Command]
  public void CmdSendHoverEvent(string hoveredCase, string hoveredPersonnage, string hoveredBallon)
  {
    Debug.Log("sending event");
    EventManager.Instance.RpcReceiveHoverEvent(hoveredCase, hoveredPersonnage, hoveredBallon);
    // Au cas où il y a un nouveau hover, la coroutine se reset
    StopAllCoroutines();

    // On lance la coroutine a qui on a donne la fonction de validation
    StartCoroutine(WaitForHoverEventValidation(hoveredCase, hoveredPersonnage, hoveredBallon));
  }

  IEnumerator WaitForHoverEventValidation(string hoveredCase, string hoveredPersonnage, string hoveredBallon)
  {
    validatedCommand = false;
    if (validatedCommand == false)
      {
        Debug.Log("wait for validation of event");
        // si la fonction n'a pas été validé au bout de 0.X secondes, on relance
        yield return new WaitForSeconds(0.5f);
      }
    if (validatedCommand == false)
      {
        Debug.Log("event not validated, resend...");
        CmdSendHoverEvent(hoveredCase, hoveredPersonnage, hoveredBallon);
      }
    StopAllCoroutines();
  }

  [Command]
  public void CmdValidateHoverEvent(string hoveredCase, string hoveredPersonnage, string hoveredBallon)
  {
    EventManager.Instance.RpcValidateHoverEvent(hoveredCase, hoveredPersonnage, hoveredBallon);
  }

  [Command]
  public void CmdClickEvent()
  {
    EventManager.Instance.RpcClickEvent();
  }

  [Command]
  public void CmdMenuContextuelClick(string buttonName)
  {
    EventManager.Instance.RpcMenuContextuelClick(buttonName);
  }

}
