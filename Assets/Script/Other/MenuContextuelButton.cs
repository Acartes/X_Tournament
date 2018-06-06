using System.Collections;
using UnityEngine;

public class MenuContextuelButton : MonoBehaviour
{

  SpriteRenderer spriteR;

  public Color colorEnter;
  public Color colorExit;
  public Color colorDisable;

  public bool collision;

  GameObject menuContextuelReplacerTooltip;
  GameObject menuContextuelTirerTooltip;

  void Start()
  {
    menuContextuelReplacerTooltip = GameObject.Find("MenuContextuelReplacerTooltip");
    menuContextuelTirerTooltip = GameObject.Find("MenuContextuelTirerTooltip");
    spriteR = GetComponent<SpriteRenderer>();

    switch (name)
      {
      case "MenuContextuelReplacer":
        menuContextuelReplacerTooltip.SetActive(false);
        break;
      case "MenuContextuelTirer":
        menuContextuelTirerTooltip.SetActive(false);
        break;
      }
  }

  public bool Collision()
  {
    if (spriteR.color == colorDisable)
      {
        return false;
      }
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
    switch (name)
      {
      case "MenuContextuelReplacer":
        menuContextuelReplacerTooltip.SetActive(false);
        break;
      case "MenuContextuelTirer":
        menuContextuelTirerTooltip.SetActive(false);
        break;
      }
  }

  public void MouseOver()
  {
    ChangeColor(colorEnter);
    switch (name)
      {
      case "MenuContextuelReplacer":
        menuContextuelReplacerTooltip.SetActive(true);
        break;
      case "MenuContextuelTirer":
        menuContextuelTirerTooltip.SetActive(true);
        break;
      }
  }

  public void Disable()
  {
    ChangeColor(colorDisable);
  }

  private void Update()
  {
    if (Collision())
      {
        collision = true;
        MouseOver();
      } else
      {
        collision = false;
        MouseExit();
      }
    if (MenuContextuel.Instance.activated)
      {
        if (name == "MenuContextuelReplacer" && SelectionManager.Instance.selectedPersonnage.actualPointMovement == 0)
          {
            Disable();
          }
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
