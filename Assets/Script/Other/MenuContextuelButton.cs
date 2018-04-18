using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContextuelButton : MonoBehaviour
{

  SpriteRenderer spriteR;

  public Color colorEnter;
  public Color colorExit;

  void Start()
  {
    spriteR = GetComponent<SpriteRenderer>();
  }

  void OnMouseExit()
  {
    ChangeColor(colorExit);
  }

  void OnMouseOver()
  {
    ChangeColor(colorEnter);
  }

  void OnMouseDown()
  {
    if (!SynchroManager.Instance.canPlayTurn())
      {
        return;
      }

    RpcFunctions.Instance.CmdMenuContextuelClick(name);

    ChangeColor(colorExit);
  }

  void OnEnable()
  {
    ChangeColor(colorExit);
    GetComponent<BoxCollider2D>().enabled = false;
    StartCoroutine(DebugCollider());
  }

  IEnumerator DebugCollider()
  {
    yield return new WaitForSeconds(0.05f);
    GetComponent<BoxCollider2D>().enabled = true;
  }

  void ChangeColor(Color newColor)
  {
    if (spriteR != null)
      spriteR.color = newColor;
  }
}
