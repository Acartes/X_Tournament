﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

  // *************** //
  // ** Variables ** //
  // *************** //


  [Tooltip("J'utilise parfois cette variable pour être sûr que le ballon soit bien centré où je veux sans bouger son transform")]
  [ReadOnly] public Vector3 offsetBallon;
  [Tooltip("S'il est coché, c'est qu'un tir de ballon est en cours")]
  [ReadOnly] public bool isShoting;
  [HideInInspector] public GameObject nextPosition;
  [HideInInspector] public bool canBounce;

  public static MenuManager Instance;

    void Awake () {
    Instance = this;
    }
  
  void OnEnable()
    {
      ClickEvent.newClickEvent += OnNewClick;
    }

  void OnDisable()
    {
      ClickEvent.newClickEvent -= OnNewClick;
    }

  public void OnNewClick ()
    { // Lors d'un click sur une case
    Phase currentPhase = TurnManager.Instance.currentPhase;
        BallonData hoveredBallon = HoverManager.Instance.hoveredBallon;
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
    PersoAction actualAction = GameManager.Instance.actualAction;

        if (currentPhase == Phase.Deplacement
        && selectedPersonnage != null
        && hoveredBallon != null
        && actualAction == PersoAction.isSelected)
          {
          if (Fonction.Instance.CheckAdjacent(hoveredBallon.gameObject, selectedPersonnage.gameObject) == true)
              {
                ShotMenu();
              }
          }
  }

    public void ShotMenu () 
    {
    GameObject menuContextuel = UIManager.Instance.menuContextuel;
      BallonData hoveredBallon = HoverManager.Instance.hoveredBallon;

      MoveBehaviour.Instance.pathes.Clear();
      SelectionManager.Instance.selectedBallon = hoveredBallon;
      menuContextuel.SetActive (true);
      TurnManager.Instance.DisableFinishTurn();
    }

    public void ReturnMenu () {
      GameObject menuContextuel = UIManager.Instance.menuContextuel;
        BallonData hoveredBallon = HoverManager.Instance.hoveredBallon;

      SelectionManager.Instance.selectedBallon = hoveredBallon;
      menuContextuel.SetActive (true);
      TurnManager.Instance.DisableFinishTurn();
  }
}
