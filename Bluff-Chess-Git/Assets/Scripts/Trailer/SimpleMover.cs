using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    [SerializeField] float startMovementSpeed;
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float acceleration;

    [SerializeField] Vector3 direction;
    [SerializeField] float startTime;

    float timePast = 0;
    float currentMovementSpeed = 0;

    void Update()
    {
        timePast += Time.deltaTime;

        if(startTime <= timePast)
        {
            
            currentMovementSpeed += acceleration * Time.deltaTime;
            if(currentMovementSpeed > maxMovementSpeed)currentMovementSpeed = maxMovementSpeed;

            Vector3 pos = transform.position;
            pos += direction * currentMovementSpeed * Time.deltaTime;
            transform.position = pos;
        }


    }
}
