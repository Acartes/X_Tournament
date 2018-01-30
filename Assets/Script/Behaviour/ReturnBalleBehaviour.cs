﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBalleBehaviour : MonoBehaviour {

  public List<GameObject> caseAction;

  public static ReturnBalleBehaviour Instance;

    void Awake () {
      Instance = this;
    }

  void OnEnable()
    {
      ClickEvent.newClickEvent += OnNewClick;
    }

  void OnDisable()
    {
      ClickEvent.newClickEvent -= OnNewClick;
    }

  public void OnNewClick ()
    { // Lors d'un click sur une case
      DoBehaviour();
    }

    public void StartBehaviour () 
    {
      Color caseColor = ColorManager.Instance.caseColor;
      GameObject selectedCase = SelectionManager.Instance.selectedCase;

      GameManager.Instance.actualAction = PersoAction.isReplacingBall;
      caseAction.Clear ();

      int xCoord = selectedCase.GetComponent<CaseData>().xCoord;
      int yCoord = selectedCase.GetComponent<CaseData>().yCoord;

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

    void DoBehaviour () {
      GameObject selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
      GameObject hoveredCase = HoverManager.Instance.hoveredCase;
      PersoAction actualAction = GameManager.Instance.actualAction;
      GameObject selectedBallon = SelectionManager.Instance.selectedBallon;

      if (actualAction == PersoAction.isWaiting
        && caseAction.Count != 0) 
        {
          if (caseAction.Contains(hoveredCase)) 
            {
              selectedBallon.transform.position = hoveredCase.transform.position;
              hoveredCase.GetComponent<CaseData>().ChangeColor(Statut.canReplace);
              StartCoroutine(EndBehaviour ());
              return;
            }
          StartCoroutine(EndBehaviour ());
          selectedPersonnage.GetComponent<PersoData> ().actualPointMovement++;
        }
    }

    void AddAction (int xCoord, int yCoord) 
    {
      Color actionPreColor = ColorManager.Instance.actionPreColor;

      if (GameObject.Find (xCoord.ToString () + " " + yCoord.ToString ()) != null
          && GameObject.Find (xCoord.ToString () + " " + yCoord.ToString ()).GetComponent<CaseData> ().personnageData != null) {
          GameObject newCase = (GameObject.Find (xCoord.ToString () + " " + yCoord.ToString ()));
          caseAction.Add(newCase);
          newCase.GetComponent<CaseData>().ChangeColor(Statut.canReturnTo);
        }
    }

  IEnumerator EndBehaviour () 
    {
      yield return new WaitForEndOfFrame();
      GameManager.Instance.actualAction = PersoAction.isSelected;

        foreach (GameObject obj in caseAction) {
          obj.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.canReturnTo);
        }
      TurnManager.Instance.StartCoroutine("EnableFinishTurn");
      CaseManager.Instance.StartCoroutine ("ShowActions");
      caseAction.Clear ();
    }
}
