using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneScaler : MonoBehaviour
{
    Camera cam;

    float magicalHeight = 1334;
    float magicalWidth = 750;
    [SerializeField] float magicalScale = 1.61f;

    void Start()
    {
        #if UNITY_STANDALONE_WIN
            return;
        #endif

        float magicalWH = magicalWidth/magicalHeight;
        float magicalHW = 1 / magicalWH;


        float width = Screen.currentResolution.width;
        float height = Screen.currentResolution.height;


        if(width > magicalWidth)
        {
            if(height > magicalHeight && height/magicalHeight > width / magicalWidth)
            {
                float newScale = (height/magicalHeight)*magicalScale;
                GetComponent<RectTransform>().localScale = new Vector3(newScale,newScale,newScale);
            }
            else
            {
                float newScale = (width/magicalWidth)*magicalScale;
                GetComponent<RectTransform>().localScale = new Vector3(newScale,newScale,newScale);
            }
            
        }


        
    }
}
