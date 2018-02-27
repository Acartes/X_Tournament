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

      if (RpcFunctions.Instance.localId == 0 && TurnManager.Instance.currentPlayer == Player.Blue)
            return;
        if (RpcFunctions.Instance.localId == 1 && TurnManager.Instance.currentPlayer == Player.Red)
            return;

      RpcFunctions.Instance.CmdMenuContextuelClick(name);


		if (spriteR != null)
		spriteR.color = colorExit;

	}
}
