using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] float scaleChangeSpeed;

    float startScale;

    [SerializeField] float minScale;
    [SerializeField] float maxScale;

    float currentScale;

    bool scaleUp = true;    

    public bool onlyDown = false;

    void Start()
    {
        startScale = transform.localScale.x;
        currentScale = startScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(onlyDown) scaleUp = false;
        ScaleUp();
        ScaleDown();
    }

    void ScaleUp()
    {
        if(!scaleUp) return;

        if(currentScale >= maxScale)scaleUp =false;
        currentScale += scaleChangeSpeed * Time.deltaTime;
        transform.localScale = new Vector3(currentScale,currentScale,currentScale);


        
    }
    void ScaleDown()
    {
        if(scaleUp) return;

        if(currentScale <= minScale)scaleUp = true;
        currentScale -= scaleChangeSpeed * Time.deltaTime;
        transform.localScale = new Vector3(currentScale,currentScale,currentScale);

        
    }

    public void Reset()
    {
        currentScale = startScale;
        onlyDown = false;
        scaleUp = true;
        
    }

}
