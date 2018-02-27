using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MenuContextuel : NetworkBehaviour {

	public static MenuContextuel Instance;

	GameObject ballon;
  bool gameInit = true;


    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log(this.GetType() + " is Instanced");
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
		ballon = GameObject.Find ("Ballon");
      if (gameInit)
        {
          UIManager.Instance.menuContextuel = this.gameObject;
          this.gameObject.SetActive(false);
          gameInit = false;
        }
        TurnManager.Instance.changeTurnEvent += OnChangeTurn;
      //  Desactive();
	}

    void OnEnable()
  {

    if (LoadingManager.Instance.isGameReady())
      {
        ballon = GameObject.Find("Ballon");
          Debug.Log(ballon.transform.position);
        GetComponent<RectTransform>().offsetMin = ballon.transform.position;
          GetComponent<RectTransform>().offsetMax = ballon.transform.position;
      }
    }

	void OnDisable () {
      //  Active();
	}

  void OnChangeTurn(object sender, PlayerArgs e)
  { // Lorsqu'un joueur termine son tour

    SelectionManager.Instance.Deselect(TurnManager.Instance.currentPhase, TurnManager.Instance.currentPlayer);

    if (MenuContextuel.Instance != null)
      {
        MenuContextuel.Instance.gameObject.SetActive(false);
      }
  } 

/*	public void Active () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("case")) {
		//	obj.GetComponent<PolygonCollider2D> ().enabled = false;
		}
		//Pathfinding.Instance.
	}*/

/*	public void Desactive () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("case")) {
			obj.GetComponent<PolygonCollider2D> ().enabled = true;
		}
	}*/
}
