using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ResourceCharger : NetworkBehaviour
{
  public override void OnStartClient()
  {
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    while (!LoadingManager.Instance.isGameReady())
      yield return new WaitForEndOfFrame();
    Init();
  }

  void Init()
  {
    StartCoroutine(GoDestroy());
  }

  IEnumerator GoDestroy()
  {
    yield return new WaitForSeconds(3f);
    Destroy(this.gameObject);
  }
}
