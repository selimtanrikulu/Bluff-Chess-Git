using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicUIForMenu : MonoBehaviour
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

        float WH = width/height;
        float HW = height/width;

        if(WH > magicalWH)
        {
            float magicalRatio = magicalWH * magicalScale;
            float newScale = magicalRatio / WH;
            GetComponent<RectTransform>().localScale = new Vector3(newScale,newScale,newScale);

        }
        else if(HW > magicalHW)
        {
            float magicalRatio = magicalHW * magicalScale;
            float newScale = magicalRatio / HW;
            GetComponent<RectTransform>().localScale = new Vector3(newScale,newScale,newScale);

        }



        
    }


}
