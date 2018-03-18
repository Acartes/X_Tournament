using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.Remoting.Messaging;

/// <summary>Gère tous les sorts.</summary>
public class SpellManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  /// <summary>Montre à quelle portée les personnages vont être projetés avant de le lancer</summary>
  public SpellData selectedSpell;
  GameObject newSummon;

  public bool isSpellCasting = false;

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
    EventManager.newClickEvent += OnNewClick;
    EventManager.newHoverEvent += OnNewHover;
  }

  void OnDisable()
  {
    if (LoadingManager.Instance.isGameReady())
      {
        TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
        EventManager.newClickEvent -= OnNewClick;
        EventManager.newHoverEvent -= OnNewHover;
      }
  }

  // *************** //
  // ** Events **    // Appel de fonctions au sein de ce script grâce à des events
  // *************** //

  void OnNewHover(object sender, HoverArgs e)
  {
    if (isSpellCasting)
      {
        selectedSpell.ShowAreaOfEffect();
        selectedSpell.ShowSummon();
      }
  }

  void OnNewClick()
  { // Lors d'un click sur une case
    if (!isSpellCasting)
      return;

    CaseData hoveredCase = HoverManager.Instance.hoveredCase;

    if ((Statut.atRange & hoveredCase.statut) == Statut.atRange)
      {
        SpellCall();
      } else
      {
        SpellEnd();
      }
  }

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

  /// <summary>Active le tooltip pour le sort</summary>
  public void SpellTooltipON(int IDSpell)
  {
    Tooltip.tooltipObj = ChooseSpell(IDSpell);
    UIManager.Instance.tooltip.SetActive(true);
  }

  /// <summary>Désactive le tooltip pour le sort</summary>
  public void SpellTooltipOFF()
  {
    UIManager.Instance.tooltip.SetActive(false);
  }

  /// <summary>Montre comment le sort doit être lancé, un précast. 
  /// Mettre un chiffre correspondant à l'ordre des boutons de sorts du personnage (0 = spell1; 1 = spell2)</summary>
  public void SpellCast(int IDSpell)
  {
    if (isSpellCasting)
      return;
      
    RpcFunctions.Instance.CmdCastSpell(IDSpell);
  }

  [ClientRpc]
  public void RpcSpellCast(int IDSpell)
  {
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
    if (selectedPersonnage == null)
      return;

    selectedSpell = ChooseSpell(IDSpell);
      
    if (selectedSpell == null)
      return;
      
    if (selectedPersonnage.actualPointAction < selectedSpell.costPA)
      return;

    GameManager.Instance.actualAction = PersoAction.isCasting;
    SelectionManager.Instance.DisablePersoSelection();
    TurnManager.Instance.DisableFinishTurn();
    selectedSpell.ShowRange();
    selectedSpell.ShowAreaOfEffect();
    selectedSpell.ShowPushEffect();
    selectedSpell.ShowSummon();
    isSpellCasting = true;
  }

  /// <summary>Le sort est lancé à un endroit</summary>
  void SpellCall()
  {
    StartCoroutine(SpellEnd());

  }

  IEnumerator SpellEnd()
  {
    isSpellCasting = false;
    foreach (CaseData obj in CaseManager.listAllCase)
      {
        obj.ChangeStatut(Statut.None, Statut.atRange);
        obj.ChangeStatut(Statut.None, Statut.atAoE);
        obj.ChangeStatut(Statut.None, Statut.atPush);
      }

    yield return new WaitForSeconds(0.5f);

    GameManager.Instance.actualAction = PersoAction.isSelected;
    SelectionManager.Instance.EnablePersoSelection();
    TurnManager.Instance.EnableFinishTurn();
  }

  /// <summary>Cible le bon sort entre les boutons</summary>
  SpellData ChooseSpell(int IDSpell)
  {
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
    if (selectedPersonnage == null)
      return null;

    if (IDSpell == 0)
      return SelectionManager.Instance.selectedPersonnage.Spell1;

    if (IDSpell == 1)
      return SelectionManager.Instance.selectedPersonnage.Spell2;

    return null;
  }
}
