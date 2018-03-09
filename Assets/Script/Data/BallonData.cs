using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/// <summary>Tout ce qu'il est possible de faire avec un ballon, ainsi que toutes ses données.</summary>
public class BallonData : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  [Header("Data")]
  [SerializeField] [EnumFlagAttribute] BallonStatut statut;
  [Space(100)]
  public bool isMoving = false;

  [Header("  Temps")]
  public float travelTimeBallon;
  [Tooltip("Utilisé pour ")]
  public float ballStrenght;

  public Direction ballonDirection;
  public CaseData ballonCase;

  [Header("Tir")]
  public GameObject menuContextuel;
  //MenuContextuel
  public GameObject selectedBallon;
  public Vector3 offsetBallon;
  public float xCoordInc;
  public float yCoordInc;
  public float xCoord;
  public float yCoord;
  public bool canRebond;
  public int casesCrossed;

  Animator animator;
  SpriteRenderer spriteR;
  bool ShineColorIsRunning = false;

  [HideInInspector] public List<Transform> movePath;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  void Awake()
  {
    name = "Ballon";
  }

  void Start()
  {
    spriteR = GetComponent<SpriteRenderer>();
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
    animator = GetComponent<Animator>();
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  /// <summary>Déplace le ballon devant le personnage ayant tiré.</summary>
  public IEnumerator Move()
  {
    casesCrossed = 0;
    CaseData hoveredCase = HoverManager.Instance.hoveredCase;
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;

    GameManager.Instance.actualAction = PersoAction.isShoting;
    TurnManager.Instance.DisableFinishTurn();
    MenuManager.Instance.isShoting = true;

    GameObject nextPosition;

    animator.SetTrigger("Roule");
    isMoving = true;
    offsetBallon = new Vector2(0, 0);
    float xCoordNext = hoveredCase.GetComponent<CaseData>().xCoord;
    float yCoordNext = hoveredCase.GetComponent<CaseData>().yCoord;

    xCoordInc = 0;
    yCoordInc = 0;
    transform.localRotation = Quaternion.Euler(0, 0, 0);
    Debug.Log(selectedPersonnage.transform.position + " " + transform.position);

    if (selectedPersonnage.transform.position.x > transform.position.x)
      {
        xCoordInc -= 0.5f;
        yCoordInc -= 0.5f;
      } else if (selectedPersonnage.transform.position.x < transform.position.x)
      {
        xCoordInc += 0.5f;
        yCoordInc += 0.5f;
      }

    if (selectedPersonnage.transform.position.y - 0.5f > transform.position.y)
      {
        xCoordInc += 0.5f;
        yCoordInc -= 0.5f;
      } else if (selectedPersonnage.transform.position.y - 0.5f < transform.position.y)
      {
        xCoordInc -= 0.5f;
        yCoordInc += 0.5f;
      }
    for (int i = 0; i < ballStrenght; i++)
      {
          
        if ((BallonStatut.isIntercepted & statut) == BallonStatut.isIntercepted)
          {
            ChangeStatut(BallonStatut.None, BallonStatut.isIntercepted);
            break;
          }
        xCoordNext += xCoordInc;
        yCoordNext += yCoordInc;
        if (GameObject.Find(xCoordNext.ToString() + " " + yCoordNext.ToString()) != null)
          {
            nextPosition = GameObject.Find(xCoordNext.ToString() + " " + yCoordNext.ToString());
            if (nextPosition.GetComponent<CaseData>().casePathfinding == PathfindingCase.NonWalkable)
              {
                if (!canRebond)
                  {
                    break;
                  }
                nextPosition = GameObject.Find(xCoordNext.ToString() + " " + yCoordNext.ToString());
              }
          } else
          {
            if (!canRebond)
              {
                goto endMove;
              }
            xCoordInc = -xCoordInc;
            yCoordInc = -yCoordInc;
            xCoordNext += xCoordInc;
            yCoordNext += yCoordInc;
            nextPosition = GameObject.Find(xCoordNext.ToString() + " " + yCoordNext.ToString());
          }

        if (xCoordNext == ballonCase.GetComponent<CaseData>().xCoord)
          {
            if (yCoordNext < ballonCase.GetComponent<CaseData>().yCoord)
              {
                ballonDirection = Direction.SudOuest;
              } else
              {
                ballonDirection = Direction.NordEst;
              }
          } else if (yCoordNext == ballonCase.GetComponent<CaseData>().yCoord)
          {
            if (xCoordNext < ballonCase.GetComponent<CaseData>().xCoord)
              {
                ballonDirection = Direction.NordOuest;
              } else
              {
                ballonDirection = Direction.SudEst;
              }
          }
        ChangeRotation();
        Vector3 startPos = transform.position;
        float fracturedTime = 0;
        float timeUnit = travelTimeBallon / 60;
        while (transform.position != nextPosition.transform.position)
          {
            fracturedTime += timeUnit + 0.01f;
            transform.position = Vector3.Lerp(startPos, nextPosition.transform.position, fracturedTime);
            yield return new WaitForEndOfFrame();
          }
        casesCrossed++;

        TackleBehaviour.Instance.CheckTackle(this.gameObject, selectedPersonnage);
      }
    endMove:
    isMoving = false;
    GameManager.Instance.actualAction = PersoAction.isSelected;
    animator.ResetTrigger("Roule");
    animator.SetTrigger("Idle");
    CaseManager.Instance.StartCoroutine("ShowActions");
    TurnManager.Instance.EnableFinishTurn();
    casesCrossed = 0;
  }

  /// <summary>Change la rotation du sprite du ballon.</summary>
  public void ChangeRotation()
  {
    switch (ballonDirection)
      {
      case Direction.SudOuest:
        transform.localRotation = Quaternion.Euler(0, 0, 180);
        break;
      case Direction.NordOuest:
        transform.localRotation = Quaternion.Euler(0, 0, 120);
        break;
      case Direction.SudEst:
        transform.localRotation = Quaternion.Euler(0, 0, 300);
        break;
      case Direction.NordEst:
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        break;
      }

    MenuManager.Instance.isShoting = false;
  }

  /// <summary>Change le statut du ballon.</summary>
  public void ChangeStatut(BallonStatut newStatut = BallonStatut.None, BallonStatut oldStatut = BallonStatut.None)
  {
    BallonStatut lastStatut = statut;
    if ((newStatut != BallonStatut.None) && !((newStatut & statut) == newStatut))
      statut += (int)newStatut;
    if ((oldStatut != BallonStatut.None) && ((oldStatut & statut) == oldStatut))
      statut -= (int)oldStatut;
  }

  /// <summary>La couleur du sprite oscille entre deux couleurs.</summary>
  public IEnumerator StartShineColor(Color color1, Color color2, float time)
  {
    if (spriteR.color == color1 && spriteR.color == color2)
      StopCoroutine(StartShineColor(color1, color2, time));

    if (ShineColorIsRunning)
      StopCoroutine(StartShineColor(color1, color2, time));

    ShineColorIsRunning = true;

    while (ShineColorIsRunning)
      {
        Color colorx = color1;
        color1 = color2;
        color2 = colorx;
        for (int i = 0; i < 100; i++)
          {
            if (!ShineColorIsRunning)
              break;

            spriteR.color += (color1 - color2) / 100;
            yield return new WaitForSeconds(time + 0.01f);
          }

      }
  }

  /// <summary>Stop la fonction StartShineColor</summary>
  public void StopShineColor()
  {
    ShineColorIsRunning = false;
  }
}
