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
  public Animator explodeEffect;

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
    takenText.transform.position = obj.transform.position;
    if (randomInt < maxInt)
      {
        takenText.GetComponent<TextMesh>().text = "Tackled!";
        takenText.GetComponent<TextMesh>().color = new Color(0, .4f, 0, 1f);
      } else
      {
        takenText.GetComponent<TextMesh>().text = "Miss!";
        takenText.GetComponent<TextMesh>().color = new Color(.8f, 0, 0, 1f);
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

  public void PRText(int PRchanged, GameObject obj, bool positiveValue = false)
  {
    if (obj.GetComponent<PersoData>() && obj.GetComponent<PersoData>().timeStunned > 0)
    {
      return;
    }
    StartCoroutine(PRTextCoroutine(PRchanged, obj, positiveValue));
  }

  IEnumerator PRTextCoroutine(int PRchanged, GameObject obj, bool positiveValue)
  {
    GameObject takenText = listTextFeedback[0];
    listTextFeedback.Remove(takenText);
    if (positiveValue)
      {
        takenText.GetComponent<TextMesh>().color = new Color(1, 0, 0, 1f);
        takenText.GetComponent<TextMesh>().text = "+" + Mathf.Abs(PRchanged).ToString() + "PR";
      } else
      {
        takenText.GetComponent<TextMesh>().color = new Color(1, 0, 0, 1f);
        takenText.GetComponent<TextMesh>().text = "-" + PRchanged.ToString() + "PR";
      }

    takenText.transform.position = obj.transform.position;


    for (int i = 0; i < 300; i++)
      {
        takenText.GetComponent<TextMesh>().color -= new Color(0, 0, 0, 0.0033f);
        takenText.transform.position += new Vector3(0, 0.01f, 0);
        yield return new WaitForEndOfFrame();
      }
    takenText.transform.position = new Vector3(999, 999, 999);
    listTextFeedback.Add(takenText);
  }

  public void ExplodeEffect(GameObject target)
  {
    explodeEffect.transform.position = target.transform.position + Vector3.up * 0.5f;
    explodeEffect.SetTrigger("BOOM");
  }
}
