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

  List<Transform> path = new List<Transform>();

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
  public void PushEffect(GameObject obj, int pushValue, CaseData caseAfflicted, PushType pushType, Direction pushDirection = Direction.Front)
  {
    if (!isPushing)
      {
        isPushing = true;
        MoveBehaviour.Instance.StopAllCoroutines();
      }

    MoveBehaviour.Instance.movePathes.Clear();
    Direction persoDirection = obj.GetComponent<PersoData>().persoDirection;
    CaseData tempCase = null;

    switch (pushType)
      {
      case PushType.FromCaster:
        for (int i = 0; i < pushValue; i++)
          {
            if (caseAfflicted.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection) != null)
              {
                tempCase = caseAfflicted.GetCaseInFront(SelectionManager.Instance.selectedPersonnage.persoDirection);
                path.Add(tempCase.transform);
                caseAfflicted = tempCase;
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

            path.Add(tempCase.transform);
            caseAfflicted = tempCase;
          }
        break;
      }
   

    StartCoroutine(Deplacement(obj.GetComponent<PersoData>().originPoint.transform.localPosition, obj.GetComponent<PersoData>(), path));

    isPushing = false;
  }

  public IEnumerator Deplacement(Vector3 originPoint, PersoData persoAfflicted, List<Transform> pathes)
  { // On déplace le personnage de case en case jusqu'au click du joueur propriétaire, et entre temps on check s'il est taclé ou non
    TurnManager.Instance.DisableFinishTurn();

    GameManager.Instance.actualAction = PersoAction.isMoving;

    foreach (Transform path in pathes)
      {
        if (path.GetComponent<CaseData>().casePathfinding == PathfindingCase.NonWalkable)
          {
              
            break;
          }

        List<Transform> savePathes = pathes;

        Vector3 startPos = persoAfflicted.transform.position;
        float fracturedTime = 0;
        float timeUnit = travelTime / 60;

        while (persoAfflicted.transform.position != path.transform.position - originPoint)
          {
            fracturedTime += timeUnit + 0.01f;
            persoAfflicted.transform.position = Vector3.Lerp(startPos, path.transform.position - originPoint, fracturedTime);
            yield return new WaitForEndOfFrame();
          }
        SelectionManager.Instance.selectedCase = path.gameObject.GetComponent<CaseData>();
      }
    SelectionManager.Instance.selectedCase = pathes[pathes.Count - 1].gameObject.GetComponent<CaseData>();
    GameManager.Instance.actualAction = PersoAction.isSelected;
    CaseManager.Instance.RemovePath();

    SelectionManager.Instance.selectedCase.GetComponent<CaseData>().casePathfinding = PathfindingCase.NonWalkable;
    pathes.Clear();

    StartCoroutine(TurnManager.Instance.EnableFinishTurn());
    yield return new WaitForEndOfFrame();
  }

}
