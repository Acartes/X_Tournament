﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallonData : MonoBehaviour
{

    [EnumFlagAttribute]
    public Statut actualStatut;

    [HideInInspector]
    public List<Transform> movePath;

    Animator animator;

    public bool isMoving = false;

    public Direction ballonDirection;
    public GameObject ballonCase;

    [Header("Tir")]
    public GameObject menuContextuel; //MenuContextuel
    public GameObject selectedBallon;
    public Vector3 offsetBallon;
    public GameObject nextPosition;
    public float travelTimeBallon;
    public float xCoordInc;
    public float yCoordInc;
    public float xCoord;
    public float yCoord;
    public bool canRebond;

  public bool isIntercepted;

    void Start()
    {
    isIntercepted = false;
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

    public IEnumerator Move(GameObject hoveredCase, GameObject selectedPersonnage, float travelTimeBallon, float ballStrenght)
    {
        TurnManager.Instance.DisableFinishTurn();
        CameraBehaviour.Instance.LongFocus(SelectionManager.Instance.selectedBallon.transform);
        ShotBehaviour.Instance.isShoting = true;

        animator.SetTrigger("Roule");
        isMoving = true;
        offsetBallon = new Vector2(0, 0);
        xCoord = hoveredCase.GetComponent<CaseData>().xCoord;
        yCoord = hoveredCase.GetComponent<CaseData>().yCoord;

        xCoordInc = 0;
        yCoordInc = 0;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (selectedPersonnage.transform.position.x > transform.position.x)
        { //offsetBallon = new Vector2 (-0.625f,0);
          //	transform.localRotation = Quaternion.Euler(0,transform.localRotation.eulerAngles.y+180,transform.localRotation.eulerAngles.z);
            xCoordInc -= 0.5f;
            yCoordInc -= 0.5f;
        }
        else if (selectedPersonnage.transform.position.x < transform.position.x)
        { //offsetBallon = new Vector2 (0.625f,0);
          //	transform.localRotation = Quaternion.Euler(0,transform.localRotation.eulerAngles.y-180,transform.localRotation.eulerAngles.z);
            xCoordInc += 0.5f;
            yCoordInc += 0.5f;
        }

        if (selectedPersonnage.transform.position.y - 0.5f > transform.position.y)
        { //offsetBallon = new Vector2 (offsetBallon.x, -0.289f);
          //	transform.localRotation = Quaternion.Euler(0,transform.localRotation.eulerAngles.y,transform.localRotation.eulerAngles.z+180);
            xCoordInc += 0.5f;
            yCoordInc -= 0.5f;
        }
        else if (selectedPersonnage.transform.position.y - 0.5f < transform.position.y)
        { //offsetBallon = new Vector2 (offsetBallon.x, 0.289f);
          //	transform.localRotation = Quaternion.Euler(0,transform.localRotation.eulerAngles.y,transform.localRotation.eulerAngles.z);
            xCoordInc -= 0.5f;
            yCoordInc += 0.5f;
        }
        for (int i = 0; i < ballStrenght; i++)
        {
          TackleBehaviour.Instance.CheckTackle(this.gameObject, selectedPersonnage);
        if (isIntercepted)
          {
            isIntercepted = false;
            break;
          }
            xCoord += xCoordInc;
            yCoord += yCoordInc;
            if (GameObject.Find(xCoord.ToString() + " " + yCoord.ToString()) != null)
            {
                nextPosition = GameObject.Find(xCoord.ToString() + " " + yCoord.ToString());
                if (nextPosition.GetComponent<CaseData>().casePathfinding == PathfindingCase.NonWalkable)
                {
                    if (!canRebond)
                    {
                        break;
                    }
                    xCoordInc = -xCoordInc;
                    yCoordInc = -yCoordInc;
                    xCoord += xCoordInc;
                    yCoord += yCoordInc;
                    nextPosition = GameObject.Find(xCoord.ToString() + " " + yCoord.ToString());
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
                xCoord += xCoordInc;
                yCoord += yCoordInc;
                nextPosition = GameObject.Find(xCoord.ToString() + " " + yCoord.ToString());
            }

            if (xCoord == ballonCase.GetComponent<CaseData>().xCoord)
            {
                if (yCoord < ballonCase.GetComponent<CaseData>().yCoord)
                {
                    ballonDirection = Direction.SudOuest;
                }
                else
                {
                    ballonDirection = Direction.NordEst;
                }
            }
            else if (yCoord == ballonCase.GetComponent<CaseData>().yCoord)
            {
                if (xCoord < ballonCase.GetComponent<CaseData>().xCoord)
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
        TurnManager.Instance.EnableFinishTurn();
        CameraBehaviour.Instance.StopLongFocus();

        CameraBehaviour.Instance.Focus(selectedPersonnage.transform);
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

        ShotBehaviour.Instance.isShoting = false;

    }
}
