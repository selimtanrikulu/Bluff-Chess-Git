using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimationPiece : MonoBehaviour
{
    [SerializeField] Vector3 rightDownStep;

    Vector3 rightUpStep;
    Vector3 leftDownStep;
    Vector3 leftUpStep;

    [SerializeField] int unitID;
    [SerializeField] float movementSpeed;
    

    bool isMoving;

    IEnumerator moveCoroutine;

    Vector3 startPos;

    void Start()
    {
        rightUpStep = rightDownStep;
        rightUpStep.y *= -1;

        leftDownStep = rightDownStep;
        leftDownStep.x *= -1;

        leftUpStep = leftDownStep;
        leftUpStep.y *= -1;

        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /*
    1 -> right down
    2 -> right up
    3 -> left down
    4 -> left up
    */

    public void MoveStepAndWait(int stepID,int stepAmount,float speedFactor)
    {
        if(isMoving)return;
        isMoving = true;
        moveCoroutine = MoveStepAndWaitC(stepID,stepAmount,speedFactor);
        StartCoroutine(moveCoroutine);

    }

    IEnumerator MoveStepAndWaitC(int stepID,int stepAmount,float speedFactor)
    {
        Vector3 targetLoc = new Vector3(0,0,0);

        if(stepID == 1)
        {
            targetLoc = transform.localPosition + rightDownStep * stepAmount;
            
        }
        else if(stepID == 2)
        {
            targetLoc = transform.localPosition + rightUpStep * stepAmount;
        }
        else if(stepID == 3)
        {
            targetLoc = transform.localPosition + leftDownStep * stepAmount;
        }
        else if(stepID == 4)
        {
            targetLoc = transform.localPosition + leftUpStep * stepAmount;
        }

        Vector3 pos = transform.localPosition;
        Vector3 moveDir = (targetLoc-pos).normalized;

        while((pos - targetLoc).magnitude > 0f)
        {
            Vector3 lastMoveDir = moveDir;
            pos += moveDir * movementSpeed * Time.deltaTime * speedFactor;
            transform.localPosition = pos;
            moveDir = (targetLoc-pos).normalized;
            if(Vector3.Dot(moveDir,lastMoveDir) < 0)break;
            yield return new WaitForSeconds(Time.deltaTime);
        }


        transform.localPosition = targetLoc;

        isMoving = false;
    }

    
    public void ResetPosition()
    {
        transform.localPosition = startPos;
    }


}
