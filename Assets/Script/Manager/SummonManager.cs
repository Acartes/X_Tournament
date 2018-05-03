using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SummonManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //

  public static SummonManager Instance;

  public SummonData lastSummonInstancied;

  public List<SummonData> summonAirRedList;
  public List<SummonData> summonAirBlueList;
  public List<SummonData> summonEarthRedList;
  public List<SummonData> summonEarthBlueList;
  public List<SummonData> summonFireRedList;
  public List<SummonData> summonFireBlueList;
  public List<SummonData> summonWaterRedList;
  public List<SummonData> summonWaterBlueList;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

  public override void OnStartClient()
  {
    if (Instance == null)
      Instance = this;
    Debug.Log(this.GetType() + " is Instanced");
  }

  void Update()
  {
    CheckSummonLimitReached();
  }

  void CheckSummonLimitReached()
  {
    if (summonAirRedList.Count != 0)
      {
        if (summonAirRedList.Count > summonAirRedList[0].limitInvoc)
          {
            if (summonAirRedList[0] == null)
              summonAirRedList.RemoveAt(0);
              
            Destroy(summonAirRedList[0].gameObject);
            summonAirRedList.RemoveAt(0);
          }
      }

    if (summonAirBlueList.Count != 0)
      {
        if (summonAirBlueList.Count > summonAirBlueList[0].limitInvoc)
          {
            if (summonAirBlueList[0] == null)
              summonAirBlueList.RemoveAt(0);
              
            Destroy(summonAirBlueList[0].gameObject);
            summonAirBlueList.RemoveAt(0);
          }
      }

    if (summonEarthRedList.Count != 0)
      {
        if (summonEarthRedList.Count > summonEarthRedList[0].limitInvoc)
          {
            if (summonEarthRedList[0] == null)
              summonEarthRedList.RemoveAt(0);
              
            Destroy(summonEarthRedList[0].gameObject);
            summonEarthRedList.RemoveAt(0);
          }
      }

    if (summonEarthBlueList.Count != 0)
      {
        if (summonEarthBlueList.Count > summonEarthBlueList[0].limitInvoc)
          {
            if (summonEarthBlueList[0] == null)
              summonEarthBlueList.RemoveAt(0);
              
            Destroy(summonEarthBlueList[0].gameObject);
            summonEarthBlueList.RemoveAt(0);
          }
      }

    if (summonFireRedList.Count != 0)
      {
        if (summonFireRedList.Count > summonFireRedList[0].limitInvoc)
          {
            if (summonFireRedList[0] == null)
              summonFireRedList.RemoveAt(0);
              
            Destroy(summonFireRedList[0].gameObject);
            summonFireRedList.RemoveAt(0);
          }
      }

    if (summonFireBlueList.Count != 0)
      {
        if (summonFireBlueList.Count > summonFireBlueList[0].limitInvoc)
          {
            if (summonFireBlueList[0] == null)
              summonFireBlueList.RemoveAt(0);
              
            Destroy(summonFireBlueList[0].gameObject);
            summonFireBlueList.RemoveAt(0);
          }
      }

    if (summonWaterRedList.Count != 0)
      {
        if (summonWaterRedList.Count > summonWaterRedList[0].limitInvoc)
          {
            if (summonWaterRedList[0] == null)
              summonWaterRedList.RemoveAt(0);
              
            Destroy(summonWaterRedList[0].gameObject);
            summonWaterRedList.RemoveAt(0);
          }
      }

    if (summonWaterBlueList.Count != 0)
      {
        if (summonWaterBlueList.Count > summonWaterBlueList[0].limitInvoc)
          {
            if (summonWaterBlueList[0] == null)
              summonWaterBlueList.RemoveAt(0);
              
            Destroy(summonWaterBlueList[0].gameObject);
            summonWaterBlueList.RemoveAt(0);
 
          }
      }
  }
}
