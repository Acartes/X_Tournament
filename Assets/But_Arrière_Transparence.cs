﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class But_Arrière_Transparence : NetworkBehaviour {

    SpriteRenderer img;
    short compteur;
    public Color transparencyAlpha;
    public Color opacityAlpha;

    // Use this for initialization
    public override void OnStartClient() { 
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
