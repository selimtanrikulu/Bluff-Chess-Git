using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStop : MonoBehaviour
{
     [SerializeField] float startMovementSpeed;
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float accelerationChangeAmount;

    [SerializeField] Vector3 direction;
    [SerializeField] float startTime;
    [SerializeField] Vector3 destination;


    float timePast = 0;
    float currentMovementSpeed = 0;


    void Update()
    {
        timePast += Time.deltaTime;

        if(startTime <= timePast)
        {
            acceleration += accelerationChangeAmount*Time.deltaTime;
            currentMovementSpeed += acceleration * Time.deltaTime;

            if(currentMovementSpeed > maxMovementSpeed)currentMovementSpeed = maxMovementSpeed;

            if((destination-transform.localPosition).magnitude < 30)
            {
                transform.localPosition = destination;
                currentMovementSpeed = startMovementSpeed;
            }
            else
            {
                Vector3 pos = transform.position;
                pos += direction * currentMovementSpeed * Time.deltaTime;
                transform.position = pos;
            }
        }

    }
}
