using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TransparencyBehaviour : NetworkBehaviour
{
  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  [Range(0, 1)] [Tooltip("Niveau de transparence")] public float alpha;
  [HideInInspector] static public TransparencyBehaviour Instance;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;

    Debug.Log(this.GetType() + " is Instanced");
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  public void CheckCaseTransparency(CaseData Case)
  { // Check s'il y a un personnage ou un ballon au dessus ou en dessous de la case ciblée pour détecter s'il doit faire une transparence ou non.

// PERSO SUR CASE CHOISI ?
    if (Case.personnageData == null)
      return;

// GET CASE HAUT ET GET CASE BAS
    CaseData upperCase = GameObject.Find((Case.xCoord - 1) + " " + (Case.yCoord + 1)) != null ? GameObject.Find((Case.xCoord + 1) + " " + (Case.yCoord - 1)).GetComponent<CaseData>() : null;
    CaseData lowerCase = GameObject.Find((Case.xCoord + 1) + " " + (Case.yCoord - 1)) != null ? GameObject.Find((Case.xCoord + 1) + " " + (Case.yCoord - 1)).GetComponent<CaseData>() : null;

    if (upperCase != null && (upperCase.personnageData != null || upperCase.ballon != null))
      ApplyTransparency(Case.personnageData);
    else
      ApplyOpacity(Case.personnageData);

    if (lowerCase != null && lowerCase.personnageData != null)
      ApplyTransparency(lowerCase.personnageData);
    else
      {
        //    ApplyOpacity(lowerCase.personnageData);
      }
  }

  public void ApplyTransparency(PersoData Perso)
  { // Applique la transparence du TransparencyBehaviour sur le personnage.
    SpriteRenderer CaseSpriteR = Perso.GetComponent<SpriteRenderer>();

    Color transparency = new Color(CaseSpriteR.color.r, CaseSpriteR.color.g, CaseSpriteR.color.b, alpha);
    Perso.GetComponent<SpriteRenderer>().color = transparency;
  }

  public void ApplyOpacity(PersoData Perso)
  { // Annule la transparence du personnage.
    SpriteRenderer CaseSpriteR = Perso.GetComponent<SpriteRenderer>();

    Color transparency = new Color(CaseSpriteR.color.r, CaseSpriteR.color.g, CaseSpriteR.color.b, 1);
    Perso.GetComponent<SpriteRenderer>().color = transparency;
  }
}
