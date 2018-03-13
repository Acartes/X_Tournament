using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>Gère tous les sorts.</summary>
public class SpellManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public List<GameObject> listSpell;
  SpellData selectedSpell;

  public static SpellManager Instance;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log(this.GetType() + " is Instanced");
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    while (!LoadingManager.Instance.isGameReady())
      yield return new WaitForEndOfFrame();
    Init();
  }

  private void Init()
  {
    TurnManager.Instance.changeTurnEvent += OnChangeTurn;
  }

  void OnDisable()
  {
    if (LoadingManager.Instance.isGameReady())
      {
        TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
      }
  }

  // *************** //
  // ** Events **    // Appel de fonctions au sein de ce script grâce à des events
  // *************** //

  void OnChangeTurn(object sender, PlayerArgs e)
  {
    switch (e.currentPlayer)
      {
      case Player.Red:
            
        break;
      case Player.Blue:
            
        break;
      }

    switch (e.currentPhase)
      {
      case Phase.Placement:

        break;
      case Phase.Deplacement:

        break;
      }
  }

  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  /// <summary>Mettre un chiffre correspondant à l'ordre des boutons de sorts du personnage (0 = spell1; 1 = spell2)</summary>
  public void SpellCast(int IDSpell)
  {
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;

    switch (IDSpell)
      {
      case 0:
        selectedSpell = SelectionManager.Instance.selectedPersonnage.Spell1;
        break;
      case 1:
        selectedSpell = SelectionManager.Instance.selectedPersonnage.Spell2;
        break;
      default:
        return;
      }

    if (selectedPersonnage.actualPointAction < selectedSpell.costPA)
      return;

    selectedPersonnage.actualPointAction -= selectedSpell.costPA;

    SelectionManager.Instance.DisablePersoSelection();
    TurnManager.Instance.DisableFinishTurn();
    ShowRange();
    ShowAreaOfEffect();
    ShowPushEffect();
  }

  void ShowRange()
  {
    int range = selectedSpell.range;
    CaseData caseOrigin = SelectionManager.Instance.selectedCase;
    List<CaseData> list = new List<CaseData>();
    List<CaseData> list2 = new List<CaseData>();
    list.Add(caseOrigin);

    for (int i = 0; i < range; i++)
      {
        foreach (CaseData obj in list)
          {
            list2.Add(obj.GetBottomLeftCase());
            list2.Add(obj.GetBottomRightCase());
            list2.Add(obj.GetTopLeftCase());
            list2.Add(obj.GetTopRightCase());
            list.Remove(obj);
          }
        list.AddRange(list2);
        list.Clear();
      }
  }

  void ShowAreaOfEffect()
  {
    int AoE = selectedSpell.areaOfEffect;
  }

  void ShowPushEffect()
  {
    int push = selectedSpell.pushValue;
  }

  /// <summary>Mettre un chiffre correspondant à l'ordre des boutons de sorts du personnage (0 = spell1; 1 = spell2)</summary>
  void SpellCall(GameObject selectedSpell)
  {
    /*  SpellData newSpell;
    SpellData selectedSpell;
    switch (IDSpell)
      {
      case 0:
        selectedSpell = SelectionManager.Instance.selectedPersonnage.Spell1;
        break;
      case 1:
        selectedSpell = SelectionManager.Instance.selectedPersonnage.Spell2;
        break;
      default:
        return;
      }
    // Instantiate(newSpell, 
*/
    SelectionManager.Instance.EnablePersoSelection();
    TurnManager.Instance.EnableFinishTurn();
  }
}
