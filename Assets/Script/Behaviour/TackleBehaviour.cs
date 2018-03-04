using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TackleBehaviour : NetworkBehaviour
{

  // *************** //
  // ** Variables ** //
  // *************** //

  [Header("  Temps")]
  [Tooltip("Durée du va et vien entre le tacleur et le taclé")]
  public float tackleAnimTime;

  bool isTackling = false;

  [HideInInspector] public static TackleBehaviour Instance;

  public SyncListInt randomIntList;
  int randomIntOrder = 0;

    // *************** //
    // ** Initialisation ** //
    // *************** //

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

  private void Init()
    {
      SetupRandomList();
    }

  IEnumerator LateOnEnable()
    {
        yield return new WaitForEndOfFrame();
        TurnManager.Instance.changeTurnEvent += OnChangeTurn;
    }

    void OnDisable()
    {
        if (LoadingManager.Instance.isGameReady())
        {
            TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
        }
    }

        // *************** //
        // ** Checkers ** //
        // *************** //

        public void SetupRandomList () 
        {
      randomIntList.Clear();
    randomIntOrder = 0;
    if (!isServer)
      return;

        for(int i = 0; i < 100; i++)
        {
        randomIntList.Add(UnityEngine.Random.Range(0, 100));
        }
     }

  //////////////////////////////////////////////////////////////////////////////////////////////////////////

  void OnChangeTurn(object sender, PlayerArgs e)
    { // Lorsqu'un joueur termine son tour
    SetupRandomList();
    }

        public void CheckTackle(GameObject movingObj, PersoData shotingPersonnage = null)
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
                randomIntOrder++;
   int randomInt = randomIntList[randomIntOrder];
                switch (movingObj.name)
                  {
                  case ("Ballon"):
                    Debug.Log(obj.owner + " " + currentPlayer);
                    if (obj.owner != currentPlayer)
                      {
                      int maxInt = 50;
                        path = movingObj.GetComponent<BallonData>().ballonCase.transform;
                        StartCoroutine(TackleEffect(obj, movingObj.transform, GraphManager.Instance.getCaseOffset(obj.gameObject)));
                        if (randomInt < maxInt)
                          {
                          FeedbackManager.Instance.ShowInit(randomInt, maxInt, path.gameObject);
                            Debug.Log("(Si inférieur à 51, il y a interception) " + randomInt + "/" + "100" + ": Interception SUCCESS");
                            GameManager.Instance.actualAction = PersoAction.isWaiting;

                            movingObj.GetComponent<BallonData>().ChangeStatut(BallonStatut.isIntercepted);
                          } 
                      }
                    break;
                  default:
                    if (obj.owner != currentPlayer)
                      {
                            StartCoroutine(TackleEffect(obj, path, GraphManager.Instance.getCaseOffset(obj.gameObject)));
                        if (movingObj.GetComponent<PersoData>().weightType == obj.weightType)
                          {
                                int maxInt = 50;
                                if (randomInt < maxInt)
                              {
                                    FeedbackManager.Instance.ShowInit(randomInt, maxInt, path.gameObject);
                                Debug.Log("(Même poids) (Si inférieur à 51 il y a tackle) " + randomInt + "/" + "100" + ": Tackle SUCCESS");
                                movingObj.GetComponent<PersoData>().isTackled = true;
                              } else
                              {
                                    FeedbackManager.Instance.ShowInit(randomInt, maxInt, path.gameObject);
                                Debug.Log("(Même poids) (Si inférieur à 51 il y a tackle) " + randomInt + "/" + "100" + ": Tackle FAILED");
                              }
                          } else
                          {
                                int maxInt = 25;
                                if (randomInt < maxInt)
                              {
                                    FeedbackManager.Instance.ShowInit(randomInt, maxInt, path.gameObject);
                                Debug.Log("(Poids différents) (Si inférieur à 26 il y a tackle) " + randomInt + "/" + "100" + ": Tackle SUCCESS");
                                movingObj.GetComponent<PersoData>().isTackled = true;
                              } else
                              {
                                    FeedbackManager.Instance.ShowInit(randomInt, maxInt, path.gameObject);
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