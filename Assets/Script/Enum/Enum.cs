using System;
using System.Runtime.Remoting.Messaging;

public enum Player
{
  Red,
  Blue,
  Neutral
}

public enum Phase
{
  Placement,
  Deplacement
}

public enum PathfindingCase
{
  Walkable,
  NonWalkable,
  None
}

public enum Element
{
  Feu,
  Air,
  Terre,
  Eau,
  Aucun
}

public enum PersoAction
{
  isMoving,
  isReplacingBall,
  isShoting,
  isIdle,
  isSelected,
  isWaiting,
  isCasting,
}

public enum Direction
{
  NordOuest,
  NordEst,
  SudOuest,
  SudEst
}

public enum WeightType
{
  Light,
  Heavy
}

[Flags] public enum Statut
{
  None = 1 << 0,
  // IsSomething
  isSelected = 1 << 1,
  isHovered = 1 << 2,
  isEnemyPerso = 1 << 3,
  isControllable = 1 << 4,
  isTackled = 1 << 5,
  isMoving = 1 << 6,
  // CanInteract
  canMove = 1 << 7,
  canBeTackled = 1 << 8,
  canPunch = 1 << 9,
  canShot = 1 << 10,
  canReplace = 1 << 11,
  // Goal
  goalRed = 1 << 12,
  goalBlue = 1 << 13,
  // Placement
  placementRed = 1 << 14,
  placementBlue = 1 << 15,
  // Effect
  willPush = 1 << 16,
  // Spells Feedback
  atRange = 1 << 17,
  atAoE = 1 << 18,
  atPush = 1 << 19,
}

[Flags] public enum BallonStatut
{
  None = 1 << 0,
  // IsSomething
  isMoving = 1 << 1,
  isIntercepted = 1 << 2,
  // CanInteract
  canBounce = 1 << 3
}