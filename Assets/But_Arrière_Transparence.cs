using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class But_Arrière_Transparence : MonoBehaviour {

    SpriteRenderer img;
    short compteur;
    public Color transparencyAlpha;
    public Color opacityAlpha;

    // Use this for initialization
    void Start () {
        img = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(compteur > 0)
        {
            img.color = transparencyAlpha;
        }
        else
        {
            img.color = opacityAlpha;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Personnage" || other.tag == "Ballon")
            ++compteur;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Personnage" || other.tag == "Ballon")
            --compteur;
    }
}
