using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallonData : MonoBehaviour
{
  [Header("Data")]
  [SerializeField] [EnumFlagAttribute] BallonStatut statut; [Space(100)]
    public bool isMoving = false;

  [Header("  Temps")]
  public float travelTimeBallon;
  [Tooltip("Utilisé pour ")]
  public float ballStrenght;

    public Direction ballonDirection;
    public CaseData ballonCase;

    [Header("Tir")]
    public GameObject menuContextuel; //MenuContextuel
    public GameObject selectedBallon;
    public Vector3 offsetBallon;
    public float xCoordInc;
    public float yCoordInc;
    public float xCoord;
    public float yCoord;
    public bool canRebond;

  Animator animator;

  [HideInInspector] public List<Transform> movePath;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Pour indiquer le statut glisse par exemple
        /*actualStatut += (int)Statut.Glisse;
		actualStatut += (int)Statut.Brule;*/

        // Si le ballon brûle, alors
        /*if ((Statut.Brule & actualStatut) == Statut.Brule) {
			Debug.Log ("1");
		} else {
			Debug.Log ("0");
		}*/
    }

    public IEnumerator Move()
    {
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
        }
        else if (selectedPersonnage.transform.position.x < transform.position.x)
        {
            xCoordInc += 0.5f;
            yCoordInc += 0.5f;
        }

        if (selectedPersonnage.transform.position.y - 0.5f > transform.position.y)
        {
            xCoordInc += 0.5f;
            yCoordInc -= 0.5f;
        }
        else if (selectedPersonnage.transform.position.y - 0.5f < transform.position.y)
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
            }
            else
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
                }
                else
                {
                    ballonDirection = Direction.NordEst;
                }
            }
            else if (yCoordNext == ballonCase.GetComponent<CaseData>().yCoord)
            {
                if (xCoordNext < ballonCase.GetComponent<CaseData>().xCoord)
                {
                    ballonDirection = Direction.NordOuest;
                }
                else
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
          TackleBehaviour.Instance.CheckTackle(this.gameObject, selectedPersonnage);
        }
        endMove:
        isMoving = false;
        GameManager.Instance.actualAction = PersoAction.isSelected;
        animator.ResetTrigger("Roule");
        animator.SetTrigger("Idle");
      CaseManager.Instance.StartCoroutine ("ShowActions");
        TurnManager.Instance.EnableFinishTurn();
    }

    void ChangeRotation()
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

  public void ChangeStatut (BallonStatut newStatut = BallonStatut.None, BallonStatut oldStatut = BallonStatut.None) 
    {
      BallonStatut lastStatut = statut;
      if ((newStatut != BallonStatut.None) && !((newStatut & statut) == newStatut)) statut += (int)newStatut;
      if ((oldStatut != BallonStatut.None) && ((oldStatut & statut) == oldStatut)) statut -= (int)oldStatut;
    }
}
