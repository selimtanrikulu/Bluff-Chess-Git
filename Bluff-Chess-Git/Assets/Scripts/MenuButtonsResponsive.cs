using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonsResponsive : MonoBehaviour
{
    float magicalHeight = 1334;
    float magicalWidth = 750;
    float magicalYPos ;

    [SerializeField] float maxYGap;


    float maxWH = 0.8f;


    void Start()
    {
        #if UNITY_STANDALONE_WIN
            return;
        #endif

        magicalYPos = GetComponent<RectTransform>().position.y;

        float magicalWH =  magicalWidth/magicalHeight;

        float width = Screen.currentResolution.width;
        float height = Screen.currentResolution.height;

        float WH = width/height;

        if(WH > magicalWH)
        {
            float yDist = ExponentialLerp(maxYGap,WH/maxWH,4);

            Vector3 pos = GetComponent<RectTransform>().position;
            GetComponent<RectTransform>().position = new Vector3(pos.x,pos.y+yDist,pos.z);
        }

        float ExponentialLerp(float to,float ratio,float exp)
        {
            float result = Mathf.Pow(ratio,exp) * maxYGap;
            return result;
        }
        
    }
}
