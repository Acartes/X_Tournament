using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosterManager : MonoBehaviour {
  
  // *************** //
  // ** Variables ** //
  // *************** //

	public List<GameObject> listHeroJ1;
	public List<GameObject> listHeroJ2;
  public List<List<GameObject>> listHeroJXToPlace = new List<List<GameObject>>();
  [ReadOnly] public List<GameObject> listHero;
  [ReadOnly] public List<GameObject> listHeroPlaced;

	[HideInInspector] public static RosterManager Instance;

	public GameObject persoCreated;

	void Awake () {
		Instance = this;
	}

	void Start () {
      GameObject persoCreated;
      listHeroJXToPlace.Add(new List<GameObject>());
      listHeroJXToPlace.Add(new List<GameObject>());
		foreach (GameObject obj in listHeroJ1) {
			persoCreated = (GameObject)Instantiate (obj, new Vector3 (999, 999, 999), Quaternion.identity);
          listHeroJXToPlace[0].Add(persoCreated);
		}

		foreach (GameObject obj in listHeroJ2) {
			persoCreated = (GameObject)Instantiate (obj, new Vector3 (999, 999, 999), Quaternion.identity);
          listHeroJXToPlace[1].Add(persoCreated);
		}
          
      listHeroJ1.Clear();
	  listHeroJ2.Clear();
      listHero.AddRange(listHeroJXToPlace[0]);
      listHero.AddRange(listHeroJXToPlace[1]);
	}
}
