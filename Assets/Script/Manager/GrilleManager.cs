using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class GrilleManager: MonoBehaviour {

	// *************** //
	// ** Variables ** //
	// *************** //
	[Header("NewMap")]
	public bool pressToGenerate;
	[Range(1,100)]
	public int largeur;
	[Range(1,100)]
	public int hauteur;
	public float hSpace = 2;
	public float lSpace = 4.25f;
	public Color caseColor;

	public float xEcartCase;
	public float yEcartCase;

	[Header("AddWaterZones")]
	public bool addWaterZonesBool;
	public Sprite notWalkable;
	[Range(1,30)]
	public int FrequencyWater;

	[Header("AddPlacementZones")]
	public int placementZone;

	[Header("Ressources")]
	public GameObject cube;
	public List<List<GameObject>> cubeAll = new List<List<GameObject>>();
	GameObject cubeNew;
	public static GrilleManager Instance;
	public Pathfinding path;

	// *********** //
	// ** Start ** //
	// *********** //

    void Awake () {
		pressToGenerate = false;
		if (Instance == null)
			Instance = this;
	}

	// ************ //
	// ** Update ** //
	// ************ //

	void Update () {
		#if UNITY_EDITOR
		if (!EditorApplication.isPlaying) {
            if (pressToGenerate)
            { 
                NewMap();
				if (path != null) {
				path.BakePathfinding(cubeAll);
				}
				pressToGenerate = false;
            }
		}
		#endif
	}

	// ************* //
	// ** Actions ** //
	// ************* //

	void ConfigureListCube () { // Détruit la grille
			foreach (Transform obj in GetComponentsInChildren<Transform>())
			{
				if (obj != this.transform) {
					DestroyImmediate (obj.gameObject);
				}
			}
		cubeAll.Clear ();
	}

	public void NewMap () { // Génère une nouvelle grille
		ConfigureListCube(); 
		GameObject.Find ("CaseManager").GetComponent<CaseManager> ().listCase.Clear ();

		for (int h = 0; h < hauteur; h++) {
            cubeAll.Add(new List<GameObject>());
            for (int l = 0; l < largeur; l++) {
				cubeNew = (GameObject)Instantiate (cube, new Vector2 (h/hSpace + l/hSpace + transform.position.x, -h/lSpace + l/lSpace + transform.position.y), Quaternion.identity);
				cubeNew.transform.parent = transform;
				cubeNew.GetComponent<SpriteRenderer> ().sortingOrder = ((int)-l + (int)h) - 100;
                cubeNew.name = h + " " + l;
				cubeNew.GetComponent<CaseData> ().xCoord = h;
				cubeNew.GetComponent<CaseData> ().yCoord = l;
				cubeNew.GetComponent<CaseData> ().ownerPlacementZone = Player.Neutral;
              cubeNew.GetComponent<SpriteRenderer>().color = ColorManager.Instance.caseColor;
            cubeNew.GetComponent<CaseData>().caseColor = ColorManager.Instance.caseColor;
                cubeAll[(int)h].Add (cubeNew);

				AddWaterZones ();
              AddPlacementZones (h);
               

				GameObject.Find ("CaseManager").GetComponent<CaseManager>().listCase.Add (cubeNew);
            }
        }
      AddWinCase(ColorManager.Instance.goalColor);
	}

	void AddWaterZones () { // Créer des zones d'eau aléatoirement /!\ A utiliser à la fin de la fonction NewMap() /!\ //
		if (addWaterZonesBool) {
			if (Random.Range (0, FrequencyWater+7) > 6) {
				cubeNew.GetComponent<CaseData> ().casePathfinding = PathfindingCase.NonWalkable;
				cubeNew.GetComponent<SpriteRenderer> ().sprite = notWalkable;
			}
		}
	}

    void AddPlacementZones (float h) { // Créer des zones de placement /!\ A utiliser à la fin de la fonction NewMap() /!\ //
      Color placementZoneRed = ColorManager.Instance.placementZoneRed;
      Color placementZoneBlue = ColorManager.Instance.placementZoneBlue;
        
        if (h < placementZone) {
			cubeNew.GetComponent<CaseData> ().ownerPlacementZone = Player.Red;
          cubeNew.GetComponent<SpriteRenderer>().color = placementZoneRed;
		}
		if (hauteur - h - 1 < placementZone) {
			cubeNew.GetComponent<CaseData> ().ownerPlacementZone = Player.Blue;
          cubeNew.GetComponent<SpriteRenderer>().color = placementZoneBlue;
		}
	}

    void AddWinCase (Color goalColor) {
    for (int i = 3; i <= 7; i++)
      {
        Debug.Log(GameObject.Find("0" + " " + i.ToString()));
          GameObject win = GameObject.Find("0" + " " + i.ToString());
        win.GetComponent<CaseData>().winCase = Player.Red;
        win.GetComponent<CaseData>().caseColor = goalColor;
          win.GetComponent<SpriteRenderer>().color = goalColor;
      }

      for (int i = 3; i <= 7; i++)
        {
          GameObject win = GameObject.Find("20" + " " + i.ToString());
          win.GetComponent<CaseData>().winCase = Player.Blue;
          win.GetComponent<CaseData>().caseColor = goalColor;
          win.GetComponent<SpriteRenderer>().color = goalColor;
        }
    }

    public List<List<GameObject>> getMap()
    {
        List<List<GameObject>> map = new List<List<GameObject>>();

        for (int h = 0; h < hauteur; h++)
        {
            map.Add(new List<GameObject>());
            for (int l = 0; l < largeur; l++)
            {
                map[h].Add(transform.GetChild(h*largeur + l).gameObject);
            }
        }
        return map;
    }
}