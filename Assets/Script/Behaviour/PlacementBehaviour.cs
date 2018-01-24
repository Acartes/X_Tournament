using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PlacementBehaviour : MonoBehaviour
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

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        ClickEvent.newClickEvent += OnNewClick;
        StartCoroutine(LateOnEnable());
    }

    IEnumerator LateOnEnable()
    {
        yield return new WaitForEndOfFrame();
        TurnManager.Instance.changeTurnEvent += OnChangeTurn;
    }

    void OnDisable()
    {
        ClickEvent.newClickEvent -= OnNewClick;
        TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
    }

    public void OnNewClick()
    { // Lors d'un click sur une case

        var currentPhase = TurnManager.Instance.currentPhase;
        var currentPlayer = TurnManager.Instance.currentPlayer;
        var hoveredCase = HoverManager.Instance.hoveredCase;

        if (currentPhase == Phase.Placement &&
          SelectionManager.Instance.selectedCase == null &&
          hoveredCase.GetComponent<CaseData>().casePathfinding == PathfindingCase.Walkable)
        {
            if (hoveredCase.GetComponent<CaseData>().ownerPlacementZone == Player.Red && currentPlayer == Player.Red)
            {
                CreatePerso(currentPhase, currentPlayer, 0);
            }
            if (hoveredCase.GetComponent<CaseData>().ownerPlacementZone == Player.Blue && currentPlayer == Player.Blue)
            {
                CreatePerso(currentPhase, currentPlayer, 1);
            }
        }
      if (SelectionManager.Instance.selectedCase != null && HoverManager.Instance.hoveredCase != null)

      if (currentPhase == Phase.Placement
        && SelectionManager.Instance.selectedCase != null
        && HoverManager.Instance.hoveredCase != null
        && SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>().persoCase != HoverManager.Instance.hoveredCase
        && hoveredCase.GetComponent<CaseData>().ownerPlacementZone == currentPlayer)
        {
          ChangePersoPosition(hoveredCase, HoverManager.Instance.hoveredPersonnage, GraphManager.Instance.offsetY);
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
            if (RosterManager.Instance.listHeroJXToPlace[playerIndex][persoToPlaceNumber] == SelectionManager.Instance.selectedPersonnage)
            {
                RosterManager.Instance.listHeroJXToPlace[playerIndex].RemoveAt(persoToPlaceNumber);
                CreatePersoPlacement(HoverManager.Instance.hoveredCase, GraphManager.Instance.offsetY, SelectionManager.Instance.selectedPersonnage);
            }
        }
    }

    void CreatePersoPlacement(GameObject hoveredCase, float offsetY, GameObject selectedPersonnage)
    { //
        if (SelectionManager.Instance.selectedPersonnage != null)
        {
            selectedPersonnage.transform.position = hoveredCase.transform.position + new Vector3(0, offsetY, 0);
            selectedPersonnage.GetComponent<PersoData>().owner = TurnManager.Instance.currentPlayer;
            selectedPersonnage.GetComponent<PersoData>().ChangeColor();
        RosterManager.Instance.listHeroPlaced.Add(selectedPersonnage);

            if (selectedPersonnage.GetComponent<PersoData>().owner == Player.Red)
            {
              selectedPersonnage.GetComponent<PersoData>().ChangeRotation (Direction.SudEst);
                // mettre icone perso rouge
            }
            else
            {
              selectedPersonnage.GetComponent<PersoData>().ChangeRotation (Direction.NordOuest);
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
                    if (RosterManager.Instance.listHeroJXToPlace[0].Count != 0)
                    {
                        SelectionManager.Instance.selectedPersonnage = RosterManager.Instance.listHeroJXToPlace[0][persoToPlaceNumber];
                    }
                    break;
                case Player.Blue:
                    if (RosterManager.Instance.listHeroJXToPlace[1].Count != 0)
                    {
                        SelectionManager.Instance.selectedPersonnage = RosterManager.Instance.listHeroJXToPlace[1][persoToPlaceNumber];
                    }
                    break;
            }
        }
    }

    public void ChangePersoPosition(GameObject hoveredCase, GameObject hoveredPersonnage, float offsetY)
    { // Change la position d'un personnage déjà placer vers la case où a cliqué le joueur possesseur.
        if (hoveredCase.GetComponent<CaseData>().casePathfinding == PathfindingCase.Walkable || hoveredPersonnage != null)
        {
            SelectionManager.Instance.selectedCase = hoveredCase;
            SelectionManager.Instance.selectedLastPersonnage = SelectionManager.Instance.selectedPersonnage;
            SelectionManager.Instance.selectedLastPersonnage.transform.position = SelectionManager.Instance.selectedCase.transform.position + new Vector3(0, offsetY, 0);
          if (hoveredPersonnage != null && SelectionManager.Instance.selectedLastCase != null)
            {
                SelectionManager.Instance.selectedPersonnage = hoveredPersonnage;
                SelectionManager.Instance.selectedPersonnage.transform.position = SelectionManager.Instance.selectedLastCase.transform.position + new Vector3(0, offsetY, 0);
            }
            else
            {
                SelectionManager.Instance.selectedPersonnage = hoveredPersonnage;
            }
        }
        SelectionManager.Instance.Deselect(TurnManager.Instance.currentPhase, TurnManager.Instance.currentPlayer);
    }
}
