using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal.Commands;

/// <summary>Gère les effets de chaque cases.</summary>
public class EffectManager : NetworkBehaviour
{
  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public static EffectManager Instance;

  bool isPushing = false;
  bool oneTime = true;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log("EffectManager is Instanced");
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    while (!LoadingManager.Instance.isGameReady())
      yield return new WaitForEndOfFrame();

  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  /// <summary>Check toutes les fonctions de cette classe.</summary>
  public void CheckAllEffect(CaseData newCase, GameObject obj)
  {
    PushCheck(newCase, obj);
  }

  public void PushCheck(CaseData newCase, GameObject obj)
  {
    if ((Statut.willPush & newCase.statut) == Statut.willPush)
      {
        if (obj.GetComponent<PersoData>() != null)
          {
            PushEffectToLeft(newCase, obj);
          }
      }
  }

  /// <summary>Check et applique l'effet de poussée.</summary>
  public void PushEffectToLeft(CaseData newCase, GameObject obj)
  {
    if (!isPushing)
      {
        isPushing = true;
        MoveBehaviour.Instance.StopAllCoroutines();
      }
    //  obj.GetComponent<PersoData>().isTackled = true;
            
    MoveBehaviour.Instance.movePathes.Clear();
    Direction persoDirection = obj.GetComponent<PersoData>().persoDirection;
    CaseData tempCase = newCase.GetCaseAtLeft(persoDirection);
    if (tempCase.personnageData != null && oneTime)
      {    
        PushEffect(tempCase, tempCase.personnageData.gameObject, persoDirection);
      }
    List<Transform> path = new List<Transform>();
    path.Add(tempCase.transform);
    StartCoroutine(MoveBehaviour.Instance.Deplacement(obj.GetComponent<PersoData>().originPoint.transform.localPosition, obj.GetComponent<PersoData>(), path));
    obj.GetComponent<PersoData>().isTackled = true;

    if (obj.GetComponent<BallonData>() != null)
      {
        obj.GetComponent<BallonData>().statut += (int)BallonStatut.isIntercepted;
      }
    isPushing = false;
  }

  public void PushEffect(CaseData newCase, GameObject obj, Direction persoDirection)
  {
    CaseData tempCase = newCase.GetTopLeftCase();
    if (tempCase.personnageData != null)
      {
        //PushEffect(tempCase, tempCase.personnageData.gameObject, persoDirection);
      }
    List<Transform> path = new List<Transform>();
    path.Add(tempCase.transform);
    
    StartCoroutine(MoveBehaviour.Instance.Deplacement(obj.GetComponent<PersoData>().originPoint.transform.localPosition, obj.GetComponent<PersoData>(), path));
  }
}
