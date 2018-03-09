using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>Tout ce qu'il est possible de faire avec une case, ainsi que tout ce qui rentre dedans.</summary>
public class CaseData : NetworkBehaviour
{
  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  [SerializeField]
  [EnumFlagAttribute]
  [Space(200)]
  public Statut statut;
  [Space(200)]
  public PersoData personnageData;
  public BallonData ballon;
  public PathfindingCase casePathfinding;
  public int xCoord;
  public int yCoord;

  SpriteRenderer spriteR;
  Statut defaultStatut;
  bool ShineColorIsRunning = false;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
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
    spriteR = GetComponent<SpriteRenderer>();
    casePathfinding = PathfindingCase.Walkable;
    defaultStatut = statut;

    TurnManager.Instance.changeTurnEvent += OnChangeTurn;
  }

  void OnDisable()
  {
    if (LoadingManager.Instance.isGameReady())
      TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
  }


  // *************** //
  // ** Events **    // Appel de fonctions au sein de ce script grâce à des events
  // *************** //

  void OnChangeTurn(object sender, PlayerArgs e)
  {

    switch (e.currentPhase)
      {
      case Phase.Placement:
        ChangeStatut();
        break;
      case Phase.Deplacement:
        statut = 0;
        ChangeStatut();
        break;
      }
  }

  // ************* //
  // ** Trigger ** // Appel de fonctions ou d'instructions grâce à la détection de collider triggers
  // ************* //

  void OnTriggerEnter2D(Collider2D col)
  {

    if (col.tag == "Personnage")
      {
        if (col.gameObject.GetComponent<PersoData>().persoCase != this)
          {
            TransparencyBehaviour.Instance.CheckCaseTransparency(this);
            personnageData = col.gameObject.GetComponent<PersoData>();
            casePathfinding = PathfindingCase.NonWalkable;
            col.gameObject.GetComponent<PersoData>().persoCase = this;
            TransparencyBehaviour.Instance.CheckCaseTransparency(this);
          }
      }

    if (col.tag == "Ballon")
      {
        if (col.gameObject.GetComponent<BallonData>().ballonCase != this)
          {
            TransparencyBehaviour.Instance.CheckCaseTransparency(this);
            ballon = col.gameObject.GetComponent<BallonData>();
            casePathfinding = PathfindingCase.NonWalkable;
            col.gameObject.GetComponent<BallonData>().ballonCase = this;
            TransparencyBehaviour.Instance.CheckCaseTransparency(this);
            col.gameObject.GetComponent<BallonData>().xCoord = xCoord;
            col.gameObject.GetComponent<BallonData>().yCoord = yCoord;
          }
      }
  }

  void OnTriggerExit2D(Collider2D col)
  {
    if (col.tag == "Personnage"
        && col.gameObject.GetComponent<BoxCollider2D>().enabled == true
        && GetComponent<PolygonCollider2D>().enabled == true)
      {

        personnageData = null;
        casePathfinding = PathfindingCase.Walkable;
        ChangeStatut(Statut.None, Statut.isSelected);
        ChangeStatut(Statut.None, Statut.isControllable);
      }

    if (col.tag == "Ballon"
        && col.gameObject.GetComponent<BoxCollider2D>().enabled == true
        && GetComponent<PolygonCollider2D>().enabled == true)
      {
        TransparencyBehaviour.Instance.CheckCaseTransparency(this);
        ballon = null;
        casePathfinding = PathfindingCase.Walkable;
        ChangeStatut(Statut.None, Statut.canShot);
      }
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  void navid()
  {
    ChangeStatut(Statut.None, Statut.canPunch);
  }

  /// <summary>Change le statut, et met à jour la couleur et le feedback lié au statut</summary>
  public void ChangeStatut(Statut newStatut = Statut.None, Statut oldStatut = Statut.None)
  {
    Statut lastStatut = statut;

    if ((newStatut != Statut.None) && !((newStatut & statut) == newStatut))
      statut += (int)newStatut;
      
    if ((oldStatut != Statut.None) && ((oldStatut & statut) == oldStatut))
      statut -= (int)oldStatut;
      
    ChangeColorByStatut(statut);
    ChangeFeedbackByStatut(statut, oldStatut);
  }

  /// <summary>Change la couleur de la case selon son statut.</summary>
  private void ChangeColorByStatut(Statut statut)
  {
    spriteR.color = ColorManager.Instance.caseColor;

    if ((Statut.goalRed & statut) == Statut.goalRed)
      spriteR.color = ColorManager.Instance.goalColor;

    if ((Statut.goalBlue & statut) == Statut.goalBlue)
      spriteR.color = ColorManager.Instance.goalColor;

    if ((Statut.placementRed & statut) == Statut.placementRed)
      spriteR.color = ColorManager.Instance.placementZoneRed;

    if ((Statut.placementBlue & statut) == Statut.placementBlue)
      spriteR.color = ColorManager.Instance.placementZoneBlue;

    if ((Statut.isSelected & statut) == Statut.isSelected)
      spriteR.color = ColorManager.Instance.selectedColor;
      
    if ((Statut.canReplace & statut) == Statut.canReplace)
      spriteR.color = ColorManager.Instance.actionPreColor;
      
    if ((Statut.canPunch & statut) == Statut.canPunch)
      spriteR.color = ColorManager.Instance.actionPreColor;

    if ((Statut.canMove & statut) == Statut.canMove)
      spriteR.color = ColorManager.Instance.moveColor;
    if ((Statut.canBeTackled & statut) == Statut.canBeTackled)
      spriteR.color = ColorManager.Instance.enemyColor;
    if ((Statut.canShot & statut) == Statut.canShot)
      spriteR.color = ColorManager.Instance.actionPreColor;
    if ((Statut.isControllable & statut) == Statut.isControllable)
      spriteR.color = ColorManager.Instance.actionPreColor;

    if ((Statut.canBeTackled & statut) == Statut.canBeTackled)
      spriteR.color = ColorManager.Instance.enemyColor;

    if ((Statut.isHovered & statut) == Statut.isHovered)
      {
        spriteR.color = ColorManager.Instance.hoverColor;
        if ((Statut.canReplace & statut) == Statut.canReplace)
          spriteR.color = ColorManager.Instance.actionColor;
        if ((Statut.canPunch & statut) == Statut.canPunch)
          spriteR.color = ColorManager.Instance.actionColor;
        if ((Statut.canShot & statut) == Statut.canShot)
          spriteR.color = ColorManager.Instance.actionColor;
        if ((Statut.isControllable & statut) == Statut.isControllable)
          spriteR.color = ColorManager.Instance.actionColor;
      }
  }

  /// <summary>Change le feedback visuel de la case selon son statut.</summary>
  private void ChangeFeedbackByStatut(Statut statut, Statut oldStatut)
  {
    if ((Statut.canBeTackled & statut) == Statut.canBeTackled)
      FeedbackManager.Instance.PredictInit(50, gameObject);
    if (oldStatut == (Statut.canBeTackled))
      FeedbackManager.Instance.PredictEnd(gameObject);
  }

  /// <summary>Change la couleur de la case.</summary>
  public void ChangeColor(Color color)
  {
    spriteR.color = color;
  }

  /// <summary>Change le sprite.</summary>
  public void ChangeSprite(Sprite sprite)
  {
    spriteR.sprite = sprite;
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

  /// <summary>Check si la case a ce statut.</summary>
  public bool CheckStatut(Statut statutChecked)
  {
    if ((statutChecked & statut) == statutChecked)
      return true;
    else
      return false;
  }

  /// <summary>Change le passage de la case en "Walkable" ou "Non walkable".</summary>
  public void ChangePathfinding(PathfindingCase pathfinding)
  {
    casePathfinding = pathfinding;
  }

  /// <summary>Récupère la case en haut de cette case.</summary>
  public GameObject GetTopCase()
  {
    GameObject newCase = (GameObject.Find(xCoord - 1 + " " + yCoord + 1) != null) ? GameObject.Find(xCoord - 1 + " " + yCoord + 1) : null;
    return newCase;
  }

  /// <summary>Récupère la case en bas de cette case.</summary>
  public GameObject GetBottomCase()
  {
    GameObject newCase = (GameObject.Find(xCoord + 1 + " " + (yCoord - 1)) != null) ? GameObject.Find(xCoord + 1 + " " + (yCoord - 1)) : null;
    return newCase;
  }

  /// <summary>Récupère la case à gauche de cette case.</summary>
  public GameObject GetLeftCase()
  {
    GameObject newCase = (GameObject.Find(xCoord - 1 + " " + (yCoord - 1)) != null) ? GameObject.Find(xCoord - 1 + " " + (yCoord - 1)) : null;
    return newCase;
  }

  /// <summary>Récupère la case à droite de cette case.</summary>
  public GameObject GetRightCase()
  {
    GameObject newCase = (GameObject.Find(xCoord + 1 + " " + yCoord + 1) != null) ? GameObject.Find(xCoord + 1 + " " + yCoord + 1) : null;
    return newCase;
  }

  /// <summary>Récupère la case en haut à gauche de cette case.</summary>
  public GameObject GetTopLeftCase()
  {
    GameObject newCase = (GameObject.Find(xCoord - 1 + " " + yCoord) != null) ? GameObject.Find(xCoord - 1 + " " + yCoord) : null;
    return newCase;
  }

  /// <summary>Récupère la case en bas à droite de cette case.</summary>
  public GameObject GetBottomRightCase()
  {
    GameObject newCase = (GameObject.Find(xCoord - 1 + " " + yCoord) != null) ? GameObject.Find(xCoord - 1 + " " + yCoord) : null;
    return newCase;
  }

  /// <summary>Récupère la case en haut à droite de cette case.</summary>
  public GameObject GetTopRightCase()
  {
    GameObject newCase = (GameObject.Find(xCoord + " " + yCoord + 1) != null) ? GameObject.Find(xCoord + " " + yCoord + 1) : null;
    return newCase;
  }

  /// <summary>Récupère la case en bas à gauche de cette case.</summary>
  public GameObject GetBottomLeftCase()
  {
    GameObject newCase = (GameObject.Find(xCoord + " " + (yCoord - 1)) != null) ? GameObject.Find(xCoord + " " + (yCoord - 1)) : null;
    return newCase;
  }

  /// <summary>Récupère la case par rapport à X et Y coordonnées de cette case.</summary>
  public GameObject GetCaseRelativeCoordinate(int x, int y)
  {
    GameObject newCase = (GameObject.Find((xCoord + x) + " " + (yCoord + y)) != null) ? GameObject.Find((xCoord + x) + " " + (yCoord + y)) : null;
    return newCase;
  }

  /// <summary>Desactive all statuts for this case.</summary>
  public void ClearAllStatut()
  {
    statut = 0;
  }

  /// <summary>Desactive all statuts for this case except what was already checked in inspector.</summary>
  public void ClearStatutToDefault()
  {
    statut = defaultStatut;
  }
}
