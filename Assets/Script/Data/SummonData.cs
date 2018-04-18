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

  public int damagePR;
  public int damagePA;
  public int damagePM;
  public bool reverseDamageOnAlly;

  public Player owner;

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

  /// <summary>Vérifie si l'invocation est censé être toujours vivant ou pas.</summary>
  public void CheckDeath()
  {
    if (actualPointResistance <= 0 && !invulnerable)
      Destroy(this.gameObject);

    if (numberEffectDisapear <= 0)
      Destroy(this.gameObject);
  }

  /// <summary>Applique les effets selon les paramètres de l'invocation.</summary>
  public void ApplyEffect(PersoData persoAfflected)
  {
    if (canPush)
      {
        EffectManager.Instance.Push(persoAfflected, pushValue, pushType, pushDirection);
      }
    if (damagePA != 0)
      {
        if (reverseDamageOnAlly && persoAfflected.owner == owner)
          {
            damagePA = -damagePA;
          }
        EffectManager.Instance.ChangePA(persoAfflected, damagePA);
      }
    if (damagePR != 0)
      {
        if (reverseDamageOnAlly && persoAfflected.owner == owner)
          {
            damagePR = -damagePR;
          }
        EffectManager.Instance.ChangePR(persoAfflected, damagePR);
      }
    if (damagePM != 0)
      {
        if (reverseDamageOnAlly && persoAfflected.owner == owner)
          {
            damagePM = -damagePM;
          }
        EffectManager.Instance.ChangePM(persoAfflected, damagePM);
      }
    numberEffectDisapear--;
  }
}
