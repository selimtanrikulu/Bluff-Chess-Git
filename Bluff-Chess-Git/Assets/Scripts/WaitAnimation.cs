using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimation : MonoBehaviour
{
    [SerializeField] GameObject shadow;
    [SerializeField] GameObject blackKingImage;

    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpSpeed;

    IEnumerator rotateCoroutine;
    IEnumerator jumpCoroutine;

    Vector3 blackKingStartPos;

    [SerializeField] float minShadowScale;
    [SerializeField] float maxShadowScale;


    void Start()
    {
        
        blackKingStartPos = blackKingImage.transform.position;



        Jump();
    }

    void Update()
    {
        //HandleShadow();
    }


    void HandleShadow()
    {
        float scale = Mathf.Lerp(minShadowScale,maxShadowScale,blackKingImage.transform.position.y-blackKingStartPos.y/ -3- blackKingStartPos.y );
        shadow.transform.localScale = new Vector3(scale,scale,scale);
    }

    void RotateDegrees()
    {
        rotateCoroutine = RotateDegreesC(30);
        StartCoroutine(rotateCoroutine);
    }

    private IEnumerator RotateDegreesC(float degrees)
    {
        float xStep = 1;

        float degreeDrop = 2;

        Vector3 startPos = blackKingImage.transform.position;


        while(degrees > 0)
        {
            float currentDegrees = 0;

            while(currentDegrees < degrees)
            {
                blackKingImage.transform.Rotate(-Vector3.forward,Time.deltaTime*rotationSpeed);
                currentDegrees += rotationSpeed*Time.deltaTime;

                Vector3 pos = blackKingImage.transform.position;
                pos.x += xStep*Time.deltaTime;
                blackKingImage.transform.position = pos;


                yield return new WaitForSeconds(Time.deltaTime);
            }

            currentDegrees = 0;
            degrees -= degreeDrop;

            while(currentDegrees < degrees * 2)
            {
                blackKingImage.transform.Rotate(Vector3.forward,Time.deltaTime*rotationSpeed);
                currentDegrees += rotationSpeed*Time.deltaTime;

                Vector3 pos = blackKingImage.transform.position;
                pos.x -= xStep*Time.deltaTime;
                blackKingImage.transform.position = pos;

                yield return new WaitForSeconds(Time.deltaTime);
            }

            currentDegrees = 0;
            degrees -= degreeDrop;

            while(currentDegrees < degrees)
            {
                blackKingImage.transform.Rotate(-Vector3.forward,Time.deltaTime*rotationSpeed);
                currentDegrees += rotationSpeed*Time.deltaTime;

                Vector3 pos = blackKingImage.transform.position;
                pos.x += xStep*Time.deltaTime;
                blackKingImage.transform.position = pos;

                yield return new WaitForSeconds(Time.deltaTime);
            }



        }


        blackKingImage.transform.position = startPos;
        blackKingImage.transform.rotation = Quaternion.identity;
        

       

    }


    void Jump()
    {
        blackKingImage.GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpSpeed);
        blackKingImage.GetComponent<Rigidbody2D>().gravityScale = 5;
        jumpCoroutine = JumpC();
        StartCoroutine(jumpCoroutine);

    }

    private IEnumerator JumpC()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        
        while(blackKingStartPos.y <= blackKingImage.transform.position.y)
        { 
            yield return new WaitForSeconds(Time.deltaTime);
        }

        blackKingImage.transform.position = blackKingStartPos;
        blackKingImage.GetComponent<Rigidbody2D>().velocity = new Vector2();

        blackKingImage.GetComponent<Rigidbody2D>().gravityScale = 0;

    }



}
