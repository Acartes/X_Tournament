using UnityEngine;
using UnityEditor;

public class Tools : EditorWindow
{

  // à mettre exemple : Debug.Log(Tools.testInt);

  static public float travelTime;
  static public float travelTimeBallon;
  static public float ballStrenght;

  [MenuItem("X's Settings/Configure")]
  private static void NewNestedOption()
    {
      if (EditorWindow.GetWindow(typeof(Tools)) != null)
      EditorWindow.GetWindow(typeof(Tools));
    }

     static public void Awake () {
      travelTime = EditorPrefs.GetFloat("travelTime");
      travelTimeBallon = EditorPrefs.GetFloat("travelTimeBallon");
      ballStrenght = EditorPrefs.GetFloat("ballStrenght");
      }



  void OnGUI()
    {
      travelTime = EditorGUILayout.FloatField("travelTime", travelTime);
      travelTimeBallon = EditorGUILayout.FloatField("travelTimeBallon", travelTimeBallon);
      ballStrenght = EditorGUILayout.FloatField("ballStrenght", ballStrenght);

        if (GUILayout.Button("Save"))
          {
          EditorPrefs.SetFloat("travelTime", travelTime);
          EditorPrefs.SetFloat("travelTimeBallon", travelTimeBallon);
          EditorPrefs.SetFloat("ballStrenght", ballStrenght);

          }

    if (GUILayout.Button("Close"))
      {
        this.Close();
      }

      var reports = CrashReport.reports;
      GUILayout.Label("Crash reports:");
        foreach (var r in reports) {
          GUILayout.BeginHorizontal();
          GUILayout.Label("Crash: " + r.time);
            if (GUILayout.Button("Log")) {
              Debug.Log(r.text);
            }
            if (GUILayout.Button("Remove")) {
              r.Remove();
            }
          GUILayout.EndHorizontal();
        }

    }
  }