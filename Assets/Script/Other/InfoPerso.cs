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
        yield return new WaitForSeconds(0.01f);

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
        if (e.currentPhase == Phase.Placement)
        {
            IsVisible(false);
        }
        if (e.currentPhase == Phase.Deplacement)
        {
            IsVisible(true);
            portraits.ChangePlayerIcons(TurnManager.Instance.currentPlayer);
        }
    }

    public void SelectPerso(PersoData newPerso)
    {
        portraits.SelectPerso(newPerso);
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
