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
        Sprite tempSpriteData;
        if (newPerso == SubPortrait3.newHoveredPersonnage)
        {
            tempSpriteData = SubPortrait3.GetComponent<Image>().sprite;
            SubPortrait3.setPortraitImage(SubPortrait2.GetComponent<Image>().sprite, SubPortrait2.newHoveredPersonnage);
            SubPortrait2.setPortraitImage(SubPortrait1.GetComponent<Image>().sprite, SubPortrait1.newHoveredPersonnage);
            SubPortrait1.setPortraitImage(MainPortrait.GetComponent<Image>().sprite, MainPortrait.newHoveredPersonnage);
            MainPortrait.setPortraitImage(tempSpriteData, newPerso);
            return;
        }
        
        if (newPerso == SubPortrait2.newHoveredPersonnage)
        {
            tempSpriteData = SubPortrait2.GetComponent<Image>().sprite;
            SubPortrait2.setPortraitImage(SubPortrait1.GetComponent<Image>().sprite, SubPortrait1.newHoveredPersonnage);
            SubPortrait1.setPortraitImage(MainPortrait.GetComponent<Image>().sprite, MainPortrait.newHoveredPersonnage);
            MainPortrait.setPortraitImage(tempSpriteData, newPerso);
            return;
        }
        if (newPerso == SubPortrait1.newHoveredPersonnage)
        {
            tempSpriteData = SubPortrait1.GetComponent<Image>().sprite;
            SubPortrait1.setPortraitImage(MainPortrait.GetComponent<Image>().sprite, MainPortrait.newHoveredPersonnage);
            MainPortrait.setPortraitImage(tempSpriteData, newPerso);
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
                    MainPortrait.setPortraitImage(Portrait_terre_rouge, perso);
                }
                if (persoName == "Air_rouge")
                {
                    SubPortrait1.setPortraitImage(Portrait_air_rouge, perso);

                }
                if (persoName == "Feu_rouge")
                {
                    SubPortrait2.setPortraitImage(Portrait_feu_rouge, perso);

                }
                if (persoName == "Eau_rouge")
                {
                    SubPortrait3.setPortraitImage(Portrait_eau_rouge, perso);

                }
            }
            if (owner == Player.Blue)
            {
                if (persoName == "Terre_bleu")
                {
                    MainPortrait.setPortraitImage(Portrait_terre_bleu, perso);
                }
                if (persoName == "Air_bleu")
                {
                    SubPortrait1.setPortraitImage(Portrait_air_bleu, perso);
                }
                if (persoName == "Feu_bleu")
                {
                    SubPortrait2.setPortraitImage(Portrait_feu_bleu, perso);
                }
                if (persoName == "Eau_bleu")
                {
                    SubPortrait3.setPortraitImage(Portrait_eau_bleu, perso);
                }
            }
        }
        if (turnNumber < 4) 
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
        Debug.Log(perso);
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
}
