using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStopMove : MonoBehaviour
{
    [SerializeField] float startMovementSpeed;
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float acceleration;

    [SerializeField] Vector3 direction;
    [SerializeField] float startTime;
    [SerializeField] float stopDuration;
    [SerializeField] Vector3 destination;


    float timePast = 0;
    float currentMovementSpeed = 0;


    void Update()
    {
        timePast += Time.deltaTime;

        if(startTime <= timePast)
        {
        
            currentMovementSpeed += acceleration * Time.deltaTime;
            if(currentMovementSpeed > maxMovementSpeed)currentMovementSpeed = maxMovementSpeed;

            if((destination-transform.localPosition).magnitude < 100)
            {
                transform.localPosition = destination;
                stopDuration -= Time.deltaTime;
                currentMovementSpeed = startMovementSpeed;
            }
            else
            {
                Vector3 pos = transform.position;
                pos += direction * currentMovementSpeed * Time.deltaTime;
                transform.position = pos;
            }
        }

        if(stopDuration <= 0)
        {
            startTime = 999;
            
            currentMovementSpeed += acceleration * Time.deltaTime;
            if(currentMovementSpeed > maxMovementSpeed)currentMovementSpeed = maxMovementSpeed;

            Vector3 pos = transform.position;
            pos += direction * currentMovementSpeed * Time.deltaTime;
            transform.position = pos;
        }


    }
}
