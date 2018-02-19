using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{

    public static LoadingManager Instance;


    // Use this for initialization
    void Start()
    {
        Instance = this;
    }

    public bool IsInstancesLoaded()
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
            || Fonction.Instance == null
            || MenuContextuel.Instance == null
            || Pathfinding.Instance == null)
        {
            return false;
        }
        else
            Debug.Log("Instantiation complete");
        return true; // et du coup bah là c'est bon
    }
}
