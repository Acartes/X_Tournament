using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HelpManager : MonoBehaviour {

    public static HelpManager Instance;
    Image helpScreen;
    Vector3 cameraPos;

	void Awake ()
    {
        Instance = this;
        helpScreen = GameObject.Find("HelpScreen").GetComponent<Image>();
        cameraPos = Camera.main.transform.position;
        HideHelpScreen();
	}

	public void ShowHelpScreen ()
    {
        if (helpScreen.enabled == true)
        {
            HideHelpScreen();
            return;
        }
        cameraPos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3 (600,600,600);
        helpScreen.enabled = true;
	}

    public void HideHelpScreen()
    {
        Camera.main.transform.position = cameraPos;
        helpScreen.enabled = false;
    }
}
