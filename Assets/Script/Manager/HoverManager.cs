using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;


public class HoverManager : NetworkBehaviour
{

    // *************** //
    // ** Variables ** //
    // *************** //

  [Header("  Hover Data")]
  [Tooltip("Case sous le curseur")]
  [ReadOnly] public CaseData hoveredCase;
  [Tooltip("Personnage sous le curseur")]
  [ReadOnly] public PersoData hoveredPersonnage;
  [Tooltip("Pathfinding tracé par le curseur")]
  [ReadOnly] public PathfindingCase hoveredPathfinding;
  [Tooltip("Ballon sous le curseur")]
  [ReadOnly] public BallonData hoveredBallon;
  [Tooltip("Case qui était précedemment sous le curseur")]
  [ReadOnly] public CaseData hoveredLastCase;

  [HideInInspector] public static HoverManager Instance;

    public EventHandler<HoverArgs> newHoverEvent;

    // *********** //
    // ** Initialisation ** //
    // *********** //

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("HoverManager is Instanced");
        StartCoroutine(waitForInit());
    }

    IEnumerator waitForInit()
    {
        while (!LoadingManager.Instance.IsInstancesLoaded())
            yield return new WaitForEndOfFrame();
        Init();
    }

    private void Init()
    {
        StartCoroutine(LateOnEnable());
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
      PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
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
                    && Fonction.Instance.CheckAdjacent(selectedPersonnage.gameObject, hoveredPersonnage.gameObject) == true)
              {
          //          hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
              }

                if (hoveredPersonnage != null
                  && hoveredPersonnage.GetComponent<PersoData>().owner == currentPlayer)
                  {
               //     hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
                  }

                if (hoveredBallon != null
                  && Fonction.Instance.CheckAdjacent(selectedPersonnage.gameObject, hoveredBallon.gameObject) == true)
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
    List<CaseData> caseAction = ReplacerBalleBehaviour.Instance.caseAction;
    Color actionColor = ColorManager.Instance.actionColor;

		foreach (CaseData obj in caseAction) {
			if (obj == hoveredCase) {
          //    hoveredCase.GetComponent<CaseData>().ChangeColor(actionColor);
			}
		}
	}
  }
}