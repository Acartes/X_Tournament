using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InfoPerso : NetworkBehaviour
{

    // *************** //
    // ** Variables ** //
    // *************** //

    public infoPersoPortraits portraits;
    public infoPersoStats stats;

    public static InfoPerso Instance;

    public Color blueOwnerColor;
    public Color redOwnerColor;

    // *********** //
    // ** Initialisation ** //
    // *********** //

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("InfoPerso is Instanced");
        StartCoroutine(waitForInit());
    }

    IEnumerator waitForInit()
    {
        while (!LoadingManager.Instance.isGameReady())
            yield return new WaitForEndOfFrame();
        Init();
    }

    private void Init()
    {
        Instance = this;
        portraits = transform.GetComponentInChildren<infoPersoPortraits>();
        stats = transform.GetComponentInChildren<infoPersoStats>();
        TurnManager.Instance.changeTurnEvent += OnChangeTurn;
    }

    void OnChangeTurn(object sender, PlayerArgs e)
    {
        if(TurnManager.Instance.currentPhase == Phase.Deplacement && RosterManager.Instance.listHeroPlaced.Count != 8)
        {
            IsVisible(false);
        }
        StartCoroutine(waitForList());
    }

    IEnumerator waitForList()
    {
        while (RosterManager.Instance.listHero.Count != 8)
            yield return new WaitForEndOfFrame();
        portraits.SetupChangePlayerIcons(TurnManager.Instance.currentPlayer, TurnManager.Instance.TurnNumber);
    }

    public void SelectPerso(PersoData newPerso)
    {
        portraits.SelectPerso(newPerso);
    }

    public void PlacePerso(PersoData newPerso)
    { // Lors d'un click sur une case
        portraits.GrayPortraitPerso(newPerso);
    }

    // ************* //
    // ** Actions ** //
    // ************* //

    void IsVisible(bool isVisible)
    { // rend l'interface visible ou invisible
        portraits.gameObject.SetActive(isVisible);
        stats.gameObject.SetActive(isVisible);
    }
}
