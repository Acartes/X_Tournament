/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlacementUI : MonoBehaviour {

  public float row;

  public List<GameObject> allPerso;

  public GameObject placementUIPerso;

  public static PlacementUI Instance;

  public float ySpace;
  public float xSpace;

  public bool canGenerate;

  GameObject placementUISelection;

	// Use this for initialization
	void Start () {
    Instance = this;
    placementUISelection = GameObject.Find("placementUISelection");
	}

    void Update () {
    if (canGenerate)
      {
        canGenerate = false;
        Generate();
      }
    }

    void Generate () {
    if (allPerso.Count != 0)
      {
        foreach (GameObject obj in allPerso)
          {
            DestroyImmediate(obj);
          }
          allPerso.Clear();
      }

    for (int i = 1; i < row+1; i++)
      {
          GameObject newRow = (GameObject)Instantiate(placementUIPerso, Vector3.zero, Quaternion.identity);
          allPerso.Add(newRow);
          newRow.transform.parent = transform;
          newRow.GetComponent<RectTransform>().anchorMin = new Vector2(((1/row)*i)-((1/row))+xSpace, 0+ySpace);
          newRow.GetComponent<RectTransform>().anchorMax = new Vector2(((1/row)*i)-xSpace, 1-ySpace);
          newRow.GetComponent<RectTransform>().offsetMin = Vector2.zero;
          newRow.GetComponent<RectTransform>().offsetMax = Vector2.zero;
      }
    placementUISelection.transform.SetAsLastSibling();
    }
}
*/