using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
  public void Push(GameObject objAfflicted, CaseData caseAfflicted, int pushValue, PushType pushType, Direction pushDirection = Direction.Front)
  {
    StartCoroutine(PushDelayed(objAfflicted, caseAfflicted, pushValue, pushType, pushDirection));
  }

  IEnumerator PushDelayed(GameObject objAfflicted, CaseData caseAfflicted, int pushValue, PushType pushType, Direction pushDirection = Direction.Front)
  {
    if (objAfflicted.GetComponent<PersoData>())
      yield return new WaitForSeconds(0.05f);
    if (PushBehaviour.Instance.ienumeratorList.Count != 0)
    {
      PushBehaviour.Instance.StopCoroutine(PushBehaviour.Instance.ienumeratorList[0]);
      Debug.Log(PushBehaviour.Instance.ienumeratorList[0].ToString());
      PushBehaviour.Instance.ienumeratorList.Remove(PushBehaviour.Instance.ienumeratorList[0]);
    }
    if (MoveBehaviour.Instance.ienumeratorList.Count != 0)
    {
      MoveBehaviour.Instance.StopCoroutine(MoveBehaviour.Instance.ienumeratorList[0]);
      Debug.Log(MoveBehaviour.Instance.ienumeratorList[0].ToString());
      MoveBehaviour.Instance.ienumeratorList.Remove(MoveBehaviour.Instance.ienumeratorList[0]);
    }

    PushBehaviour.Instance.PushCheck(objAfflicted, pushValue, caseAfflicted, pushType, pushDirection);
    PushBehaviour.Instance.MultiplePushStart();

  }

  /// <summary>Un push multiple. Ne marche pas avec la tornade.</summary>
  public void MultiplePush(GameObject objAfflicted, CaseData caseAfflicted, int pushValue, PushType pushType, Direction pushDirection = Direction.Front)
  {
    StartCoroutine(MultiplePushDelayed(objAfflicted, caseAfflicted, pushValue, pushType, pushDirection));
  }

  IEnumerator MultiplePushDelayed(GameObject objAfflicted, CaseData caseAfflicted, int pushValue, PushType pushType, Direction pushDirection = Direction.Front)
  {
    if (objAfflicted.GetComponent<PersoData>())
      yield return new WaitForSeconds(0.10f);

    if (objAfflicted.GetComponent<BallonData>() && objAfflicted.GetComponent<BallonData>().isPushed == false)
    {
      yield return new WaitForSeconds(0.12f);
    }
    if (objAfflicted.GetComponent<BallonData>() && objAfflicted.GetComponent<BallonData>().isPushed == true)
    {
      yield return new WaitForSeconds(0.02f);

      if (PushBehaviour.Instance.ienumeratorList.Count != 0)
      {
        PushBehaviour.Instance.StopCoroutine(PushBehaviour.Instance.ienumeratorList[0]);
        Debug.Log(PushBehaviour.Instance.ienumeratorList[0].ToString());
        PushBehaviour.Instance.ienumeratorList.Remove(PushBehaviour.Instance.ienumeratorList[0]);
      }
      if (MoveBehaviour.Instance.ienumeratorList.Count != 0)
      {
        MoveBehaviour.Instance.StopCoroutine(MoveBehaviour.Instance.ienumeratorList[0]);
        Debug.Log(MoveBehaviour.Instance.ienumeratorList[0].ToString());
        MoveBehaviour.Instance.ienumeratorList.Remove(MoveBehaviour.Instance.ienumeratorList[0]);
      }
    }


    PushBehaviour.Instance.PushCheck(objAfflicted, pushValue, caseAfflicted, pushType, pushDirection);
    PushBehaviour.Instance.MultiplePushStart();
  }


  /// <summary>Augmente ou diminue le nombre de PA de la cible.</summary>
  public void ChangePA(int number)
  {
    GameManager.Instance.manaGlobalActual += number;
    if(number < 0)
    GameManager.Instance.manaGlobalActual = Mathf.Clamp(GameManager.Instance.manaGlobalActual, 0, GameManager.Instance.manaGlobalMax); // on peut pas dépasser le max
  }

  public void ChangePr(PersoData persoAfflicted, int number)
  {
    persoAfflicted.actualPointResistance += number;
    persoAfflicted.actualPointResistance = Mathf.Clamp(persoAfflicted.actualPointResistance, 0, persoAfflicted.maxPointResistance); // on peut pas dépasser le max
    InfoPerso.Instance.stats.updatePr(persoAfflicted.actualPointResistance);

  }

  public void ChangePm(PersoData persoAfflicted, int number)
  {
    persoAfflicted.actualPointMovement += number;
    persoAfflicted.actualPointMovement = Mathf.Clamp(persoAfflicted.actualPointMovement, 0, persoAfflicted.maxPointMovement); // on peut pas dépasser le max
    InfoPerso.Instance.stats.updatePm(persoAfflicted.actualPointMovement);
  }

  public void ChangePADebuff(int number)
  {
    GameManager.Instance.paDebuff += number;
  }

  public void ChangePmDebuff(PersoData persoAfflicted, int number)
  {
    persoAfflicted.pmDebuff += number;
  }

  public void ChangePr(SummonData summonAfflicted, int number)
  {
    summonAfflicted.actualPointResistance += number;
  }

  public void Rotate(GameObject objAfflicted, Direction newDirection)
  {
    Debug.Log(objAfflicted.GetComponent<SummonData>());
    Debug.Log(newDirection);
    if (objAfflicted.GetComponent<SummonData>())
      objAfflicted.GetComponent<SummonData>().pushDirection = newDirection;
  }
}
