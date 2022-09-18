using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfigure : MonoBehaviour
{
    Camera cam;

    float magicalWidthHeightRatio = 0.5622188f;
    float magicalSize = 5;

    float magicalProduct;

    void Start()
    {
        #if UNITY_STANDALONE_WIN
            return;
        #endif


        

        magicalProduct = magicalSize * magicalWidthHeightRatio;

        cam = FindObjectOfType<Camera>();

        float width = Screen.currentResolution.width;
        float height = Screen.currentResolution.height;

        float widthHeightRatio = width/height;

        if(widthHeightRatio > magicalWidthHeightRatio) return;

        float supposedSize = magicalProduct / widthHeightRatio;

        cam.orthographicSize = supposedSize;
    }

    void Update()
    {
        
    }
}
