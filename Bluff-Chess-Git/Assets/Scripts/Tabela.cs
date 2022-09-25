using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tabela : MonoBehaviour
{
    Vector3 startPos;
    [SerializeField] Vector3 topPos;

    int state = 0;

    float timer = 0;

    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float topRotation;

    float currentRotation;
    int rand1;
    int rand2;

    void Start()
    {
        startPos = transform.localPosition;    
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
        
    }


    void Animate()
    {
        if(state == 0)
        {
            if(timer < 2)
            {
                timer += Time.deltaTime;
            }
            else
            {
                state = 1;
                timer = 0;
            }
        }
        else if(state == 1)
        {
            if(transform.localPosition.y > topPos.y)
            {
                transform.localPosition = topPos;
                state = 2;
            }
            else
            {
                Ascend();
            }

        }
        else if(state == 2)
        {
            if(timer < 0.2f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                state = 3;
                timer = 0;
                rand1 = Random.Range(0,2);
                rand2 = Random.Range(0,2);
            }
        }
        else if(state == 3)
        {
            if(currentRotation >= topRotation)
            {
                currentRotation = 0;
                state = 4;
                transform.rotation = Quaternion.identity;
            }
            else
            {
                currentRotation += Time.deltaTime * rotationSpeed;
                transform.Rotate(new Vector3(rand1==1?14.94f:-14.94f,rand2==1?8.66f:-8.66f,0),rotationSpeed*Time.deltaTime);
            }
        }
        else if(state == 4)
        {
            if(timer < 0.1f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                state = 5;
                timer = 0;
            }
        }
        else if(state == 5)
        {
            if((transform.localPosition.y < startPos.y))
            {
                transform.localPosition = startPos;
                state = 0;
            }
            else
            {
                Descend();
            }
        }




    }


    void Ascend()
    {
        Vector3 pos = transform.localPosition;
        pos.y += movementSpeed * Time.deltaTime;
        transform.localPosition = pos;
    }

    void Descend()
    {
        Vector3 pos = transform.localPosition;
        pos.y -= movementSpeed * Time.deltaTime;
        transform.localPosition = pos;
    }

}
