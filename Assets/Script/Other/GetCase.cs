using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCase : MonoBehaviour {

    public CaseData node;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CaseData coll = collision.transform.GetComponent<CaseData>();

        if (coll != null)
            node = coll;
    }

}
