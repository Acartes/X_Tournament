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

    public infoPersosPortrait portrait1;
    public infoPersosPortrait portrait2;
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
        IsVisible(false);
        while (RosterManager.Instance.listHero.Count != 8)
            yield return new WaitForEndOfFrame();
        portrait1.SetupChangePlayerIcons(Player.Red, TurnManager.Instance.TurnNumber);
        portrait2.SetupChangePlayerIcons(Player.Blue, TurnManager.Instance.TurnNumber);
        IsVisible(true);
    }

    public void PersoSelected(PersoData newPerso)
    {
        if (GameManager.Instance.currentPlayer == Player.Red)
            portrait1.SelectPerso(newPerso);
        if (GameManager.Instance.currentPlayer == Player.Blue)
            portrait2.SelectPerso(newPerso);
    }

    public void PersoPlaced(PersoData newPerso)
    { // Lors d'un click sur une case
        if (GameManager.Instance.currentPlayer == Player.Red)
            portrait1.GrayPortraitPerso(newPerso);
        if (GameManager.Instance.currentPlayer == Player.Blue)
            portrait2.GrayPortraitPerso(newPerso);
    }

    public void PersoRemoved(PersoData newPerso)
    { // Lors d'un click sur une case
        if (GameManager.Instance.currentPlayer == Player.Red)
            portrait1.UnGrayPortraitPerso(newPerso);
        if (GameManager.Instance.currentPlayer == Player.Blue)
            portrait2.UnGrayPortraitPerso(newPerso);
    }

    // ************* //
    // ** Actions ** //
    // ************* //

    void IsVisible(bool isVisible)
    { // rend l'interface visible ou invisible
        portrait1.gameObject.SetActive(isVisible);
        portrait2.gameObject.SetActive(isVisible);
        stats.gameObject.SetActive(isVisible);
    }
}
