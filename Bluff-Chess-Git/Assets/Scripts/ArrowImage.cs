using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowImage : MonoBehaviour
{
    Rigidbody2D rb;
    
    [SerializeField] float verticalForce;
    [SerializeField] float YDist;
    float maxY;
    float minY;



    public float centerY;
    public float centerX;


    bool isAscending;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        centerY = transform.position.y;
    }

    void Update()
    {
        Ascend();
        Descend();

        Debug.Log("Hello");

        maxY = centerY + YDist;
        minY = centerY - YDist;



    }



    void Ascend()
    {


        if(!isAscending)return;
        
        rb.AddForce(new Vector2(0,1)*verticalForce * Time.deltaTime);

        if(transform.position.y > maxY)
        {
            isAscending = false;
        } 
    }


    void Descend()
    {
        if(isAscending) return;

        rb.AddForce(new Vector2(0,-1)*verticalForce * Time.deltaTime);

        if(transform.position.y < minY)
        {
            isAscending = true;
        } 
    }
}
