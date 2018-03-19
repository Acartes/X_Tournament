using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{

  public static SpellData tooltipObj;
  Text UIText;

  void Awake()
  {
    UIText = GetComponent<Text>();
  }

  void OnEnable()
  {
    if (tooltipObj == null)
      return;
      
    switch (name)
      {
      case "tooltipTitle":
        UIText.text = tooltipObj.tooltipTitle;
        break;
      case "tooltipEffect":
        UIText.text = tooltipObj.tooltipEffect;
        break;
      case "tooltipRange":
        UIText.text = tooltipObj.tooltipRange;
        break;
      }
  }
}
