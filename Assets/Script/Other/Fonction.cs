using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Fonction : NetworkBehaviour {

  public static Fonction Instance;


    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log(this.GetType() + " is Instanced");
    }

    public bool CheckAdjacent(GameObject firstObj, GameObject secondObj) 
    { // Cette condition check si firstObj est à côté de secondObj (1case)

      float xCaseOffset = CaseManager.Instance.xCaseOffset;
      float yCaseOffset = CaseManager.Instance.yCaseOffset;

      float firstPosX = 0;
      float secondPosX = 0;
      float firstPosY = 0;
      float secondPosY = 0;

      // l'objet est-il un personnage ?
      if (firstObj.GetComponent<PersoData>() != null && firstObj.GetComponent<PersoData>().persoCase != null)
        {
          firstPosX = firstObj.GetComponent<PersoData>().persoCase.transform.position.x;
          firstPosY = firstObj.GetComponent<PersoData>().persoCase.transform.position.y;
        }

      if (secondObj.GetComponent<PersoData>() != null && secondObj.GetComponent<PersoData>().persoCase != null)
        {
          secondPosX = secondObj.GetComponent<PersoData>().persoCase.transform.position.x;
          secondPosY = secondObj.GetComponent<PersoData>().persoCase.transform.position.y;
        }
      // l'objet est-il un ballon ?
      if (firstObj.GetComponent<BallonData>() != null && firstObj.GetComponent<BallonData>().ballonCase != null)
        {
          firstPosX = firstObj.GetComponent<BallonData>().ballonCase.transform.position.x;
          firstPosY = firstObj.GetComponent<BallonData>().ballonCase.transform.position.y;
        }

      if (secondObj.GetComponent<BallonData>() != null && secondObj.GetComponent<BallonData>().ballonCase != null)
        {
          secondPosX = secondObj.GetComponent<BallonData>().ballonCase.transform.position.x;
          secondPosY = secondObj.GetComponent<BallonData>().ballonCase.transform.position.y;
        }
      // l'objet est-il une case ?
      if (firstObj.GetComponent<CaseData>() != null)
        {
          firstPosX = firstObj.transform.position.x;
          firstPosY = firstObj.transform.position.y;
        }

      if (secondObj.GetComponent<CaseData>() != null)
        {
          secondPosX = secondObj.transform.position.x;
          secondPosY = secondObj.transform.position.y;
        }

      if ((secondPosX < firstPosX + xCaseOffset && secondPosX > firstPosX - xCaseOffset) && (firstPosY < secondPosY + yCaseOffset && firstPosY > secondPosY - yCaseOffset))
        {
          return true;
      } else
        {
          return false;
        }
    }

  public GameObject GetCase (int xCoord, int yCoord, GameObject selectedCase = null, GameObject selectedPersonnage = null) 
    {
      if (selectedCase != null)
        {
          GameObject.Find((selectedCase.GetComponent<CaseData>().xCoord + xCoord).ToString()
            + " "
            + (selectedCase.GetComponent<CaseData>().yCoord + yCoord).ToString());
        }
      if (selectedPersonnage != null)
        {
          GameObject.Find((selectedPersonnage.GetComponent<PersoData>().persoCase.GetComponent<CaseData>().xCoord + xCoord).ToString()
            + " "
            + (selectedPersonnage.GetComponent<PersoData>().persoCase.GetComponent<CaseData>().yCoord + yCoord).ToString());
        }
      return null;

    }
}
