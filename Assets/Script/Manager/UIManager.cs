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
  public GameObject menuContextuel;
  public GameObject tooltip;
  public bool isScoreChanging = false;
  public Image spell1;
  public Image spell2;

  public Sprite defaultButtonSpellSprite;

  public static UIManager Instance;

  // ******************** //
  // ** Initialisation ** // Fonctions de départ, non réutilisable
  // ******************** //

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
        ChangeBanner(0);
        break;
      case Player.Blue:
        ChangeBanner(1);
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

  void ChangeBanner(int x)
  {
    for (int i = 0; i < 2; i++)
      {
        if (i == x)
          {
            banner[i].GetComponent<Image>().color = bannerColor[i];
            bannerText[i].GetComponent<Text>().color = bannerTextColor[i];
          } else
          {
            banner[i].GetComponent<Image>().color = bannerColor[i] - new Color(0.75f, 0.75f, 0.75f, 0.5f);
            bannerText[i].GetComponent<Text>().color = bannerTextColor[i] - new Color(0.75f, 0.75f, 0.75f, 0.5f);
          }
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

    if (winner == Player.Blue)
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
      }
    if (winner == Player.Red)
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


}
