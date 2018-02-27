using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReplacerBalleBehaviour : NetworkBehaviour
{

  // *************** //
  // ** Variables ** //
  // *************** //

  [Header("  Temps")]
  [Tooltip("La durée d'un déplacement entre deux cases.")]
  public List<CaseData> caseAction;


  public static ReplacerBalleBehaviour Instance;

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("ReplacerBalleBehaviour is Instanced");
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
      EventManager.newClickEvent += OnNewClick;
    }

    void OnDisable()
    {
      EventManager.newClickEvent -= OnNewClick;
    }

  public void OnNewClick ()
    { // Lors d'un click sur une case
          TirReplacerBalle();
    }

  public void ReplacerBalle () 
	{ // Action de replacer la balle après avoir cliquer sur le bouton approprié sur le menu contextuel
      Color caseColor = ColorManager.Instance.caseColor;
      CaseData selectedCase = SelectionManager.Instance.selectedCase;

      GameManager.Instance.actualAction = PersoAction.isReplacingBall;
		caseAction.Clear ();

       int xCoord = selectedCase.GetComponent<CaseData>().xCoord;
       int yCoord = selectedCase.GetComponent<CaseData>().yCoord;

      ReplacerBalleAdd((xCoord + 1), (yCoord + 0));
      ReplacerBalleAdd((xCoord - 1), (yCoord + 0));
      ReplacerBalleAdd((xCoord + 0), (yCoord + 1));
      ReplacerBalleAdd((xCoord + 0), (yCoord - 1));
	}

    void ReplacerBalleAdd (int xCoord, int yCoord) {
      Color actionPreColor = ColorManager.Instance.actionPreColor;

        if (GameObject.Find (xCoord.ToString () + " " + yCoord.ToString ()) != null
          && GameObject.Find (xCoord.ToString () + " " + yCoord.ToString ()).GetComponent<CaseData> ().casePathfinding != PathfindingCase.NonWalkable) {
            CaseData newCase = (GameObject.Find (xCoord.ToString () + " " + yCoord.ToString ())).GetComponent<CaseData>();
         caseAction.Add(newCase);
          newCase.GetComponent<CaseData>().ChangeColor(Statut.canReplace);
        }
    }

    public void TirReplacerBalle () {
      PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
        CaseData hoveredCase = HoverManager.Instance.hoveredCase;
      PersoAction actualAction = GameManager.Instance.actualAction;
      BallonData selectedBallon = SelectionManager.Instance.selectedBallon;

        if (actualAction == PersoAction.isReplacingBall
          && caseAction.Count != 0) 
        {
              if (caseAction.Contains(hoveredCase)) 
                {
					selectedBallon.transform.position = hoveredCase.transform.position;
              hoveredCase.GetComponent<CaseData>().ChangeColor(Statut.canReplace);
                  StartCoroutine(ReplacerBalleEnd ());
					return;
			}
          StartCoroutine(ReplacerBalleEnd ());
			selectedPersonnage.GetComponent<PersoData> ().actualPointMovement++;
		}
	}

  IEnumerator ReplacerBalleEnd () 
    { // Action de replacer la balle qui se termine après avoir placer la balle 
      Color caseColor = ColorManager.Instance.caseColor;
    
      yield return new WaitForEndOfFrame();
      GameManager.Instance.actualAction = PersoAction.isSelected;
   
        foreach (CaseData obj in caseAction) {
     //     obj.GetComponent<CaseData>().colorLock = false;
          obj.ChangeColor(Statut.None, Statut.canReplace);
        }
      TurnManager.Instance.StartCoroutine("EnableFinishTurn");
      CaseManager.Instance.StartCoroutine ("ShowActions");
      caseAction.Clear ();
    }
}
