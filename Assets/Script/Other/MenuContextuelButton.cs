using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContextuelButton : MonoBehaviour
{

  SpriteRenderer spriteR;

  public Color colorEnter;
  public Color colorExit;

  public bool collision;

  void Start()
  {
    spriteR = GetComponent<SpriteRenderer>();
  }

  public bool Collision()
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

  public void MouseExit()
  {
    ChangeColor(colorExit);
  }

  public void MouseOver()
  {
    ChangeColor(colorEnter);
  }

  private void Update()
  {
    if (Collision())
    {
      collision = true;
      MouseOver();
    }
    else
    {
      collision = false;
      MouseExit();
    }
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
