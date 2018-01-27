using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitInteractive : MonoBehaviour {

    public GameObject newHoveredPersonnage;

    public void HoverPerso() // hover comme chez HoverEvent
    {

        GameObject hoveredCase = newHoveredPersonnage.GetComponent<PersoData>().persoCase;
        GameObject hoveredPersonnage = newHoveredPersonnage;
        PathfindingCase hoveredPathfinding = newHoveredPersonnage.GetComponent<PersoData>().persoCase.GetComponent<CaseData>().casePathfinding;
        GameObject hoveredBallon = null;

        HoverEvent.newHoverEvent(this, new HoverArgs(hoveredCase, hoveredPersonnage, hoveredPathfinding, hoveredBallon));
    }

    public void UnHoverPerso() // exit comme chez HoverEvent
    {

        PathfindingCase hoveredPathfinding = newHoveredPersonnage.GetComponent<PersoData>().persoCase.GetComponent<CaseData>().casePathfinding;

        HoverEvent.newHoverEvent(this, new HoverArgs(null, null, hoveredPathfinding, null));
    }

    public void ClickPerso()
    {
        ClickEvent.newClickEvent();

        StartCoroutine(waitForHover()); // très important sinon le code visuel s'execute après le hover
    }

    IEnumerator waitForHover() // très important sinon le code visuel s'execute après le hover
    {
        yield return new WaitForSeconds(0.05f);
        HoverPerso();
    }
}
