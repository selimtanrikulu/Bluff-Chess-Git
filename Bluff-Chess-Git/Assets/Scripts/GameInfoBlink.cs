using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoBlink : MonoBehaviour
{
    [SerializeField] float scaleChangeSpeed;

    Vector3 startScale;
    float startAlpha;

    [SerializeField] Vector3 minScale;
    [SerializeField] Vector3 maxScale;

    Vector3 currentScale;

    IEnumerator blinkOneTimeCoroutine;

    bool coroutineUsing = false;

    void Start()
    {
        startScale = transform.localScale;
        currentScale = startScale;

        BlinkOneTime();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BlinkOneTimeC()
    {   

        coroutineUsing = true;
        currentScale = startScale;
        //GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);

        while(currentScale.x < maxScale.x)
        {
            ScaleUp();
            //AlphaUp();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        while(currentScale.x > minScale.x)
        {
            ScaleDown();
            //AlphaDown();
            yield return new WaitForSeconds(Time.deltaTime);
        }

        coroutineUsing = false;

    }

    void ScaleUp()
    {
        currentScale += scaleChangeSpeed * Time.deltaTime * startScale.normalized;
        transform.localScale = currentScale;  
    }
    void ScaleDown()
    {
        currentScale -= scaleChangeSpeed * Time.deltaTime * startScale.normalized;
        transform.localScale = currentScale;
    }

    void AlphaUp()
    {
        GetComponent<SpriteRenderer>().color += new Color(0,0,0,(scaleChangeSpeed * ((maxScale.x-minScale.x) / (maxScale-minScale).normalized.x))*Time.deltaTime);
    }

    void AlphaDown()
    {
        GetComponent<SpriteRenderer>().color -= new Color(0,0,0,(scaleChangeSpeed * ((maxScale.x-minScale.x) / (maxScale-minScale).normalized.x))*Time.deltaTime);
    }

    public void BlinkOneTime()
    {
        if(coroutineUsing)return;

        //inactivated
        return;

        blinkOneTimeCoroutine = BlinkOneTimeC();
        StartCoroutine(blinkOneTimeCoroutine);
    }
}
