using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour
{
    GameUI gameUI;
    [SerializeField] string buttonName;
    BoxCollider2D myCollider;

    [SerializeField] Color pressedColor;
    SpriteRenderer spriteRenderer;

    bool iWasPressed;

    void Start()
    {
        gameUI = FindObjectOfType<GameUI>();
        myCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckMouseClicks();
        HandleColor();
    }

    void CheckMouseClicks()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(IsClickedOn())iWasPressed = true;
            else iWasPressed = false;
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(IsReleasedOn())
            {
                if(iWasPressed)
                {
                    ApplyButtonClick();
                }
            }
            iWasPressed = false;
        }
    }

    void HandleColor()
    {
        if(iWasPressed)
        {
            spriteRenderer.color = pressedColor;
        }
        else
        {
            spriteRenderer.color = new Color(1,1,1,1);
        }
    }

    bool IsReleasedOn()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            if(hit.collider == myCollider) return true;
        }
        return false;
    }

    bool IsClickedOn()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            if(hit.collider == myCollider) return true;
        }
        return false;
    }


    void ApplyButtonClick()
    {
        if(buttonName == "Leave")
        {
            gameUI.LeaveButtonOnClick();
        }
        else if(buttonName == "KingClaim")
        {
            gameUI.ClaimKingButtonOnClick();
        }
        else if(buttonName == "BluffClaim")
        {
            gameUI.ClaimBluffButtonOnClick();
        }
        else if(buttonName == "Cancel")
        {
            gameUI.DontClaimKingButtonOnClick();
        }
        else if(buttonName == "Pass")
        {
            gameUI.PassButtonOnClick();
        }
        else if(buttonName == "Randomize")
        {
            gameUI.RandomizeButtonOnClick();
        }
        else if(buttonName == "Ready")
        {
            gameUI.ReadyButtonOnClick();
        }
    }
}
