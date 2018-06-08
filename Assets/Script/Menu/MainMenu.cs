using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

  public void PlayGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void QuitGame()
  {
    Application.Quit();
  }

  public void Parsec()
  {
    Debug.Log("aazazaza");
    Application.OpenURL("https://parsecgaming.com/");
  }
}
