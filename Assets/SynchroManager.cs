﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SynchroManager : NetworkBehaviour
{

    public bool validatedCommand = true;

    public static SynchroManager Instance;

    public override void OnStartClient()
    {
        Instance = this;
        Debug.Log(this.GetType() + " is Instanced");

    }

    public bool canPlayTurn()
    {
        if (!GameManager.Instance.isSoloGame)
        {
            if (RpcFunctions.Instance.localId == 0 && TurnManager.Instance.currentPlayer == Player.Blue)
                return false;
            if (RpcFunctions.Instance.localId == 1 && TurnManager.Instance.currentPlayer == Player.Red)
                return false;
        }
        return true;
    }

    public bool canSendCommand()
    {
        return validatedCommand;
    }

    public IEnumerator WaitForHoverEventValidation(string hoveredCase, string hoveredPersonnage, string hoveredBallon)
    {
        validatedCommand = false;
        if (validatedCommand == false)
        {
            Debug.Log("wait for validation of event");
            // si la fonction n'a pas été validé au bout de 0.X secondes, on relance
            yield return new WaitForSeconds(0.5f);
        }
        if (validatedCommand == false)
        {
            Debug.Log("event not validated, resend...");
            RpcFunctions.Instance.CmdSendHoverEvent(hoveredCase, hoveredPersonnage, hoveredBallon);
        }
        StopAllCoroutines();
    }

    [Command]
    public void CmdValidateHoverEvent(string hoveredCase, string hoveredPersonnage, string hoveredBallon)
    {
        SynchroManager.Instance.RpcValidateHoverEvent(hoveredCase, hoveredPersonnage, hoveredBallon);
    }

    public IEnumerator WaitForClickEventValidation()
    {
        validatedCommand = false;
        if (validatedCommand == false)
        {
            Debug.Log("wait for validation of event");
            // si la fonction n'a pas été validé au bout de 0.X secondes, on relance
            yield return new WaitForSeconds(0.5f);
        }
        if (validatedCommand == false)
        {
            Debug.Log("event not validated, resend...");
            SynchroManager.Instance.RpcValidateClickEvent();
        }
        StopAllCoroutines();
    }

    [ClientRpc]
    public void RpcReceiveHoverEvent(string hoveredCaseString, string hoveredPersonnageString, string hoveredBallonString)
    {
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }
        EventManager.Instance.HoverEvent(hoveredCaseString, hoveredPersonnageString, hoveredBallonString);

        SynchroManager.Instance.CmdValidateHoverEvent(hoveredCaseString, hoveredPersonnageString, hoveredBallonString);
    }

    [ClientRpc]
    public void RpcValidateHoverEvent(string hoveredCaseString, string hoveredPersonnageString, string hoveredBallonString)
    {
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }
        // le sender a bien reçu la validation que la fonction a été effectuée chez le receiver
        SynchroManager.Instance.validatedCommand = true;
        Debug.Log("event validated");

        EventManager.Instance.HoverEvent(hoveredCaseString, hoveredPersonnageString, hoveredBallonString);
    }

    [Command]
    public void CmdValidateClickEvent()
    {
        RpcValidateClickEvent();
    }

    [ClientRpc]
    public void RpcReceiveClickEvent()
    {
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }

        EventManager.newClickEvent();

        SynchroManager.Instance.CmdValidateClickEvent();
    }

    [ClientRpc]
    public void RpcValidateClickEvent()
    {
        if (!SynchroManager.Instance.canPlayTurn())
        {
            return;
        }
        // le sender a bien reçu la validation que la fonction a été effectuée chez le receiver
        SynchroManager.Instance.validatedCommand = true;
        EventManager.newClickEvent();
    }
}
