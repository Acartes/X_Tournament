using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RosterManager : NetworkBehaviour
{

    // *************** //
    // ** Variables ** //
    // *************** //

    public List<PersoData> listHeroJ1;
    public List<PersoData> listHeroJ2;
    public List<List<PersoData>> listHeroJXToPlace = new List<List<PersoData>>();
    [ReadOnly] public List<PersoData> listHero;
    [ReadOnly] public List<PersoData> listHeroPlaced;

    [HideInInspector] public static RosterManager Instance;

    public PersoData persoCreated;

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("RosterManager is Instanced");
        StartCoroutine(waitForInit());
    }

    IEnumerator waitForInit()
    {
        while (!LobbyManager.Instance.IsInstancesLoaded())
            yield return new WaitForEndOfFrame();
        Init();
    }

    private void Init()
    {
        GameObject persoCreated;
        listHeroJXToPlace.Add(new List<PersoData>());
        listHeroJXToPlace.Add(new List<PersoData>());
        foreach (PersoData obj in listHeroJ1)
        {
            persoCreated = (GameObject)Instantiate(obj.gameObject, new Vector3(999, 999, 999), Quaternion.identity);
            persoCreated.GetComponent<PersoData>().owner = Player.Red;
            listHeroJXToPlace[0].Add(persoCreated.GetComponent<PersoData>());
        }

        foreach (PersoData obj in listHeroJ2)
        {
            persoCreated = (GameObject)Instantiate(obj.gameObject, new Vector3(999, 999, 999), Quaternion.identity);
            persoCreated.GetComponent<PersoData>().owner = Player.Blue;
            listHeroJXToPlace[1].Add(persoCreated.GetComponent<PersoData>());
        }

        listHeroJ1.Clear();
        listHeroJ2.Clear();
        listHero.AddRange(listHeroJXToPlace[0]);
        listHero.AddRange(listHeroJXToPlace[1]);
    }
}
