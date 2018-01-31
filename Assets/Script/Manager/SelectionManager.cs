using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

  public GameObject selectedLastCase;
  public GameObject selectedLastPersonnage;
  public GameObject selectedBallon;
  public GameObject selectedCase;
  public GameObject selectedPersonnage;
  
  public static SelectionManager Instance;

    void Awake () {
    Instance = this;
  }

  void OnEnable()
    {
      ClickEvent.newClickEvent += OnNewClick;
      StartCoroutine (LateOnEnable());
    }

    IEnumerator LateOnEnable() {
      yield return new WaitForEndOfFrame ();
      TurnManager.Instance.changeTurnEvent += OnChangeTurn;
    }

  void OnDisable()
    {
      ClickEvent.newClickEvent -= OnNewClick;
      TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
    }
      
  public void OnNewClick ()
    { // Lors d'un click sur une case

    var hoveredPersonnage = HoverManager.Instance.hoveredPersonnage;
    var currentPhase = TurnManager.Instance.currentPhase;
    var currentPlayer = TurnManager.Instance.currentPlayer;
    var actualAction = GameManager.Instance.actualAction;
    var hoveredCase = HoverManager.Instance.hoveredCase;
    var pathes = MoveBehaviour.Instance.pathes;
    var selectedColor = ColorManager.Instance.selectedColor;
    var moveColor = ColorManager.Instance.moveColor;
    var caseColor = ColorManager.Instance.caseColor;

          selectedLastCase = selectedCase;
          switch (currentPhase)
            {
              case (Phase.Placement):
            if (hoveredPersonnage != null
              && SelectionManager.Instance.selectedCase == null
              && hoveredPersonnage.GetComponent<PersoData> ().owner == currentPlayer)
                  {
                    SelectPerso(hoveredCase, hoveredPersonnage, selectedColor, currentPhase, currentPlayer, actualAction);
                  }

                break;
              case (Phase.Deplacement):
                  if (hoveredPersonnage != null && hoveredPersonnage.GetComponent<PersoData> ().owner == currentPlayer) { // changement de personnage selectionné
                    SelectPerso (hoveredCase, hoveredPersonnage, selectedColor, currentPhase, currentPlayer, actualAction);
                  }
                break;
        }
    }
  void OnChangeTurn(object sender, PlayerArgs e)
    { // Lorsqu'un joueur termine son tour

      Deselect (TurnManager.Instance.currentPhase, TurnManager.Instance.currentPlayer);

        switch (e.currentPhase) {
          case Phase.Deplacement:
            ResetSelection(ColorManager.Instance.caseColor);
            break;
          case Phase.Placement:
            break;
        }
    }

  public void ResetSelection(Color caseColor)
    {
      if (selectedCase != null)
        {
          selectedCase.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.isSelected);
        }
      selectedCase = null;
      selectedPersonnage = null;
    }

  public void Deselect(Phase currentPhase, Player currentPlayer)
    {
      MoveBehaviour.Instance.HidePath ();
      MoveBehaviour.Instance.pathes.Clear ();
      GameManager.Instance.actualAction = PersoAction.isSelected;
      CaseManager.Instance.StartCoroutine ("ShowActions");

      if (selectedLastCase != null)
        {
            if (currentPhase == Phase.Placement) {
              selectedLastCase.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.isSelected);
            } else {
              selectedLastCase.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.isSelected);
            }
        }
      selectedPersonnage = null;
      selectedCase = null;

      if (currentPhase == Phase.Placement)
        {
          PlacementBehaviour.Instance.NextToPlace(TurnManager.Instance.currentPhase, TurnManager.Instance.currentPlayer);
        }
    }

  public void SelectPerso(GameObject hoveredCase, GameObject hoveredPersonnage, Color selectedColor, Phase currentPhase, Player currentPlayer, PersoAction actualAction)
    {
      Deselect (currentPhase, currentPlayer);
      selectedCase = hoveredCase;
      selectedPersonnage = hoveredPersonnage;

      selectedCase.GetComponent<CaseData>().caseColor = selectedColor;
      selectedCase.GetComponent<CaseData>().ChangeColor(Statut.isSelected);
      GameManager.Instance.actualAction = PersoAction.isSelected;
      CaseManager.Instance.StartCoroutine ("ShowActions");
    }
}
