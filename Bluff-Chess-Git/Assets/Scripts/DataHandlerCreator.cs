using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using UdpKit;

[BoltGlobalBehaviour]
public class DataHandlerCreator : GlobalEventListener
{
    float createBotTimeMin = 35;
    float createBotTimeMax = 25;


    float createBotTime;

    bool gameLockedAfterStart = false;

    Settings settings;

    IEnumerator lockGameCoroutine;
    bool coroutineStarted = false;


    bool gameStarted = false;

    void Start()
    {
        GameObject dataHandler = BoltNetwork.Instantiate( BoltPrefabs.DataHandler,new Vector2(0,0),transform.rotation);
        settings = FindObjectOfType<Settings>();
        createBotTime = Random.Range(createBotTimeMin,createBotTimeMax);
    }

    void Update()
    {
        if(!BoltNetwork.IsServer)return;


        if(!gameStarted && FindObjectsOfType<DataHandler>().Length > 1)
        {
            gameStarted = true;
            PhotonRoomProperties token = new PhotonRoomProperties();
            token.IsOpen = false; // set if the room will be open to be joined
            token.IsVisible = false; // set if the room will be visible
            BoltMatchmaking.UpdateSession(token);
            return;
        }

        if(settings.privateGame || gameStarted) return;

        if(createBotTime < 5 && !gameStarted)
        {
            if(!coroutineStarted)
            {
                coroutineStarted = true;
                lockGameCoroutine = LockGameC(5);
                StartCoroutine(lockGameCoroutine);
            }
        }
        else
        {
            createBotTime -= Time.deltaTime;
        }
    
    }



    private IEnumerator LockGameC(float after)
    {
        PhotonRoomProperties token = new PhotonRoomProperties();
        token.IsOpen = false; // set if the room will be open to be joined
        token.IsVisible = false; // set if the room will be visible

        BoltMatchmaking.UpdateSession(token);

        yield return new WaitForSeconds(after);


        if(FindObjectsOfType<DataHandler>().Length <= 1)
        {
            GameObject dataHandler = BoltNetwork.Instantiate(BoltPrefabs.DataHandler, new Vector2(0, 0), transform.rotation);
            DataHandler myDataHandler = dataHandler.GetComponent<DataHandler>();
            myDataHandler.isBot = true;

            Player[] players = FindObjectsOfType<Player>();
            foreach(Player player in players)
            {
                if (player.playerNo == 2) player.isBot = true;
            }
        }


    }


}
