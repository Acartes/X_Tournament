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
    PushBehaviour.Instance.PushCheck(objAfflicted, pushValue, caseAfflicted, pushType, pushDirection);
    PushBehaviour.Instance.MultiplePushStart();
  }

  /// <summary>Augmente ou diminue le nombre de PA de la cible.</summary>
  public void ChangePA(int number)
  {
    GameManager.Instance.manaGlobalActual += number;
    GameManager.Instance.manaGlobalActual = Mathf.Clamp(GameManager.Instance.manaGlobalActual, 0, GameManager.Instance.manaGlobalMax); // on peut pas dépasser le max
  }

  public void ChangePR(PersoData persoAfflicted, int number)
  {
    persoAfflicted.actualPointResistance += number;
    persoAfflicted.actualPointResistance = Mathf.Clamp(persoAfflicted.actualPointResistance, 0, persoAfflicted.maxPointResistance); // on peut pas dépasser le max
  }

  public void ChangePM(PersoData persoAfflicted, int number)
  {
    persoAfflicted.actualPointMovement += number;
    persoAfflicted.actualPointMovement = Mathf.Clamp(persoAfflicted.actualPointMovement, 0, persoAfflicted.maxPointMovement); // on peut pas dépasser le max
  }

  public void ChangePADebuff(int number)
  {
    GameManager.Instance.paDebuff += number;
  }

  public void ChangePRDebuff(PersoData persoAfflicted, int number)
  {
    persoAfflicted.prDebuff += number;
  }

  public void ChangePMDebuff(PersoData persoAfflicted, int number)
  {
    persoAfflicted.pmDebuff += number;
  }

  public void ChangePR(SummonData summonAfflicted, int number)
  {
    summonAfflicted.actualPointResistance += number;
  }
}
