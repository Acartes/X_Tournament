﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SelectionScreen : MonoBehaviour {

	public List<GameObject> selectionScreenPortraitList;

	public static SelectionScreen Instance;

	public string tagName;

	public float anchorInitMinX;
	public float anchorInitMaxX;
	public float anchorInitMinY;
	public float anchorInitMaxY;

	public float anchorIncX;
	public float anchorIncY;
	public int y = 0;
	public int x = 0;

	// Use this for initialization
	void Awake() {
		if (Instance == null) {
			Instance = this;
		}
		y = 0;
		x = 0;
	}
	
	// Update is called once per frame
	void Update () {
		ConfigurePortraits ();
		UIResize ();
	}

	void ConfigurePortraits () {
		selectionScreenPortraitList.Clear ();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tagName)) {
			selectionScreenPortraitList.Add (obj);
		}
	}

	void UIResize () {
		foreach (GameObject obj in selectionScreenPortraitList) {
			x++;
			if (x * anchorIncX > 0.9f) {
				x = 1;
				y--;
			}

			obj.GetComponent<RectTransform> ().anchorMin = new Vector2 (anchorInitMinX + anchorIncX*x, anchorInitMinY + anchorIncY*y);
			obj.GetComponent<RectTransform> ().anchorMax = new Vector2 (anchorInitMaxX + anchorIncX*x, anchorInitMaxY + anchorIncY*y);
		}
		y = 0;
		x = 0;
	}

}
