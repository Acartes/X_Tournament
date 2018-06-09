using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ManaManager : NetworkBehaviour
{

	public static ManaManager Instance;
	public int manaMaxRed;
	public int manaActuelRed;
	public int manaMaxBlue;
	public int manaActuelBlue;

	Text remainingManaRed;
	Text remainingManaBlue;
	GameObject manaBarRedTopBar;
	GameObject manaBarRedBotBar;
	GameObject manaBarBlueTopBar;
	GameObject manaBarBlueBotBar;
	public List<GameObject> manaBarRedBarBilles = new List<GameObject>();
	public List<GameObject> manaBarBlueBarBilles = new List<GameObject>();

	GameObject manaBarRedFX;
	GameObject manaBarBlueFX;

	Color initialBilleColor = new Color();

	// ******************** //
	// ** Initialisation ** // Fonctions de départ, non réutilisable
	// ******************** //

	public override void OnStartClient()
	{
		if (Instance == null)
			Instance = this;
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
		remainingManaRed = GameObject.Find("manaBarRedText").GetComponent<Text>();
		remainingManaBlue = GameObject.Find("manaBarBlueText").GetComponent<Text>();
		manaBarRedTopBar = GameObject.Find("manaBarRedTopBar");
		manaBarRedBotBar = GameObject.Find("manaBarRedBotBar");
		manaBarBlueTopBar = GameObject.Find("manaBarBlueTopBar");
		manaBarBlueBotBar = GameObject.Find("manaBarBlueBotBar");
		manaBarRedFX = GameObject.Find("manaBarRedFX");
		manaBarBlueFX = GameObject.Find("manaBarBlueFX");

		manaBarRedFX.SetActive(false);
		manaBarBlueFX.SetActive(false);

		List<GameObject> manaBarRedTopBarBilles = new List<GameObject>();
		List<GameObject> manaBarRedBotBarBilles = new List<GameObject>();
		List<GameObject> manaBarBlueTopBarBilles = new List<GameObject>();
		List<GameObject> manaBarBlueBotBarBilles = new List<GameObject>();

		foreach (Transform bille in manaBarRedTopBar.GetComponentsInChildren<Transform>())
		{
			manaBarRedTopBarBilles.Add(bille.gameObject);
		}
		foreach (Transform bille in manaBarRedBotBar.GetComponentsInChildren<Transform>())
		{
			manaBarRedBotBarBilles.Add(bille.gameObject);
		}
		foreach (Transform bille in manaBarBlueTopBar.GetComponentsInChildren<Transform>())
		{
			manaBarBlueTopBarBilles.Add(bille.gameObject);
		}
		foreach (Transform bille in manaBarBlueBotBar.GetComponentsInChildren<Transform>())
		{
			manaBarBlueBotBarBilles.Add(bille.gameObject);
		}

		manaBarRedTopBarBilles.RemoveAt(0);
		manaBarRedBotBarBilles.RemoveAt(0);
		manaBarBlueTopBarBilles.RemoveAt(0);
		manaBarBlueBotBarBilles.RemoveAt(0);
		initialBilleColor = manaBarRedTopBarBilles[0].GetComponent<Image>().color;

		for (int i = 0; i < manaBarRedTopBarBilles.Count; i++)
		{
			manaBarRedBarBilles.Add(manaBarRedTopBarBilles[i]);
			manaBarRedBarBilles.Add(manaBarRedBotBarBilles[i]);
			manaBarBlueBarBilles.Add(manaBarBlueTopBarBilles[i]);
			manaBarBlueBarBilles.Add(manaBarBlueBotBarBilles[i]);
		}

		manaMaxRed = 20;
		manaActuelRed = 20;
		manaMaxBlue = 20;
		manaActuelBlue = 20;
	}

	// *************** //
	// ** Fonctions ** // Fonctions réutilisables ailleurs
	// *************** //

	/// <summary>Change les points de mana actuel du joueur de tel montant.</summary>
	public void ChangeActualMana(Player player, int value)
	{
		switch (player)
		{
			case Player.Red:
				manaActuelRed -= value;
				break;
			case Player.Blue:
				manaActuelBlue -= value;
				break;
		}
		UpdateMana();
	}

	/// <summary>Change les points de mana max du joueur de tel montant.</summary>
	public void ChangeMaxMana(Player player, int value, bool updateActualManaToo)
	{
		switch (player)
		{
			case Player.Red:
				manaMaxRed -= value;
				if (updateActualManaToo)
					manaActuelRed = manaMaxRed;
				break;
			case Player.Blue:
				manaMaxBlue -= value;
				if (updateActualManaToo)
					manaActuelBlue = manaMaxBlue;
				break;
		}
		UpdateMana();
	}

	/// <summary>Update les valeurs affichées sur les barres de mana des deux joueurs.</summary>
	void UpdateMana()
	{
		remainingManaRed.text = "Mana : " + manaActuelRed + " / " + manaMaxRed;
		remainingManaBlue.text = "Mana : " + manaActuelBlue + " / " + manaMaxBlue;

		for (int i = 0; i < manaBarRedBarBilles.Count; i++)
		{
			manaBarRedBarBilles[i].GetComponent<Image>().color = initialBilleColor;
			manaBarRedBarBilles[i].SetActive(true);
			if (i > manaActuelRed - 1)
				manaBarRedBarBilles[i].SetActive(false);
		}

		for (int i = 0; i < manaBarBlueBarBilles.Count; i++)
		{
			manaBarBlueBarBilles[i].GetComponent<Image>().color = initialBilleColor;
			manaBarBlueBarBilles[i].SetActive(true);
			if (i > manaActuelBlue - 1)
				manaBarBlueBarBilles[i].SetActive(false);
		}
	}

	public void Actived()
	{
		if (SelectionManager.Instance.selectedPersonnage == null)
			return;
		
		switch (SelectionManager.Instance.selectedPersonnage.owner)
		{
			case Player.Red:
				manaBarRedFX.SetActive(true);
				break;
			case Player.Blue:
				manaBarBlueFX.SetActive(true);
				break;
		}
	}

	public void Desactived()
	{
		if (SelectionManager.Instance.selectedPersonnage == null)
			return;
		
		switch (SelectionManager.Instance.selectedPersonnage.owner)
		{
			case Player.Red:
				manaBarRedFX.SetActive(false);
				break;
			case Player.Blue:
				manaBarBlueFX.SetActive(false);
				break;
		}
	}

	public void SpellButtonFeedbackON(int cost)
	{
		if (SelectionManager.Instance.selectedPersonnage.owner == Player.Red)
		{
			remainingManaRed.text = "Mana : " + manaActuelRed + " / " + manaMaxRed + "<color=#ff0000ff> - " + cost + "</color>";
			for (int i = 0; i < manaBarRedBarBilles.Count; i++)
			{
				if (i > manaActuelRed - 1 - cost)
				{
					manaBarRedBarBilles[i].GetComponent<Image>().color = new Color(1, 0, 0, 0.6f);
				}
			}
		} else
		{
			remainingManaBlue.text = "Mana : " + manaActuelBlue + " / " + manaMaxBlue + "<color=#ff0000ff> - " + cost + "</color>";
			for (int i = 0; i < manaBarRedBarBilles.Count; i++)
			{
				if (i > manaActuelBlue - 1 - cost)
				{
					manaBarBlueBarBilles[i].GetComponent<Image>().color = new Color(1, 0, 0, 0.6f);
				}
			}
		}
	}

	public void SpellButtonFeedbackOFF()
	{
		UpdateMana();
	}
}
