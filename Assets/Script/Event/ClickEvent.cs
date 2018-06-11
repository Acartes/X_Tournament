using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityEngine.SceneManagement;
using System.Globalization;

public class ClickEvent : NetworkBehaviour
{

	void OnMouseDown()
	{
//		LobbyManager.Instance.StopHost();
//		LobbyManager.Instance.StopServer();
//		return;

		if (!SynchroManager.Instance.canPlayTurn())
		{
			return;
		}

		if (UIManager.Instance.UIIsHovered)
			return;

		if (HoverManager.Instance.hoveredCase != null)
		{
			RpcFunctions.Instance.CmdSendClickEvent();
		}
	}
}
