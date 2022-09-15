using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using UdpKit;

[BoltGlobalBehaviour]
public class DataHandlerCreator : GlobalEventListener
{
    float createBotTime = 15;

    Settings settings;
    void Start()
    {
        GameObject dataHandler = BoltNetwork.Instantiate( BoltPrefabs.DataHandler,new Vector2(0,0),transform.rotation);
        settings = FindObjectOfType<Settings>();
    }

    void Update()
    {
        if (FindObjectsOfType<DataHandler>().Length > 1 || settings.privateGame) return;

        

        if(createBotTime < 0)
        {
            GameObject dataHandler = BoltNetwork.Instantiate(BoltPrefabs.DataHandler, new Vector2(0, 0), transform.rotation);
            DataHandler myDataHandler = dataHandler.GetComponent<DataHandler>();
            myDataHandler.isBot = true;

            PhotonRoomProperties token = new PhotonRoomProperties();
            token.IsOpen = true; // set if the room will be open to be joined
            token.IsVisible = false; // set if the room will be visible

            BoltMatchmaking.UpdateSession(token);

            Player[] players = FindObjectsOfType<Player>();
            foreach(Player player in players)
            {
                if (player.playerNo == 2) player.isBot = true;
            }

        }
        else
        {
            createBotTime -= Time.deltaTime;
        }
        


    }


}
