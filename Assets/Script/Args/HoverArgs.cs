using System;
using UnityEngine;

public class HoverArgs : EventArgs {

	public GameObject hoveredCase;
	public GameObject hoveredPersonnage;
	public GameObject hoveredBallon;
	public PathfindingCase Pathfinding;

	public HoverArgs(GameObject hoveredCase, GameObject hoveredPersonnage, PathfindingCase Pathfinding, GameObject hoveredBallon)
	{
		this.hoveredCase = hoveredCase;
		this.hoveredPersonnage = hoveredPersonnage;
		this.Pathfinding = Pathfinding;
		this.hoveredBallon = hoveredBallon;
	}
}