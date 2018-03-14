using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>Gère toutes les variables et fonctions lié au hover du curseur.</summary>
public class HoverManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  [Header("  Hover Data")]
  [Tooltip("Case sous le curseur")]
  [ReadOnly] public CaseData hoveredCase;
  [Tooltip("Personnage sous le curseur")]
  [ReadOnly] public PersoData hoveredPersonnage;
  [Tooltip("Ballon sous le curseur")]
  [ReadOnly] public BallonData hoveredBallon;
  [Tooltip("Case qui était précedemment sous le curseur")]
  [ReadOnly] public CaseData hoveredLastCase;

  [HideInInspector] public static HoverManager Instance;

  public EventHandler<HoverArgs> newHoverEvent;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log("HoverManager is Instanced");
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    while (!LoadingManager.Instance.isGameReady())
      yield return new WaitForEndOfFrame();
    Init();
  }

  private void Init()
  {
    StartCoroutine(LateOnEnable());
  }

  IEnumerator LateOnEnable()
  {
    yield return new WaitForEndOfFrame();
    EventManager.newHoverEvent += OnNewHover;
  }

  void OnDisable()
  {
    EventManager.newHoverEvent -= OnNewHover;
  }

  // *************** //
  // ** Events **    // Appel de fonctions au sein de ce script grâce à des events
  // *************** //

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
    hoveredBallon = e.hoveredBallon;

    changeColorEnter();


  }
      
  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  /// <summary>Change la couleur de la case lors d'un enter d'hover.</summary>
  private void changeColorEnter()
  { // Change la couleur de la case qui est sur le curseur

    Player currentPlayer = GameManager.Instance.currentPlayer;
    Phase currentPhase = GameManager.Instance.currentPhase;
    Color actionColor = ColorManager.Instance.actionColor;
    Color moveColor = ColorManager.Instance.moveColor;
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
    Color hoverColor = ColorManager.Instance.hoverColor;
    PersoAction actualAction = GameManager.Instance.actualAction;

    hoveredCase.GetComponent<CaseData>().ChangeStatut(Statut.isHovered);

    if (hoveredCase.GetComponent<SpriteRenderer>().color == ColorManager.Instance.actionPreColor)
      hoveredCase.GetComponent<SpriteRenderer>().color = ColorManager.Instance.actionColor;
  }

  /// <summary>Change la couleur de la case lors d'un exit d'hover.</summary>
  private void changeColorExit(Phase currentPhase)
  { // Change la couleur de la case qui était sur le curseur
    switch (currentPhase)
      {
      case (Phase.Placement):
        hoveredCase.GetComponent<CaseData>().ChangeStatut(Statut.None, Statut.isHovered);
        break;
      case (Phase.Deplacement):
        hoveredCase.GetComponent<CaseData>().ChangeStatut(Statut.None, Statut.isHovered);

        if (SelectionManager.Instance.selectedPersonnage != null
            && hoveredCase.casePathfinding == PathfindingCase.Walkable
            && GameManager.Instance.actualAction == PersoAction.isSelected)
          {
            MoveBehaviour.Instance.createPath();
            Pathfinding.Instance.StartPathfinding();
          } else
          {
            CaseManager.Instance.RemovePath();
          }
        break;
      }
  }
}