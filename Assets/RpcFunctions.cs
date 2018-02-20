using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RpcFunctions : NetworkBehaviour {

    public static RpcFunctions Instance;

    public int localId;

    public override void OnStartLocalPlayer()
    {
        Instance = this;
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
    [Command]
    public void CmdPlacePerso(string hoveredCase, float offsetY, string selectedPersonnage)
    {
        PlacementBehaviour.Instance.RpcCreatePersoPlacement(hoveredCase, offsetY, selectedPersonnage);
    }
  [Command]
  public void CmdDeplacement(Color caseColor, float offsetY, GameObject selectedPersonnage)
    {
      MoveBehaviour.Instance.RpcDeplacement(caseColor, offsetY, selectedPersonnage);
    }
}
