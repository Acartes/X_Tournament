using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
public class ColorManager : NetworkBehaviour {

  // *************** //
  // ** Variables ** //
  // *************** //
  [Header("  Couleur des cases")]
  public Color hoverColor;
  public Color hoverStrongColor;
  public Color caseColor;
  public Color enemyColor;
  public Color moveColor;
  public Color actionPreColor;
  public Color actionColor;
  public Color placementZoneRed;
  public Color placementZoneBlue;
  public Color isMovingColor;
  public Color selectedColor;
  public Color goalColor;

  [Header("  Couleur des personnages")]
  public Color punchedPersonnageColor;

  [HideInInspector] public static ColorManager Instance;

    // *************** //
    // ** Initialisation ** //
    // *************** //
    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("ColorManager is Instanced");

    }
}
