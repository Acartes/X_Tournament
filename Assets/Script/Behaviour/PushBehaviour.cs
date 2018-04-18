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

  List<Transform> path = new List<Transform>();

  PersoData persoAfflicted = null;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log(this.GetType() + " is Instanced");
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  /// <summary>Check et applique l'effet de poussée.</summary>
  public void PushCheck(GameObject obj, int pushValue, CaseData caseAfflicted, PushType pushType, Direction pushDirection = Direction.Front)
  {
    MoveBehaviour.Instance.movePathes.Clear();
    persoAfflicted = obj.GetComponent<PersoData>();
    Direction persoDirection = persoAfflicted.persoDirection;
    
    CaseData tempCase = null;

    path.Clear();

    switch (pushType)
      {
      case PushType.FromCaster:
        int y = pushValue;
        while (y != 0)
          {
            if (y > 0)
              {
                y--;
                if (caseAfflicted.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection) != null)
                  {
                    tempCase = caseAfflicted.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection);
                    path.Add(tempCase.transform);
                    caseAfflicted = tempCase;
                  }
              }
            if (y < 0)
              {
                y++;
                if (caseAfflicted.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection) != null)
                  {
                    tempCase = caseAfflicted.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection);
                    path.Add(tempCase.transform);
                    caseAfflicted = tempCase;
                  }
              }
          }
        break;
      case PushType.FromTarget:
        for (int i = 0; i < pushValue; i++)
          {
            if (pushDirection == Direction.Left)
              tempCase = caseAfflicted.GetCaseAtLeft(persoDirection);

            if (pushDirection == Direction.Right)
              tempCase = caseAfflicted.GetCaseAtRight(persoDirection);

            if (pushDirection == Direction.Back)
              tempCase = caseAfflicted.GetCaseAtBack(persoDirection);

            if (pushDirection == Direction.Front)
              tempCase = caseAfflicted.GetCaseInFront(persoDirection);

            if (tempCase == null)
              break;

            if (tempCase.casePathfinding == PathfindingCase.NonWalkable)
              break;
            path.Add(tempCase.transform);
            caseAfflicted = tempCase;
          }
        break;
      }

    foreach (CaseData caseObj in CaseManager.Instance.GetAllCaseWithStatut(Statut.atPush))
      {
        caseObj.GetComponent<CaseData>().ChangeStatut(Statut.None, Statut.atPush);
      }

    if (path.Count == 0)
      return;
   
    foreach (Transform pathObj in path)
      {
        pathObj.GetComponent<CaseData>().ChangeStatut(Statut.atPush);
      }

    
  }

  public void PushStart()
  {
    MoveBehaviour.Instance.StopAllCoroutines();
    StopAllCoroutines();

    if (path.Count == 0)
      return;
      
    StartCoroutine(Deplacement(persoAfflicted.originPoint.transform.localPosition, persoAfflicted, path));
  }

  public IEnumerator Deplacement(Vector3 originPoint, PersoData persoAfflicted, List<Transform> pathes)
  { // On déplace le personnage de case en case jusqu'au click du joueur propriétaire, et entre temps on check s'il est taclé ou non
    TurnManager.Instance.DisableFinishTurn();

    GameManager.Instance.actualAction = PersoAction.isMoving;
    Transform lastPath = null;
    foreach (Transform path in pathes)
      {
       
        if (path.GetComponent<CaseData>().casePathfinding == PathfindingCase.NonWalkable)
          {
              
            break;
          }
        lastPath = path;
        List<Transform> savePathes = pathes;

        Vector3 startPos = persoAfflicted.transform.position;
        float fracturedTime = 0;
        float timeUnit = travelTime / 60;
 
        persoAfflicted.RotateTowardsReversed(path.gameObject);

        while (persoAfflicted.transform.position != path.transform.position - originPoint)
          {
            fracturedTime += timeUnit + 0.01f;
            persoAfflicted.transform.position = Vector3.Lerp(startPos, path.transform.position - originPoint, fracturedTime);
            yield return new WaitForEndOfFrame();
          }
      }
    GameManager.Instance.actualAction = PersoAction.isSelected;
    CaseManager.Instance.RemovePath();

    if (lastPath != null)
      lastPath.GetComponent<CaseData>().casePathfinding = PathfindingCase.NonWalkable;
      
    pathes.Clear();

    StartCoroutine(TurnManager.Instance.EnableFinishTurn());
    yield return new WaitForEndOfFrame();
  }

}
