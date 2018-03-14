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

  SpellData selectedSpell;
  List<CaseData> rangeList = new List<CaseData>();
  List<CaseData> AoEList = new List<CaseData>();
  List<CaseData> pushList = new List<CaseData>();

  bool isSpellCasting = false;

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
      ShowAreaOfEffect();
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

  /// <summary>Mettre un chiffre correspondant à l'ordre des boutons de sorts du personnage (0 = spell1; 1 = spell2)</summary>
  public void SpellCast(int IDSpell)
  {
    RpcFunctions.Instance.CmdCastSpell(IDSpell);
  }

  [ClientRpc]
  public void RpcSpellCast(int IDSpell)
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
    if (selectedSpell == null)
      return;
    if (selectedPersonnage.actualPointAction < selectedSpell.costPA)
      return;

    isSpellCasting = true;
    GameManager.Instance.actualAction = PersoAction.isCasting;
    SelectionManager.Instance.DisablePersoSelection();
    TurnManager.Instance.DisableFinishTurn();
    ShowRange();
    ShowAreaOfEffect();
    ShowPushEffect();
  }

  void ShowRange()
  {
    int range = selectedSpell.range;
    List<CaseData> list = new List<CaseData>();
    List<CaseData> list2 = new List<CaseData>();
    CaseData selectedCase = SelectionManager.Instance.selectedCase;
    list.Add(selectedCase);

    if (selectedSpell.isLinear)
      {
        for (int i = 0; i < range; i++)
          {
            if (selectedCase.GetCaseRelativeCoordinate(i, 0) != null)
              list.Add(selectedCase.GetCaseRelativeCoordinate(i, 0));

            if (selectedCase.GetCaseRelativeCoordinate(-i, 0) != null)
              list.Add(selectedCase.GetCaseRelativeCoordinate(-i, 0));

            if (selectedCase.GetCaseRelativeCoordinate(0, i) != null)
              list.Add(selectedCase.GetCaseRelativeCoordinate(0, i));

            if (selectedCase.GetCaseRelativeCoordinate(0, -i) != null)
              list.Add(selectedCase.GetCaseRelativeCoordinate(0, -i));
          }
      } else
      {
        for (int i = 0; i < range; i++)
          {
            foreach (CaseData obj in list)
              {
                if (obj.GetBottomLeftCase() != null)
                  list2.Add(obj.GetBottomLeftCase());

                if (obj.GetBottomRightCase() != null)
                  list2.Add(obj.GetBottomRightCase());
                    
                if (obj.GetTopLeftCase() != null)
                  list2.Add(obj.GetTopLeftCase());

                if (obj.GetTopRightCase() != null)
                  list2.Add(obj.GetTopRightCase());
              }
            list.AddRange(list2);
            list2.Clear();
          }
        list.Remove(selectedCase);
      }
    if (rangeList.Count != 0)
      rangeList.Clear();
    rangeList.AddRange(list);
    foreach (CaseData obj in rangeList)
      {
        if (obj != null)
          obj.ChangeStatut(Statut.atRange);
      }

  }

  void ShowAreaOfEffect()
  {
    foreach (CaseData obj in CaseManager.Instance.GetAllCaseWithStatut(Statut.atAoE))
      {
        obj.ChangeStatut(Statut.None, Statut.atAoE);
      }

    int AoE = selectedSpell.areaOfEffect;
    List<CaseData> list = new List<CaseData>();
    List<CaseData> list2 = new List<CaseData>();
    CaseData hoveredCase = HoverManager.Instance.hoveredCase;
    list.Add(hoveredCase);
    for (int i = 0; i < AoE; i++)
      {
        foreach (CaseData obj in list)
          {
            if (obj.GetBottomLeftCase() != null)
              list2.Add(obj.GetBottomLeftCase());

            if (obj.GetBottomRightCase() != null)
              list2.Add(obj.GetBottomRightCase());

            if (obj.GetTopLeftCase() != null)
              list2.Add(obj.GetTopLeftCase());

            if (obj.GetTopRightCase() != null)
              list2.Add(obj.GetTopRightCase());
          }
        list.AddRange(list2);
        list2.Clear();
      }
    list.Remove(hoveredCase);

    if (AoEList.Count != 0)
      AoEList.Clear();
      
    AoEList.AddRange(list);
    foreach (CaseData obj in AoEList)
      {
        obj.ChangeStatut(Statut.atAoE);
      }
  }

  void ShowPushEffect()
  {
    int push = selectedSpell.pushValue;
    List<CaseData> list = new List<CaseData>();
    List<CaseData> list2 = new List<CaseData>();
    list.Add(SelectionManager.Instance.selectedCase);

    if (selectedSpell.isLinear)
      {
        for (int i = 0; i < push; i++)
          {
            foreach (CaseData obj in list)
              {
                if (obj.GetCaseRelativeCoordinate(i, 0) != null)
                  list.Add(obj.GetCaseRelativeCoordinate(i, 0));
              }
          }
      }

    if (pushList.Count != 0)
      pushList.Clear();
      
    pushList.AddRange(list);
    foreach (CaseData obj in pushList)
      {
        obj.ChangeStatut(Statut.atPush);
      }
  }

  /// <summary>Mettre un chiffre correspondant à l'ordre des boutons de sorts du personnage (0 = spell1; 1 = spell2)</summary>
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
}
