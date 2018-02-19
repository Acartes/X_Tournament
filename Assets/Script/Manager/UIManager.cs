using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : NetworkBehaviour {

	public List<GameObject> banner;
	public List<GameObject> bannerText;
	public List<Color> bannerColor;
	public List<Color> bannerTextColor;
	public GameObject phaseText;
	public List<string> phaseTextMessage;

	int nbrPlayer = 2;

	public int scoreRed = 0;
	public int scoreBlue = 0;
	public GameObject scoreRedGMB;
	public GameObject scoreBlueGMB;
	public GameObject messageGeneral;
  public GameObject menuContextuel;
 

	public bool isScoreChanging = false;

	public static UIManager Instance;


    public override void OnStartClient()
    {
        if (Instance == null)
            Instance = this;
        Debug.Log(this.GetType() + " is Instanced");
        StartCoroutine(waitForInit());
    }

    IEnumerator waitForInit()
    {
        while (!LobbyManager.Instance.IsInstancesLoaded())
            yield return new WaitForEndOfFrame();
        Init();
    }

    private void Init()
    {
        banner.Add (GameObject.Find ("UIJ1Banner"));
      banner.Add (GameObject.Find ("UIJ2Banner"));
		bannerText.Add (GameObject.Find ("UIJ1BannerText"));
      bannerText.Add (GameObject.Find ("UIJ2BannerText"));
		phaseText = GameObject.Find ("phaseText");


		scoreRedGMB = GameObject.Find ("scoreRedGMB");
		scoreBlueGMB = GameObject.Find ("scoreBlueGMB");
		messageGeneral = GameObject.Find ("messageGeneral");

		for (int i = 0; i < nbrPlayer; i++) {
			bannerColor.Add (banner[i].GetComponent<Image> ().color);
			bannerTextColor.Add (bannerText[i].GetComponent<Text> ().color);
		}
		TurnManager.Instance.changeTurnEvent += OnChangeTurn;
	}

    void OnDisable()
    {
        if (LobbyManager.Instance.IsInstancesLoaded())
        {
            TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
        }
	}
	
	void OnChangeTurn (object sender, PlayerArgs e) {
		switch (e.currentPlayer) {
		case Player.Red:
			ChangeBanner (0);
			break;
		case Player.Blue:
			ChangeBanner (1);
			break;
		}

		switch (e.currentPhase) {
		case Phase.Placement:
			ChangePhase (0);
			break;
		case Phase.Deplacement:
			ChangePhase (1);
			break;
		}
	}

	void ChangeBanner (int x) {
		for (int i = 0; i < nbrPlayer; i++) {
			if (i == x) {
				banner [i].GetComponent<Image> ().color = bannerColor [i];
				bannerText [i].GetComponent<Text> ().color = bannerTextColor [i];
			} else {
				banner [i].GetComponent<Image> ().color = bannerColor [i] - new Color (0.75f, 0.75f, 0.75f, 0.5f);
				bannerText [i].GetComponent<Text> ().color = bannerTextColor [i] - new Color (0.75f, 0.75f, 0.75f, 0.5f);
			}
		}
	}

	void ChangePhase (int x) {
			phaseText.GetComponent<Text> ().text = phaseTextMessage [x];
	}

	public IEnumerator ScoreChange (Player winCase) {
		isScoreChanging = true;

		GameManager.Instance.PauseGame ();

		if (winCase == Player.Blue) {
			scoreRed++;
			scoreRedGMB.GetComponent<Text> ().text = scoreRed.ToString();
			messageGeneral.GetComponent<Text> ().text = "J1 marque 1 point !";
			messageGeneral.GetComponent<Text> ().color = new Color (1, 0, 0,0);
			for (int i = 0; i < 20; i++) {
				yield return new WaitForSeconds (0.01f);
				messageGeneral.GetComponent<Text> ().color += new Color (0, 0, 0,0.05f);
			}
			yield return new WaitForSeconds (1f);
			for (int i = 0; i < 20; i++) {
				yield return new WaitForSeconds (0.01f);
				messageGeneral.GetComponent<Text> ().color -= new Color (0, 0, 0,0.05f);
			}
		}
		if (winCase == Player.Red) {
			scoreBlue++;
			scoreBlueGMB.GetComponent<Text> ().text = scoreBlue.ToString();
			messageGeneral.GetComponent<Text> ().text = "J2 marque 1 point !";
			messageGeneral.GetComponent<Text> ().color = new Color (0, 0, 1,0);
			for (int i = 0; i < 20; i++) {
				yield return new WaitForSeconds (0.01f);
				messageGeneral.GetComponent<Text> ().color += new Color (0, 0, 0,0.05f);
			}
			yield return new WaitForSeconds (0.2f);
			for (int i = 0; i < 20; i++) {
				yield return new WaitForSeconds (0.01f);
				messageGeneral.GetComponent<Text> ().color -= new Color (0, 0, 0,0.05f);
			}

		}

		StartCoroutine(GameManager.Instance.NewManche ());

		isScoreChanging = false;
	}


}
