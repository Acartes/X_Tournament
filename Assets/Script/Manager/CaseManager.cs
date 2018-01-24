using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CaseManager : MonoBehaviour
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
    void Awake()
    {
      listCase.Clear ();
    foreach (GameObject cubeNew in GameObject.FindGameObjectsWithTag("case"))
      {
        listCase.Add(cubeNew);
      }

        Instance = this;
    }

    void OnEnable()
    {
        StartCoroutine(LateOnEnable());
    }

    IEnumerator LateOnEnable()
    {
        yield return new WaitForEndOfFrame();
        TurnManager.Instance.changeTurnEvent += OnChangeTurn;
    }

    void OnDisable()
    {
        TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
    }

    void Start()
    {
        foreach (GameObject obj in listCase)
        {
            CaseData objCaseData = obj.GetComponent<CaseData>();
            if (objCaseData.winCase != Player.Neutral)
            {
                objCaseData.caseColor = ColorManager.Instance.goalColor;
            }
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

                break;
            case (Phase.Placement):
                ChangeColorPlacement();
                break;
        }
    }

    void Update()
    {
        ShowActions();
    }

    // *************** //
    // ** Actions ** //
    // *************** //
    void ChangeColorPlacement()
    { // Change la couleur des cases de placements avec un couleur du joueur approprié.
        foreach (GameObject obj in listCase)
        {
            obj.GetComponent<CaseData>().ChangeColor(obj.GetComponent<CaseData>().initColor);
        }
    }

    public void ShowActions()
    { // Affiche en violet les actions possible par le joueur.

        Player currentPlayer = GameManager.Instance.currentPlayer;
        Phase currentPhase = GameManager.Instance.currentPhase;
        Color actionPreColor = ColorManager.Instance.actionPreColor;
        GameObject selectedPersonnage = SelectionManager.Instance.selectedPersonnage;
        Color selectedColor = ColorManager.Instance.selectedColor;

        switch (currentPhase)
        {
            case (Phase.Placement):
                break;
            case (Phase.Deplacement):
                foreach (GameObject caseCompared in listCase)
                {
                    Color newColor = ColorManager.Instance.caseColor;

                    if (!ShotBehaviour.Instance.isShoting && !MenuContextuel.Instance.gameObject.activeInHierarchy)
                    {

                        GameObject persoCompared = caseCompared.GetComponent<CaseData>().personnageData;
                        GameObject ballonCompared = caseCompared.GetComponent<CaseData>().caseBallon;

                        if (persoCompared != null && persoCompared.GetComponent<PersoData>().owner == currentPlayer)
                        {
                            newColor = actionPreColor;
                        }

                        if (SelectionManager.Instance.selectedPersonnage != null)
                        {
                            if (persoCompared != null &&
                            persoCompared.GetComponent<PersoData>().owner != currentPlayer &&
                          Fonction.Instance.CheckAdjacent(persoCompared, selectedPersonnage) == true)
                            {
                                newColor = actionPreColor;
                            }

                            if (persoCompared == selectedPersonnage)
                            {
                                newColor = selectedColor;
                            }

                            if (ballonCompared != null &&
                          Fonction.Instance.CheckAdjacent(selectedPersonnage, ballonCompared) == true)
                            {
                                newColor = actionPreColor;
                            }
                        }

                        if (newColor != caseCompared.GetComponent<CaseData>().caseColor)
                        {
                            caseCompared.GetComponent<CaseData>().ChangeColor(newColor, true);
                        }
                    }
                }
                break;
        }
    }
}
