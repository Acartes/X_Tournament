using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AfterFeedbackManager : NetworkBehaviour
{

  public static AfterFeedbackManager Instance;

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
    listTextFeedback.AddRange(GameObject.FindGameObjectsWithTag("textAfterFeedback"));
  }

  public void TackleText(int randomInt, int maxInt, GameObject obj)
  {
    StartCoroutine(TackleTextCoroutine(randomInt, maxInt, obj));
  }

  IEnumerator TackleTextCoroutine(int randomInt, int maxInt, GameObject obj)
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

  public void PRText(int PRchanged, GameObject obj)
  {
    StartCoroutine(PRTextCoroutine(PRchanged, obj));
  }

  IEnumerator PRTextCoroutine(int PRchanged, GameObject obj)
  {
    Debug.Log(obj.name);
    GameObject takenText = listTextFeedback[0];
    listTextFeedback.Remove(takenText);
    takenText.GetComponent<TextMesh>().text = "-" + PRchanged.ToString();
    takenText.transform.position = obj.transform.position;

    takenText.GetComponent<TextMesh>().color = new Color(1, 0, 0, 1f);

    for (int i = 0; i < 300; i++)
      {
        takenText.GetComponent<TextMesh>().color -= new Color(0, 0, 0, 0.0033f);
        takenText.transform.position += new Vector3(0, 0.01f, 0);
        yield return new WaitForEndOfFrame();
      }
    takenText.transform.position = new Vector3(999, 999, 999);
    listTextFeedback.Add(takenText);
  }
}
