using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;

public class CaseManager : NetworkBehaviour
{

    // *************** //
    // ** Variables ** //
    // *************** //
    [Header("  Cases")]
    [Tooltip("La liste de toutes les cases de la grille.")]
    [ReadOnly]
    public List<GameObject> listCase;
    [Tooltip("Distance entre les cases en x.")]
    public float xCaseOffset = 1;
    [Tooltip("Distance entre les cases en y.")]
    public float yCaseOffset = 0.3f;

    [HideInInspector] public static CaseManager Instance;

    // *************** //
    // ** Initialisation ** //
    // *************** //

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("CaseManager is Instanced");
        StartCoroutine(waitForInit());
    }

    IEnumerator waitForInit()
    {
        while (!LobbyManager.Instance.IsInstancesLoaded())
            yield return new WaitForEndOfFrame();
        Init();
    }

    void Init()
    {
      listCase.Clear ();
    foreach (GameObject cubeNew in GameObject.FindGameObjectsWithTag("case"))
      {
        listCase.Add(cubeNew);
      }
        StartCoroutine(LateOnEnable());
    }

    IEnumerator LateOnEnable()
    {
        yield return new WaitForEndOfFrame();
        TurnManager.Instance.changeTurnEvent += OnChangeTurn;
        foreach (GameObject obj in listCase)
        {
            CaseData objCaseData = obj.GetComponent<CaseData>();
            if (objCaseData.winCase != Player.Neutral)
            {
                objCaseData.caseColor = ColorManager.Instance.goalColor;
            }
        }
    }

    void OnDisable()
    {
        if (LobbyManager.Instance.IsInstancesLoaded())
        {
            TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
        }
    }

    // *************** //
    // ** Events ** //
    // *************** //

    void OnChangeTurn(object sender, PlayerArgs e)
    { // Lorsqu'un joueur termine son tour
        switch (e.currentPhase)
        {
            case (Phase.Deplacement):
            StartCoroutine (ShowActions());
                break;
            case (Phase.Placement):
                break;
        }
    }
      
    // *************** //
    // ** Actions ** //
    // *************** //

    public IEnumerator ShowActions()
    { // Affiche en violet les actions possible par le joueur.
      yield return new WaitForEndOfFrame();
        Player currentPlayer = GameManager.Instance.currentPlayer;
        Phase currentPhase = GameManager.Instance.currentPhase;
        Color actionPreColor = ColorManager.Instance.actionPreColor;
        PersoData selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
        Color selectedColor = ColorManager.Instance.selectedColor;

        switch (currentPhase)
        {
            case (Phase.Placement):
                break;
            case (Phase.Deplacement):
                foreach (GameObject caseCompared in listCase)
                {
                    Color newColor = ColorManager.Instance.caseColor;

                if (!MenuContextuel.Instance.gameObject.activeInHierarchy)
                    {
                        PersoData persoCompared = caseCompared.GetComponent<CaseData>().personnageData;
                        BallonData ballonCompared = caseCompared.GetComponent<CaseData>().ballon;


                        if (persoCompared != null && persoCompared.GetComponent<PersoData>().owner == currentPlayer)
                        {
                        caseCompared.GetComponent<CaseData>().ChangeColor(Statut.isAllyPerso);
                        }

                        if (SelectionManager.Instance.selectedPersonnage != null)
                        {
                            if (persoCompared != null
                          && persoCompared.GetComponent<PersoData>().owner != currentPlayer
                          && Fonction.Instance.CheckAdjacent(persoCompared.gameObject, selectedPersonnage.gameObject) == true)
                            {
                            caseCompared.GetComponent<CaseData>().ChangeColor(Statut.canPunch);
                            }

                        if (ballonCompared != null
                          && Fonction.Instance.CheckAdjacent(selectedPersonnage.gameObject, ballonCompared.gameObject) == true)
                          {
                            caseCompared.GetComponent<CaseData>().ChangeColor(Statut.canShot);
                            //newColor = actionPreColor;
                          }

                            if (persoCompared == selectedPersonnage)
                            {
                            caseCompared.GetComponent<CaseData>().ChangeColor(Statut.isSelected);
                                //newColor = selectedColor;
                            }
                        }

              //      if (newColor != ColorManager.Instance.caseColor)
                 //       {
                 //           caseCompared.GetComponent<CaseData>().ChangeColor(newColor);
                //        }
                    }
                }
                break;
        }
    }
}