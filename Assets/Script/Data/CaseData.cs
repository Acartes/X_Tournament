using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CaseData : NetworkBehaviour
{
    // *********** //
    // ** Variables ** //
    // *********** //

    [Header("Data")]
    [SerializeField]
    [EnumFlagAttribute]
    Statut statut;[Space(100)]
    public PersoData personnageData;
    public GameObject objectData;
    public BallonData ballon;
    public PathfindingCase casePathfinding;
    public Element caseElement;
    public int xCoord;
    public int yCoord;
    public Player winCase;
    public CaseData thisCase;

    [Header("PlacementColor")]
    public Color initColor;
    public Player ownerPlacementZone;
    public bool redZone;
    public bool blueZone;

    [Header("NormalColor")]
    public Color caseColor;

    [Header("ElementalSprite")]
    public Sprite SpriteAucun;
    public Sprite SpriteFeu;

    public GameObject prefabPunch;

    public SpriteRenderer spriteR;

    [HideInInspector] public bool colorLock;

    // *********** //
    // ** Start ** //
    // *********** //

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
        if (winCase != Player.Neutral) statut = Statut.isGoal;
        spriteR = GetComponent<SpriteRenderer>();
        thisCase = GetComponent<CaseData>();
        colorLock = false;
        initColor = GetComponent<SpriteRenderer>().color;
        if (winCase != Player.Neutral)
        {
            caseColor = ColorManager.Instance.goalColor;
        }
        PlacementColor();
        caseElement = Element.Aucun;
        casePathfinding = PathfindingCase.Walkable;

        TurnManager.Instance.changeTurnEvent += OnChangePhase;
    }

    void OnDisable()
    {
        if(LoadingManager.Instance.isGameReady())
        TurnManager.Instance.changeTurnEvent -= OnChangePhase;
    }


    // *********** //
    // ** Event ** //
    // *********** //

    void OnChangePhase(object sender, PlayerArgs e)
    {

        switch (e.currentPhase)
        {
            case Phase.Placement:
                ChangeColor(Statut.canPlace);
                break;
            case Phase.Deplacement:
                statut = 0;
                ChangeColor();
                break;
        }
    }

    // ************* //
    // ** Trigger ** //
    // ************* //

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Personnage")
        {
            if (col.gameObject.GetComponent<PersoData>().persoCase != thisCase)
            {
                TransparencyBehaviour.CheckTransparency(col.gameObject, 1f);
                personnageData = col.gameObject.GetComponent<PersoData>();
                casePathfinding = PathfindingCase.NonWalkable;
                col.gameObject.GetComponent<PersoData>().persoCase = thisCase;
                TransparencyBehaviour.CheckTransparency(col.gameObject, 0.5f);
            }
        }

        if (col.tag == "Ballon")
        {
            if (col.gameObject.GetComponent<BallonData>().ballonCase != thisCase)
            {
                TransparencyBehaviour.CheckTransparency(col.gameObject, 1f);
                ballon = col.gameObject.GetComponent<BallonData>();
                casePathfinding = PathfindingCase.NonWalkable;
                col.gameObject.GetComponent<BallonData>().ballonCase = thisCase;
                TransparencyBehaviour.CheckTransparency(col.gameObject, 0.5f);
                col.gameObject.GetComponent<BallonData>().xCoord = xCoord;
                col.gameObject.GetComponent<BallonData>().yCoord = yCoord;
            }

            if (winCase != Player.Neutral)
            {
                StartCoroutine(UIManager.Instance.ScoreChange(winCase));
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
            ChangeColor(Statut.None, Statut.isSelected);
            ChangeColor(Statut.None, Statut.isAllyPerso);
        }

        if (col.tag == "Ballon"
          && col.gameObject.GetComponent<BoxCollider2D>().enabled == true
          && GetComponent<PolygonCollider2D>().enabled == true)
        {
            TransparencyBehaviour.CheckTransparency(col.gameObject, 1f);
            ballon = null;
            casePathfinding = PathfindingCase.Walkable;
            ChangeColor(Statut.None, Statut.canShot);
        }
    }

    // ************* //
    // ** Actions ** //
    // ************* //

    public void PlacementColor()
    { // Its the color when a player can place his personnages here at Placement Phase
        ChangeColor(Statut.None, Statut.canPlace);
    }

    void NormalColor(Color caseColor)
    { // Switch to a neutral color
        ChangeColor(Statut.None, Statut.canPlace);
    }

    public void ResetColor()
    {
        spriteR.color = caseColor;
    }

    public void ChangeColor(Statut newStatut = Statut.None, Statut oldStatut = Statut.None)
    {
        Statut lastStatut = statut;
        if ((newStatut != Statut.None) && !((newStatut & statut) == newStatut)) statut += (int)newStatut;
        if ((oldStatut != Statut.None) && ((oldStatut & statut) == oldStatut)) statut -= (int)oldStatut;

        spriteR.color = ColorManager.Instance.caseColor;
        if ((Statut.isSelected & statut) == Statut.isSelected) spriteR.color = ColorManager.Instance.selectedColor;
        if ((Statut.canReplace & statut) == Statut.canReplace) spriteR.color = ColorManager.Instance.actionPreColor;
        if ((Statut.canPunch & statut) == Statut.canPunch)
        {
            spriteR.color = ColorManager.Instance.actionPreColor;

        }
        if ((Statut.canMove & statut) == Statut.canMove) spriteR.color = ColorManager.Instance.moveColor;
        if ((Statut.canBeTackled & statut) == Statut.canBeTackled) spriteR.color = ColorManager.Instance.enemyColor;
        if ((Statut.canShot & statut) == Statut.canShot) spriteR.color = ColorManager.Instance.actionPreColor;
        if ((Statut.isAllyPerso & statut) == Statut.isAllyPerso) spriteR.color = ColorManager.Instance.actionPreColor;

        if ((Statut.canPlace & statut) == Statut.canPlace
          && ownerPlacementZone == Player.Red) spriteR.color = ColorManager.Instance.placementZoneRed;
        if ((Statut.canPlace & statut) == Statut.canPlace
          && ownerPlacementZone == Player.Blue) spriteR.color = ColorManager.Instance.placementZoneBlue;

        if ((Statut.isGoal & statut) == Statut.isGoal) spriteR.color = ColorManager.Instance.goalColor;
        if ((Statut.canBeTackled & statut) == Statut.canBeTackled) spriteR.color = ColorManager.Instance.enemyColor;

        if ((Statut.isHovered & statut) == Statut.isHovered)
        {
            spriteR.color = ColorManager.Instance.hoverColor;
            if ((Statut.canReplace & statut) == Statut.canReplace) spriteR.color = ColorManager.Instance.actionColor;
            if ((Statut.canPunch & statut) == Statut.canPunch) spriteR.color = ColorManager.Instance.actionColor;
            if ((Statut.canShot & statut) == Statut.canShot) spriteR.color = ColorManager.Instance.actionColor;
            if ((Statut.isAllyPerso & statut) == Statut.isAllyPerso) spriteR.color = ColorManager.Instance.actionColor;
        }
      ChangeFeedback(newStatut, oldStatut);
    }

  void ChangeFeedback(Statut newStatut = Statut.None, Statut oldStatut = Statut.None)
    {
      if ((Statut.canBeTackled & statut) == Statut.canBeTackled) FeedbackManager.Instance.PredictInit(50, gameObject);
      if (oldStatut == (Statut.canBeTackled)) FeedbackManager.Instance.PredictEnd(gameObject);
    }
}
