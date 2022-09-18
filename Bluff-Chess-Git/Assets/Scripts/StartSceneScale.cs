using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneScale : MonoBehaviour
{
    Camera cam;


    void Start()
    {
        #if UNITY_STANDALONE_WIN
            return;
        #endif

        float newScale = 0;

        float width = Screen.currentResolution.width;
        float height = Screen.currentResolution.height;

    }
}
