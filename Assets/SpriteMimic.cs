using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SpriteMimic : MonoBehaviour {

    public SpriteRenderer sourceRender;
    public Image selfRender;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        selfRender = GetComponent<Image>();
        sourceRender = transform.parent.parent.GetComponent<SpriteRenderer>();
        selfRender.sprite = sourceRender.sprite;
	}
}
