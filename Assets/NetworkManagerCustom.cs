using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class LobbyManager : NetworkManager
{
    static public LobbyManager Instance;

    public int playerId;

    public override void OnStartServer()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    protected int currentPlayers;

    public override void OnServerConnect(NetworkConnection conn)
    {
        playerId = conn.connectionId;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == onlineScene)
        {
        }
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public bool IsInstancesLoaded(){

        // Ajouter toutes les instances qui doivent être chargées pour lancer le jeu
        if ( MenuManager.Instance == null
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
            || Fonction.Instance == null
            || MenuContextuel.Instance == null
            || Pathfinding.Instance == null) {
            return false;
        }
        else
            Debug.Log("Instantiation complete");
        return true; // et du coup bah là c'est bon
        }
}
