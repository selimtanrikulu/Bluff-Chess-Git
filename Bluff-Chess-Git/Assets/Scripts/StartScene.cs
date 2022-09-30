using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;



public class StartScene : MonoBehaviour
{
    [SerializeField] float startAfter;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Image background;

    bool videoStarted = false;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayVideo());


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
        else if(videoStarted)
        {
            startAfter -= Time.deltaTime;
        }
    }


    IEnumerator PlayVideo(){
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared){
            yield return waitForSeconds;
            break;
             //yield return null;
        }
        videoStarted = true;
        background.gameObject.SetActive(true);
        videoPlayer.Play();
       
    }
   


}
