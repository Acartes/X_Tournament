﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PushBehaviour : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public static PushBehaviour Instance;

  bool isPushing = false;

  public float travelTime;

  public List<Transform> pathList = new List<Transform>();

  PersoData persoAfflicted = null;
  public CaseData caseFinalShow = null; // la case à montrer pour le pré-rendu
  GameObject objAfflicted;

  public int pushValue;
  public int retainedPushValue;
  /// <summary>
  /// Est-ce que la tornade fait des dégats alors que le perso ne bouge pas?
  /// </summary>
  bool stillTornadoDamage;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;

    MultiplePushStart();
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  /// <summary>Check et prépare l'effet de poussée.</summary>
  public void PushCheck(GameObject obj, int givenPushValue, CaseData caseAfflicted, PushType pushType, Direction pushDirection = Direction.Front)
  {
    Direction persoDirection = Direction.None;

    pushValue = givenPushValue;
    if (obj.GetComponent<BallonData>() != null)
    {
      if (obj.GetComponent<BallonData>().isMoving)
      {
       // if (SelectionManager.Instance.selectedPersonnage.shotStrenght - obj.GetComponent<BallonData>().casesCrossed > 0)
       //   pushValue += SelectionManager.Instance.selectedPersonnage.shotStrenght - obj.GetComponent<BallonData>().casesCrossed;
        persoDirection = obj.GetComponent<BallonData>().ballonDirection;
        obj.GetComponent<BallonData>().StopMove();
        obj.GetComponent<BallonData>().StopAllCoroutines();
      }
    }

    objAfflicted = obj;
    if (objAfflicted.GetComponent<PersoData>() != null)
    {
      foreach (Transform path in MoveBehaviour.Instance.movePathes)
      {
        path.GetComponent<CaseData>().ChangeStatut(Statut.None, Statut.isMoving);
      }
      //      pushValue += objAfflicted.GetComponent<PersoData>().pushedDebt;
      objAfflicted.GetComponent<PersoData>().pushedDebt = 0;
      MoveBehaviour.Instance.movePathes.Clear();
      persoAfflicted = objAfflicted.GetComponent<PersoData>();
      persoDirection = persoAfflicted.persoDirection;
      Debug.Log("pushDebt is " + objAfflicted.GetComponent<PersoData>().pushedDebt);
      Debug.Log("pushValue is " + pushValue);
    }

    if (objAfflicted.GetComponent<SummonData>() != null)
    {
      persoDirection = objAfflicted.GetComponent<SummonData>().summonDirection;
    }

    CaseData tempCase = null;

    pathList.Clear();

    switch (pushType)
    {
      case PushType.FromCaster:
      GetShownCase(obj, pushValue, caseAfflicted, pushType, pushDirection);
      int y = pushValue;
      pushValue = Mathf.Abs(pushValue);
      while (y != 0)
      {
        if (y > 0)
        {
          y--;
          if (caseAfflicted.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection) != null || tempCase.summonData != null && tempCase.summonData.name.Contains("Air"))
          {
            tempCase = caseAfflicted.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection); ;
            pathList.Add(tempCase.transform);
            caseAfflicted = tempCase;
          }
        }
        if (y < 0)
        {
          y++;
          if (caseAfflicted.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection) != null || tempCase.summonData != null && tempCase.summonData.name.Contains("Air"))
          {
            tempCase = caseAfflicted.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection);
            pathList.Add(tempCase.transform);
            caseAfflicted = tempCase;
          }
        }
      }
      break;
      case PushType.FromTarget:
      GetShownCase(obj, pushValue, caseAfflicted, pushType, pushDirection);
      stillTornadoDamage = false;

      for (int i = 0; i < pushValue; i++)
      {
        if (pushDirection == Direction.Left)
          tempCase = caseAfflicted.GetCaseAtLeft(persoDirection);

        if (pushDirection == Direction.Right)
          tempCase = caseAfflicted.GetCaseAtRight(persoDirection);

        if (pushDirection == Direction.Back)
        {
          tempCase = caseAfflicted.GetCaseAtBack(persoDirection);
        }

        if (pushDirection == Direction.Front)
          tempCase = caseAfflicted.GetCaseInFront(persoDirection);

        if (tempCase == null || tempCase.casePathfinding == PathfindingCase.NonWalkable || tempCase.summonData != null && tempCase.summonData.name.Contains("Air"))
        {
          caseAfflicted.casePathfinding = PathfindingCase.Walkable;
          pathList.Add(caseAfflicted.transform);
          stillTornadoDamage = true;
          break;
        }
        else
        {
          pathList.Add(tempCase.transform);
          caseAfflicted = tempCase;
        }
      }

      break;
      case PushType.FromTerrain:
      for (int i = 0; i < pushValue; i++)
      {
        if (pushDirection == Direction.SudOuest)
          tempCase = caseAfflicted.GetBottomLeftCase();

        if (pushDirection == Direction.SudEst)
          tempCase = caseAfflicted.GetBottomRightCase();

        if (pushDirection == Direction.NordOuest)
          tempCase = caseAfflicted.GetTopLeftCase();

        if (pushDirection == Direction.NordEst)
          tempCase = caseAfflicted.GetTopRightCase();

        pathList.Add(tempCase.transform);
        caseAfflicted = tempCase;
        if (tempCase == null || tempCase.casePathfinding == PathfindingCase.NonWalkable || tempCase.summonData != null && tempCase.summonData.name.Contains("Air"))
        {
          break;
        }
      }
      break;
    }

    foreach (CaseData caseObj in CaseManager.Instance.GetAllCaseWithStatut(Statut.atPush))
    {
      caseObj.GetComponent<CaseData>().ChangeStatut(Statut.None, Statut.atPush);
    }


  }

  public void PushStart()
  {
    MoveBehaviour.Instance.StopAllCoroutines();
    StopAllCoroutines();

    StartCoroutine(Deplacement(objAfflicted, pathList));
  }

  public void MultiplePushStart()
  {
    StartCoroutine(Deplacement(objAfflicted, pathList));
  }

  public IEnumerator Deplacement(GameObject objAfflicted, List<Transform> pathes)
  { // On déplace le personnage de case en case jusqu'au click du joueur propriétaire, et entre temps on check s'il est taclé ou non
    TurnManager.Instance.DisableFinishTurn();

    GameManager.Instance.actualAction = PersoAction.isMoving;
    Transform lastPath = null;

    List<Transform> tempPath = pathes.GetRange(0, pathes.Count);
    bool stopDeplacement = false;

    Vector3 originPoint = Vector3.zero;

    if (objAfflicted.GetComponent<PersoData>() != null)
    {
      originPoint = objAfflicted.GetComponent<PersoData>().originPoint.transform.localPosition;
      objAfflicted.GetComponent<PersoData>().isPushed = true;
    }

    if (objAfflicted.GetComponent<BallonData>() != null)
    {
      originPoint = objAfflicted.GetComponent<BallonData>().offsetBallon;
      objAfflicted.GetComponent<BallonData>().isPushed = true;
    }

    if (objAfflicted.GetComponent<SummonData>() != null)
    {
      originPoint = objAfflicted.GetComponent<SummonData>().originPoint.transform.localPosition;
    }

    bool objectCollision = false;

    PersoData persoSelected = SelectionManager.Instance.selectedPersonnage;

    foreach (Transform path in tempPath)
    {

      if (path.GetComponent<CaseData>().casePathfinding == PathfindingCase.NonWalkable)
      {
        if (path.GetComponent<CaseData>().summonData != null)
        {
          AfterFeedbackManager.Instance.PRText(1, path.gameObject);
          path.GetComponent<CaseData>().summonData.actualPointResistance--;
        }
        if (path.GetComponent<CaseData>().personnageData != null)
        {
          if (path.GetComponent<CaseData>().personnageData.timeStunned == 0)
          {
            AfterFeedbackManager.Instance.PRText(1, path.gameObject);
            path.GetComponent<CaseData>().personnageData.actualPointResistance--;
          }
        }

        if (objAfflicted.GetComponent<PersoData>() != null)
        {
          if (objAfflicted.GetComponent<PersoData>().timeStunned == 0)
          {
            AfterFeedbackManager.Instance.PRText(1, objAfflicted);
            objAfflicted.GetComponent<PersoData>().actualPointResistance--;
          }
        }
        if (objAfflicted.GetComponent<SummonData>() != null && !objAfflicted.GetComponent<SummonData>().invulnerable)
        {
          AfterFeedbackManager.Instance.PRText(1, objAfflicted);
          objAfflicted.GetComponent<SummonData>().actualPointResistance--;
        }
        objectCollision = true;
        break;
      }

      lastPath = path;
      Vector3 startPos = objAfflicted.transform.position;
      float fracturedTime = 0;
      float timeUnit = travelTime / 60;

      if (objAfflicted.GetComponent<PersoData>() != null)
      {
        objAfflicted.GetComponent<PersoData>().RotateTowardsReversed(path.gameObject);
      }

      if (objAfflicted.GetComponent<BallonData>() != null)
      {
        objAfflicted.GetComponent<BallonData>().RotateTowardsReversed(path.gameObject);
      }

      if (path.GetComponent<CaseData>() != null && path.GetComponent<CaseData>().summonData != null && (path.GetComponent<CaseData>().summonData.name.Contains("Air")
        || path.GetComponent<CaseData>().summonData.stopBall))
      {
        stopDeplacement = true;
      }

      while (fracturedTime < 1)
      {
        fracturedTime += timeUnit + 0.01f;
        objAfflicted.transform.position = Vector3.Lerp(startPos, path.transform.position - originPoint, fracturedTime);
        yield return new WaitForEndOfFrame();
      }

      if (stopDeplacement)
      {
        break;
      }

      if (stillTornadoDamage && objAfflicted.GetComponent<SummonData>() == null)
      {
        CaseData afflictedCase = null;
        PersoData persoAfficted = objAfflicted.GetComponent<PersoData>();
        Direction rightDirection = Direction.None;
        if (persoAfficted != null)
        {
          if (persoAfficted.timeStunned == 0)
          {
            AfterFeedbackManager.Instance.PRText(1, objAfflicted);
            persoAfficted.actualPointResistance--;
            afflictedCase = persoAfficted.persoCase;
            rightDirection = persoAfficted.persoDirection;
          }
        }
        BallonData ballonAfficted = objAfflicted.GetComponent<BallonData>();
        if (ballonAfficted != null)
        {
          afflictedCase = ballonAfficted.ballonCase;
          rightDirection = ballonAfficted.ballonDirection;
        }

        CaseData rightCase = afflictedCase.GetCaseAtRight(rightDirection);
        if (rightCase != null)
        {
          if (rightCase.personnageData != null)
          {
            if (persoAfficted.timeStunned == 0)
            {
              AfterFeedbackManager.Instance.PRText(1, rightCase.gameObject);
              rightCase.GetComponent<CaseData>().personnageData.actualPointResistance--;
            }
          }
          if (rightCase.summonData != null)
          {
            AfterFeedbackManager.Instance.PRText(1, rightCase.gameObject);
            rightCase.GetComponent<CaseData>().summonData.actualPointResistance--;
          }
        }
      }
      pushValue--;

      if (objAfflicted.GetComponent<PersoData>())
      {
        objAfflicted.GetComponent<PersoData>().pushedDebt = pushValue;
      }
    }
    GameManager.Instance.actualAction = PersoAction.isSelected;
    SelectionManager.Instance.selectedCase = persoSelected.persoCase;
    CaseManager.Instance.RemovePath();

    if (objectCollision == false && objAfflicted.GetComponent<PersoData>() != null)
    {
      if (pushValue > tempPath.Count)
      {
        if (objAfflicted.GetComponent<PersoData>().timeStunned == 0)
        {
          AfterFeedbackManager.Instance.PRText(1, objAfflicted);
          objAfflicted.GetComponent<PersoData>().actualPointResistance--;
        }
      }
    }

    retainedPushValue = pushValue;
    tempPath.Clear();

    StartCoroutine(TurnManager.Instance.EnableFinishTurn());
    yield return new WaitForEndOfFrame();
  }

  public void GetShownCase(GameObject obj, int givenPushValue, CaseData caseAfflicted, PushType pushType, Direction pushDirection = Direction.Front)
  {
    CaseData nextCase = caseAfflicted;
    int y = pushValue;
    caseFinalShow = null;
    if (pushType == PushType.FromCaster)
    { // tornade à implémenter
      while (y != 0)
      {
        if (y > 0)
        {
          y--;
          if (nextCase.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection) != null)
          {
            nextCase = nextCase.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection);
            if (nextCase.casePathfinding == PathfindingCase.NonWalkable)
            {
              caseFinalShow = nextCase.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection);
            }
          }
        }
        if (y < 0)
        {
          y++;
          if (nextCase.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection) != null)
          {
            nextCase = nextCase.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection);
            if (nextCase.casePathfinding == PathfindingCase.NonWalkable)
            {
              if (y == 1)
                caseFinalShow = caseAfflicted;
              else
                caseFinalShow = nextCase.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection);
              break;
            }
          }
        }
      }
      if (caseFinalShow == null)
      {
        caseFinalShow = nextCase;
      }
    }
    else if (pushType == PushType.FromTarget)
    {
      if (pushDirection == Direction.Left)
        caseFinalShow = nextCase.GetCaseAtLeft(SelectionManager.Instance.selectedPersonnage.persoDirection);

      if (pushDirection == Direction.Right)
        caseFinalShow = nextCase.GetCaseAtRight(SelectionManager.Instance.selectedPersonnage.persoDirection);

      if (pushDirection == Direction.Back)
      {
        caseFinalShow = nextCase.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection);
      }

      if (pushDirection == Direction.Front)
        caseFinalShow = nextCase.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection);

      pushDirection = SelectionManager.Instance.selectedPersonnage.persoDirection;

      for (int i = 0; i < pushValue; i++)
      {
        if (nextCase != null)
          if (nextCase.summonData != null)
            if (nextCase.summonData.name.Contains("Air"))
            {
              if (pushDirection == Direction.NordEst)
                pushDirection = Direction.NordOuest;
              if (pushDirection == Direction.NordOuest)
                pushDirection = Direction.SudOuest;
              if (pushDirection == Direction.SudOuest)
                pushDirection = Direction.SudEst;
              if (pushDirection == Direction.SudEst)
                pushDirection = Direction.NordEst;
            }
        /*
        if (pushDirection == Direction.Left)
          caseFinalShow = nextCase.GetCaseAtLeft(SelectionManager.Instance.selectedPersonnage.persoDirection);

        if (pushDirection == Direction.Right)
          caseFinalShow = nextCase.GetCaseAtRight(SelectionManager.Instance.selectedPersonnage.persoDirection);

        if (pushDirection == Direction.Back)
          caseFinalShow = nextCase.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection);

        if (pushDirection == Direction.Front)
          caseFinalShow = nextCase.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection);

        if (tempCase == null || tempCase.casePathfinding == PathfindingCase.NonWalkable)
        {
          caseAfflicted.casePathfinding = PathfindingCase.Walkable;
          pathList.Add(caseAfflicted.transform);
          stillTornadoDamage = true;
          break;
        }
        else
        {
          pathList.Add(tempCase.transform);
          caseAfflicted = tempCase;
        }*/
      }

      //GetCaseAtRight(objAfflicted.GetComponent<PersoData>().persoDirection);
    }

  }
}
