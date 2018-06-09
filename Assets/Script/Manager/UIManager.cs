﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;

/// <summary>Gère tous les feedback de type UI, sur un canvas ou bien des feedback.</summary>
public class UIManager : NetworkBehaviour
{

	// *************** //
	// ** Variables ** // Toutes les variables sans distinctions
	// *************** //

	Animator animChangeTurn;
	GameObject banner;
	Text bannerJ1Text;
	Text bannerJ2Text;
	public Image directSpell;
	public Image indirectSpell;
	public List<Color> bannerColor;
	public List<Color> bannerTextColor;
	GameObject phaseText;
	public List<string> phaseTextMessage;
	public int scoreRed = 0;
	public int scoreBlue = 0;
	GameObject scoreRedGMB;
	GameObject scoreBlueGMB;
	GameObject messageGeneral;
	public Image Victory;
	public GameObject menuContextuel;
	public GameObject tooltip;
	public bool isScoreChanging = false;
	public Image spell1;
	public Image spell2;
	public Text spellButtonText1;
	public Text spellButtonText2;
	public float statsOffset;
	public float manaOffset;
	GameObject StatsRed;
	GameObject StatsBlue;
	public bool UIIsHovered = false;

	public Sprite defaultButtonSpellSprite;

	public static UIManager Instance;

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
		directSpell = GameObject.Find("DirectSpell").GetComponent<Image>();
		indirectSpell = GameObject.Find("IndirectSpell").GetComponent<Image>();
		scoreBlueGMB = GameObject.Find("scoreBlueGMB");
		scoreRedGMB = GameObject.Find("scoreRedGMB");
		scoreBlueGMB = GameObject.Find("scoreBlueGMB");
		phaseText = GameObject.Find("phaseText");
		messageGeneral = GameObject.Find("messageGeneral");
		banner = GameObject.Find("Banner");
		bannerJ1Text = GameObject.Find("PseudoJ1").GetComponent<Text>();
		bannerJ2Text = GameObject.Find("PseudoJ2").GetComponent<Text>();
		animChangeTurn = GameObject.Find("Gameplay Feedback").GetComponent<Animator>();
		TurnManager.Instance.changeTurnEvent += OnChangeTurn;
	}

	void OnDisable()
	{
		if (LoadingManager.Instance != null && LoadingManager.Instance.isGameReady())
		{
			TurnManager.Instance.changeTurnEvent -= OnChangeTurn;
		}
	}

	// *************** //
	// ** Events **    // Appel de fonctions au sein de ce script grâce à des events
	// *************** //

	void OnChangeTurn(object sender, PlayerArgs e)
	{
		animChangeTurn.SetTrigger(e.currentPlayer.ToString());

		switch (e.currentPlayer)
		{
			case Player.Red:
				ChangeBanner(Player.Red);
				break;
			case Player.Blue:
				ChangeBanner(Player.Blue);
				break;
		}

		switch (e.currentPhase)
		{
			case Phase.Placement:
				ChangePhase(0);
				break;
			case Phase.Deplacement:
				ChangePhase(1);
				break;
		}
	}

	public void ChangeBanner(Player activePlayer)
	{
		if (activePlayer == Player.Red)
		{
			banner.GetComponent<Image>().color = bannerColor[0];
			bannerJ1Text.GetComponent<Text>().color = bannerTextColor[0];
			bannerJ2Text.GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		}

		if (activePlayer == Player.Blue)
		{
			banner.GetComponent<Image>().color = bannerColor[1];
			bannerJ1Text.GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			bannerJ2Text.GetComponent<Text>().color = bannerTextColor[1];
		}
	}

	void ChangePhase(int x)
	{
		phaseText.GetComponent<Text>().text = phaseTextMessage[x];
	}

	// *************** //
	// ** Fonctions ** // Fonctions réutilisables ailleurs
	// *************** //

	/// <summary>Le gagnant marque un point.</summary>
	public IEnumerator ScoreChange(Player winner)
	{
		isScoreChanging = true;

		if (winner == Player.Red)
		{
			scoreRed++;
			scoreRedGMB.GetComponent<Text>().text = scoreRed.ToString();
			messageGeneral.GetComponent<Text>().text = "J1 marque 1 point !";
			messageGeneral.GetComponent<Text>().color = new Color(1, 0, 0, 0);
			for (int i = 0; i < 20; i++)
			{
				yield return new WaitForSeconds(0.01f);
				messageGeneral.GetComponent<Text>().color += new Color(0, 0, 0, 0.05f);
			}
			yield return new WaitForSeconds(1f);
			for (int i = 0; i < 20; i++)
			{
				yield return new WaitForSeconds(0.01f);
				messageGeneral.GetComponent<Text>().color -= new Color(0, 0, 0, 0.05f);
			}
			SelectionManager.Instance.selectedPersonnage = RosterManager.Instance.listHero[0];
			GameManager.Instance.ChangeCurrentPlayer(Player.Blue);


		}
		if (winner == Player.Blue)
		{
          
			scoreBlue++;
			scoreBlueGMB.GetComponent<Text>().text = scoreBlue.ToString();
			messageGeneral.GetComponent<Text>().text = "J2 marque 1 point !";
			messageGeneral.GetComponent<Text>().color = new Color(0, 0, 1, 0);
			for (int i = 0; i < 20; i++)
			{
				yield return new WaitForSeconds(0.01f);
				messageGeneral.GetComponent<Text>().color += new Color(0, 0, 0, 0.05f);
			}
			yield return new WaitForSeconds(0.2f);
			for (int i = 0; i < 20; i++)
			{
				yield return new WaitForSeconds(0.01f);
				messageGeneral.GetComponent<Text>().color -= new Color(0, 0, 0, 0.05f);
			}
			SelectionManager.Instance.selectedPersonnage = RosterManager.Instance.listHero[4];
			GameManager.Instance.ChangeCurrentPlayer(Player.Red);

		}

		if (scoreRed == 2 || scoreBlue == 2)
		{
			Victory.transform.gameObject.SetActive(true);
			if (scoreRed == 2)
			{
				Victory.transform.GetComponentInChildren<Text>().color = Color.red;
				Victory.transform.GetComponentInChildren<Text>().text = "Victoire du joueur rouge";
			}
			if (scoreBlue == 2)
			{
				Victory.transform.GetComponentInChildren<Text>().color = Color.blue;
				Victory.transform.GetComponentInChildren<Text>().text = "Victoire du joueur bleu";
			}
		}
		StartCoroutine(GameManager.Instance.NewManche());

		isScoreChanging = false;
	}

	/// <summary>Change le sprite des boutons de sorts par rapport au personnage selectionné.</summary>
	public void ChangeSpriteSpellButton(PersoData selectedPerso)
	{
		spell1.sprite = defaultButtonSpellSprite;
		spell2.sprite = defaultButtonSpellSprite;
	
		if (selectedPerso.Spell1 != null)
		{
			spell1.sprite = selectedPerso.Spell1.buttonSprite;
			ChangeSpellTextCost(GameObject.Find("DirectSpellCost").GetComponent<Text>(), selectedPerso.Spell1.costPA);
			for (int i = 0; i < SpellManager.Instance.spellName.Count; i++)
			{
				if (SpellManager.Instance.spellName[i] == selectedPerso.Spell1.name)
				{
					ChangeSpellTextLimit(GameObject.Find("DirectSpellLimit").GetComponent<Text>(), SpellManager.Instance.spellUse[i], selectedPerso.Spell1.maxUsePerTurn);
				}
			}
		}

		if (selectedPerso.Spell2 != null)
		{
			spell2.sprite = selectedPerso.Spell2.buttonSprite;
			ChangeSpellTextCost(GameObject.Find("IndirectSpellCost").GetComponent<Text>(), selectedPerso.Spell2.costPA);
			for (int i = 0; i < SpellManager.Instance.spellName.Count; i++)
			{
				if (SpellManager.Instance.spellName[i] == selectedPerso.Spell2.name)
				{
					ChangeSpellTextLimit(GameObject.Find("IndirectSpellLimit").GetComponent<Text>(), SpellManager.Instance.spellUse[i], selectedPerso.Spell2.maxUsePerTurn);
				}
			}
		}
	}

	/// <summary>Change le sprite des boutons de sorts par rapport au personnage selectionné.</summary>
	public void ChangeSpellTextCost(Text spellCost, int cost)
	{
		spellCost.text = " ";
		if (cost != 0)
		{
			spellCost.text = cost.ToString();
		}
	}

	/// <summary>Change le sprite des boutons de sorts par rapport au personnage selectionné.</summary>
	public void ChangeSpellTextLimit(Text spellLimit, int limit, int limitMax)
	{
		spellLimit.text = " ";
		if (limitMax != 0)
		{
			spellLimit.text = (limitMax - limit).ToString() + "/" + limitMax.ToString();
		}
	}

	public void HideStats()
	{ 
		if (StatsRed == null)
			StatsRed = GameObject.Find("StatsRed");

		if (StatsBlue == null)
			StatsBlue = GameObject.Find("StatsBlue");

		StatsRed.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
		StatsBlue.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
	}

	public void GreyButton(string spell)
	{
		if (spell == "spell 1")
		{
			spell1.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
			directSpell.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
		}

		if (spell == "spell 2")
		{
			spell2.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
			indirectSpell.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
		}
	}

	public void UngreyButton(string spell)
	{
		if (spell == "spell 1")
		{
			spell1.color = new Color(1, 1, 1, 1);
			directSpell.color = new Color(1, 1, 1, 1);
		}

		if (spell == "spell 2")
		{
			spell2.color = new Color(1, 1, 1, 1);
			indirectSpell.color = new Color(1, 1, 1, 1);
		}
	}
}
