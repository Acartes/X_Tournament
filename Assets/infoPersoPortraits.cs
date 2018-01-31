using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoPersoPortraits : MonoBehaviour
{

    public GameObject MainPortrait;
    public GameObject SubPortrait1;
    public GameObject SubPortrait2;
    public GameObject SubPortrait3;

    public Sprite Portrait_terre_rouge;
    public Sprite Portrait_feu_rouge;
    public Sprite Portrait_air_rouge;
    public Sprite Portrait_eau_rouge;

    public Sprite Portrait_terre_bleu;
    public Sprite Portrait_feu_bleu;
    public Sprite Portrait_air_bleu;
    public Sprite Portrait_eau_bleu;


    // Use this for initialization
    public void setMainPortrait(Sprite newSprite, Player owner)
    {
        MainPortrait.GetComponent<Image>().sprite = GetIcon(newSprite, owner);
    }
    // Use this for initialization
    public void setSubPortrait1(Sprite newSprite, Player owner)
    {
        SubPortrait1.GetComponent<Image>().sprite = GetIcon(newSprite, owner);

    }
    // Use this for initialization
    public void setSubPortrait2(Sprite newSprite, Player owner)
    {
        SubPortrait2.GetComponent<Image>().sprite = GetIcon(newSprite, owner);

    }
    // Use this for initialization
    public void setSubPortrait3(Sprite newSprite, Player owner)
    {
        SubPortrait3.GetComponent<Image>().sprite = GetIcon(newSprite, owner);
    }

    public void Clear()
    {
        MainPortrait.GetComponent<Image>().sprite = null;
        SubPortrait1.GetComponent<Image>().sprite = null;
        SubPortrait2.GetComponent<Image>().sprite = null;
        SubPortrait3.GetComponent<Image>().sprite = null;
    }

    Sprite GetIcon(Sprite sprite, Player owner)
    {

        if(owner == Player.Red)
        {
            if (sprite.name == "Air_rouge" || sprite.name == "Air_dos_rouge")
                return Portrait_air_rouge;
            if (sprite.name == "Terre_rouge" || sprite.name == "Terre_dos_rouge")
                return Portrait_terre_rouge;
            if (sprite.name == "Feu_rouge" || sprite.name == "Feu_dos_rouge")
                return Portrait_feu_rouge;
            if (sprite.name == "Eau_rouge" || sprite.name == "Eau_dos_rouge")
                return Portrait_eau_rouge;
        }
        else
        {
            if (sprite.name == "Air_bleu" || sprite.name == "Air_dos_bleu")
                return Portrait_air_bleu;
            if (sprite.name == "Terre_bleu" || sprite.name == "Terre_dos_bleu")
                return Portrait_terre_bleu;
            if (sprite.name == "Feu_bleu" || sprite.name == "Feu_dos_bleu")
                return Portrait_feu_bleu;
            if (sprite.name == "Eau_bleu" || sprite.name == "Eau_dos_bleu")
                return Portrait_eau_bleu;
        }
        return null;
    }
}
