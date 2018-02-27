using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class MoveBehaviour : NetworkBehaviour
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

    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log("MoveBehaviour is Instanced");
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
    }

    void OnDisable()
    {
      EventManager.newClickEvent -= OnNewClick;
    }

    void OnNewClick()
    { // Lors d'un click sur une case
      StartCoroutine(LateOnNewClick()); 
    }

    IEnumerator LateOnNewClick() {
    yield return new WaitForEndOfFrame();
      CaseData hoveredCase = HoverManager.Instance.hoveredCase;
      PersoAction actualAction = GameManager.Instance.actualAction;
      PathfindingCase casePathfinding = hoveredCase.GetComponent<CaseData>().casePathfinding;

      if (pathes.Count != 0
        && actualAction == PersoAction.isSelected
        && pathes.Contains(hoveredCase.transform))
        {
          Debug.Log("SendDeplacement");
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
            ShowPath();
        }
    }

    public void ShowPath()
    { // Montre la route de déplacement

    Color moveColor = ColorManager.Instance.moveColor;
        Player currentPlayer = GameManager.Instance.currentPlayer;
        Color enemyColor = ColorManager.Instance.enemyColor;

        foreach (Transform path in this.pathes)
        {
          path.GetComponent<CaseData>().ChangeColor(Statut.canMove);
            foreach (PersoData persoCompared in RosterManager.Instance.listHero)
            {
                if (persoCompared.owner != currentPlayer
                  && persoCompared.persoCase != null
                && Fonction.Instance.CheckAdjacent(path.gameObject, persoCompared.gameObject) == true)
                {
                  path.GetComponent<CaseData>().ChangeColor(Statut.canBeTackled);
                }
            }
        }
    }

    public void HidePath()
    { // Cache la route de déplacement
        foreach (Transform path in this.pathes)
        {
          path.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.canMove);
          path.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.canBeTackled);
        }
    }

    // *************** //
    // ** Déplacement ** //
    // *************** //

    public void SendDeplacement()
    { // On stock la route de déplacement et on la colore, puis on appelle la fonction de déplacement
        Color isMovingColor = ColorManager.Instance.isMovingColor;
        Color caseColor = ColorManager.Instance.caseColor;

        foreach (Transform path in pathes)
        {
          path.GetComponent<CaseData>().ChangeColor(Statut.isMoving);
        }
      Debug.Log("Deplacement");
      StartCoroutine(Deplacement(GraphManager.Instance.getCaseOffset(SelectionManager.Instance.selectedPersonnage.gameObject), SelectionManager.Instance.selectedPersonnage));
    }

   public IEnumerator Deplacement(float offsetY, PersoData selectedPersonnage)
    { // On déplace le personnage de case en case jusqu'au click du joueur propriétaire, et entre temps on check s'il est taclé ou non
        TurnManager.Instance.DisableFinishTurn();

        selectedPersonnage.GetComponent<PersoData>().movePath = pathes;
        GameManager.Instance.actualAction = PersoAction.isMoving;

        foreach (Transform path in pathes)
        {
            if (selectedPersonnage.isTackled)
            {
              path.GetComponent<CaseData>().ChangeColor(Statut.isTackled, Statut.isMoving);
            }
            else
            {
                Vector3 startPos = SelectionManager.Instance.selectedPersonnage.transform.position;
                float fracturedTime = 0;
                float timeUnit = travelTime / 60;

                selectedPersonnage.RotateTowards(path.transform.position);

                while (selectedPersonnage.transform.position != path.transform.position + new Vector3(0, offsetY, 0))
                {
                    fracturedTime += timeUnit + 0.01f;
                    selectedPersonnage.transform.position = Vector3.Lerp(startPos, path.transform.position + new Vector3(0, offsetY, 0), fracturedTime);
                    yield return new WaitForEndOfFrame();
                }
                SelectionManager.Instance.selectedCase = path.gameObject.GetComponent<CaseData>();
              path.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.isMoving);
              TackleBehaviour.Instance.CheckTackle(selectedPersonnage.gameObject);
            }
        }
        SelectionManager.Instance.selectedCase = pathes[pathes.Count - 1].gameObject.GetComponent<CaseData>();
      SelectionManager.Instance.selectedCase.GetComponent<CaseData>().ChangeColor(Statut.None, Statut.isMoving);
        GameManager.Instance.actualAction = PersoAction.isSelected;

        if (!selectedPersonnage.isTackled)
        {
            SelectionManager.Instance.selectedCase.GetComponent<CaseData>().casePathfinding = PathfindingCase.NonWalkable;
            SelectionManager.Instance.selectedPersonnage.actualPointMovement -= pathes.Count - 1;
            pathes.Clear();
        }
        else
        {
            selectedPersonnage.isTackled = false;
            SelectionManager.Instance.selectedPersonnage.actualPointMovement = 0;
            pathes.Clear();
        }

        TurnManager.Instance.StartCoroutine("EnableFinishTurn");
      CaseManager.Instance.StartCoroutine ("ShowActions");
        yield return new WaitForEndOfFrame();
    }
}