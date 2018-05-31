﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms;
using System.Security.Cryptography;

/// <summary>Tout ce qu'il est possible de faire avec un personnage, ainsi que toutes ses données.</summary>
public class PersoData : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public WeightType weightType;
  public Player owner;
  public int actualPointMovement;
  public int maxPointMovement;
  public int actualPointAction;
  public int maxPointAction;
  public int actualPointResistance;
  public int maxPointResistance;

  public int pmDebuff;
  public int paDebuff;
  /// <summary>Modifie la portée du tir.</summary>
  public int shotStrenght;
  public Direction persoDirection;
  public CaseData persoCase;
  public GameObject originPoint;
  public Sprite faceSprite;
  public Sprite backSprite;

  public SpellData Spell1 = null;
  public SpellData Spell2 = null;

  SpriteRenderer spriteR;
  bool ShineColorIsRunning = false;

  public bool isTackled = false;
  public int timeStunned = 0;

  public Animator animator;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  void Start()
  {
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    yield return new WaitForEndOfFrame();
    while (TurnManager.Instance == null)
    {
      yield return null;
    }
    Init();
  }

  public void Init()
  {
    spriteR = GetComponentInChildren<SpriteRenderer>();
    animator = GetComponentInChildren<Animator>();
    animator.SetBool("Idle", true);

    gameObject.name = spriteR.sprite.name;
    isTackled = false;
    actualPointMovement = maxPointMovement;

    RosterManager.Instance.listHero.Add(this);
    TurnManager.Instance.changeTurnEvent += OnChangeTurn;
  }

  void OnDisable()
  {
    TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
  }

  private void Update()
  {
    CheckDeath();
  }

  // *************** //
  // ** Events **    // Appel de fonctions au sein de ce script grâce à des events
  // *************** //

  public void OnChangeTurn(object sender, PlayerArgs e)
  {
    if (e.currentPlayer == owner)
    {
      ResetPM();
      ResetPA();
      EffectManager.Instance.ChangePM(this, pmDebuff);
      pmDebuff = 0;
      if (timeStunned == 1)
      {
        actualPointResistance = maxPointResistance;
        timeStunned = 0;
      }
      if(timeStunned > 0)
      {
        Debug.Log("ISSOU");
        timeStunned--;
      }
    }
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  /// <summary>Vérifie si l'invocation est censé être toujours vivant ou pas.</summary>
  public void CheckDeath()
  {
    if (actualPointResistance <= 0 && timeStunned == 0){
      timeStunned = 3;
      if(SelectionManager.Instance.selectedPersonnage == this)
      {
        SelectionManager.Instance.Deselect();
      }
    }
  }

  /// <summary>Fixe les PM actuel du personnage à ses PM max.</summary>
  public void ResetPM()
  {
    actualPointMovement = maxPointMovement;
  }

  /// <summary>Fixe les PM actuel du personnage à ses PA max.</summary>
  public void ResetPA()
  {
    actualPointAction = maxPointAction;
  }

  /// <summary>Fixe les PM actuel du personnage à ses PR max.</summary>
  public void ResetPR()
  {
    actualPointResistance = maxPointResistance;
  }

  /// <summary>Change la rotation du sprite du personnage dans la direction donnée.</summary>
  public void ChangeRotation(Direction direction)
  {
    persoDirection = direction;
    switch (persoDirection)
    {
      case Direction.SudOuest:
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        animator.SetBool("Back", false);
        animator.SetBool("Front", true);
        break;
      case Direction.NordOuest:
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        animator.SetBool("Front", false);
        animator.SetBool("Back", true);
        break;
      case Direction.SudEst:
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        animator.SetBool("Back", false);
        animator.SetBool("Front", true);
        break;
      case Direction.NordEst:
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        animator.SetBool("Front", false);
        animator.SetBool("Back", true);
        break;
    }
  }

  /// <summary>Change la direction du personnage en direction de la case ciblée.</summary>
  public void RotateTowards(GameObject targetCasePosGMB)
  {
    if (persoCase == null)
      return;

    Vector3 targetCasePos = targetCasePosGMB.transform.position;
    Vector3 originCasePos = persoCase.transform.position;

    if (originCasePos.x > targetCasePos.x && originCasePos.y > targetCasePos.y)
      ChangeRotation(Direction.SudOuest);

    if (originCasePos.x > targetCasePos.x && originCasePos.y < targetCasePos.y)
      ChangeRotation(Direction.NordOuest);

    if (originCasePos.x < targetCasePos.x && originCasePos.y > targetCasePos.y)
      ChangeRotation(Direction.SudEst);

    if (originCasePos.x < targetCasePos.x && originCasePos.y < targetCasePos.y)
      ChangeRotation(Direction.NordEst);
  }

  public void RotateTowardsReversed(GameObject targetCasePosGMB)
  {
    Vector3 targetCasePos = targetCasePosGMB.transform.position;
    Vector3 originCasePos = persoCase.transform.position;

    if (originCasePos.x > targetCasePos.x && originCasePos.y > targetCasePos.y)
      ChangeRotation(Direction.NordEst);

    if (originCasePos.x > targetCasePos.x && originCasePos.y < targetCasePos.y)
      ChangeRotation(Direction.SudEst);

    if (originCasePos.x < targetCasePos.x && originCasePos.y > targetCasePos.y)
      ChangeRotation(Direction.NordOuest);

    if (originCasePos.x < targetCasePos.x && originCasePos.y < targetCasePos.y)
      ChangeRotation(Direction.SudOuest);
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
