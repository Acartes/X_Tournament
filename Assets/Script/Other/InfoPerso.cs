using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPerso : MonoBehaviour
{

    public GameObject infoPersoPortrait;
    public GameObject infoPersoStats;

    public static InfoPerso Instance;

    public PersoData selectedPersoData;
    public SpriteRenderer selectedSpriteRenderer;

    public Color blueOwnerColor;
    public Color redOwnerColor;
    public Color noneOwnerColor;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        infoPersoPortrait = GameObject.Find("infoPersoPortrait");
        infoPersoStats = GameObject.Find("infoPersoStats");
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectionManager.Instance.selectedPersonnage != null)
        {
            infoPersoPortrait.SetActive(true);
            infoPersoStats.SetActive(true);

            selectedPersoData = SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>();
            selectedSpriteRenderer = SelectionManager.Instance.selectedPersonnage.GetComponent<SpriteRenderer>();

            switch (selectedPersoData.owner)
            {
                case Player.Blue:
                    GetComponent<Image>().color = blueOwnerColor;
                    break;
                case Player.Red:
                    GetComponent<Image>().color = redOwnerColor;
                    break;
            }

            infoPersoPortrait.GetComponent<Image>().sprite = selectedSpriteRenderer.sprite;
            infoPersoStats.GetComponent<Text>().text = (
              selectedSpriteRenderer.sprite.name + "\n" +
              "PR : " + selectedPersoData.actualPointResistance.ToString() + "/" + selectedPersoData.pointResistance.ToString() + "\n"
              + "PM : " + selectedPersoData.actualPointMovement.ToString() + "/" + selectedPersoData.pointMovement.ToString() + "\n"
              + "Portée : " + selectedPersoData.range.ToString() + "\n"
              + "Poids : " + selectedPersoData.weightType.ToString() + "\n"
            );
        }
        else
        {
            GetComponent<Image>().color = noneOwnerColor;
            infoPersoPortrait.SetActive(false);
            infoPersoStats.SetActive(false);
        }
    }
}
