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

        if (RpcFunctions.Instance.localId == 0 && TurnManager.Instance.currentPlayer == Player.Blue)
            return;
        if (RpcFunctions.Instance.localId == 1 && TurnManager.Instance.currentPlayer == Player.Red)
            return;

        RpcFunctions.Instance.CmdMenuContextuelClick(name);

        ChangeColor(colorExit);
    }

    void OnEnable()
    {
        ChangeColor(colorExit);
    }

    void ChangeColor(Color newColor)
    {
        if (spriteR != null)
            spriteR.color = newColor;
    }
}
