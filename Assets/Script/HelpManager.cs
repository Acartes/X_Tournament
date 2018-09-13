using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HelpManager : MonoBehaviour {

    public static HelpManager Instance;

    Image helpScreen;
    Image cross;
    Image right;
    Image left;
    Image sumary;

    Vector3 cameraPos;

    public List<Sprite> pageList;
    int pageActual;

	void Awake ()
    {
        Instance = this;
        helpScreen = GameObject.Find("HelpScreen").GetComponent<Image>();
        cross = GameObject.Find("HelpScreen/Cross").GetComponent<Image>();
        right = GameObject.Find("HelpScreen/Right").GetComponent<Image>();
        left = GameObject.Find("HelpScreen/Left").GetComponent<Image>();
        sumary = GameObject.Find("HelpScreen/Sumary").GetComponent<Image>();
        cameraPos = Camera.main.transform.position;
        pageActual = 0;
        HideHelpScreen();
	}

	public void ShowHelpScreen ()
    {
        if (helpScreen.enabled == true)
        {
            HideHelpScreen();
            return;
        }
        ChangeImageStatut(true);
        cameraPos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3 (600,600,600);
        
	}

    public void HideHelpScreen()
    {
        Camera.main.transform.position = cameraPos;
        ChangeImageStatut(false);
    }

    public void NextPage(int increment)
    {
        pageActual += increment;

        if (pageActual > pageList.Count-1)
        {
            pageActual = 0;
        }

        if (pageActual < 0)
        {
            pageActual = pageList.Count-1;
        }

        helpScreen.sprite = pageList[pageActual];
    }

    void ChangeImageStatut(bool isEnabled)
    {
        helpScreen.enabled = isEnabled;
        cross.enabled = isEnabled;
        right.enabled = isEnabled;
        left.enabled = isEnabled;
        sumary.enabled = isEnabled;
    }
}
