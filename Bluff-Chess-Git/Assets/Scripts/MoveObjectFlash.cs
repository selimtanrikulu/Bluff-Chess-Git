using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectFlash : MonoBehaviour
{
    [SerializeField] float flashSpeed;
    SpriteRenderer spriteRenderer;

    bool brighten = false;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    void Update()
    {
        Brighten();
        Darken();
    }

    void Brighten()
    {
        if(!brighten) return;

        Color currentColor = spriteRenderer.color;
        currentColor.a -= flashSpeed *Time.deltaTime;
        spriteRenderer.color = currentColor;
        if(currentColor.a <= 0)brighten = false;
        
    }


    void Darken()
    {
        if(brighten) return;

        Color currentColor = spriteRenderer.color;
        currentColor.a += flashSpeed *Time.deltaTime;
        spriteRenderer.color = currentColor;
        if(currentColor.a >= 1)brighten = true;

    }
}
