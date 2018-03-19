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

  public bool pushAtRange;
  public int pushValue;
  public int pushRange;
  public Direction pushDirection;

  public Transform originPoint;

  public bool disapearWhenHurt;

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

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //



}
