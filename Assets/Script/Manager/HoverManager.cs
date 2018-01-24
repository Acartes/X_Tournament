using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HoverManager : MonoBehaviour
{

    // *************** //
    // ** Variables ** //
    // *************** //

  [Header("  Hover Data")]
  [Tooltip("Case sous le curseur")]
  [ReadOnly] public GameObject hoveredCase;
  [Tooltip("Personnage sous le curseur")]
  [ReadOnly] public GameObject hoveredPersonnage;
  [Tooltip("Pathfinding tracé par le curseur")]
  [ReadOnly] public PathfindingCase hoveredPathfinding;
  [Tooltip("Ballon sous le curseur")]
  [ReadOnly] public GameObject hoveredBallon;
  [Tooltip("Case qui était précedemment sous le curseur")]
  [ReadOnly] public GameObject hoveredLastCase;

  [HideInInspector] public static HoverManager Instance;

    public EventHandler<HoverArgs> newHoverEvent;

    // *********** //
    // ** Initialisation ** //
    // *********** //

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
		StartCoroutine (LateOnEnable());
    }

	IEnumerator LateOnEnable() {
		yield return new WaitForEndOfFrame ();
        HoverEvent.newHoverEvent += OnNewHover;
	}

    void OnDisable()
    {
        HoverEvent.newHoverEvent -= OnNewHover;
    }

    // ************ //
    // ** Events ** //
    // ************ //

    void OnNewHover(object sender, HoverArgs e)
  { // Curseur se trouve sur une case ou quitte une case
    if (hoveredCase != null)
      {
        hoveredLastCase = hoveredCase;
        changeColorExit(GameManager.Instance.currentPhase);
      }

    hoveredPersonnage = e.hoveredPersonnage;
    hoveredCase = e.hoveredCase;
    hoveredPathfinding = e.Pathfinding;
    hoveredBallon = e.hoveredBallon;

      changeColorEnter(GameManager.Instance.currentPlayer, GameManager.Instance.currentPhase, ColorManager.Instance.actionColor, ColorManager.Instance.travelColor, SelectionManager.Instance.selectedPersonnage);

      OnNewHoverAction(GameManager.Instance.actualAction, SelectionManager.Instance.selectedPersonnage);
  }

  // ************ //
  // ** Checkers ** //
  // ************ //

    void OnNewHoverAction (PersoAction actualAction, GameObject selectedPersonnage) 
    { // Vérifie si certaines actions sont possibles lorsqu'il y a un nouveau hover sur une case
          if (selectedPersonnage != null && hoveredCase != null && hoveredCase.GetComponent<CaseData>().casePathfinding == PathfindingCase.Walkable && actualAction == PersoAction.isSelected)
            {
              Pathfinding.Instance.StartPathfinding(GrilleManager.Instance.getMap(), selectedPersonnage.transform, hoveredCase.transform);
            }

        if (actualAction == PersoAction.isReplacingBall) {
          CheckIfAction (ColorManager.Instance.actionColor);
        }
  }

    // ************* //
    // ** Actions ** //
    // ************* //

  void changeColorEnter(Player currentPlayer, Phase currentPhase, Color actionColor, Color travelColor, GameObject selectedPersonnage)
    { // Change la couleur de la case qui est sur le curseur
      
            switch (currentPhase)
            {
      case (Phase.Placement):
            hoveredCase.GetComponent<CaseData>().ChangeColor(travelColor);
                    break;
          case (Phase.Deplacement):
            if (SelectionManager.Instance.selectedPersonnage != null && hoveredCase.GetComponent<CaseData>().casePathfinding == PathfindingCase.NonWalkable)
              {
                MoveBehaviour.Instance.HidePath();
                if (hoveredPersonnage != null && hoveredPersonnage.GetComponent<PersoData>().owner != currentPlayer)
                  {
                    if (Fonction.Instance.CheckAdjacent(selectedPersonnage, hoveredPersonnage) == true)
                  {
                        hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
                  }
                  }
                if (hoveredPersonnage != null && hoveredPersonnage.GetComponent<PersoData>().owner == currentPlayer)
                  {
                    hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
                  }
                if (hoveredBallon != null)
                  {
                    if (Fonction.Instance.CheckAdjacent(selectedPersonnage, hoveredBallon) == true)
                  {
                        hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
                  }
                  }
              }
                else if (SelectionManager.Instance.selectedPersonnage != null
              && hoveredCase.GetComponent<CaseData>().casePathfinding == PathfindingCase.Walkable
              && GameManager.Instance.actualAction == PersoAction.isSelected)
                    {
                    MoveBehaviour.Instance.createPath();
                    }

                    break;
            }
    }

  void changeColorExit(Phase currentPhase)
    { // Change la couleur de la case qui était sur le curseur
            switch (currentPhase)
            {
      case (Phase.Placement):
            hoveredCase.GetComponent<CaseData>().ChangeColor(hoveredCase.GetComponent<CaseData>().initColor);
                    break;
			case (Phase.Deplacement):
            hoveredCase.GetComponent<CaseData>().ChangeColor(hoveredCase.GetComponent<CaseData> ().caseColor);
                    break;
            }
    }

  void CheckIfAction (Color actionColor)
	{ // Action de replacer la balle qui se termine après avoir placer la balle 
		foreach (GameObject obj in ReplacerBalleBehaviour.Instance.caseAction) {
			if (obj == hoveredCase) {
              hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
			}
		}
	}
}