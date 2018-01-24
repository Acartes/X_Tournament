using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColorManager : MonoBehaviour {

  // *************** //
  // ** Variables ** //
  // *************** //
  [Header("  Couleur des cases")]
  public Color hoverColor;
  public Color hoverStrongColor;
  public Color caseColor;
  public Color enemyColor;
  public Color travelColor;
  public Color actionPreColor;
  public Color actionColor;
  public Color placementZoneRed;
  public Color placementZoneBlue;
  public Color moveColor;
  public Color selectedColor;
  public Color goalColor;

  [Header("  Couleur des personnages")]
  public Color punchedPersonnageColor;

  [HideInInspector] public static ColorManager Instance;

  // *************** //
  // ** Initialisation ** //
  // *************** //
    void Awake () {
      if (Instance == null)
        {
          Instance = this;
        }
  }

	void Update () {
    if (Instance == null)
      {
        Instance = this;
      }
	}
}
