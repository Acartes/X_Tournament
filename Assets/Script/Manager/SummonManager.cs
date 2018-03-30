using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SummonManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public static SummonManager Instance;

  public SummonData lastSummonInstancied;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log(this.GetType() + " is Instanced");
  }
}
