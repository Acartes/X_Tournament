using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {

	public int TurnNumber;

	public static TurnManager Instance;

	public EventHandler<PlayerArgs> changeTurnEvent;

	public Player currentPlayer = Player.Red;
	public Phase currentPhase = Phase.Placement;

  public GameObject finishTurnButton;

  bool canChangeTurn = true;

	void Awake () {
		if (Instance == null)
			Instance = this;
	}

	IEnumerator Start () {
      finishTurnButton = GameObject.Find ("finishTurn");
		yield return new WaitForEndOfFrame ();
		changeTurnEvent (this, new PlayerArgs (currentPlayer, currentPhase));
	}

    public void ChangeTurn()
    {
        if (canChangeTurn)
        {
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
    }

    public void ChangePhase (int number) {
		switch (number) {
		case 0:
			currentPhase = Phase.Placement;
			break;
		case 1:
			currentPhase = Phase.Deplacement;
			break;
		}
		SendValue (currentPlayer, currentPhase);
	}

	void SendValue (Player player, Phase phase) {
		if (changeTurnEvent != null) {
			changeTurnEvent (this, new PlayerArgs (currentPlayer, currentPhase));
		}
	}

   public void DisableFinishTurn () {
    if (finishTurnButton.GetComponent<Button>().interactable != false)
      {
        finishTurnButton.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
        finishTurnButton.GetComponent<Button>().interactable = false;
        canChangeTurn = false;
      }
    }

    public IEnumerator EnableFinishTurn () {
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

