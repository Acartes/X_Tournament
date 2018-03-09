using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;

/// <summary>Gère toute les cases du jeu, et permet de connaître la distance qui les sépare les uns des autres en x et y avec xCaseOffset et yCaseOffset.</summary>
public class CaseManager : NetworkBehaviour
{

    // *************** //
    // ** Variables ** // Toutes les variables sans distinctions
    // *************** //
    [Header("  Cases")]
    [Tooltip("La liste de toutes les cases de la grille.")]
    [ReadOnly]
    static public List<GameObject> listAllCase;
    [Tooltip("Distance entre les cases en x.")]
    public float xCaseOffset = 1;
    [Tooltip("Distance entre les cases en y.")]
    public float yCaseOffset = 0.3f;

    [HideInInspector] public static CaseManager Instance;

    // ******************** //
    // ** Initialisation ** // Fonctions de départ, non réutilisable
    // ******************** //

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("CaseManager is Instanced");
        StartCoroutine(waitForInit());
    }

    IEnumerator waitForInit()
    {
        while (!LoadingManager.Instance.isGameReady())
            yield return new WaitForEndOfFrame();
        Init();
    }

    void Init()
    {
        listAllCase.Clear();
        foreach (GameObject cubeNew in GameObject.FindGameObjectsWithTag("case"))
        {
            listAllCase.Add(cubeNew);
        }
    }

    // *************** //
    // ** Fonctions ** // Fonctions réutilisables ailleurs
    // *************** //

    void ClearAllCases()
    {
        /// <summary>Remet la valeur de toutes les cases par défaut</summary> 
        foreach (GameObject newCaseGMB in listAllCase)
        {
            CaseData newCase = newCaseGMB.GetComponent<CaseData>();
            newCase.ballon = null;
            newCase.casePathfinding = PathfindingCase.Walkable;
            newCase.personnageData = null;
            newCase.statut = 0;
        }
    }

    /// <summary>Colore toutes les cases de la couleur choisi.</summary> 
    public void PaintAllCase(Color newColor)
    {
        foreach (GameObject newCaseGMB in listAllCase)
        {
            newCaseGMB.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    /// <summary>Change le sprite de toutes les cases avec le sprite choisi.</summary> 
    public void ChangeSpriteAllCase(Sprite newSprite)
    {
        foreach (GameObject newCaseGMB in listAllCase)
        {
            newCaseGMB.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    /// <summary>Obtenir toutes les cases du jeu. </summary>
    public List<CaseData> GetAllCase()
    {
        List<CaseData> newList = new List<CaseData>();

        foreach (GameObject newCaseGMB in listAllCase)
        {
            CaseData newCase = newCaseGMB.GetComponent<CaseData>();
            newList.Add(newCase);
        }

        return newList;
    }

    /// <summary>Obtenir toutes les cases possédant un ballon.</summary>
    public List<CaseData> GetAllCaseWithBallon()
    {
        List<CaseData> newList = new List<CaseData>();

        foreach (GameObject newCaseGMB in listAllCase)
        {
            CaseData newCase = newCaseGMB.GetComponent<CaseData>();
            if (newCase.ballon != null)
                newList.Add(newCase);
        }
        return newList;
    }

    /// <summary>Obtenir toutes les cases possédant un personnage.</summary>
    public List<CaseData> GetAllCaseWithPersonnage()
    {
        List<CaseData> newList = new List<CaseData>();

        foreach (GameObject newCaseGMB in listAllCase)
        {
            CaseData newCase = newCaseGMB.GetComponent<CaseData>();
            if (newCase.personnageData != null)
                newList.Add(newCase);
        }
        return newList;
    }

    /// <summary>Obtenir toutes les cases sans personnage et sans ballon.</summary>
    public List<CaseData> GetAllCaseWithNothing()
    {
        List<CaseData> newList = new List<CaseData>();

        foreach (GameObject newCaseGMB in listAllCase)
        {
            CaseData newCase = newCaseGMB.GetComponent<CaseData>();
            if (newCase.personnageData == null && newCase.ballon == null)
                newList.Add(newCase);
        }
        return newList;
    }

    /// <summary>Obtenir toutes les cases avec le statut choisi.</summary>
    public List<CaseData> GetAllCaseWithStatut(Statut statut)
    {
        List<CaseData> newList = new List<CaseData>();

        foreach (GameObject newCaseGMB in listAllCase)
        {
            CaseData newCase = newCaseGMB.GetComponent<CaseData>();
            if ((statut & newCase.statut) == statut)
                newList.Add(newCase);
        }
        return newList;
    }

    /// <summary>Obtenir toutes les cases avec la couleur choisie.</summary>
    public List<CaseData> GetAllCaseWithColor(Color color)
    {
        List<CaseData> newList = new List<CaseData>();

        foreach (GameObject newCaseGMB in listAllCase)
        {
            CaseData newCase = newCaseGMB.GetComponent<CaseData>();
            SpriteRenderer SpriteR = newCaseGMB.GetComponent<SpriteRenderer>();
            if (SpriteR.color == color)
                newList.Add(newCase);
        }
        return newList;
    }
    public void RemovePath()
    { // Cache la route de déplacement
        foreach (GameObject newCaseGMB in listAllCase)
        {
            CaseData newCase = newCaseGMB.GetComponent<CaseData>();
            newCase.ChangeStatut(Statut.None, Statut.canMove);
            newCase.ChangeStatut(Statut.None, Statut.canBeTackled);
        }
    }

    public void DisableAllColliders()
    { // Cache la route de déplacement
        foreach (GameObject newCaseGMB in listAllCase)
        {
            newCaseGMB.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
    public void EnableAllColliders()
    { // Cache la route de déplacement
        foreach (GameObject newCaseGMB in listAllCase)
        {
            newCaseGMB.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }

    public GameObject GetCase(int xCoord, int yCoord, GameObject selectedCase = null, GameObject selectedPersonnage = null)
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
        }
        else
        {
            return false;
        }
    }

}
