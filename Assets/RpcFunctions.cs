using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RpcFunctions : NetworkBehaviour {

    public static RpcFunctions Instance;

    public override void OnStartLocalPlayer()
    {
        Instance = this;
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
}
