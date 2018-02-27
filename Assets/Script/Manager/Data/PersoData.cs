using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PersoData : NetworkBehaviour {

  public WeightType weightType;
	public Player owner;
    public int pointMovement;
    public int actualPointMovement;
    public int pointAction;
	public int actualPointResistance;
	public int pointResistance;
	public int range;

	[HideInInspector]
	public List<Transform> movePath;

	public Direction persoDirection;

	public List<Color> PlayerColor;

	public CaseData persoCase;

	public Vector3 feetPointOffset;

  public Sprite faceSprite;
  public Sprite backSprite;

  SpriteRenderer SpriteR;

  public bool isTackled = false;

    void Awake () {
      
    SpriteR = GetComponent<SpriteRenderer>();
    gameObject.name = SpriteR.sprite.name;
    }

    // Use this for initialization
    void Start() { 
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
      gameObject.name = SpriteR.sprite.name;
      isTackled = false;
      ChangeColor();
      actualPointMovement = pointMovement;

      if (owner == Player.Red)
        {
          if (RosterManager.Instance.listHeroJXToPlace.Count < 1)
          {
              
              RosterManager.Instance.listHeroJXToPlace.Add(new List<PersoData>());
          }
          RosterManager.Instance.listHeroJXToPlace[0].Add(this);
        }
      if (owner == Player.Blue)
        {
          if (RosterManager.Instance.listHeroJXToPlace.Count < 2)
            {
              RosterManager.Instance.listHeroJXToPlace.Add(new List<PersoData>());
            }
          RosterManager.Instance.listHeroJXToPlace[1].Add(this);
        }
      
    RosterManager.Instance.listHero.Add(this);
      TurnManager.Instance.changeTurnEvent += resetPointMovement;
    }

    // ************ //
    // ** Events ** //
    // ************ //

    void resetPointMovement(object sender, PlayerArgs e) {
        if(e.currentPlayer == owner)
        {
            actualPointMovement = pointMovement;
        }
    }

	public void ChangeColor () {
		switch (owner) {
		case Player.Red:
			GetComponent<SpriteRenderer> ().color = PlayerColor [0];
			break;
		case Player.Blue:
			GetComponent<SpriteRenderer> ().color = PlayerColor [1];
			break;
		}
	}

    public void ChangeRotation(Direction direction)
  {
    persoDirection = direction;
    switch (persoDirection)
      {
      case Direction.SudOuest:
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        SpriteR.sprite = faceSprite;
        break;
      case Direction.NordOuest:
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        SpriteR.sprite = backSprite;
        break;
      case Direction.SudEst:
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        SpriteR.sprite = faceSprite;
        break;
      case Direction.NordEst:
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        SpriteR.sprite = backSprite;
        break;
      }
  }

  // ************ //
  // ** Actions ** //
  // ************ //

    public void RotateTowards (Vector3 targetCasePos) {
      Vector3 originCasePos = persoCase.transform.position;

      if (originCasePos.x > targetCasePos.x && originCasePos.y > targetCasePos.y)
        {
          ChangeRotation (Direction.SudOuest);
        }
      if (originCasePos.x > targetCasePos.x && originCasePos.y < targetCasePos.y)
        {
          ChangeRotation (Direction.NordOuest);
        }
      if (originCasePos.x < targetCasePos.x && originCasePos.y > targetCasePos.y)
        {
          ChangeRotation (Direction.SudEst);
        }
      if (originCasePos.x < targetCasePos.x && originCasePos.y < targetCasePos.y)
        {
          ChangeRotation (Direction.NordEst);
        }
      
    } 
}
