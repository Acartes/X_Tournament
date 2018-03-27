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

  /// <summary>Montre à quelle portée les personnages vont être projetés avant de le lancer</summary>
  public SpellData selectedSpell;
  public SummonData newSummon;
  bool spellSuccess = false;

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
        if (newSummon != null)
          selectedSpell.ShowSummon(newSummon);
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
        StartCoroutine(SpellEnd());
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
    Debug.Log("GGGGGGGGGGGGGGGGGGG"); 
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
    if (selectedPersonnage == null)
      return;

    selectedSpell = ChooseSpell(IDSpell);
      
    if (selectedSpell == null)
      return;
      
    if (selectedPersonnage.actualPointAction < selectedSpell.costPA)
      return;

    Debug.Log("ZZZZZZZZZZZZZZz"); 
    GameManager.Instance.actualAction = PersoAction.isCasting;
    SelectionManager.Instance.DisablePersoSelection();
    TurnManager.Instance.DisableFinishTurn();
    CaseData hoveredCase = HoverManager.Instance.hoveredCase;
    selectedSpell.ShowRange();
    selectedSpell.ShowAreaOfEffect();
    selectedSpell.ShowPushEffect();
    //  newSummon = null;
    if (selectedSpell.summonedObj != null)
      {
        newSummon = (SummonData)Instantiate(selectedSpell.summonedObj, hoveredCase.transform.position + selectedSpell.summonedObj.transform.position - selectedSpell.summonedObj.originPoint.position, Quaternion.identity);
        newSummon.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        selectedSpell.ShowSummon(newSummon);
      }

    isSpellCasting = true;
  }

  /// <summary>Le sort est lancé à un endroit</summary>
  void SpellCall()
  {
    CaseData hoveredCase = HoverManager.Instance.hoveredCase;
    if ((Statut.atRange & hoveredCase.statut) != Statut.atRange)
      return;

    spellSuccess = true;

    if (newSummon != null)
      {
        newSummon.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        newSummon.GetComponent<BoxCollider2D>().enabled = true;
      }
    foreach (CaseData obj in CaseManager.listAllCase)
      {

        if ((Statut.atAoE & obj.statut) == Statut.atAoE && obj.personnageData != null)
          {
            Debug.Log(SelectionManager.Instance.selectedPersonnage.name);
            SelectionManager.Instance.selectedPersonnage.RotateTowards(obj.gameObject);
            selectedSpell.ApplyEffect(obj.personnageData);
          }
        if ((Statut.atPush & obj.statut) == Statut.atPush)
          {
              
          }
      }
    StartCoroutine(SpellEnd());
  }

  IEnumerator SpellEnd()
  {
    isSpellCasting = false;
    foreach (CaseData obj in CaseManager.listAllCase)
      {
        obj.ChangeStatut(Statut.None, Statut.atRange);
        obj.ChangeStatut(Statut.None, Statut.atAoE);
      }

    if (!spellSuccess && newSummon != null)
      {
        Debug.Log("eeeeeeeeeeeee");
        DestroyImmediate(newSummon.gameObject);

      }

    spellSuccess = false;

    yield return new WaitForSeconds(0.5f);

    GameManager.Instance.actualAction = PersoAction.isSelected;
    SelectionManager.Instance.EnablePersoSelection();
    StartCoroutine(TurnManager.Instance.EnableFinishTurn());
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
