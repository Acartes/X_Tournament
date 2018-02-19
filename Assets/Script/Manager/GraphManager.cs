using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GraphManager : NetworkBehaviour
{

    // *************** //
    // ** Variables ** //
    // *************** //

    public static GraphManager Instance;

    // *************** //
    // ** Initialisation ** //
    // *************** //

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("GraphManager is Instanced");
    }

    public float getCaseOffset(GameObject go)
    {
        return go.GetComponent<SpriteRenderer>().bounds.size.y / 2.3f;
    }
}
