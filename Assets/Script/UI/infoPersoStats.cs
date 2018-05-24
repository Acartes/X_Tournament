﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;

public class infoPersoStats : NetworkBehaviour
{
  public GameObject PrRed;
  public GameObject PmRed;
  public GameObject PoRed;
 
  public GameObject PrBlue;
  public GameObject PmBlue;
  public GameObject PoBlue;

  // Use this for initialization
  public void changePr(int pr, int maxPr)
  {
    if (SelectionManager.Instance.selectedPersonnage.owner == Player.Red)
      PrRed.GetComponent<Text>().text = pr + " ";
    if (SelectionManager.Instance.selectedPersonnage.owner == Player.Blue)
      PrBlue.GetComponent<Text>().text = pr + " ";
  }
  // Use this for initialization
  public void changePm(int pm, int maxPm)
  {
    if (SelectionManager.Instance.selectedPersonnage.owner == Player.Red)
      PmRed.GetComponent<Text>().text = pm + " ";
    if (SelectionManager.Instance.selectedPersonnage.owner == Player.Blue)
      PmBlue.GetComponent<Text>().text = pm + " ";
  }
  // Use this for initialization
  public void changePo(int po, int maxPo)
  {
    if (SelectionManager.Instance.selectedPersonnage.owner == Player.Red)
      PoRed.GetComponent<Text>().text = po + " ";
    if (SelectionManager.Instance.selectedPersonnage.owner == Player.Blue)
      PoBlue.GetComponent<Text>().text = po + " ";
  }
}
