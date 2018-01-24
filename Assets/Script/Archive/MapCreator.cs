using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class MapCreator : MonoBehaviour {

    public bool activate;
    public int x = 3;
    public int y = 3;
    int tempx;
    int tempy;
    float xWidth;
    float yHeight;
    Vector2 startPoint;

    public GameObject toInstantiate;

    // Use this for initialization
    void Start () {
        xWidth = toInstantiate.GetComponent<SpriteRenderer>().sprite.rect.width;
        yHeight = toInstantiate.GetComponent<SpriteRenderer>().sprite.rect.height;
        startPoint = Vector3.zero;
    }

    // Update is called once per frame
    void Update () {
        if (activate)
        {
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            for (tempy = 0; tempy < y; tempy++)
            {
                for (tempx = 0; tempx < x; tempx++)
                {
                    GameObject go = Instantiate(toInstantiate, transform);
                    go.transform.position = go.transform.position = new Vector3(startPoint.x + tempx * (xWidth) / 200, startPoint.y + tempx * (yHeight) / 200, 0);
                }
                startPoint = new Vector2(startPoint.x + (xWidth) / 200, startPoint.y - (yHeight) / 200);
            }
            activate = false;
        }
    }

    void DeleteAllChidren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
