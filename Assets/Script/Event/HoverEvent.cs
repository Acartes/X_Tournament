using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class HoverEvent : NetworkBehaviour {

	public static EventHandler<HoverArgs> newHoverEvent;

	public CaseData hoveredCase;
	public PersoData hoveredPersonnage;
	public BallonData hoveredBallon;
	public PathfindingCase hoveredPathfinding;

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
        this.enabled = true;
    }

    void OnMouseOver()
    {
        if (!enabled || !LoadingManager.Instance.isGameReady())
            return;
		hoveredCase = this.GetComponent<CaseData>();
		hoveredPersonnage = GetComponent<CaseData> ().personnageData;
		hoveredPathfinding = GetComponent<CaseData> ().casePathfinding;
		hoveredBallon = GetComponent<CaseData> ().ballon;

        newHoverEvent (this, new HoverArgs (hoveredCase, hoveredPersonnage, hoveredPathfinding, hoveredBallon));
	}
    void OnMouseExit()
    {
        if (!enabled || !LoadingManager.Instance.isGameReady())
            return;

		hoveredCase = this.GetComponent<CaseData>();
		hoveredPersonnage = GetComponent<CaseData>().personnageData;
		hoveredPathfinding = GetComponent<CaseData>().casePathfinding;
		hoveredBallon = GetComponent<CaseData> ().ballon;

		//newHoverEvent (this, new HoverArgs (hoveredCase, hoveredPersonnage, hoveredPathfinding, hoveredBallon));
    }
}
