using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour {
  
  // *************** //
  // ** Variables ** //
  // *************** //

  public float offsetY;

  public static GraphManager Instance;

  // *************** //
  // ** Initialisation ** //
  // *************** //

    void Awake () {
    Instance = this;
  }
}
