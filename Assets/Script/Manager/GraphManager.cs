using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour {
  
  // *************** //
  // ** Variables ** //
  // *************** //

  public static GraphManager Instance;

  // *************** //
  // ** Initialisation ** //
  // *************** //

    void Awake () {
    Instance = this;
  }

    public float getCaseOffset(GameObject go)
    {
        return go.GetComponent<SpriteRenderer>().bounds.size.y / 2.3f;
    }
}
