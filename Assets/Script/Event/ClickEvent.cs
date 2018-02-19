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
		if (HoverManager.Instance.hoveredCase != null) {
            if (LobbyManager.Instance.playerId == 0 && GameManager.Instance.currentPlayer == Player.Blue)
                return;
            if (LobbyManager.Instance.playerId == 1 && GameManager.Instance.currentPlayer == Player.Red)
                return;
            newClickEvent();
        }
    }
}
