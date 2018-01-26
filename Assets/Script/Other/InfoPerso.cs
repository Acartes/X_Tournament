using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPerso : MonoBehaviour
{

    public GameObject infoPersoPortraits;
    public GameObject infoPersoStats;

    public static InfoPerso Instance;

    public PersoData selectedPersoData;
    public SpriteRenderer selectedSpriteRenderer;

    public Color blueOwnerColor;
    public Color redOwnerColor;
    public Color noneOwnerColor;

    public List<GameObject> characterList;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
        infoPersoPortraits = GameObject.Find("infoPersoPortraits");
        infoPersoStats = GameObject.Find("infoPersoStats");
        characterList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectionManager.Instance.selectedPersonnage != null)
        {
            characterList.Clear();

            infoPersoPortraits.SetActive(true);
            infoPersoStats.SetActive(true);

            selectedPersoData = SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>();
            selectedSpriteRenderer = SelectionManager.Instance.selectedPersonnage.GetComponent<SpriteRenderer>();

            switch (selectedPersoData.owner)
            {
                case Player.Red:
                    infoPersoPortraits.GetComponent<infoPersoPortraits>().Clear();
                    break;
                case Player.Blue:
                    infoPersoPortraits.GetComponent<infoPersoPortraits>().Clear();
                    break;
            }

            if (GameObject.FindGameObjectsWithTag("Personnage") == null)
                return;

            foreach (GameObject perso in GameObject.FindGameObjectsWithTag("Personnage"))
            {
                if (perso.GetComponent<PersoData>().owner == selectedPersoData.owner)
                    characterList.Add(perso);
            }


            infoPersoPortraits.GetComponent<infoPersoPortraits>().setMainPortrait(selectedSpriteRenderer.sprite, selectedPersoData.owner);

            characterList.Remove(SelectionManager.Instance.selectedPersonnage);
            if (characterList.Count > 0)
                infoPersoPortraits.GetComponent<infoPersoPortraits>().setSubPortrait1(characterList[0].GetComponent<SpriteRenderer>().sprite, characterList[0].GetComponent<PersoData>().owner);
            if (characterList.Count > 1)
                infoPersoPortraits.GetComponent<infoPersoPortraits>().setSubPortrait2(characterList[1].GetComponent<SpriteRenderer>().sprite, characterList[1].GetComponent<PersoData>().owner);
            if (characterList.Count > 2)
                infoPersoPortraits.GetComponent<infoPersoPortraits>().setSubPortrait3(characterList[2].GetComponent<SpriteRenderer>().sprite, characterList[2].GetComponent<PersoData>().owner);

            infoPersoStats.GetComponent<infoPersoStats>().changePr(selectedPersoData.actualPointResistance, selectedPersoData.pointResistance);
            infoPersoStats.GetComponent<infoPersoStats>().changePm(selectedPersoData.actualPointMovement, selectedPersoData.pointMovement);
            infoPersoStats.GetComponent<infoPersoStats>().changePo(selectedPersoData.actualPointResistance, selectedPersoData.pointResistance);
        }
        else
        {
            GetComponent<Image>().color = noneOwnerColor;
            infoPersoPortraits.SetActive(false);
            infoPersoStats.SetActive(false);
        }
    }
}
