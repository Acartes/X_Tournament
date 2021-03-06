﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleBehaviour : MonoBehaviour {

  // *************** //
  // ** Variables ** //
  // *************** //

  [Header("  Temps")]
  [Tooltip("Durée du va et vien entre le tacleur et le taclé")]
  public float tackleAnimTime;

  bool isTackling = false;

  [HideInInspector] public static TackleBehaviour Instance;

  // *************** //
  // ** Initialisation ** //
  // *************** //

    void Awake () {
    Instance = this;
  }

  // *************** //
  // ** Checkers ** //
  // *************** //

  public void CheckTackle (GameObject movingObj, PersoData shotingPersonnage = null) 
    { // Vérifie si le personnage peut être taclé, et si c'est le cas, fait un test de chance pour savoir s'il est taclé
      Transform path = SelectionManager.Instance.selectedCase.transform;
      Player currentPlayer = TurnManager.Instance.currentPlayer;
      float xCaseOffset = CaseManager.Instance.xCaseOffset;
      float yCaseOffset = CaseManager.Instance.yCaseOffset;

    foreach (PersoData obj in RosterManager.Instance.listHeroPlaced)
      {
          if (movingObj != null && obj != shotingPersonnage)
          {
              if (obj != movingObj && Fonction.Instance.CheckAdjacent(obj.gameObject, movingObj.gameObject) == true)
              {

                int randomInt = UnityEngine.Random.Range(0, 100);

                switch (movingObj.name)
                  {
                  case ("Ballon"):
                    path = movingObj.GetComponent<BallonData>().ballonCase.transform;
                        StartCoroutine(TackleEffect(obj, path, GraphManager.Instance.getCaseOffset(obj.gameObject)));
                        if (randomInt < 50)
                          {
                            Debug.Log("(Si inférieur à 51, il y a interception) " + randomInt + "/" + "100" + ": Interception SUCCESS");
                            GameManager.Instance.actualAction = PersoAction.isWaiting;

                            movingObj.GetComponent<BallonData>().ChangeStatut(BallonStatut.isIntercepted);
                          } 
                    break;
                  default:
                    if (obj.owner != currentPlayer)
                      {
                            StartCoroutine(TackleEffect(obj, path, GraphManager.Instance.getCaseOffset(obj.gameObject)));
                        if (movingObj.GetComponent<PersoData>().weightType == obj.weightType)
                          {
                            if (randomInt < 50)
                              {
                                Debug.Log("(Même poids) (Si inférieur à 51 il y a tackle) " + randomInt + "/" + "100" + ": Tackle SUCCESS");
                                movingObj.GetComponent<PersoData>().isTackled = true;
                              } else
                              {
                                Debug.Log("(Même poids) (Si inférieur à 51 il y a tackle) " + randomInt + "/" + "100" + ": Tackle FAILED");
                              }
                          } else
                          {

                            if (randomInt < 25)
                              {
                                Debug.Log("(Poids différents) (Si inférieur à 26 il y a tackle) " + randomInt + "/" + "100" + ": Tackle SUCCESS");
                                movingObj.GetComponent<PersoData>().isTackled = true;
                              } else
                              {
                                Debug.Log("(Poids différents) (Si inférieur à 26 il y a tackle) " + randomInt + "/" + "100" + ": Tackle FAILED");
                              }
                          }
                      }
                    break;
                  }
              }
          }
      }
}

  // *************** //
  // ** Tacle ** //
  // *************** //

    IEnumerator TackleEffect (PersoData punchingPersonnage, Transform path, float offsetY) 
    { // Effet visuel à chaque fois que le personnage se déplaçant se fait taclé
    if (punchingPersonnage.GetComponent<BoxCollider2D>().enabled != false)
      {

        punchingPersonnage.GetComponent<BoxCollider2D>().enabled = false;

        Vector3 startPos = punchingPersonnage.transform.position;

        float fracturedTime = 0;
        float timeUnit = tackleAnimTime / 60;

        while (fracturedTime < 2)
          {
            fracturedTime += timeUnit + 0.01f;
            punchingPersonnage.transform.position = Vector3.Lerp(startPos, path.position + new Vector3(0, offsetY, 0), fracturedTime);
            yield return new WaitForEndOfFrame();
          }

        fracturedTime = 0;

        while (fracturedTime < 2)
          {
            fracturedTime += timeUnit + 0.01f;
            punchingPersonnage.transform.position = Vector3.Lerp(path.position + new Vector3(0, offsetY, 0), startPos, fracturedTime);
            yield return new WaitForEndOfFrame();
          }
        punchingPersonnage.GetComponent<BoxCollider2D>().enabled = true;
      }
    }
}