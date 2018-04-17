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

    private void Update()
    {
            if (Collision())
            {
                OnMouseOver();
                if (Input.GetKey("mouse 0"))
                {
                OnMouseDown();
                }
            }
            else
                OnMouseExit();
    }

    bool Collision()
    {
        if (Camera.current == null)
            return false;
        if (Input.mousePosition.x <= Camera.current.WorldToScreenPoint(new Vector3(transform.position.x - spriteR.bounds.size.x / 2, transform.position.y, transform.position.z)).x
            || Input.mousePosition.x >= Camera.current.WorldToScreenPoint(new Vector3(transform.position.x + spriteR.bounds.size.x / 2, transform.position.y, transform.position.z)).x)
            return false;
        if (Input.mousePosition.y <= Camera.current.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - spriteR.bounds.size.y / 2, transform.position.z)).y
            || Input.mousePosition.y <= Camera.current.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - spriteR.bounds.size.y / 2, transform.position.z)).y)
            return false;

        return true;
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
    Debug.Log("LOL");
    if (RpcFunctions.Instance.localId == 0 && TurnManager.Instance.currentPlayer == Player.Blue)
      return;
    if (RpcFunctions.Instance.localId == 1 && TurnManager.Instance.currentPlayer == Player.Red)
      return;

    RpcFunctions.Instance.CmdMenuContextuelClick(name);
        CaseManager.Instance.EnableAllColliders();
        ChangeColor(colorExit);
  }

  void OnEnable()
  {
    ChangeColor(colorExit);
    StartCoroutine(DebugCollider());
  }

  IEnumerator DebugCollider()
  {
    yield return new WaitForSeconds(0.05f);
  }

  void ChangeColor(Color newColor)
  {
    if (spriteR != null)
      spriteR.color = newColor;
  }
}
