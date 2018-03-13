using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class RpcFunctions : NetworkBehaviour
{



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
  public void CmdChangeTurn()
  {
    TurnManager.Instance.RpcChangeTurn();
  }

  [Command]
  public void CmdFirstTurn()
  {
    TurnManager.Instance.RpcFirstTurn();
  }
  /*    [Command]
    public void CmdPlacePerso(string hoveredCase, float offsetY, string selectedPersonnage)
    {
        PlacementBehaviour.Instance.RpcCreatePersoPlacement(hoveredCase, offsetY, selectedPersonnage);
    }*/
  /* [Command]
  public void CmdDeplacement(float offsetY, string selectedPersonnage)
    {
    if (localId == 0 && isLocalPlayer)
      {
        MoveBehaviour.Instance.RpcDeplacement(offsetY, selectedPersonnage);
      }
    }*/

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
  public void CmdHoverEvent(string hoveredCase, string hoveredPersonnage, string hoveredBallon)
  {
    EventManager.Instance.RpcHoverEvent(hoveredCase, hoveredPersonnage, hoveredBallon);
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
