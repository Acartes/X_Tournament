using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoPersoStats : MonoBehaviour {

    public GameObject Pr;
    public GameObject Pm;
    public GameObject Po; // no homo

    void Start()
    {
        changePm(SelectionManager.Instance.selectedPersonnage.maxPointMovement, SelectionManager.Instance.selectedPersonnage.maxPointMovement);
    }
    /*IEnumerator waitForEvent()
    {
        while (!LoadingManager.Instance.isGameReady())
            yield return new WaitForEndOfFrame();

        changeTurnEvent(this, new PlayerArgs(currentPlayer, currentPhase));
    }*/
    // Use this for initialization
    public void changePr(int pr, int maxPr)
    {
        Pr.GetComponent<Text>().text = "PR \n" + pr + "/" + maxPr;
    }
    // Use this for initialization
    public void changePm(int pm, int maxPm)
    {
        Pm.GetComponent<Text>().text = "PM \n" + pm + "/" + maxPm;

    }
    // Use this for initialization
    public void changePo(int po, int maxPo)
    {
        Po.GetComponent<Text>().text = "PO \n" + po + "/" + maxPo;

    }
}
