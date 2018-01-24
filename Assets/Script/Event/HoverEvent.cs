using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HoverEvent : MonoBehaviour {

    public new bool enabled = true;

	public static EventHandler<HoverArgs> newHoverEvent;

	public GameObject hoveredCase;
	public GameObject hoveredPersonnage;
	public GameObject hoveredBallon;
	public PathfindingCase hoveredPathfinding;

    void Awake () {
    this.enabled = false;
    StartCoroutine(LateAwake());
  }

    IEnumerator LateAwake () {
      yield return new WaitForEndOfFrame();
      this.enabled = true;
    }

    void OnMouseOver()
    {
        if (!enabled)
            return;
		hoveredCase = this.gameObject;
		hoveredPersonnage = GetComponent<CaseData> ().personnageData;
		hoveredPathfinding = GetComponent<CaseData> ().casePathfinding;
		hoveredBallon = GetComponent<CaseData> ().caseBallon;

        newHoverEvent (this, new HoverArgs (hoveredCase, hoveredPersonnage, hoveredPathfinding, hoveredBallon));
	}
    void OnMouseExit()
    {
        if (!enabled)
            return;

		hoveredCase = this.gameObject;
		hoveredPersonnage = GetComponent<CaseData>().personnageData;
		hoveredPathfinding = GetComponent<CaseData>().casePathfinding;
		hoveredBallon = GetComponent<CaseData> ().caseBallon;

		//newHoverEvent (this, new HoverArgs (hoveredCase, hoveredPersonnage, hoveredPathfinding, hoveredBallon));
    }
}
