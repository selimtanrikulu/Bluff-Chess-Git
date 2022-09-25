using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public bool privateGame;
    public string nickName;
    public int avatarIndex;

    public bool audioOpen = true;


    void Awake()
    {
        if(FindObjectsOfType<Settings>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        LoadSettings();   
    }

    void Update()
    {
        
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("avatarIndex",avatarIndex);
        PlayerPrefs.SetString("nickName",nickName);
        PlayerPrefs.SetInt("audioOpen",audioOpen?1:0);
    }

    void LoadSettings()
    {
        audioOpen = (PlayerPrefs.GetInt("audioOpen")==1)?true:false;
        nickName = PlayerPrefs.GetString("nickName");
        avatarIndex = PlayerPrefs.GetInt("avatarIndex");


        //first time opened game
        if(nickName.Length < 1)
        {

            nickName = "Player";
            avatarIndex = 0;
            audioOpen = true;
            

        }

        
    }


}
