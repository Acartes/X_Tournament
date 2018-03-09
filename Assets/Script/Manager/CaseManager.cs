using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;

/// <summary>Gère toute les cases du jeu, et permet de connaître la distance qui les sépare les uns des autres en x et y avec xCaseOffset et yCaseOffset.</summary>
public class CaseManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //
  [Header("  Cases")]
  [Tooltip("La liste de toutes les cases de la grille.")] 
  [ReadOnly]
  static public List<GameObject> listAllCase;
  [Tooltip("Distance entre les cases en x.")]
  public float xCaseOffset = 1;
  [Tooltip("Distance entre les cases en y.")]
  public float yCaseOffset = 0.3f;

  [HideInInspector] public static CaseManager Instance;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log("CaseManager is Instanced");
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    while (!LoadingManager.Instance.isGameReady())
      yield return new WaitForEndOfFrame();
    Init();
  }

  void Init()
  {
    listAllCase.Clear();
    foreach (GameObject cubeNew in GameObject.FindGameObjectsWithTag("case"))
      {
        listAllCase.Add(cubeNew);
      }
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  public void ClearAllCases()
  {
    /// <summary>Remet la valeur de toutes les cases par défaut</summary> 
    foreach (GameObject newCaseGMB in listAllCase)
      {
        CaseData newCase = newCaseGMB.GetComponent<CaseData>();
        newCase.ballon = null;
        newCase.casePathfinding = PathfindingCase.Walkable;
        newCase.personnageData = null;
        newCase.statut = 0;
      }
  }

  /// <summary>Colore toutes les cases de la couleur choisi.</summary> 
  static public void PaintAllCase(Color newColor)
  {
    foreach (GameObject newCaseGMB in listAllCase)
      {
        newCaseGMB.GetComponent<SpriteRenderer>().color = newColor;
      }
  }

  /// <summary>Change le sprite de toutes les cases avec le sprite choisi.</summary> 
  static public void ChangeSpriteAllCase(Sprite newSprite)
  {
    foreach (GameObject newCaseGMB in listAllCase)
      {
        newCaseGMB.GetComponent<SpriteRenderer>().sprite = newSprite;
      }
  }

  /// <summary>Obtenir toutes les cases du jeu. </summary>
  static public List<CaseData> GetAllCase()
  {
    List<CaseData> newList = new List<CaseData>();

    foreach (GameObject newCaseGMB in listAllCase)
      {
        CaseData newCase = newCaseGMB.GetComponent<CaseData>();
        newList.Add(newCase);
      }

    return newList;
  }

  /// <summary>Obtenir toutes les cases possédant un ballon.</summary>
  static public List<CaseData> GetAllCaseWithBallon()
  {
    List<CaseData> newList = new List<CaseData>();

    foreach (GameObject newCaseGMB in listAllCase)
      {
        CaseData newCase = newCaseGMB.GetComponent<CaseData>();
        if (newCase.ballon != null)
          newList.Add(newCase);
      }
    return newList;
  }

  /// <summary>Obtenir toutes les cases possédant un personnage.</summary>
  static public List<CaseData> GetAllCaseWithPersonnage()
  {
    List<CaseData> newList = new List<CaseData>();

    foreach (GameObject newCaseGMB in listAllCase)
      {
        CaseData newCase = newCaseGMB.GetComponent<CaseData>();
        if (newCase.personnageData != null)
          newList.Add(newCase);
      }
    return newList;
  }

  /// <summary>Obtenir toutes les cases sans personnage et sans ballon.</summary>
  static public List<CaseData> GetAllCaseWithNothing()
  {
    List<CaseData> newList = new List<CaseData>();

    foreach (GameObject newCaseGMB in listAllCase)
      {
        CaseData newCase = newCaseGMB.GetComponent<CaseData>();
        if (newCase.personnageData == null && newCase.ballon == null)
          newList.Add(newCase);
      }
    return newList;
  }

  /// <summary>Obtenir toutes les cases avec le statut choisi.</summary>
  static public List<CaseData> GetAllCaseWithStatut(Statut statut)
  {
    List<CaseData> newList = new List<CaseData>();

    foreach (GameObject newCaseGMB in listAllCase)
      {
        CaseData newCase = newCaseGMB.GetComponent<CaseData>();
        if ((statut & newCase.statut) == statut)
          newList.Add(newCase);
      }
    return newList;
  }

  /// <summary>Obtenir toutes les cases avec la couleur choisie.</summary>
  static public List<CaseData> GetAllCaseWithColor(Color color)
  {
    List<CaseData> newList = new List<CaseData>();

    foreach (GameObject newCaseGMB in listAllCase)
      {
        CaseData newCase = newCaseGMB.GetComponent<CaseData>();
        SpriteRenderer SpriteR = newCaseGMB.GetComponent<SpriteRenderer>();
        if (SpriteR.color == color)
          newList.Add(newCase);
      }
    return newList;
  }
}
