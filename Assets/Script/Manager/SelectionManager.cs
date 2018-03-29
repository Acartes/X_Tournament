using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>Gère la selection du joueur, que ce soit un personnage selectionné, une case ou un ballon.</summary>
public class SelectionManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public CaseData selectedLastCase;
  public PersoData selectedLastPersonnage;
  public BallonData selectedBallon;
  public CaseData selectedCase;
  public PersoData selectedPersonnage;

  bool isDisablePersoSelection = false;
  
  public static SelectionManager Instance;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

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
      yield return new WaitForEndOfFrame();
    Init();
  }

  private void Init()
  {
    EventManager.newClickEvent += OnNewClick;
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

    // *************** //
    // ** Events **    // Appel de fonctions au sein de ce script grâce à des events
    // *************** //

    public void OnNewClick()
    { // Lors d'un click sur une case

        if (isDisablePersoSelection)
            return;

        PersoData hoveredPersonnage = HoverManager.Instance.hoveredPersonnage;
        Phase currentPhase = TurnManager.Instance.currentPhase;
        Player currentPlayer = TurnManager.Instance.currentPlayer;
        PersoAction actualAction = GameManager.Instance.actualAction;
        CaseData hoveredCase = HoverManager.Instance.hoveredCase;
        List<Transform> pathes = MoveBehaviour.Instance.movePathes;
        Color selectedColor = ColorManager.Instance.selectedColor;
        Color moveColor = ColorManager.Instance.moveColor;
        Color caseColor = ColorManager.Instance.caseColor;

        selectedLastCase = selectedCase;
        switch (currentPhase)
        {
            case (Phase.Placement):
                return; // c'est le scriptPlacementBehaviour qui s'occupe des clicks de phase de placement
                break;
            case (Phase.Deplacement):
                if (hoveredPersonnage != null && hoveredPersonnage.owner == currentPlayer)
                { // changement de personnage selectionné
                    SelectPerso(hoveredCase, hoveredPersonnage, selectedColor, currentPhase, currentPlayer, actualAction);
                }
                break;
        }
    }

  void OnChangeTurn(object sender, PlayerArgs e)
  { // Lorsqu'un joueur termine son tour

        Deselect();

        switch (e.currentPhase)
      {
      case Phase.Deplacement:
        ResetSelection(ColorManager.Instance.caseColor);
        break;
      case Phase.Placement:
                StartCoroutine(preSelectFirstPerso());
                break;
      }
    }

    IEnumerator preSelectFirstPerso()
    {
        while (RosterManager.Instance.listHero.Count != 8)
            yield return new WaitForEndOfFrame();
        if (TurnManager.Instance.currentPlayer == Player.Red)
        {
            selectedPersonnage = RosterManager.Instance.listHero[0];
        }
        if (TurnManager.Instance.currentPlayer == Player.Blue)
        {
            selectedPersonnage = RosterManager.Instance.listHero[4];
        }
    }

    // *************** //
    // ** Fonctions ** // Fonctions réutilisables ailleurs
    // *************** //

    public void ResetSelection(Color caseColor)
  {
    if (selectedCase != null)
      {
        selectedCase.ChangeStatut(Statut.None, Statut.isSelected);
      }
    selectedCase = null;
    selectedPersonnage = null;
  }

  public void Deselect()
  {
    CaseManager.Instance.RemovePath();
    MoveBehaviour.Instance.movePathes.Clear();
    GameManager.Instance.actualAction = PersoAction.isSelected;

    if (selectedLastCase != null)
      selectedLastCase.ChangeStatut(Statut.None, Statut.isSelected);

    selectedPersonnage = null;
    selectedCase = null;
     
  }

  public void SelectPerso(CaseData hoveredCase, PersoData hoveredPersonnage, Color selectedColor, Phase currentPhase, Player currentPlayer, PersoAction actualAction)
  {
        Deselect();
        selectedCase = hoveredCase;
    selectedPersonnage = hoveredPersonnage;

        UIManager.Instance.ChangeSpriteSpellButton(selectedPersonnage);
        InfoPerso.Instance.PersoSelected(hoveredPersonnage);

        selectedCase.ChangeStatut(Statut.isSelected);
    GameManager.Instance.actualAction = PersoAction.isSelected;
  }

  public void DisablePersoSelection()
  {
    isDisablePersoSelection = true;
  }

  public void EnablePersoSelection()
  {
    isDisablePersoSelection = false;
  }
}
