using System;
using UnityEngine;

public class HoverArgs : EventArgs {

	public CaseData hoveredCase;
	public PersoData hoveredPersonnage;
	public BallonData hoveredBallon;
	public PathfindingCase Pathfinding;

	public HoverArgs(CaseData hoveredCase, PersoData hoveredPersonnage, PathfindingCase Pathfinding, BallonData hoveredBallon)
	{
		this.hoveredCase = hoveredCase;
		this.hoveredPersonnage = hoveredPersonnage;
		this.Pathfinding = Pathfinding;
		this.hoveredBallon = hoveredBallon;
	}
}