using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PortraitInteractive : NetworkBehaviour
{

  public PersoData newHoveredPersonnage;
    Text po;
    Text pm;
    Text pr;

    public override void OnStartClient()
    {
        StartCoroutine(waitForInit());
    }

    IEnumerator waitForInit()
    {
        while (!LoadingManager.Instance.isGameReady())
        {
            yield return new WaitForEndOfFrame();
        }
        Init();
    }

    private void Init()
    {
        foreach(Text obj in GetComponentsInChildren<Text>())
        {
            if (obj.name == "Pr")
                pr = obj;
            if (obj.name == "Pm")
                pm = obj;
            if (obj.name == "Po")
                po = obj;
        }
    }

    private void Update()
    {
        if (newHoveredPersonnage != null)
        {
            pr.text = newHoveredPersonnage.actualPointResistance.ToString();
            pm.text = newHoveredPersonnage.actualPointMovement.ToString();
            po.text = newHoveredPersonnage.shotStrenght.ToString();
        }
    }

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

    UIManager.Instance.UIIsHovered = true;

    if (newHoveredPersonnage == null)
      return;

    if (newHoveredPersonnage.timeStunned > 0)
      return;

    string hoveredCase = newHoveredPersonnage.persoCase != null ? newHoveredPersonnage.persoCase.name : "null";
    string hoveredPersonnage = newHoveredPersonnage.name;
    string hoveredBallon = "null";

    RpcFunctions.Instance.CmdSendHoverEvent(hoveredCase, hoveredPersonnage, hoveredBallon);
  }

  public void UnHoverPerso() // exit comme chez HoverEvent
  {
    UIManager.Instance.UIIsHovered = false;

    if (!enabled || !LoadingManager.Instance.isGameReady())
      {
        UIManager.Instance.UIIsHovered = false;
        RpcFunctions.Instance.CmdSendHoverEvent("null", "null", "null");
      }
  }

  public void ClickPerso()
  {
    if (GameManager.Instance.currentPlayer == newHoveredPersonnage.owner)
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
