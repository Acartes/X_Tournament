using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class ClickEvent : NetworkBehaviour
{

    void OnMouseDown()
    {
        if (SynchroManager.Instance.canSendCommand())
        {
            return;
        }
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }

        if (HoverManager.Instance.hoveredCase != null)
        {
            RpcFunctions.Instance.CmdSendClickEvent();
        }
    }
}
