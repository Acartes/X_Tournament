using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class TurnManager : NetworkBehaviour
{
    public int TurnNumber;

    public static TurnManager Instance;

    public EventHandler<PlayerArgs> changeTurnEvent;

    public Player currentPlayer = Player.Red;
    public Phase currentPhase = Phase.Placement;

    public GameObject finishTurnButton;

    bool canChangeTurn = true;

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log(this.GetType() + " is Instanced");
        StartCoroutine(waitForInit());
    }

    IEnumerator waitForInit()
    {
        while(!LoadingManager.Instance.isGameReady())
            yield return new WaitForEndOfFrame();
        StartCoroutine(InitGame());
    }

    IEnumerator InitGame()
    {
        yield return new WaitForSeconds(0.3f);
        if(isServer)
            RpcFunctions.Instance.CmdFirstTurn();
        finishTurnButton = GameObject.Find("finishTurn");
    }

    public void CmdChangeTurn()
    {
        if (!canChangeTurn)
        {
            return;
        }
        RpcFunctions.Instance.CmdChangeTurn();
    }

    [ClientRpc]
    public void RpcFirstTurn()
    {
        changeTurnEvent(this, new PlayerArgs(currentPlayer, currentPhase));
    }

    [ClientRpc]
    public void RpcChangeTurn()
    {
        Debug.Log("rpc");
        Debug.Log("Sending command on all clients");

        switch (currentPlayer)
        {
            case Player.Red:
                currentPlayer = Player.Blue;
                break;
            case Player.Blue:
                currentPlayer = Player.Red;
                break;
        }
        TurnNumber++;
        SendValue(currentPlayer, currentPhase);

        if (TurnNumber == 2)
            ChangePhase(1);

    }

    public void ChangePhase(int number)
    {
        switch (number)
        {
            case 0:
                currentPhase = Phase.Placement;
                break;
            case 1:
                currentPhase = Phase.Deplacement;
                break;
        }
        SendValue(currentPlayer, currentPhase);
    }

    void SendValue(Player player, Phase phase)
    {
        if (changeTurnEvent != null)
        {
            changeTurnEvent(this, new PlayerArgs(currentPlayer, currentPhase));
        }
    }

    public void DisableFinishTurn()
    {
        if (finishTurnButton.GetComponent<Button>().interactable != false)
        {
            finishTurnButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
            finishTurnButton.GetComponent<Button>().interactable = false;
            canChangeTurn = false;
        }
    }

    public IEnumerator EnableFinishTurn()
    {
        if (finishTurnButton.GetComponent<Button>().interactable != true)
        {
            finishTurnButton.GetComponent<Button>().enabled = false;
            yield return new WaitForSeconds(0.01f);
            finishTurnButton.GetComponent<Button>().enabled = true;
            finishTurnButton.GetComponent<Image>().color = new Color(0.8f, 0.7f, 0f);
            finishTurnButton.GetComponent<Button>().interactable = true;
            canChangeTurn = true;
        }

    }

}

