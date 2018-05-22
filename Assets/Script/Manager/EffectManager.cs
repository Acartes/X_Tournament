﻿using System.Collections;
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
PushBehaviour.Instance.StopAllCoroutines();
    PushBehaviour.Instance.PushCheck(objAfflicted, pushValue, caseAfflicted, pushType, pushDirection);
    PushBehaviour.Instance.PushStart();
  }

  /// <summary>Augmente ou diminue le nombre de PA de la cible.</summary>
  public void ChangePA(int number)
  {
    GameManager.Instance.manaGlobalActual += number;
  }

  public void ChangePR(PersoData persoAfflicted, int number)
  {
    persoAfflicted.actualPointResistance += number;
  }

  public void ChangePM(PersoData persoAfflicted, int number)
  {
    persoAfflicted.actualPointMovement += number;
  }

  public void ChangePADebuff(PersoData persoAfflicted, int number)
  {
    persoAfflicted.paDebuff += number;
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
