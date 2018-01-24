using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseData : MonoBehaviour {
	// ** Variables ** //
	[Header("Data")]
	public GameObject personnageData;
	public GameObject objectData;
	public GameObject caseBallon;
	public PathfindingCase casePathfinding;
	public Element caseElement;
	public int xCoord;
	public int yCoord;
	public Player winCase;

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

  SpriteRenderer spriteR;

  [HideInInspector] public bool colorLock;

    // *********** //
    // ** Start ** //
    // *********** //

    void Start () {
    colorLock = false;
   spriteR = GetComponent<SpriteRenderer>();
		initColor = GetComponent<SpriteRenderer>().color;
        if (winCase != Player.Neutral) {
        caseColor = ColorManager.Instance.goalColor;
            }
		
		PlacementColor ();
		caseElement = Element.Aucun;
      casePathfinding = PathfindingCase.Walkable;

		TurnManager.Instance.changeTurnEvent += OnChangePhase;
	}

	void OnDisable () {
		TurnManager.Instance.changeTurnEvent -= OnChangePhase;
	}

	// *********** //
	// ** Event ** //
	// *********** //

	void OnChangePhase(object sender, PlayerArgs e) {

		switch (e.currentPhase) {
		case Phase.Placement:

			break;
		case Phase.Deplacement:
            NormalColor (caseColor);
			break;
		}
	}

	// ************* //
	// ** Trigger ** //
	// ************* //

	void OnTriggerStay2D (Collider2D col) {
	/*		if (col.tag == "Personnage") {
        if (col.gameObject.GetComponent<PersoData>().persoCase == null)
          {
            personnageData = col.gameObject;
            casePathfinding = PathfindingCase.NonWalkable;
            col.gameObject.GetComponent<PersoData>().persoCase = this.gameObject;
            col.gameObject.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            TransparencyBehaviour.CheckTransparency(col.gameObject, 0.5f);
          }
			}*/

			if (col.tag == "Ballon") {
          if (col.gameObject.GetComponent<BallonData> ().ballonCase == null)
          {
            caseBallon = col.gameObject;
            casePathfinding = PathfindingCase.NonWalkable;
            col.gameObject.GetComponent<BallonData>().ballonCase = this.gameObject;
            col.gameObject.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
              TransparencyBehaviour.CheckTransparency(col.gameObject, 0.5f);
          }
			}
	}

	void OnTriggerEnter2D (Collider2D col) {

    if (col.tag == "Personnage")
      {
        if (col.gameObject.GetComponent<PersoData>().persoCase != this.gameObject)
          {
              personnageData = col.gameObject;
              casePathfinding = PathfindingCase.NonWalkable;
              col.gameObject.GetComponent<PersoData>().persoCase = this.gameObject;
              col.gameObject.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
              TransparencyBehaviour.CheckTransparency(col.gameObject, 0.5f);
          }
      }

		if (col.tag == "Ballon") {
			if (winCase != Player.Neutral) {
				StartCoroutine(UIManager.Instance.ScoreChange (winCase));
			}
		}
	}

	void OnTriggerExit2D (Collider2D col) {
		if (col.tag == "Personnage" && GetComponent<PolygonCollider2D>().isActiveAndEnabled == true) 
        {
          TransparencyBehaviour.CheckTransparency(col.gameObject, 1f);
		personnageData = null;
		casePathfinding = PathfindingCase.Walkable;
      //  col.gameObject.GetComponent<PersoData>().persoCase = null;
		}

		if (col.tag == "Ballon" && GetComponent<PolygonCollider2D>().isActiveAndEnabled == true) 
        {
          TransparencyBehaviour.CheckTransparency(col.gameObject, 1f);
			caseBallon = null;
          col.gameObject.GetComponent<BallonData> ().ballonCase = null;
			casePathfinding = PathfindingCase.Walkable;
		}
	}

	// ************* //
	// ** Actions ** //
	// ************* //

	public void PlacementColor () { // Its the color when a player can place his personnages here at Placement Phase
    spriteR.color = initColor;
	}

    void NormalColor (Color caseColor) { // Switch to a neutral color
    spriteR.color = caseColor;
	}

    public void ResetColor () {
      spriteR.color = caseColor;
    }

    public void ChangeColor (Color newColor, bool isPersistant = false, PathfindingCase newPath = PathfindingCase.None) {
    if (colorLock)
      return;
      
      if (newPath != PathfindingCase.None)
      {
        casePathfinding = newPath;
      }

    if (isPersistant)
      {
        caseColor = newColor;
      }
    spriteR.color = newColor;
    }

    public void changeElement(Element newElem)
    {
        caseElement = newElem;
        switch (newElem)
        {
            case Element.Feu:
                GetComponent<SpriteRenderer>().sprite = SpriteFeu;
                break;
            case Element.Air:
                break;
            case Element.Terre:
                break;
            case Element.Eau:
                break;
            case Element.Aucun:
                GetComponent<SpriteRenderer>().sprite = SpriteAucun;
                break;
            default:
                break;
        }
    }
}
