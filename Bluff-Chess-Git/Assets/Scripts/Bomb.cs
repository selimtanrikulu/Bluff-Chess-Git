using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [SerializeField] Image flameImage;
    [SerializeField] Image circleImage;
    
    [SerializeField] Sprite[] flameSprites;
    

    int currentFlameImageIndex = 0;

    [SerializeField] float changeFlameImageCD;
    float changeFlameImageCDCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeFlameImage();
    }


    void ChangeFlameImage()
    {
        if(changeFlameImageCDCounter < 0)
        {
            currentFlameImageIndex ++;
            if(currentFlameImageIndex >= flameSprites.Length)
            {
                currentFlameImageIndex = 0;
            }

            flameImage.sprite = flameSprites[currentFlameImageIndex];
            changeFlameImageCDCounter = changeFlameImageCD;
        }
        else
        {
            changeFlameImageCDCounter -= Time.deltaTime;
        }


    }

}
