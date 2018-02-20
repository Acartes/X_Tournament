using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class ClickEvent : NetworkBehaviour
{

	public static System.Action newClickEvent;

    private void OnMouseDown()
    {
        if (RpcFunctions.Instance.localId == 0 && TurnManager.Instance.currentPlayer == Player.Blue)
            return;
        if (RpcFunctions.Instance.localId == 1 && TurnManager.Instance.currentPlayer == Player.Red)
            return;

        if (HoverManager.Instance.hoveredCase != null) {
            newClickEvent();
        }
    }
}
