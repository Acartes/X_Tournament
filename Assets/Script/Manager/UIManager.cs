using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>Gère tous les feedback de type UI, sur un canvas ou bien des feedback.</summary>
public class UIManager : NetworkBehaviour
{

  // *************** //
  // ** Variables ** // Toutes les variables sans distinctions
  // *************** //
  
  public Animator animChangeTurn;
  public List<GameObject> banner;
  public List<GameObject> bannerText;
  public List<Color> bannerColor;
  public List<Color> bannerTextColor;
  public GameObject phaseText;
  public List<string> phaseTextMessage;
  public int scoreRed = 0;
  public int scoreBlue = 0;
  public GameObject scoreRedGMB;
  public GameObject scoreBlueGMB;
  public GameObject messageGeneral;
  public Image Victory;
  public GameObject menuContextuel;
  public GameObject tooltip;
  public bool isScoreChanging = false;
  public Image spell1;
  public Image spell2;
  public Text remainingMana;
  public float statsOffset;
  public float manaOffset;
  GameObject StatsRed;
  GameObject StatsBlue;

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
        SwitchStatsSide(Player.Red);
        break;
      case Player.Blue:
        ChangeBanner(Player.Blue);
        SwitchStatsSide(Player.Blue);
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
        banner[0].GetComponent<Image>().color = bannerColor[0];
        bannerText[0].GetComponent<Text>().color = bannerTextColor[0];
        banner[1].GetComponent<Image>().color = bannerColor[1] - new Color(0.75f, 0.75f, 0.75f, 0.5f);
        bannerText[1].GetComponent<Text>().color = bannerTextColor[1] - new Color(0.75f, 0.75f, 0.75f, 0.5f);
      }

    if (activePlayer == Player.Blue)
      {
        banner[1].GetComponent<Image>().color = bannerColor[1];
        bannerText[1].GetComponent<Text>().color = bannerTextColor[1];
        banner[0].GetComponent<Image>().color = bannerColor[0] - new Color(0.75f, 0.75f, 0.75f, 0.5f);
        bannerText[0].GetComponent<Text>().color = bannerTextColor[0] - new Color(0.75f, 0.75f, 0.75f, 0.5f);
      }
  }

  void SwitchStatsSide(Player activePlayer)
  {
    if (TurnManager.Instance.TurnNumber < 3)
      {
        return;
      }

    float tempStatsOffset = statsOffset;
    float tempManaOffset = manaOffset;
    if (activePlayer == Player.Red)
      {
        tempStatsOffset = -tempStatsOffset;
      }
    InfoPerso.Instance.stats.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(InfoPerso.Instance.stats.transform.GetComponent<RectTransform>().anchoredPosition.x + tempStatsOffset, InfoPerso.Instance.stats.transform.GetComponent<RectTransform>().anchoredPosition.y);
    if (activePlayer == Player.Red)
      {
        spell1.transform.parent.GetComponentInParent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        spell2.transform.parent.GetComponentInParent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        remainingMana.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
      } else
      {
        spell1.transform.parent.GetComponentInParent<RectTransform>().anchoredPosition = new Vector2(tempStatsOffset, 0);
        spell2.transform.parent.GetComponentInParent<RectTransform>().anchoredPosition = new Vector2(tempStatsOffset, 0);
        remainingMana.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(remainingMana.transform.GetComponent<RectTransform>().anchoredPosition.x + tempManaOffset, remainingMana.transform.GetComponent<RectTransform>().anchoredPosition.y);
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
      spell1.sprite = selectedPerso.Spell1.buttonSprite;

    if (selectedPerso.Spell2 != null)
      spell2.sprite = selectedPerso.Spell2.buttonSprite;
  }

  /// <summary>Change le sprite des boutons de sorts par rapport au personnage selectionné.</summary>
  public void UpdateRemaningMana()
  {
    remainingMana.text = GameManager.Instance.manaGlobalActual + " / " + GameManager.Instance.manaGlobalMax + " mana.";
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
}
