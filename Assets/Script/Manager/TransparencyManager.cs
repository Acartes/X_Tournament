using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TransparencyManager : NetworkBehaviour
{
  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  [Range(0, 1)] [Tooltip("Niveau de transparence")] public float alpha;
  [HideInInspector] static public TransparencyManager Instance;

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

  public void CheckCaseTransparency(CaseData Case, bool doRecursive = true)
  { // Check s'il y a un personnage ou un ballon au dessus ou en dessous de la case ciblée pour détecter s'il doit faire une transparence ou non.

    Debug.Log(GameManager.Instance.currentPhase);
      // GET CASE HAUT ET GET CASE BAS

      CaseData upperCase = Case.GetTopCase();
    CaseData lowerCase = Case.GetBottomCase();

    if (upperCase != null && upperCase.personnageData != null
        && Case != null && Case.personnageData != null)
      {
        ApplyTransparency(Case.personnageData);
      } else if (upperCase != null && upperCase.personnageData == null
                 && Case != null && Case.personnageData != null)
      {
        ApplyOpacity(Case.personnageData);
      }

    if (doRecursive)
      {
        CheckCaseTransparency(upperCase, false);
        CheckCaseTransparency(lowerCase, false);
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
