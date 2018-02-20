using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DebugGetId : NetworkBehaviour
{

    private void Update()
    {
        GetComponent<Text>().text = RpcFunctions.Instance.playerControllerId.ToString();
    }
}
