using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PortraitInteractive : NetworkBehaviour
{

    public PersoData newHoveredPersonnage;

    public void setPortraitImage(Sprite newSprite, PersoData newPerso)
    {
        GetComponent<Image>().sprite = newSprite;
        newHoveredPersonnage = newPerso;
    }

    public void HoverPerso() // hover comme chez HoverEvent
    {
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }
        if (!enabled || !LoadingManager.Instance.isGameReady())
            return;

        string hoveredCase = newHoveredPersonnage.persoCase.name;
        string hoveredPersonnage = newHoveredPersonnage.name;
        string hoveredBallon = "null";

        RpcFunctions.Instance.CmdSendHoverEvent(hoveredCase, hoveredPersonnage, hoveredBallon);
    }

    public void UnHoverPerso() // exit comme chez HoverEvent
    {
        if (!enabled || !LoadingManager.Instance.isGameReady())
        {
            RpcFunctions.Instance.CmdSendHoverEvent("null", "null", "null");
        }
    }

    public void ClickPerso()
    {
        RpcFunctions.Instance.CmdSendClickEvent();
    }
}
