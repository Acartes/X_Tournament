using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FeedbackManager : NetworkBehaviour
{

  public static FeedbackManager Instance;

  public List<GameObject> listTextFeedback;
  public List<GameObject> listTextFeedbackPredict;
  public List<GameObject> listPersoPredict;

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

    InitGame();
  }

  void InitGame()
  {
    listTextFeedback.AddRange(GameObject.FindGameObjectsWithTag("textFeedback"));
  }

  public void ShowInit(int randomInt, int maxInt, GameObject obj)
  {
    StartCoroutine(Show(randomInt, maxInt, obj));
  }

  IEnumerator Show(int randomInt, int maxInt, GameObject obj)
  {
    GameObject takenText = listTextFeedback[0];
    listTextFeedback.Remove(takenText);
    takenText.GetComponent<TextMesh>().text = randomInt + "/" + maxInt;
    takenText.transform.position = obj.transform.position;
    if (randomInt < maxInt)
      {
        takenText.GetComponent<TextMesh>().color = new Color(0, 1, 0, 1f);
      } else
      {
        takenText.GetComponent<TextMesh>().color = new Color(1, 0, 0, 1f);
      }

    for (int i = 0; i < 300; i++)
      {
        takenText.GetComponent<TextMesh>().color -= new Color(0, 0, 0, 0.0033f);
        takenText.transform.position += new Vector3(0, 0.01f, 0);
        yield return new WaitForEndOfFrame();
      }
    takenText.transform.position = new Vector3(999, 999, 999);
    listTextFeedback.Add(takenText);
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
