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

public enum PersoAction {
	isMoving,
	isReplacingBall,
	isShoting,
    isIdle,
    isSelected,
    isWaiting
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

// Pour indiquer le statut glisse par exemple
/*statut += (int)Statut.Glisse;
        statut += (int)Statut.Brule;*/

// Si le ballon brûle, alors
/*if ((Statut.Brule & statut) == Statut.Brule) {
            Debug.Log ("1");
        } else {
            Debug.Log ("0");
        }*/

[Flags] public enum Statut {
  brule = 1 << 0,
  glisse = 1 << 1,
  inammovible = 1 << 2,
  isSelected = 1 << 3,
  isHovered = 1 << 4,
  canPunch = 1 << 5,
  canShot = 1 << 6,
  canReplace = 1 << 7,
  isEnemyPerso = 1 << 8,
  isAllyPerso = 1 << 9,
  isWalkable = 1 << 10,
  isTackled = 1 << 11,
  canMove = 1 << 12,
  canBeTackled = 1 << 13,
  isMoving = 1 << 14,
  canPlace = 1 << 15,
  isGoal = 1 << 16,
  None = 1 << 17,
  canReturnTo = 1 << 18
}

[Flags] public enum BallonStatut {
  None = 1 << 0,
  isMoving = 1 << 1,
  canBounce = 1 << 2,
  isIntercepted = 1 << 3,
  isReturned = 1 << 4,
  a = 1 << 5,
  b = 1 << 6,
  c = 1 << 7,
  d = 1 << 8,
  e = 1 << 9,
  f = 1 << 10,
  g = 1 << 11,
  h = 1 << 12,
  i = 1 << 13,
  j = 1 << 14,
  k = 1 << 15,
  l = 1 << 16,
  m = 1 << 17
}