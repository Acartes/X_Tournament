using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MoveBehaviour : MonoBehaviour
{
    // *************** //
    // ** Variables ** //
    // *************** //

    [Header("  Temps")]
    [Tooltip("La durée d'un déplacement entre deux cases.")]
    public float travelTime;

    [Header("  Chemin du pathfinding")]
    [ReadOnly]
    public List<GameObject> GoPathes; // déplacement
    [ReadOnly] public List<Transform> pathes; // déplacement

    [HideInInspector] public static MoveBehaviour Instance;

    // *************** //
    // ** Initialisation ** //
    // *************** //

    void Awake()
    {
      Instance = this;
    }

    void OnEnable()
    {
        ClickEvent.newClickEvent += OnNewClick;
    }

    void OnDisable()
    {
        ClickEvent.newClickEvent -= OnNewClick;
    }

    void OnNewClick()
    { // Lors d'un click sur une case
      StartCoroutine(LateOnNewClick()); 
    }

    IEnumerator LateOnNewClick() {
    yield return new WaitForEndOfFrame();
      GameObject hoveredCase = HoverManager.Instance.hoveredCase;
      PersoAction actualAction = GameManager.Instance.actualAction;
      PathfindingCase casePathfinding = hoveredCase.GetComponent<CaseData>().casePathfinding;

      if (pathes.Count != 0
        && actualAction == PersoAction.isSelected
        && pathes.Contains(hoveredCase.transform))
        {
          SendDeplacement();
        }
  }

    // *************** //
    // ** Pathfinding ** //
    // *************** //

    public void createPath()
    { // Créé une route de déplacement pour un personnage
        if (GameManager.Instance.actualAction == PersoAction.isMoving)
            return;

        HidePath();
        this.pathes.Clear();

        foreach (GameObject path in GoPathes)
        {
            this.pathes.Add(path.transform);
        }

        // Si le personnage n'a pas assez de PM, alors la route n'est pas créé
        if (this.pathes.Count > SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>().actualPointMovement + 1)
        {
            this.pathes.Clear();
        }
        else
        {
            ShowPath(ColorManager.Instance.travelColor, GameManager.Instance.currentPlayer, ColorManager.Instance.enemyColor);
        }
    }

    public void ShowPath(Color travelColor, Player currentPlayer, Color enemyColor)
    { // Montre la route de déplacement
        foreach (Transform path in this.pathes)
        {
            path.GetComponent<CaseData>().ChangeColor(travelColor);
            foreach (GameObject persoCompared in RosterManager.Instance.listHero)
            {
                if (persoCompared.GetComponent<PersoData>().owner != currentPlayer
                  && persoCompared.GetComponent<PersoData>().persoCase != null
                && Fonction.Instance.CheckAdjacent(path.gameObject, persoCompared) == true)
                {
                    path.GetComponent<CaseData>().ChangeColor(enemyColor);
                }
            }
        }
    }

    public void HidePath()
    { // Cache la route de déplacement
        foreach (Transform path in this.pathes)
        {
            path.GetComponent<CaseData>().ResetColor();
        }
    }

    // *************** //
    // ** Déplacement ** //
    // *************** //

    public void SendDeplacement()
    { // On stock la route de déplacement et on la colore, puis on appelle la fonction de déplacement
        Color moveColor = ColorManager.Instance.moveColor;
        Color caseColor = ColorManager.Instance.caseColor;

        foreach (Transform path in pathes)
        {
            path.GetComponent<CaseData>().ChangeColor(moveColor);
        }

        StartCoroutine(Deplacement(ColorManager.Instance.caseColor, GraphManager.Instance.offsetY, SelectionManager.Instance.selectedPersonnage));
    }

    IEnumerator Deplacement(Color caseColor, float offsetY, GameObject selectedPersonnage)
    { // On déplace le personnage de case en case jusqu'au click du joueur propriétaire, et entre temps on check s'il est taclé ou non
        TurnManager.Instance.DisableFinishTurn();

        selectedPersonnage.GetComponent<PersoData>().movePath = pathes;
        GameManager.Instance.actualAction = PersoAction.isMoving;

        foreach (Transform path in pathes)
        {
            if (selectedPersonnage.GetComponent<PersoData>().isTackled)
            {
                path.GetComponent<CaseData>().casePathfinding = PathfindingCase.Walkable;
                path.GetComponent<CaseData>().ChangeColor(caseColor);
            }
            else
            {
                Vector3 startPos = SelectionManager.Instance.selectedPersonnage.transform.position;
                float fracturedTime = 0;
                float timeUnit = travelTime / 60;

                selectedPersonnage.GetComponent<PersoData>().RotateTowards(path.transform.position);

                while (selectedPersonnage.transform.position != path.transform.position + new Vector3(0, offsetY, 0))
                {
                    fracturedTime += timeUnit + 0.01f;
                    selectedPersonnage.transform.position = Vector3.Lerp(startPos, path.transform.position + new Vector3(0, offsetY, 0), fracturedTime);
                    yield return new WaitForEndOfFrame();
                }
                SelectionManager.Instance.selectedCase = path.gameObject;
                path.GetComponent<CaseData>().ChangeColor(caseColor, false, PathfindingCase.Walkable);
              TackleBehaviour.Instance.CheckTackle(selectedPersonnage);
            }
        }
        SelectionManager.Instance.selectedCase = pathes[pathes.Count - 1].gameObject;
        SelectionManager.Instance.selectedCase.GetComponent<CaseData>().ChangeColor(caseColor);
        GameManager.Instance.actualAction = PersoAction.isSelected;

        if (!selectedPersonnage.GetComponent<PersoData>().isTackled)
        {
            SelectionManager.Instance.selectedCase.GetComponent<CaseData>().casePathfinding = PathfindingCase.NonWalkable;
            SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>().actualPointMovement -= pathes.Count - 1;
            pathes.Clear();
        }
        else
        {
            selectedPersonnage.GetComponent<PersoData>().isTackled = false;
            SelectionManager.Instance.selectedPersonnage.GetComponent<PersoData>().actualPointMovement = 0;
            pathes.Clear();
        }

        TurnManager.Instance.StartCoroutine("EnableFinishTurn");

        yield return new WaitForEndOfFrame();
    }
}