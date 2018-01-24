using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContextuel : MonoBehaviour {

	public static MenuContextuel Instance;

	GameObject ballon;
  bool gameInit = true;

	void Awake () {
		Instance = this;
		ballon = GameObject.Find ("Ballon");
	}

    void Start () {
      if (gameInit)
        {
          UIManager.Instance.menuContextuel = this.gameObject;
          this.gameObject.SetActive(false);
          gameInit = false;
        }
    }

	void OnEnable () {
      StartCoroutine (LateOnEnable());
		transform.position = ballon.transform.position;
		Active ();
	}

    IEnumerator LateOnEnable() {
      yield return new WaitForEndOfFrame ();
      TurnManager.Instance.changeTurnEvent += OnChangeTurn;
    }

	void OnDisable () {
		Desactive ();
	}

  void OnChangeTurn(object sender, PlayerArgs e)
  { // Lorsqu'un joueur termine son tour

    SelectionManager.Instance.Deselect(TurnManager.Instance.currentPhase, TurnManager.Instance.currentPlayer);

    if (MenuContextuel.Instance != null)
      {
        MenuContextuel.Instance.gameObject.SetActive(false);
      }
  } 

	public void Active () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("case")) {
			obj.GetComponent<PolygonCollider2D> ().enabled = false;
		}
		//Pathfinding.Instance.
	}

	public void Desactive () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("case")) {
			obj.GetComponent<PolygonCollider2D> ().enabled = true;
		}
	}
}
