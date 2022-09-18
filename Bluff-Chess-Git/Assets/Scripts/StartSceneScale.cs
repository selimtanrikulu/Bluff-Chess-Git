using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneScale : MonoBehaviour
{
    Camera cam;

    float magicalHeight = 1334;
    float magicalWidth = 750;
    
    float magicalScale = 2.1f;


    void Start()
    {
        #if UNITY_STANDALONE_WIN
            return;
        #endif



        float width = Screen.currentResolution.width;
        float height = Screen.currentResolution.height;

        if(width > magicalWidth)
        {
            if(height > magicalHeight && height - magicalHeight > width - magicalWidth)
            {
                float newScale = (height/magicalHeight) * magicalScale;
                GetComponent<RectTransform>().localScale = new Vector3(newScale,newScale,newScale);
            }
            else
            {
                float newScale = (width/magicalWidth) * magicalScale;
                GetComponent<RectTransform>().localScale = new Vector3(newScale,newScale,newScale);
            }

            

        }


        if(height > magicalHeight)
        {

        }

    }
}
