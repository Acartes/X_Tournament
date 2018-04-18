using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PortraitInteractive : NetworkBehaviour
{

    public PersoData newHoveredPersonnage;

    public void setPortraitData(PortraitInteractive newPortrait)
    {
        GetComponent<Image>().sprite = newPortrait.GetComponent<Image>().sprite;
        GetComponent<Image>().color = newPortrait.GetComponent<Image>().color;
        newHoveredPersonnage = newPortrait.newHoveredPersonnage;
    }
    public void setPortraitData(Sprite newSprite, Color newColor, PersoData newPersoData)
    {
        GetComponent<Image>().sprite = newSprite;
        GetComponent<Image>().color = newColor;
        newHoveredPersonnage = newPersoData;
    }

    public void HoverPerso() // hover comme chez HoverEvent
    {
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }

        if (!enabled || !LoadingManager.Instance.isGameReady())
            return;

        string hoveredCase = newHoveredPersonnage.persoCase != null ? newHoveredPersonnage.persoCase.name : "null";
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
        if(GameManager.Instance.currentPlayer == newHoveredPersonnage.owner)
        RpcFunctions.Instance.CmdSendClickEvent();
    }

    public void GrayPortrait()
    {
        GetComponent<Image>().color = Color.grey;
    }

    public void UnGrayPortrait()
    {
        GetComponent<Image>().color = Color.white;
    }
}
