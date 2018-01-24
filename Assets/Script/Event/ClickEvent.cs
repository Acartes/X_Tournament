using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickEvent : MonoBehaviour
{

	public static System.Action newClickEvent;

    private void OnMouseDown()
    {
		if (HoverManager.Instance.hoveredCase != null) {
		newClickEvent ();
		}
    }
}
