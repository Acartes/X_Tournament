﻿using System.Collections;
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
  bool spellSuccess = false;

  public bool isSpellCasting = false;

  public GameObject ownerCircle;

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
    EventManager.newClickEvent += OnNewClick;
    EventManager.newHoverEvent += OnNewHover;
  }

  void OnDisable()
  {
    if (LoadingManager.Instance != null && LoadingManager.Instance.isGameReady())
      {
        EventManager.newClickEvent -= OnNewClick;
        EventManager.newHoverEvent -= OnNewHover;
      }
  }

  // *************** //
  // ** Events **    // Appel de fonctions au sein de ce script grâce à des events
  // *************** //

  void OnNewHover(object sender, HoverArgs e)
  {
    if (GameManager.Instance.actualAction != PersoAction.isCasting)
      return;

    selectedSpell.ShowAllFeedbacks();
    SelectionManager.Instance.selectedPersonnage.RotateTowards(HoverManager.Instance.hoveredCase.gameObject);
  }

  void OnNewClick()
  { // Lors d'un click sur une case
    if (GameManager.Instance.actualAction != PersoAction.isCasting)
      return;

    SpellCaseClick();
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
  public void SpellButtonClick(int IDSpell)
  {
    if (GameManager.Instance.actualAction == PersoAction.isCasting)
      return;

    selectedSpell = ChooseSpell(IDSpell);

    // enough PA? (global PA/mana)
    if (GameManager.Instance.manaGlobalActual < selectedSpell.costPA)
      {
        Debug.Log("PAS ASSEZ DE PA");
        selectedSpell = null;
        return;
      }

    RpcFunctions.Instance.CmdSpellButtonClick(IDSpell);
  }

  [ClientRpc]
  public void RpcSpellButtonClick(int IDSpell)
  {
    PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
    selectedPersonnage.animator.SetBool("Idle", false);
    selectedPersonnage.animator.SetBool("Cast", true);

    if (selectedPersonnage == null)// perso exist?
      return;

    if (selectedSpell == null)// spell exist?
      return;



    GameManager.Instance.actualAction = PersoAction.isCasting;
    SelectionManager.Instance.DisablePersoSelection();
    TurnManager.Instance.DisableFinishTurn();

    selectedSpell.newRangeList();
    selectedSpell.newTargetList();
    selectedSpell.ShowAllFeedbacks();
  }

  /// <summary>Le sort est lancé à un endroit</summary>
  void SpellCaseClick()
  {
    CaseData hoveredCase = HoverManager.Instance.hoveredCase;


    if ((Statut.canTarget & hoveredCase.statut) != Statut.canTarget)
      {
        StartCoroutine(SpellEnd());
        return;
      }

    spellSuccess = true;
    GameManager.Instance.manaGlobalActual -= selectedSpell.costPA;
    UIManager.Instance.UpdateRemaningMana();

    if (SummonManager.Instance.lastSummonInstancied != null)
      {
        SummonData lastSummonInstancied = SummonManager.Instance.lastSummonInstancied;
        lastSummonInstancied.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        GameObject newOwnerCircle = (GameObject)Instantiate(ownerCircle, lastSummonInstancied.originPoint.position, Quaternion.identity);
        newOwnerCircle.transform.parent = lastSummonInstancied.originPoint;
        lastSummonInstancied.GetComponent<BoxCollider2D>().enabled = true;
        SummonManager.Instance.AddSummon(lastSummonInstancied);
        if (lastSummonInstancied.owner == Player.Red)
          {
            newOwnerCircle.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.4f);
          }
        if (lastSummonInstancied.owner == Player.Blue)
          {
            newOwnerCircle.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.4f);
          }
      }
          
    foreach (CaseData obj in CaseManager.listAllCase)
      {
        if ((Statut.atAoE & obj.statut) == Statut.atAoE)
          {
            if (((ObjectType.AllyPerso & selectedSpell.affectedTarget) == ObjectType.AllyPerso) && obj.personnageData != null)
              {
                selectedSpell.ApplyEffect(obj.personnageData.gameObject);
              }

            if (((ObjectType.Ballon & selectedSpell.affectedTarget) == ObjectType.Ballon) && obj.ballon != null)
              {
                selectedSpell.ApplyEffect(obj.ballon.gameObject);
              }

            if (((ObjectType.Invoc & selectedSpell.affectedTarget) == ObjectType.Invoc) && obj.summonData != null)
              {
                selectedSpell.ApplyEffect(obj.summonData.gameObject);
              }
          }
      }
    StartCoroutine(SpellEnd());
  }

  void SpellStart()
  {

  }

  public IEnumerator SpellEnd()
  {
    SelectionManager.Instance.selectedPersonnage.animator.SetBool("Cast", false);
    SelectionManager.Instance.selectedPersonnage.animator.SetBool("Idle", true);
    GameManager.Instance.actualAction = PersoAction.isWaiting;
    foreach (CaseData obj in CaseManager.listAllCase)
      {
        obj.ChangeStatut(Statut.None, Statut.atRange);
        obj.ChangeStatut(Statut.None, Statut.atAoE);
        obj.ChangeStatut(Statut.None, Statut.atPush);
        obj.ChangeStatut(Statut.None, Statut.canTarget);
      }

    if (!spellSuccess && SummonManager.Instance.lastSummonInstancied != null)
      {
        DestroyImmediate(SummonManager.Instance.lastSummonInstancied.gameObject);
      }
    SummonManager.Instance.lastSummonInstancied = null;
    spellSuccess = false;

    yield return new WaitForSeconds(0.5f);

    GameManager.Instance.actualAction = PersoAction.isSelected;
    SelectionManager.Instance.EnablePersoSelection();
    MoveBehaviour.Instance.StopAllCoroutines();
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
