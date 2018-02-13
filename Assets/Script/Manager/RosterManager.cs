using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosterManager : MonoBehaviour {
  
  // *************** //
  // ** Variables ** //
  // *************** //

	public List<PersoData> listHeroJ1;
	public List<PersoData> listHeroJ2;
  public List<List<PersoData>> listHeroJXToPlace = new List<List<PersoData>>();
  [ReadOnly] public List<PersoData> listHero;
  [ReadOnly] public List<PersoData> listHeroPlaced;

	[HideInInspector] public static RosterManager Instance;

	public PersoData persoCreated;

	void Awake () {
		Instance = this;
	}

	void Start () {
        GameObject persoCreated;
      listHeroJXToPlace.Add(new List<PersoData>());
      listHeroJXToPlace.Add(new List<PersoData>());
		foreach (PersoData obj in listHeroJ1) {
			persoCreated = (GameObject)Instantiate (obj.gameObject, new Vector3 (999, 999, 999), Quaternion.identity);
          persoCreated.GetComponent<PersoData>().owner = Player.Red;
          listHeroJXToPlace[0].Add(persoCreated.GetComponent<PersoData>());
		}

		foreach (PersoData obj in listHeroJ2) {
			persoCreated = (GameObject)Instantiate (obj.gameObject, new Vector3 (999, 999, 999), Quaternion.identity);
          persoCreated.GetComponent<PersoData>().owner = Player.Blue;
          listHeroJXToPlace[1].Add(persoCreated.GetComponent<PersoData>());
		}
          
      listHeroJ1.Clear();
	  listHeroJ2.Clear();
      listHero.AddRange(listHeroJXToPlace[0]);
      listHero.AddRange(listHeroJXToPlace[1]);
	}
}
