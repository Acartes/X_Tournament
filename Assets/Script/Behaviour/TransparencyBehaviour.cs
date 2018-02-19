using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TransparencyBehaviour : NetworkBehaviour
{

  static public TransparencyBehaviour Instance;

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("TransparencyBehaviour is Instanced");
    }

    public static void CheckTransparency (GameObject selectedObj, float alpha) 
    {
      CaseData thisCase;
      if (selectedObj.name == "Ballon")
        {
          thisCase = selectedObj.GetComponent<BallonData>().ballonCase;
      } else
        {
          thisCase = selectedObj.GetComponent<PersoData>().persoCase;
        }

    if (thisCase == null)
      return;

      CaseData go = thisCase.GetComponent<CaseData>();

      CaseData upperCase = GameObject.Find((go.xCoord - 1) + " " + (go.yCoord + 1)).GetComponent<CaseData>();
        CaseData lowerCase = GameObject.Find(go.xCoord + 1 + " " + (go.yCoord - 1)).GetComponent<CaseData>();

      if (upperCase != null && (upperCase.GetComponent<CaseData>().personnageData != null || upperCase.GetComponent<CaseData>().ballon != null))
        {

      } else if (lowerCase != null && lowerCase.GetComponent<CaseData>().personnageData != null)
        {
          thisCase = lowerCase;
        } else {
          return;
        }
    if (thisCase.GetComponent<CaseData>().personnageData != null)
      {
        SpriteRenderer thisCaseSpriteR = thisCase.GetComponent<CaseData>().personnageData.GetComponent<SpriteRenderer>();
        Color transparency = new Color(thisCaseSpriteR.color.r, thisCaseSpriteR.color.g, thisCaseSpriteR.color.b, alpha);
        thisCase.GetComponent<CaseData>().personnageData.GetComponent<SpriteRenderer>().color = transparency;
      }
    }

    IEnumerator Delay () {
      yield return new WaitForEndOfFrame();
    }
}
