using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class infoPersoPortraits : NetworkBehaviour
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
        Sprite tempSprite;
        Color tempColor;
        PersoData tempPersoData;
        if (newPerso == SubPortrait3.newHoveredPersonnage)
        {
            tempSprite = SubPortrait3.GetComponent<Image>().sprite;
            tempColor = SubPortrait3.GetComponent<Image>().color;
            tempPersoData = SubPortrait3.newHoveredPersonnage;
            SubPortrait3.setPortraitData(SubPortrait2);
            SubPortrait2.setPortraitData(SubPortrait1);
            SubPortrait1.setPortraitData(MainPortrait);
            MainPortrait.setPortraitData(tempSprite, tempColor, tempPersoData);
            return;
        }
        
        if (newPerso == SubPortrait2.newHoveredPersonnage)
        {
            tempSprite = SubPortrait2.GetComponent<Image>().sprite;
            tempColor = SubPortrait2.GetComponent<Image>().color;
            tempPersoData = SubPortrait2.newHoveredPersonnage;
            SubPortrait2.setPortraitData(SubPortrait1);
            SubPortrait1.setPortraitData(MainPortrait);
            MainPortrait.setPortraitData(tempSprite, tempColor, tempPersoData);
            return;
        }
        if (newPerso == SubPortrait1.newHoveredPersonnage)
        {
            tempSprite = SubPortrait1.GetComponent<Image>().sprite;
            tempColor = SubPortrait1.GetComponent<Image>().color;
            tempPersoData = SubPortrait1.newHoveredPersonnage;
            SubPortrait1.setPortraitData(MainPortrait);
            MainPortrait.setPortraitData(tempSprite, tempColor, tempPersoData);
            return;
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
