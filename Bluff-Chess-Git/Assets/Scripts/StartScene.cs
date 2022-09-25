using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] float startAfter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startAfter < 0)
        {
            if(PlayerPrefs.GetString("nickName").Length < 1)
            {
                SceneManager.LoadScene("Tutorial");
            }
            else 
            {
                SceneManager.LoadScene("Menu");
            }
            
        }   
        else
        {
            startAfter -= Time.deltaTime;
        }
    }
}
