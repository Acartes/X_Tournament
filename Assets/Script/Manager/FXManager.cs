using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework;
using UnityEngine.Timeline;

/// <summary>Gère tous les sorts.</summary>
public class FXManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  /// <summary>Montre à quelle portée les personnages vont être projetés avant de le lancer</summary>

  public List<GameObject> listFX;

  public static FXManager Instance;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log(this.GetType() + " is Instanced");
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
    listFX.AddRange(GameObject.FindGameObjectsWithTag("fxFeedback"));
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.A))
      {
        foreach (GameObject takenFX in GameObject.FindGameObjectsWithTag("fxFeedback"))
          {
            Debug.Log("AAA");
            takenFX.GetComponent<Animation>().playAutomatically = true;

          }
      }
  }

  public void Show(AnimationClip clipToPlay, Transform newPos)
  {
    GameObject takenFX = listFX[0];
    listFX.Remove(takenFX);

    takenFX.transform.position = newPos.localPosition;
    takenFX.GetComponent<Animation>().clip = clipToPlay;
    //takenFX.GetComponent<>().
    StartCoroutine(BackToList(clipToPlay.length, takenFX));
  }

  IEnumerator BackToList(float lenght, GameObject takenFX)
  {
    yield return new WaitForSeconds(lenght);
    //  takenFX.transform.position = new Vector3(999, 999, 999);
    //  listFX.Add(takenFX);
  }
}
