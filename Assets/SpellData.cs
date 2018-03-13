using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>Tout ce qu'il est possible de faire avec un sort, ainsi que toutes ses données.</summary>
public class SpellData : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public int costPA;

  public int range;
  public bool isLinear;

  public int areaOfEffect;

  public int damagePR;
  public int damagePA;
  public int damagePM;
  public Element elementCreated;

  public int pushValue;
  public bool hurtWhenStopped;

  public GameObject summonedObj;
  public bool isDirect;

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
