using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MenuManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** //
  // *************** //


  [Tooltip("J'utilise parfois cette variable pour être sûr que le ballon soit bien centré où je veux sans bouger son transform")]
  [ReadOnly]
  public Vector3 offsetBallon;
  [Tooltip("S'il est coché, c'est qu'un tir de ballon est en cours")]
  [ReadOnly]
  public bool isShoting;
  [HideInInspector] public GameObject nextPosition;
  [HideInInspector] public bool canBounce;

  public static MenuManager Instance;

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

  void Init()
  {
    EventManager.newClickEvent += OnNewClick;
  }

  void OnDisable()
  {
    EventManager.newClickEvent -= OnNewClick;
  }

  public void OnNewClick()
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
        if (CaseManager.Instance.CheckAdjacent(hoveredBallon.gameObject, selectedPersonnage.gameObject) == true)
          {
            ShotMenu();
          }
      }
  }

  public void ShotMenu()
  {
    GameObject menuContextuel = UIManager.Instance.menuContextuel;
    BallonData hoveredBallon = HoverManager.Instance.hoveredBallon;

    MoveBehaviour.Instance.pathes.Clear();
    SelectionManager.Instance.selectedBallon = hoveredBallon;
    menuContextuel.transform.position = hoveredBallon.transform.position;
    TurnManager.Instance.DisableFinishTurn();
  }

  public void ReturnMenu()
  {
    GameObject menuContextuel = UIManager.Instance.menuContextuel;
    BallonData hoveredBallon = HoverManager.Instance.hoveredBallon;

    SelectionManager.Instance.selectedBallon = hoveredBallon;
    menuContextuel.SetActive(true);
    TurnManager.Instance.DisableFinishTurn();
  }
}
