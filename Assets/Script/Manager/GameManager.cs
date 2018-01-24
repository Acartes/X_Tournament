using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  // *************** //
  // ** Variables ** //
  // *************** //

  [Tooltip("Joueur qui joue (Red, Blue, Neutral)")]
  public Player currentPlayer;
  [Tooltip("Joueur qui joue (Red=0, Blue=1, Neutral=2)")]
  public int indexPlayer;
  [Tooltip("Phase actuel (Placement, Deplacement)")]
  public Phase currentPhase;
  [Tooltip("Quel est l'action qu'entreprend le joueur (isMoving, isReplacingBall, isShoting, isIdle)")]
  public PersoAction actualAction;

  public static GameManager Instance;

  // *************** //
  // ** Initialisation ** //
  // *************** //

	void Awake () {
		Instance = this;
      actualAction = PersoAction.isIdle;
	}

  void OnEnable()
    {
      StartCoroutine (LateOnEnable());
    }

    IEnumerator LateOnEnable() {
      yield return new WaitForEndOfFrame ();
      TurnManager.Instance.changeTurnEvent += OnChangeTurn;
    }

  void OnDisable()
    {
      TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
    }

  // *************** //
  // ** Events ** //
  // *************** //

  void OnChangeTurn(object sender, PlayerArgs e)
    { // Un joueur a terminé son tour
      currentPhase = e.currentPhase;
      currentPlayer = e.currentPlayer;

      GameManager.Instance.actualAction = PersoAction.isIdle;
    }

  // *************** //
  // ** Actions ** //
  // *************** //

	public void PauseGame () 
    { // Le jeu est en pause.
		TurnManager.Instance.enabled = false;
	}

	public IEnumerator NewManche () 
    { // Le tour recommence à la phase de placement.
		TurnManager.Instance.enabled = true;

      RosterManager.Instance.listHeroJXToPlace[0] = RosterManager.Instance.listHeroJ1;
      RosterManager.Instance.listHeroJXToPlace[1] = RosterManager.Instance.listHeroJ2;
		foreach (GameObject obj in RosterManager.Instance.listHeroJ1) {
			obj.transform.position = new Vector3 (999, 999, 999);
		}
		foreach (GameObject obj in RosterManager.Instance.listHeroJ2) {
			obj.transform.position = new Vector3 (999, 999, 999);
		}

		GameObject.Find ("Ballon").transform.position = GameObject.Find ("10 5").transform.position;

		SelectionManager.Instance.selectedCase = null;
		SelectionManager.Instance.selectedPersonnage = null;
      SelectionManager.Instance.selectedLastCase = null;
      SelectionManager.Instance.selectedLastPersonnage = null;
		SelectionManager.Instance.selectedPersonnage= null;
		yield return new WaitForEndOfFrame();
		TurnManager.Instance.TurnNumber = 0;
		TurnManager.Instance.ChangePhase(0);
    }
}
