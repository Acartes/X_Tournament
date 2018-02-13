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
      if (hoveredLastCase == null || hoveredLastCase != hoveredCase)
      {
        hoveredLastCase = hoveredCase;
      }

      if (hoveredCase != null)
        {
          changeColorExit(GameManager.Instance.currentPhase);
        }
    Debug.Log(hoveredCase);
      Debug.Log(hoveredLastCase);

    hoveredPersonnage = e.hoveredPersonnage;
    hoveredCase = e.hoveredCase;
    hoveredPathfinding = e.Pathfinding;
    hoveredBallon = e.hoveredBallon;

      changeColorEnter();
    CheckIfAction ();
        }

    // ************* //
    // ** Actions ** //
    // ************* //

  void changeColorEnter()
    { // Change la couleur de la case qui est sur le curseur

      Player currentPlayer = GameManager.Instance.currentPlayer;
      Phase currentPhase = GameManager.Instance.currentPhase;
      Color actionColor = ColorManager.Instance.actionColor;
      Color moveColor = ColorManager.Instance.moveColor;
      GameObject selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
    Color hoverColor = ColorManager.Instance.hoverColor;
    PersoAction actualAction = GameManager.Instance.actualAction;

            switch (currentPhase)
            {
      case (Phase.Placement):
            hoveredCase.GetComponent<CaseData>().ChangeColor(Statut.isHovered);
                    break;
          case (Phase.Deplacement):
            if (SelectionManager.Instance.selectedPersonnage != null)
              {
                MoveBehaviour.Instance.HidePath();
            if (hoveredPersonnage != null
                    && hoveredPersonnage.GetComponent<PersoData>().owner != currentPlayer
                    && Fonction.Instance.CheckAdjacent(selectedPersonnage, hoveredPersonnage) == true)
              {
          //          hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
              }

                if (hoveredPersonnage != null
                  && hoveredPersonnage.GetComponent<PersoData>().owner == currentPlayer)
                  {
               //     hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
                  }

                if (hoveredBallon != null
                  && Fonction.Instance.CheckAdjacent(selectedPersonnage, hoveredBallon) == true)
                  {
                 //   hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
                  }

                if (hoveredCase.GetComponent<CaseData>().casePathfinding == PathfindingCase.Walkable
                  && actualAction == PersoAction.isSelected)
                  {
                    MoveBehaviour.Instance.createPath();

                if (hoveredLastCase != hoveredCase)
                  {
                        Pathfinding.Instance.StartPathfinding();
                  }
                  }
              }
                    break;
            }
    }

  void changeColorExit(Phase currentPhase)
    { // Change la couleur de la case qui était sur le curseur
            switch (currentPhase)
            {
      case (Phase.Placement):
            hoveredCase.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.isHovered);
                    break;
			case (Phase.Deplacement):
            hoveredCase.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.isHovered);
                    break;
            }
    }

  void CheckIfAction ()
	{ // Action de replacer la balle qui se termine après avoir placer la balle 
      PersoAction actualAction = GameManager.Instance.actualAction;

      if (actualAction == PersoAction.isReplacingBall) 
      {
    List<GameObject> caseAction = ReplacerBalleBehaviour.Instance.caseAction;
    Color actionColor = ColorManager.Instance.actionColor;

		foreach (GameObject obj in caseAction) {
			if (obj == hoveredCase) {
          //    hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
			}
		}
	}
  }
}