using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContextuelButton : MonoBehaviour {

	SpriteRenderer spriteR;

	public Color colorEnter;
	public Color colorExit;

	public int coutReplacement;

	void Start () {
		spriteR = GetComponent<SpriteRenderer> ();
	}

	void OnMouseExit () {
		if (spriteR != null)
		spriteR.color = colorExit;
	}

	void OnMouseOver () {
		if (spriteR != null)
		spriteR.color = colorEnter;
	}

	void OnMouseDown () {
		switch (name) {
		case ("MenuContextuelReplacer"):
              if (SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData> ().actualPointMovement < coutReplacement) {
				return;
			}
			SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData> ().actualPointMovement--;
            ReplacerBalleBehaviour.Instance.ReplacerBalle ();

		break;
		case ("MenuContextuelTirer"):
            ShotBehaviour.Instance.TirDeplaceBalle();
            TurnManager.Instance.StartCoroutine("EnableFinishTurn");
		break;
		case ("MenuContextuelNothing"):
            TurnManager.Instance.StartCoroutine("EnableFinishTurn");
		break;
	}
		if (spriteR != null)
		spriteR.color = colorExit;
		
		MenuContextuel.Instance.gameObject.SetActive (false);
	}
}
