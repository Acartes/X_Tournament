using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBehaviour : MonoBehaviour {

  // *************** //
  // ** Variables ** //
  // *************** //

  [Header("  Temps")]
  public float travelTimeBallon;
  [Tooltip("Utilisé pour ")]
  public float ballStrenght;
  [Tooltip("J'utilise parfois cette variable pour être sûr que le ballon soit bien centré où je veux sans bouger son transform")]
  [ReadOnly] public Vector3 offsetBallon;
  [Tooltip("S'il est coché, c'est qu'un tir de ballon est en cours")]
  [ReadOnly] public bool isShoting;
  [HideInInspector] public GameObject nextPosition;
  [HideInInspector] public bool canBounce;

  public static ShotBehaviour Instance;

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
    GameObject hoveredBallon = HoverManager.Instance.hoveredBallon;
    GameObject selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
    PersoAction actualAction = GameManager.Instance.actualAction;

        if (currentPhase == Phase.Deplacement
        && selectedPersonnage != null
        && hoveredBallon != null
        && actualAction == PersoAction.isSelected)
          {
          if (Fonction.Instance.CheckAdjacent(hoveredBallon, selectedPersonnage) == true)
              {
                TirFunctions(hoveredBallon, UIManager.Instance.menuContextuel);
              }
          }
  }

    public void TirFunctions (GameObject hoveredBallon, GameObject menuContextuel) {
      SelectionManager.Instance.selectedBallon = hoveredBallon;
      menuContextuel.SetActive (true);
      TurnManager.Instance.DisableFinishTurn();
    }

    public void TirDeplaceBalle () {
      GameObject hoveredCase = HoverManager.Instance.hoveredCase;
      GameObject selectedBallon = SelectionManager.Instance.selectedBallon;

      GameManager.Instance.actualAction = PersoAction.isShoting;
      offsetBallon = new Vector2 (0,0);
      TurnManager.Instance.StartCoroutine("EnableFinishTurn");
      StartCoroutine(selectedBallon.GetComponent<BallonData> ().Move (hoveredCase, SelectionManager.Instance.selectedPersonnage, travelTimeBallon, ballStrenght));
    }
}
