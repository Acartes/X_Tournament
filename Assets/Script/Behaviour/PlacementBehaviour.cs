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
    Debug.Log("PlacementBehaviour is Instanced");
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
    if (LoadingManager.Instance.isGameReady())
      {
        EventManager.newClickEvent -= OnNewClick;
        TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
      }
  }


  public void OnNewClick()
  { // Lors d'un click sur une case
    Debug.Log("OnNewClick");
    Phase currentPhase = TurnManager.Instance.currentPhase;
    Player currentPlayer = TurnManager.Instance.currentPlayer;
    CaseData hoveredCase = HoverManager.Instance.hoveredCase;

    Statut statut = hoveredCase.statut;

    if (currentPhase == Phase.Placement &&
        SelectionManager.Instance.selectedCase == null &&
        hoveredCase.GetComponent<CaseData>().casePathfinding == PathfindingCase.Walkable)
      {
        Debug.Log("beforeCreatePerso");
        if ((Statut.placementRed & statut) == Statut.placementRed && currentPlayer == Player.Red)
          {
            Debug.Log("createPerso");
            CreatePerso(currentPhase, currentPlayer, 0);
          }
        if ((Statut.placementBlue & statut) == Statut.placementBlue && currentPlayer == Player.Blue)
          {
            Debug.Log("createPerso");
            CreatePerso(currentPhase, currentPlayer, 1);
          }
      }
    if (SelectionManager.Instance.selectedCase != null && HoverManager.Instance.hoveredCase != null)
    if (currentPhase == Phase.Placement
        && SelectionManager.Instance.selectedCase != null
        && HoverManager.Instance.hoveredCase != null
        && SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>().persoCase != HoverManager.Instance.hoveredCase
        && ((Statut.placementRed & statut) == Statut.placementRed && currentPlayer == Player.Red)
        || ((Statut.placementBlue & statut) == Statut.placementBlue && currentPlayer == Player.Blue))
      {
        ChangePersoPosition(hoveredCase, HoverManager.Instance.hoveredPersonnage);
        SelectionManager.Instance.Deselect(TurnManager.Instance.currentPhase, TurnManager.Instance.currentPlayer);
      }
  }

  void OnChangeTurn(object sender, PlayerArgs e)
  { // Lorsqu'un joueur termine son tour
    switch (e.currentPhase)
      {
      case Phase.Deplacement:

        break;
      case Phase.Placement:
        PlacementBehaviour.Instance.NextToPlace(TurnManager.Instance.currentPhase, TurnManager.Instance.currentPlayer);
        break;
      }
  }

  public void CreatePerso(Phase currentPhase, Player currentPlayer, int playerIndex)
  { // On créé un personnage sur une case
    if (RosterManager.Instance.listHeroJXToPlace[playerIndex].Count != 0)
      {
        if (RosterManager.Instance.listHeroJXToPlace[playerIndex][persoToPlaceNumber] != SelectionManager.Instance.selectedPersonnage)
          {
            SelectionManager.Instance.selectedPersonnage = RosterManager.Instance.listHeroJXToPlace[playerIndex][persoToPlaceNumber];

          }
          
        RosterManager.Instance.listHeroJXToPlace[playerIndex].RemoveAt(persoToPlaceNumber);

        Debug.Log("createPersoPlacement");
        CreatePersoPlacement(HoverManager.Instance.hoveredCase, SelectionManager.Instance.selectedPersonnage);
      }
  }

  public void CreatePersoPlacement(CaseData hoveredCase, PersoData selectedPersonnage)
  { //
    if (SelectionManager.Instance.selectedPersonnage != null)
      {
        Debug.Log("transformedperso");
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

        SelectionManager.Instance.Deselect(TurnManager.Instance.currentPhase, TurnManager.Instance.currentPlayer);

      }
  }

  public void NextToPlace(Phase currentPhase, Player currentPlayer)
  { // Indique quel prochain personnage à placer.
    if (currentPhase == Phase.Placement)
      {
        persoToPlaceNumber = 0;
        switch (currentPlayer)
          {
          case Player.Red:
            if (RosterManager.Instance.listHeroJXToPlace.Count > 0)
              {
                if (RosterManager.Instance.listHeroJXToPlace[0].Count != 0)
                  {
                    SelectionManager.Instance.selectedPersonnage = RosterManager.Instance.listHeroJXToPlace[0][persoToPlaceNumber];
                  }
              }
            break;
          case Player.Blue:
            if (RosterManager.Instance.listHeroJXToPlace.Count > 1)
              {
                if (RosterManager.Instance.listHeroJXToPlace[1].Count != 0)
                  {
                    SelectionManager.Instance.selectedPersonnage = RosterManager.Instance.listHeroJXToPlace[1][persoToPlaceNumber];
                  }
              }
            break;
          }
      }
  }

  public void ChangePersoPosition(CaseData hoveredCase, PersoData hoveredPersonnage)
  { // Change la position d'un personnage déjà placer vers la case où a cliqué le joueur possesseur.
    if (hoveredCase.GetComponent<CaseData>().casePathfinding == PathfindingCase.Walkable || hoveredPersonnage != null)
      {
        SelectionManager.Instance.selectedCase = hoveredCase;
        SelectionManager.Instance.selectedLastPersonnage = SelectionManager.Instance.selectedPersonnage;
        SelectionManager.Instance.selectedLastPersonnage.transform.position = SelectionManager.Instance.selectedCase.transform.position + hoveredPersonnage.originPoint.transform.position;
        if (hoveredPersonnage != null && SelectionManager.Instance.selectedLastCase != null)
          {
            SelectionManager.Instance.selectedPersonnage = hoveredPersonnage;
            SelectionManager.Instance.selectedPersonnage.transform.position = SelectionManager.Instance.selectedLastCase.transform.position + hoveredPersonnage.originPoint.transform.position;
          } else
          {
            SelectionManager.Instance.selectedPersonnage = hoveredPersonnage;
          }
      }
    SelectionManager.Instance.Deselect(TurnManager.Instance.currentPhase, TurnManager.Instance.currentPlayer);
  }
}
