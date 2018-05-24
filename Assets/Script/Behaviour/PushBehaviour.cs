using System.Collections;
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
  public List<IEnumerator> ienumeratorList = new List<IEnumerator>();

  PersoData persoAfflicted = null;
  public CaseData caseFinalShow = null; // la case à montrer pour le pré-rendu
  GameObject objAfflicted;

  public int pathRestant = 0;

  int pushValue;
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
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  /// <summary>Check et applique l'effet de poussée.</summary>
  public void PushCheck(GameObject obj, int givenPushValue, CaseData caseAfflicted, PushType pushType, Direction pushDirection = Direction.Front)
  {
    Direction persoDirection = Direction.None;
    int caseNumberRestant = 0;

    pushValue = givenPushValue;

    caseNumberRestant = pathRestant;

    if (obj.GetComponent<BallonData>() != null)
    {
      if (obj.GetComponent<BallonData>().isMoving)
      {
        caseNumberRestant = SelectionManager.Instance.selectedPersonnage.shotStrenght - obj.GetComponent<BallonData>().casesCrossed;
        obj.GetComponent<BallonData>().StopMove();
        obj.GetComponent<BallonData>().StopAllCoroutines();
      }
      persoDirection = obj.GetComponent<BallonData>().ballonDirection;
    }

    objAfflicted = obj;
    if (objAfflicted.GetComponent<PersoData>() != null)
    {
      caseNumberRestant = pathRestant;
      foreach (Transform path in MoveBehaviour.Instance.movePathes)
      {
        path.GetComponent<CaseData>().ChangeStatut(Statut.None, Statut.isMoving);
      }
      MoveBehaviour.Instance.movePathes.Clear();
      persoAfflicted = objAfflicted.GetComponent<PersoData>();
      persoDirection = persoAfflicted.persoDirection;
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
        GetShownCase(obj, givenPushValue, caseAfflicted, pushType, pushDirection);
        int y = pushValue;
        while (y != 0)
        {
          if (y > 0)
          {
            y--;
            if (caseAfflicted.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection) != null)
            {
              tempCase = caseAfflicted.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection); ;
              pathList.Add(tempCase.transform);
              caseAfflicted = tempCase;
            }
          }
          if (y < 0)
          {
            y++;
            if (caseAfflicted.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection) != null)
            {
              tempCase = caseAfflicted.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection);
              pathList.Add(tempCase.transform);
              caseAfflicted = tempCase;
            }
          }
        }
        break;
      case PushType.FromTarget:
        GetShownCase(obj, givenPushValue, caseAfflicted, pushType, pushDirection);
        stillTornadoDamage = false;
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
          }
        }

        if (caseNumberRestant != 0)
        {

          for (int i = 0; i < caseNumberRestant - 1; i++)
          {
            if (pushDirection == Direction.Left)
              tempCase = pathList[i].GetComponent<CaseData>().GetCaseAtLeft(persoDirection);

            if (pushDirection == Direction.Right)
              tempCase = pathList[i].GetComponent<CaseData>().GetCaseAtRight(persoDirection);

            if (pushDirection == Direction.Back)
              tempCase = pathList[i].GetComponent<CaseData>().GetCaseAtBack(persoDirection);

            if (pushDirection == Direction.Front)
              tempCase = pathList[i].GetComponent<CaseData>().GetCaseInFront(persoDirection);

            if (tempCase == null || tempCase.casePathfinding == PathfindingCase.NonWalkable)
            {
              break;
            }
            pathList.Add(tempCase.transform);
            caseAfflicted = tempCase;
          }
        }
        break;
      case PushType.FromTerrain:
        if (pushDirection == Direction.SudOuest)
          tempCase = caseAfflicted.GetBottomLeftCase();

        if (pushDirection == Direction.SudEst)
          tempCase = caseAfflicted.GetBottomRightCase();

        if (pushDirection == Direction.NordOuest)
          tempCase = caseAfflicted.GetTopLeftCase();

        if (pushDirection == Direction.NordEst)
          tempCase = caseAfflicted.GetTopRightCase();
        if (tempCase == null || tempCase.casePathfinding == PathfindingCase.NonWalkable)
        {
          break;
        }
        pathList.Add(tempCase.transform);
        caseAfflicted = tempCase;
        break;

    }

    foreach (CaseData caseObj in CaseManager.Instance.GetAllCaseWithStatut(Statut.atPush))
    {
      caseObj.GetComponent<CaseData>().ChangeStatut(Statut.None, Statut.atPush);
    }

    if (pathList.Count == 0)
      return;
  }

  public void PushStart()
  {
    MoveBehaviour.Instance.StopAllCoroutines();
    StopAllCoroutines();

    ienumeratorList.Add(Deplacement(objAfflicted, pathList));
    StartCoroutine(ienumeratorList[ienumeratorList.Count - 1]);
  }

  public IEnumerator Deplacement(GameObject objAfflicted, List<Transform> pathes)
  { // On déplace le personnage de case en case jusqu'au click du joueur propriétaire, et entre temps on check s'il est taclé ou non
    TurnManager.Instance.DisableFinishTurn();
    IEnumerator thisFunc = ienumeratorList[ienumeratorList.Count - 1];

    GameManager.Instance.actualAction = PersoAction.isMoving;
    Transform lastPath = null;

    Vector3 originPoint = Vector3.zero;

    if (objAfflicted.GetComponent<PersoData>() != null)
    {
      originPoint = objAfflicted.GetComponent<PersoData>().originPoint.transform.localPosition;
    }

    if (objAfflicted.GetComponent<BallonData>() != null)
    {
      originPoint = objAfflicted.GetComponent<BallonData>().offsetBallon;
    }

    if (objAfflicted.GetComponent<SummonData>() != null)
    {
      originPoint = objAfflicted.GetComponent<SummonData>().originPoint.transform.localPosition;
    }

    pathRestant = pathes.Count;

    bool objectCollision = false;

    foreach (Transform path in pathes)
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
          AfterFeedbackManager.Instance.PRText(1, path.gameObject);
          path.GetComponent<CaseData>().personnageData.actualPointResistance--;
        }

        if (objAfflicted.GetComponent<PersoData>() != null)
        {
          AfterFeedbackManager.Instance.PRText(1, objAfflicted);
          objAfflicted.GetComponent<PersoData>().actualPointResistance--;
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

      while (objAfflicted.transform.position != path.transform.position - originPoint)
      {
        fracturedTime += timeUnit + 0.01f;
        objAfflicted.transform.position = Vector3.Lerp(startPos, path.transform.position - originPoint, fracturedTime);
        yield return new WaitForEndOfFrame();
      }
      if (stillTornadoDamage)
      {
        AfterFeedbackManager.Instance.PRText(1, objAfflicted);
        objAfflicted.GetComponent<PersoData>().actualPointResistance--;
        CaseData afflictedCase = objAfflicted.GetComponent<PersoData>().persoCase;
        CaseData rightCase = afflictedCase.GetCaseAtRight(objAfflicted.GetComponent<PersoData>().persoDirection);
        if (rightCase != null)
        {
          if (rightCase.personnageData != null)
          {
            Debug.Log(rightCase.personnageData.name);
            Debug.Break();
            AfterFeedbackManager.Instance.PRText(1, rightCase.gameObject);
            rightCase.GetComponent<CaseData>().personnageData.actualPointResistance--;
          }
          if (rightCase.summonData != null)
          {
            AfterFeedbackManager.Instance.PRText(1, rightCase.gameObject);
            rightCase.GetComponent<CaseData>().summonData.actualPointResistance--;
          }
        }
      }
      pathRestant--;
    }
    GameManager.Instance.actualAction = PersoAction.isSelected;
    SelectionManager.Instance.selectedCase = SelectionManager.Instance.selectedPersonnage.persoCase;
    CaseManager.Instance.RemovePath();

    if (objectCollision == false && objAfflicted.GetComponent<PersoData>() != null)
    {
      if (pushValue > pathes.Count)
      {
        AfterFeedbackManager.Instance.PRText(1, objAfflicted);
        objAfflicted.GetComponent<PersoData>().actualPointResistance--;
      }
    }
    if (lastPath != null)
      lastPath.GetComponent<CaseData>().casePathfinding = PathfindingCase.NonWalkable;

    pathes.Clear();
    pathRestant = 0;

    StartCoroutine(TurnManager.Instance.EnableFinishTurn());
    ienumeratorList.Remove(thisFunc);
    yield return new WaitForEndOfFrame();
  }

  public void GetShownCase(GameObject obj, int givenPushValue, CaseData caseAfflicted, PushType pushType, Direction pushDirection = Direction.Front)
  {
    Debug.Log(caseAfflicted);
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
        caseFinalShow = nextCase.GetCaseAtBack(SelectionManager.Instance.selectedPersonnage.persoDirection);

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
