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

    public Sprite Portrait_terre_bleu;
    public Sprite Portrait_feu_bleu;
    public Sprite Portrait_air_bleu;


    // Use this for initialization
    public void setMainPortrait(Sprite newSprite, Player owner)
    {
        MainPortrait.GetComponent<Image>().sprite = GetIcon(newSprite, owner);
        Debug.Log(owner);
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
            if (sprite.name == "Air" || sprite.name == "Air_dos")
                return Portrait_air_rouge;
            if (sprite.name == "Terre" || sprite.name == "Terre_dos")
                return Portrait_terre_rouge;
            if (sprite.name == "Feu" || sprite.name == "Feu_dos")
                return Portrait_feu_rouge;
        }
        else
        {
            if (sprite.name == "Air" || sprite.name == "Air_dos")
                return Portrait_air_bleu;
            if (sprite.name == "Terre" || sprite.name == "Terre_dos")
                return Portrait_terre_bleu;
            if (sprite.name == "Feu" || sprite.name == "Feu_dos")
                return Portrait_feu_bleu;
        }
        Debug.LogError("No sprite was possible to show. Check conditions.");
        return null;
    }
}
