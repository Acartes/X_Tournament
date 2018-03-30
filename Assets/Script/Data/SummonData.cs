using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>Tout ce qu'il est possible de faire avec une invocation, ainsi que toutes ses données.</summary>
public class SummonData : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public bool isStatic;

  public bool isBlockingLineOfSight;

  public bool isTraversable;

  /// <summary>Push (positif) ou pull(negatif)</summary>
  public bool canPush;
  public int pushValue;
  public int pushRange;
  public PushType pushType;
  public Direction pushDirection;

  public Transform originPoint;

  public CaseData caseActual;

  public int actualPointResistance;
  public int maxPointResistance;
  public bool invulnerable;
  public bool hurtWhenBounce;
  public int numberEffectDisapear;

  public int limitInvoc;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  void Start()
  { 
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    yield return new WaitForEndOfFrame();
    while (TurnManager.Instance == null)
      {
        yield return null;
      }
    Init();
  }

  public void Init()
  {

  }

  void Update()
  {
    CheckDeath();
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  public void CheckDeath()
  {
    if (actualPointResistance <= 0 && !invulnerable)
      Destroy(this.gameObject);

    if (numberEffectDisapear <= 0)
      Destroy(this.gameObject);
  }

  public void ApplyEffect(PersoData persoAfflected)
  {
    if (canPush)
      {
        EffectManager.Instance.Push(persoAfflected, pushValue, pushType, pushDirection);
        numberEffectDisapear--;
      }
  }
}
