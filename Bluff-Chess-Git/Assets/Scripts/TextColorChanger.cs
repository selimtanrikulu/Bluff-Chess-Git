using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextColorChanger : MonoBehaviour
{
    TextMeshProUGUI tmp;
    [SerializeField] float colorChangeSpeed;

    [SerializeField] float startRed;
    [SerializeField] float endRed;

    bool ascend = false;


    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();

        tmp.color = new Color(startRed, 0, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Color currentColor = tmp.color;


        if(ascend)
        {
            currentColor.r += colorChangeSpeed * Time.deltaTime;
        }
        else
        {
            currentColor.r -= colorChangeSpeed * Time.deltaTime;
        }

        tmp.color = currentColor;
        
        if(currentColor.r > endRed)
        {
            ascend = false;
        }
        else if(currentColor.r < startRed)
        {
            ascend = true;
        }
    }
}
