﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InfoPerso : NetworkBehaviour
{

  // *************** //
  // ** Variables ** //
  // *************** //

    public GameObject infoPersoPortraits;
    public GameObject infoPersoStats;

    public static InfoPerso Instance;

    public Color blueOwnerColor;
    public Color redOwnerColor;
    public Color noneOwnerColor;

    public List<PersoData> characterList;

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
        infoPersoPortraits = GameObject.Find("infoPersoPortraits");
        infoPersoStats = GameObject.Find("infoPersoStats");
        characterList = new List<PersoData>();
    }

  // ************* //
  // ** Actions ** //
  // ************* //

    void Update()
    {
      if (!LoadingManager.Instance.isGameReady())
        ChangeUI();
    }

    void ChangeUI()
    {
      /*  if (SelectionManager.Instance.selectedPersonnage != null)
        {
            characterList.Clear();

            IsVisible(true);

          PersoData selectedPersoData = SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>();
          SpriteRenderer selectedSpriteRenderer = SelectionManager.Instance.selectedPersonnage.GetComponent<SpriteRenderer>();

          infoPersoPortraits.GetComponent<infoPersoPortraits>().Clear();

            if (GameObject.FindGameObjectsWithTag("Personnage") == null)
                return;

          foreach (PersoData perso in RosterManager.Instance.listHero)
            {
                if (perso.GetComponent<PersoData>().owner == selectedPersoData.owner)
                    characterList.Add(perso);
            }
              
            infoPersoPortraits.GetComponent<infoPersoPortraits>().setMainPortrait(selectedSpriteRenderer.sprite, selectedPersoData.owner);

            characterList.Remove(SelectionManager.Instance.selectedPersonnage);
            if (characterList.Count > 0)
                infoPersoPortraits.GetComponent<infoPersoPortraits>().setSubPortrait1(characterList[0].GetComponent<SpriteRenderer>().sprite, characterList[0].GetComponent<PersoData>().owner);
          infoPersoPortraits.GetComponent<infoPersoPortraits>().SubPortrait1.GetComponent<PortraitInteractive>().newHoveredPersonnage = characterList[0];
            if (characterList.Count > 1)
                infoPersoPortraits.GetComponent<infoPersoPortraits>().setSubPortrait2(characterList[1].GetComponent<SpriteRenderer>().sprite, characterList[1].GetComponent<PersoData>().owner);
            infoPersoPortraits.GetComponent<infoPersoPortraits>().SubPortrait2.GetComponent<PortraitInteractive>().newHoveredPersonnage = characterList[1];
            if (characterList.Count > 2)
                infoPersoPortraits.GetComponent<infoPersoPortraits>().setSubPortrait3(characterList[2].GetComponent<SpriteRenderer>().sprite, characterList[2].GetComponent<PersoData>().owner);
            infoPersoPortraits.GetComponent<infoPersoPortraits>().SubPortrait3.GetComponent<PortraitInteractive>().newHoveredPersonnage = characterList[2];

            infoPersoStats.GetComponent<infoPersoStats>().changePr(selectedPersoData.actualPointResistance, selectedPersoData.pointResistance);
            infoPersoStats.GetComponent<infoPersoStats>().changePm(selectedPersoData.actualPointMovement, selectedPersoData.pointMovement);
            infoPersoStats.GetComponent<infoPersoStats>().changePo(selectedPersoData.actualPointResistance, selectedPersoData.pointResistance);
        }
        else
        {
            //  GetComponent<Image>().color = noneOwnerColor;
          IsVisible(false);
        }*/
    }

    void IsVisible (bool isVisible) 
    { // rend l'interface visible ou invisible
      infoPersoPortraits.SetActive(isVisible);
      infoPersoStats.SetActive(isVisible);
    }
}
