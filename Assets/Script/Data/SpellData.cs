using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>Tout ce qu'il est possible de faire avec un sort, ainsi que toutes ses données.</summary>
public class SpellData : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public int costPA;

  public int range;
  public bool isLinear;

  public int areaOfEffect;

  public int damagePR;
  public int damagePA;
  public int damagePM;
  public Element elementCreated;

  public int pushValue;
  public bool hurtWhenStopped;

  public SummonData summonedObj;
  public bool isDirect;

  public Sprite buttonSprite;

  public Statut newStatut;

  List<CaseData> rangeList = new List<CaseData>();
  List<CaseData> AoEList = new List<CaseData>();
  List<CaseData> pushList = new List<CaseData>();

  [TextArea]public string tooltipTitle;
  [TextArea]public string tooltipRange;
  [TextArea]public string tooltipEffect;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  void Start()
  { 
    StartCoroutine(waitForInit());
  }

  IEnumerator waitForInit()
  {
    yield return new WaitForEndOfFrame();
    while (TurnManager.Instance == null)
      {
        yield return null;
      }
    Init();
  }

  public void Init()
  {
   
  }
  
  // *************** //
  // ** Fonctions ** // Fonctions réutilisables ailleurs
  // *************** //

  /// <summary>Montre la portée du sort avant de le lancer</summary>
  public void ShowRange()
  {
    List<CaseData> list = new List<CaseData>();
    List<CaseData> list2 = new List<CaseData>();
    CaseData selectedCase = SelectionManager.Instance.selectedCase;
    list.Add(selectedCase);

    // get des cases
    if (isLinear)
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

    // on montre les cases qu'on a get
    if (rangeList.Count != 0)
      rangeList.Clear();
      
    rangeList.AddRange(list);

    foreach (CaseData obj in rangeList)
      {
        if (obj != null)
          obj.ChangeStatut(Statut.atRange);
      }

  }

  /// <summary>Montre l'AoE du sort avant de le lancer</summary>
  public void ShowAreaOfEffect()
  {
    foreach (CaseData obj in CaseManager.Instance.GetAllCaseWithStatut(Statut.atAoE))
      {
        obj.ChangeStatut(Statut.None, Statut.atAoE);
      }

    int AoE = areaOfEffect;
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

  /// <summary>Montre à quelle portée les personnages vont être projetés avant de le lancer</summary>
  public void ShowPushEffect()
  {
    int push = pushValue;
    List<CaseData> list = new List<CaseData>();
    List<CaseData> list2 = new List<CaseData>();
    list.Add(SelectionManager.Instance.selectedCase);

    if (isLinear)
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


  /// <summary>Montre l'invocation avant de le lancer</summary>
  public void ShowSummon(SummonData summon)
  {
    CaseData hoveredCase = HoverManager.Instance.hoveredCase;
    summon.transform.position = hoveredCase.transform.position + summon.transform.position - summon.originPoint.position;
  }
}
