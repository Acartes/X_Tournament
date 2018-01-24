﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyBehaviour : MonoBehaviour {

  static public TransparencyBehaviour Instance;

    void Awake () {
      Instance = this;
    }

    public static void CheckTransparency (GameObject selectedObj, float alpha) {
      GameObject thisCase;
      if (selectedObj.name == "Ballon")
        {
          thisCase = selectedObj.GetComponent<BallonData>().ballonCase;
      } else
        {
          thisCase = selectedObj.GetComponent<PersoData>().persoCase;
        }

      CaseData go = thisCase.GetComponent<CaseData>();

      GameObject upperCase = GameObject.Find((go.xCoord - 1) + " " + (go.yCoord + 1));
      GameObject lowerCase = GameObject.Find(go.xCoord + 1 + " " + (go.yCoord - 1));


      if (upperCase != null && (upperCase.GetComponent<CaseData>().personnageData != null || upperCase.GetComponent<CaseData>().caseBallon != null))
        {
          thisCase = thisCase;
      } else if (lowerCase != null && lowerCase.GetComponent<CaseData>().personnageData != null)
        {
          thisCase = lowerCase;
        } else {
          return;
        }
      SpriteRenderer thisCaseSpriteR = thisCase.GetComponent<CaseData>().personnageData.GetComponent<SpriteRenderer>();
      Color transparency = new Color(thisCaseSpriteR.color.r, thisCaseSpriteR.color.g, thisCaseSpriteR.color.b, alpha);
      thisCase.GetComponent<CaseData>().personnageData.GetComponent<SpriteRenderer>().color = transparency;
    }

    IEnumerator Delay () {
      yield return new WaitForEndOfFrame();
    }
}
