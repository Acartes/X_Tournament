using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;


public class LoadingManager : NetworkBehaviour
{
  [SyncVar] public bool P1_GameReady = false;
  [SyncVar] public bool P2_GameReady = false;

  public static LoadingManager Instance;

  int loadedPlayers;
  bool gameReady = false;
  bool startingGame = false;

  [SyncVar] public bool allGameReady = false;

  bool firstGameReady = true;

  public override void OnStartClient()
  {
    Instance = this;
  }



  [ClientRpc]
  public void RpcIsAllGameReady()
  {
    Debug.Log("UPDATE RpcIsAllGameReady");
    if (!P1_GameReady)
      {
        P1_GameReady = true;
      } else
      {
        P2_GameReady = true;
      }

    if (P1_GameReady && P2_GameReady)
      {
        allGameReady = true;
      }
  }

  public bool isGameReady()
  {
    if (gameReady)
      {
        if (!allGameReady)
          {
            if (firstGameReady)
              {
                firstGameReady = false;
                Debug.Log("isAllGameReady");
                RpcFunctions.Instance.CmdIsAllGameReady();
              }
            return false;
          } else
          {
            return true;
          }
      } else if (IsInstancesLoaded() && playerLoaded() && !startingGame)
      {
        startingGame = true;
        StartCoroutine(waitForReady());
      }
    return false;
  }

  IEnumerator waitForReady()
  {
    yield return new WaitForSeconds(0.01f);
    gameReady = true;
  }

  bool IsInstancesLoaded()
  {
    // Ajouter toutes les instances qui doivent être chargées pour lancer le jeu
    if (MenuManager.Instance == null
        || MoveBehaviour.Instance == null
        || PlacementBehaviour.Instance == null
        || ReplacerBalleBehaviour.Instance == null
        || TackleBehaviour.Instance == null
        || TransparencyBehaviour.Instance == null
        || CaseManager.Instance == null
        || ColorManager.Instance == null
        || GameManager.Instance == null
        || GraphManager.Instance == null
        || GrilleManager.Instance == null
        || HoverManager.Instance == null
        || InfoPerso.Instance == null
        || RosterManager.Instance == null
        || SelectionManager.Instance == null
        || TurnManager.Instance == null
        || UIManager.Instance == null
        || MenuContextuel.Instance == null
        || Pathfinding.Instance == null
        || RpcFunctions.Instance == null
        || EventManager.Instance == null)
      {
        return false;
      } else
      {
        Debug.Log("Instantiation complete");
        return true; // et du coup bah là c'est bon
      }
  }

  public bool playerLoaded()
  {
    if (loadedPlayers == 2)
      {
        return true;
      } else
      {
        loadedPlayers++;
        Debug.Log("Player" + loadedPlayers + " loaded.");
      }
    return false;
  }

}
