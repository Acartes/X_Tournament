using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class infoPersosPortrait : NetworkBehaviour
{

    public PortraitInteractive MainPortrait;
    public PortraitInteractive SubPortrait3;
    public PortraitInteractive SubPortrait2;
    public PortraitInteractive SubPortrait1;

    public Sprite Portrait_terre_rouge;
    public Sprite Portrait_feu_rouge;
    public Sprite Portrait_air_rouge;
    public Sprite Portrait_eau_rouge;

    public Sprite Portrait_terre_bleu;
    public Sprite Portrait_feu_bleu;
    public Sprite Portrait_air_bleu;
    public Sprite Portrait_eau_bleu;

    public void SelectPerso(PersoData newPerso)
    {
    SubPortrait3.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
    SubPortrait2.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
    SubPortrait1.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
    MainPortrait.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

    if (newPerso == MainPortrait.newHoveredPersonnage)
    {
      MainPortrait.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
    }
    if (newPerso == SubPortrait1.newHoveredPersonnage)
    {
      SubPortrait1.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
    }
      if (newPerso == SubPortrait2.newHoveredPersonnage)
    {
        SubPortrait2.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
      }

    if (newPerso == SubPortrait3.newHoveredPersonnage)
    {
        SubPortrait3.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
      }

  }

  public void SetupChangePlayerIcons(Player owner, int turnNumber)
    {
        foreach (PersoData perso in RosterManager.Instance.listHero)
        {
            string persoName = perso.gameObject.name;

            if (owner == Player.Red)
            {
                if (persoName == "Terre_rouge")
                {
                    MainPortrait.setPortraitData(Portrait_terre_rouge, Color.white, perso);
                }
                if (persoName == "Air_rouge")
                {
                    SubPortrait1.setPortraitData(Portrait_air_rouge, Color.white, perso);

                }
                if (persoName == "Feu_rouge")
                {
                    SubPortrait2.setPortraitData(Portrait_feu_rouge, Color.white, perso);

                }
                if (persoName == "Eau_rouge")
                {
                    SubPortrait3.setPortraitData(Portrait_eau_rouge, Color.white, perso);

                }
            }
            if (owner == Player.Blue)
            {
                if (persoName == "Terre_bleu")
                {
                    MainPortrait.setPortraitData(Portrait_terre_bleu, Color.white, perso);
                }
                if (persoName == "Air_bleu")
                {
                    SubPortrait1.setPortraitData(Portrait_air_bleu, Color.white, perso);
                }
                if (persoName == "Feu_bleu")
                {
                    SubPortrait2.setPortraitData(Portrait_feu_bleu, Color.white, perso);
                }
                if (persoName == "Eau_bleu")
                {
                    SubPortrait3.setPortraitData(Portrait_eau_bleu, Color.white, perso);
                }
            }
        }
        // au premier tour de jeu on met les portraits en blanc
        if (turnNumber == 3) 
        {
            UnGrayAllPortraits();
        }
    }

    public void UnGrayAllPortraits()
    {
        MainPortrait.UnGrayPortrait();
        SubPortrait1.UnGrayPortrait();
        SubPortrait2.UnGrayPortrait();
        SubPortrait3.UnGrayPortrait();
    }
    public void GrayPortraitPerso(PersoData perso)
    {
        if (MainPortrait.newHoveredPersonnage == perso)
        {
            MainPortrait.GrayPortrait();
        }
        if (SubPortrait1.newHoveredPersonnage == perso)
        {
            SubPortrait1.GrayPortrait();
        }
        if (SubPortrait2.newHoveredPersonnage == perso)
        {
            SubPortrait2.GrayPortrait();
        }
        if (SubPortrait3.newHoveredPersonnage == perso)
        {
            SubPortrait3.GrayPortrait();
        }
    }
    public void UnGrayPortraitPerso(PersoData perso)
    {
        if (MainPortrait.newHoveredPersonnage == perso)
        {
            MainPortrait.UnGrayPortrait();
        }
        if (SubPortrait1.newHoveredPersonnage == perso)
        {
            SubPortrait1.UnGrayPortrait();
        }
        if (SubPortrait2.newHoveredPersonnage == perso)
        {
            SubPortrait2.UnGrayPortrait();
        }
        if (SubPortrait3.newHoveredPersonnage == perso)
        {
            SubPortrait3.UnGrayPortrait();
        }
    }
}
