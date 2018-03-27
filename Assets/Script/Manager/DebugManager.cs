using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{

  AoEType selectedAoE = AoEType.Circle;
	
  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F1))
      selectedAoE = AoEType.Circle;

    if (Input.GetKeyDown(KeyCode.F2))
      selectedAoE = AoEType.Croix;

    if (Input.GetKeyDown(KeyCode.F3))
      selectedAoE = AoEType.Carre;

    if (Input.GetKey(KeyCode.Alpha1))
      foreach (CaseData obj in CaseManager.Instance.GetCaseByAoEFromCase(HoverManager.Instance.hoveredCase, 1, selectedAoE))
        {
          obj.ChangeStatut(Statut.atAoE);
        }

    if (Input.GetKey(KeyCode.Alpha2))
      foreach (CaseData obj in CaseManager.Instance.GetCaseByAoEFromCase(HoverManager.Instance.hoveredCase, 2, selectedAoE))
        {
          obj.ChangeStatut(Statut.atAoE);
        }
      
    if (Input.GetKey(KeyCode.Alpha3))
      foreach (CaseData obj in CaseManager.Instance.GetCaseByAoEFromCase(HoverManager.Instance.hoveredCase, 3, selectedAoE))
        {
          obj.ChangeStatut(Statut.atAoE);
        }
      
    if (Input.GetKey(KeyCode.Alpha4))
      foreach (CaseData obj in CaseManager.Instance.GetCaseByAoEFromCase(HoverManager.Instance.hoveredCase, 4, selectedAoE))
        {
          obj.ChangeStatut(Statut.atAoE);
        }
    if (Input.GetKey(KeyCode.Alpha5))
      foreach (CaseData obj in CaseManager.Instance.GetCaseByAoEFromCase(HoverManager.Instance.hoveredCase, 5, selectedAoE))
        {
          obj.ChangeStatut(Statut.atAoE);
        }
    if (Input.GetKeyDown(KeyCode.Alpha6))
      {
        foreach (CaseData obj in CaseManager.Instance.GetAllCaseWithStatut(Statut.atAoE))
          {
            obj.ChangeStatut(Statut.None, Statut.atAoE);
          }
      }
        
  }
}
