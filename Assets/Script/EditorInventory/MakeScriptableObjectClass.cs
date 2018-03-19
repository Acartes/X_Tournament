using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/List", order = 1)]
public class MyScriptableObjectClass : ScriptableObject
{
  public List<int> balleInterceptionForHeavy;
  public List<int> balleInterceptionForLight;
}