﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BeforeFeedbackManager : NetworkBehaviour
{

  public static BeforeFeedbackManager Instance;

  public List<GameObject> listTextFeedback;
  public List<GameObject> listTextFeedbackPredict;
  public List<GameObject> listPersoPredict;

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    while (!LoadingManager.Instance.isGameReady())
      yield return new WaitForEndOfFrame();

    InitGame();
  }

  void InitGame()
  {
    listTextFeedback.AddRange(GameObject.FindGameObjectsWithTag("textBeforeFeedback"));
  }

  public void PredictInit(int maxInt, GameObject obj)
  {

    if (listPersoPredict.Count != 0)
      {
        foreach (GameObject similarObj in listPersoPredict)
          {
            if (similarObj == obj)
              return;

          }
      }
    if (listTextFeedback.Count == 0)
      return;

    GameObject takenText = listTextFeedback[0];
    takenText.name = obj.name + " " + "feedback";
    listTextFeedback.Remove(takenText);
    listTextFeedbackPredict.Add(takenText);
    listPersoPredict.Add(obj);
    takenText.GetComponent<TextMesh>().text = maxInt + "%";
    takenText.transform.position = obj.transform.position;
    takenText.GetComponent<TextMesh>().color = new Color(1, 1, 0, 1f);
  }

  public void PredictEnd(GameObject obj)
  {
    if (GameObject.Find(obj.name + " " + "feedback") == null)
      return;

    listPersoPredict.Remove(obj);
    GameObject objText = GameObject.Find(obj.name + " " + "feedback");
    objText.name = "textFeedback";
    listTextFeedback.Add(objText);
    objText.transform.position = new Vector3(999, 999, 999);
  }
}
