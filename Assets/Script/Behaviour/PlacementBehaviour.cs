using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class PlacementBehaviour : NetworkBehaviour
{
  // *************** //
  // ** Variables ** //
  // *************** //

  [SerializeField]
  Material redMat;
  [SerializeField]
  Material blueMat;

  [Tooltip("Index du personnage à placer")]
  int persoToPlaceNumber = -1;

  public static PlacementBehaviour Instance;

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log(this.GetType() + " is Instanced");
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    while (!LoadingManager.Instance.isGameReady())
      {
        yield return new WaitForEndOfFrame();
      }
    Init();
  }

  private void Init()
  {
    EventManager.newClickEvent += OnNewClick;
    StartCoroutine(LateOnEnable());
  }

  IEnumerator LateOnEnable()
  {
    yield return new WaitForEndOfFrame();
    TurnManager.Instance.changeTurnEvent += OnChangeTurn;
  }

  void OnDisable()
  {
    if (LoadingManager.Instance != null && LoadingManager.Instance.isGameReady())
      {
        EventManager.newClickEvent -= OnNewClick;
        TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
      }
  }


  public void OnNewClick()
  {
    // Lors d'un click sur une case
    Phase currentPhase = TurnManager.Instance.currentPhase;
    Player currentPlayer = TurnManager.Instance.currentPlayer;
    CaseData hoveredCase = HoverManager.Instance.hoveredCase;

        if (currentPhase != Phase.Placement)
            return;

        // Place un perso ami sur une case vide
        if (currentPhase == Phase.Placement &&
        HoverManager.Instance.hoveredCase != null && HoverManager.Instance.hoveredPersonnage == null &&
        hoveredCase.casePathfinding == PathfindingCase.Walkable)
      {
        Statut statut = hoveredCase.statut;

        Debug.Log("Place un perso ami sur une case vide");
        if ((Statut.placementRed & statut) == Statut.placementRed && currentPlayer == Player.Red)
          {
            CreatePerso(currentPhase, currentPlayer, 0);
          }
        if ((Statut.placementBlue & statut) == Statut.placementBlue && currentPlayer == Player.Blue)
          {
            CreatePerso(currentPhase, currentPlayer, 1);
          }
      }
        // si on selectionne un personnage à partir d'un portrait
        else if (HoverManager.Instance.hoveredPersonnage != null && HoverManager.Instance.hoveredCase == null)
      {
        Debug.Log("si on selectionne un personnage à partir d'un portrait");

        SelectionManager.Instance.selectedPersonnage = HoverManager.Instance.hoveredPersonnage; // total forcage, préférer SelectPerso() in-game
        InfoPerso.Instance.PersoSelected(SelectionManager.Instance.selectedPersonnage); // total forcage, préférer SelectPerso() in-game
      }

        // Fait disparaître un perso placé sur une case
        else if (HoverManager.Instance.hoveredPersonnage != null && HoverManager.Instance.hoveredCase != null &&
      hoveredCase.personnageData.owner == currentPlayer)
      {
        Debug.Log("Fait disparaître un perso placé sur une case");
        SelectionManager.Instance.selectedPersonnage = HoverManager.Instance.hoveredPersonnage; // total forcage, préférer SelectPerso() in-game

        InfoPerso.Instance.PersoSelected(SelectionManager.Instance.selectedPersonnage);
        InfoPerso.Instance.PersoRemoved(SelectionManager.Instance.selectedPersonnage);
        ChangePersoPosition(null, SelectionManager.Instance.selectedPersonnage); // total forcage, préférer SelectPerso() in-game
      }
  }

  void OnChangeTurn(object sender, PlayerArgs e)
  { // Lorsqu'un joueur termine son tour
    switch (e.currentPhase)
      {
      case Phase.Deplacement:

        break;
      case Phase.Placement:
        break;
      }
  }

  public void CreatePerso(Phase currentPhase, Player currentPlayer, int playerIndex)
  { // On créé un personnage sur une case


    CreatePersoPlacement(HoverManager.Instance.hoveredCase, SelectionManager.Instance.selectedPersonnage);
    //  }
  }

  public void CreatePersoPlacement(CaseData hoveredCase, PersoData selectedPersonnage)
  { //
    if (SelectionManager.Instance.selectedPersonnage != null && hoveredCase.casePathfinding == PathfindingCase.Walkable)
      {
        selectedPersonnage.transform.position = hoveredCase.transform.position - selectedPersonnage.originPoint.transform.localPosition;
        selectedPersonnage.owner = TurnManager.Instance.currentPlayer;
        RosterManager.Instance.listHeroPlaced.Add(selectedPersonnage);

        if (selectedPersonnage.GetComponent<PersoData>().owner == Player.Red)
          {
            selectedPersonnage.GetComponent<PersoData>().ChangeRotation(Direction.SudEst);
            // mettre icone perso rouge
          } else
          {
            selectedPersonnage.GetComponent<PersoData>().ChangeRotation(Direction.NordOuest);
            // mettre icone perso bleu
          }
        hoveredCase.personnageData = selectedPersonnage;
        HoverManager.Instance.hoveredPersonnage = selectedPersonnage;
        InfoPerso.Instance.PersoPlaced(selectedPersonnage);
      }
  }

  public void ChangePersoPosition(CaseData hoveredCase, PersoData selectedPersonnage)
  { // Change la position d'un personnage déjà placé vers la case où a cliqué le joueur possesseur.

    if (hoveredCase == null)
      {
        selectedPersonnage.transform.position = Vector3.one * 999;
        return;
      }
    if (hoveredCase.GetComponent<CaseData>().casePathfinding == PathfindingCase.Walkable || selectedPersonnage != null)
      {
        SelectionManager.Instance.selectedCase = hoveredCase;
        SelectionManager.Instance.selectedLastPersonnage = SelectionManager.Instance.selectedPersonnage;
        SelectionManager.Instance.selectedLastPersonnage.transform.position = SelectionManager.Instance.selectedCase.transform.position + selectedPersonnage.originPoint.transform.position;
        if (selectedPersonnage != null && SelectionManager.Instance.selectedLastCase != null)
          {
            SelectionManager.Instance.selectedPersonnage = selectedPersonnage;
            SelectionManager.Instance.selectedPersonnage.transform.position = SelectionManager.Instance.selectedLastCase.transform.position + selectedPersonnage.originPoint.transform.position;
          } else
          {
            SelectionManager.Instance.selectedPersonnage = selectedPersonnage;
          }
      }
    SelectionManager.Instance.Deselect();
  }
}
