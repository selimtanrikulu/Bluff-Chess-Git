using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class Unit : MonoBehaviour
{


    [SerializeField] int myIndex;
    [SerializeField] public string unitType;
    [SerializeField] public int teamNo;

    GameController gameController;

    DataHandler dataHandler;

    [SerializeField] Sprite hideSpriteWhite;
    [SerializeField] Sprite hideSpriteBlack;

    [SerializeField] Sprite startSprite;


    IEnumerator unHideCoroutine;


    [SerializeField] public GameObject moveImageObject;
    [SerializeField] Sprite moveKnight;
    [SerializeField] Sprite moveKing;
    [SerializeField] Sprite moveQueen;
    [SerializeField] Sprite moveBishop;
    [SerializeField] Sprite moveRook;


    AudioController audioController;


    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        audioController = FindObjectOfType<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!dataHandler)
        {
            DataHandler[] dataHandlers = FindObjectsOfType<DataHandler>();
            foreach(DataHandler dataHandler in dataHandlers)
            {
                if(dataHandler.entity.IsOwner && !dataHandler.isBot)this.dataHandler = dataHandler;
            }
            return;
        }
        //HandleBlockIndex();

        
        UpdateLocation();
    }

    public void GetHide()
    {
        if(teamNo == 1)GetComponent<SpriteRenderer>().sprite = hideSpriteWhite;
        else GetComponent<SpriteRenderer>().sprite = hideSpriteBlack;



    }

    //----------
    public void UnHideForSeconds(float seconds)
    {
        unHideCoroutine = UnHideForSecondsC(seconds);
        StartCoroutine(unHideCoroutine);
    }

    IEnumerator UnHideForSecondsC(float seconds)
    {
        GetUnHide();
        yield return new WaitForSeconds(seconds);
        if(teamNo != FindObjectOfType<GameUI>().myPlayer.playerNo) GetHide();

    }
    //-----------

    void GetUnHide()
    {
        GetComponent<SpriteRenderer>().sprite = startSprite;
    }

    void HandleBlockIndex()
    {
        if(dataHandler.GetUnitsBlockIndex(myIndex) > -1)dataHandler.SetUnitIndexToBlock(myIndex,dataHandler.GetUnitsBlockIndex(myIndex));
    }

    void UpdateLocation()
    {
        if(dataHandler.GetUnitsBlockIndex(myIndex) == -3)
        {
            audioController.PlaySound("sacrificeSound");
        }


        if (dataHandler.GetUnitsBlockIndex(myIndex) == -1) 
        {
            GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        }
            

        else if(dataHandler.GetUnitsBlockIndex(myIndex) == -2 || (dataHandler.GetUnitsBlockIndex(myIndex) == -3 && unitType == "Pawn"))
        {
            GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
            FindObjectOfType<GameUI>().AddImageToGraveyard(unitType,teamNo,transform.position);
            Destroy(gameObject);
        } 
        else if(dataHandler.GetUnitsBlockIndex(myIndex) == -3)
        {
            GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
            FindObjectOfType<GameUI>().AddImageToGraveyard("Empty",teamNo,transform.position);
            Destroy(gameObject);
        }
        else 
        {
            GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
            bool locationChanged = false;

            if((transform.position - gameController.allBlocks[dataHandler.GetUnitsBlockIndex(myIndex)].transform.position).magnitude > 0.1f)
            {
                locationChanged = true;
                
            }

            if(locationChanged && dataHandler.GetGameState() == 2)
            {


                gameController.allBlocks[gameController.GetBlockIndexAtLocation(transform.position)].GetYellow();
                gameController.allBlocks[dataHandler.GetUnitsBlockIndex(myIndex)].GetYellow();


                //Activate "like" move image
                if(dataHandler.GetWhoseTurn() == FindObjectOfType<GameUI>().myPlayer.playerNo)
                {
                    moveImageObject.SetActive(true);

                    if(dataHandler.GetKingClaimed())
                    {
                        moveImageObject.GetComponent<SpriteRenderer>().sprite = moveKing;
                    }
                    else
                    {
                        string whatTypeMovementDone = gameController.allBlocks[gameController.GetBlockIndexAtLocation(transform.position)].WhatTypeCanMoveToBlock(gameController.allBlocks[dataHandler.GetUnitsBlockIndex(myIndex)]);
                        if(whatTypeMovementDone == "Knight")moveImageObject.GetComponent<SpriteRenderer>().sprite = moveKnight;
                        else if(whatTypeMovementDone == "Rook")moveImageObject.GetComponent<SpriteRenderer>().sprite = moveRook;
                        else if(whatTypeMovementDone == "Bishop")moveImageObject.GetComponent<SpriteRenderer>().sprite = moveBishop;
                    }
                    
                }
                
                

            }

            transform.position = gameController.allBlocks[dataHandler.GetUnitsBlockIndex(myIndex)].transform.position;

            
            

        }

    }

    public void MoveToBlockIndex(int index)
    {
        dataHandler.SetUnitIndexToBlock(-1,dataHandler.GetUnitsBlockIndex(myIndex));
        dataHandler.SetBlockIndexToUnit(myIndex,index);
        dataHandler.SetUnitIndexToBlock(myIndex,index);
    }

    public void SelfDestroy()
    {
        dataHandler.SetUnitIndexToBlock(-1,dataHandler.GetUnitsBlockIndex(myIndex));
        Destroy(gameObject);
    }

}
