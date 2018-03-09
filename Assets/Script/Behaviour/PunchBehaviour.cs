using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Diagnostics;

public class PunchBehaviour : NetworkBehaviour
{

  public PersoData punchedPersonnage;

  public static PunchBehaviour Instance;

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
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
  }

  void OnDisable()
  {
    EventManager.newClickEvent -= OnNewClick;
  }

  public void OnNewClick()
  {

    PersoData hoveredPersonnage = HoverManager.Instance.hoveredPersonnage;
    Phase currentPhase = TurnManager.Instance.currentPhase;
    Player currentPlayer = TurnManager.Instance.currentPlayer;
    PersoAction actualAction = GameManager.Instance.actualAction;
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;

    if (actualAction == PersoAction.isSelected &&
        currentPhase == Phase.Deplacement &&
        selectedPersonnage != null &&
        hoveredPersonnage != null &&
        hoveredPersonnage.GetComponent<PersoData>().owner != currentPlayer &&
        selectedPersonnage.GetComponent<PersoData>().actualPointMovement != 0 &&
        Fonction.Instance.CheckAdjacent(selectedPersonnage.gameObject, hoveredPersonnage.gameObject) == true)
      {
        Punch(HoverManager.Instance.hoveredPersonnage);
      }
  }

  public IEnumerator Punch(PersoData hoveredPersonnage)
  {
    SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>().actualPointMovement--;
    punchedPersonnage = hoveredPersonnage;
    punchedPersonnage.actualPointResistance--;
    Color punchedPersonnageColor = punchedPersonnage.GetComponent<SpriteRenderer>().color;
    punchedPersonnage.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);

    for (int i = 100; i > 0; i = i - 10)
      {
        yield return new WaitForSeconds(0.01f);
        punchedPersonnage.GetComponent<SpriteRenderer>().color = new Color((0.01f * i) + (punchedPersonnageColor.r - (0.01f * i)), (0.01f * i) + (punchedPersonnageColor.g - (0.01f * i)), 0 + (punchedPersonnageColor.b - (0.01f * i)));
      }
  }

}
