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
        manaBarRedBarBilles[i].SetActive(true);
        if (i > manaActuelRed - 1)
          manaBarRedBarBilles[i].SetActive(false);
      }

    for (int i = 0; i < manaBarBlueBarBilles.Count; i++)
      {
        manaBarBlueBarBilles[i].SetActive(true);
        if (i > manaActuelBlue - 1)
          manaBarBlueBarBilles[i].SetActive(false);
      }
  }
}
