using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitInteractive : MonoBehaviour {

    public PersoData newHoveredPersonnage;

    public void HoverPerso() // hover comme chez HoverEvent
    {

        CaseData hoveredCase = newHoveredPersonnage.persoCase;
        PersoData hoveredPersonnage = newHoveredPersonnage;
        PathfindingCase hoveredPathfinding = newHoveredPersonnage.persoCase.casePathfinding;

        HoverEvent.newHoverEvent(this, new HoverArgs(hoveredCase, hoveredPersonnage, hoveredPathfinding, null));
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
