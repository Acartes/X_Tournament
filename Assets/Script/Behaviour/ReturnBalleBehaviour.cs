// NOTE: Cette mécanique a été annulé vers début Mars, il consistait à renvoyer la balle à l'envoyeur en infligeant des dégât si il était intercepté
// L'effet changeait quelque peu selon le poids du personnage qui interceptait la balle.

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReturnBalleBehaviour : NetworkBehaviour
{

  public List<CaseData> caseAction;

  public static ReturnBalleBehaviour Instance;

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
  { // Lors d'un click sur une case
    DoBehaviour();
  }

  public void StartBehaviour()
  {
    Color caseColor = ColorManager.Instance.caseColor;
    CaseData selectedCase = SelectionManager.Instance.selectedCase;

    GameManager.Instance.actualAction = PersoAction.isReplacingBall;
    caseAction.Clear();

    int xCoord = selectedCase.xCoord;
    int yCoord = selectedCase.yCoord;

    for (int i = 0; i < 6; i++)
      {
        for (int y = 0; y < 6; y++)
          {
            AddAction((xCoord + y), (yCoord + i));
            AddAction((xCoord + y), (yCoord - i));
            AddAction((xCoord - y), (yCoord + i));
            AddAction((xCoord - y), (yCoord - i));
          }
      }
  }

  void DoBehaviour()
  {
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
    CaseData hoveredCase = HoverManager.Instance.hoveredCase;
    PersoAction actualAction = GameManager.Instance.actualAction;
    BallonData selectedBallon = SelectionManager.Instance.selectedBallon;

    if (actualAction == PersoAction.isWaiting
        && caseAction.Count != 0)
      {
        if (caseAction.Contains(hoveredCase))
          {
            selectedBallon.transform.position = hoveredCase.transform.position;
            hoveredCase.GetComponent<CaseData>().ChangeStatut(Statut.canReplace);
            StartCoroutine(EndBehaviour());
            return;
          }
        StartCoroutine(EndBehaviour());
        selectedPersonnage.GetComponent<PersoData>().actualPointMovement++;
      }
  }

  void AddAction(int xCoord, int yCoord)
  {
    Color actionPreColor = ColorManager.Instance.actionPreColor;

    if (GameObject.Find(xCoord.ToString() + " " + yCoord.ToString()) != null
        && GameObject.Find(xCoord.ToString() + " " + yCoord.ToString()).GetComponent<CaseData>().personnageData != null)
      {
        CaseData newCase = (GameObject.Find(xCoord.ToString() + " " + yCoord.ToString())).GetComponent<CaseData>();
        caseAction.Add(newCase);
        newCase.GetComponent<CaseData>().ChangeStatut(Statut.canReturnTo);
      }
  }

  IEnumerator EndBehaviour()
  {
    yield return new WaitForEndOfFrame();
    GameManager.Instance.actualAction = PersoAction.isSelected;

    foreach (CaseData obj in caseAction)
      {
        obj.ChangeStatut(Statut.None, Statut.canReturnTo);
      }
    TurnManager.Instance.StartCoroutine("EnableFinishTurn");
    caseAction.Clear();
  }
}
*/