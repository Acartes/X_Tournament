using System;

public enum Player {
		Red,
		Blue,
		Neutral
}

public enum Phase {
	Placement,
	Deplacement
}

public enum PathfindingCase {
	Walkable,
	NonWalkable,
  None
}

public enum Element {
	Feu,
	Air,
	Terre,
	Eau,
	Aucun
}

[Flags] public enum Statut {
	Brule = 1 << 0,
	Glisse = 1 << 1,
	Inammovible = 1 << 2
}

public enum PersoAction {
	isMoving,
	isReplacingBall,
	isShoting,
  isIdle,
  isSelected
}

public enum Direction {
	NordOuest,
	NordEst,
	SudOuest,
	SudEst
}

public enum WeightType {
  Light,
  Heavy
}