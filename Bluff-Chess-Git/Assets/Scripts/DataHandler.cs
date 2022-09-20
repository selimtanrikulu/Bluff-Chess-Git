using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using UdpKit;


public class DataHandler : EntityBehaviour<IDataHandler>
{


    DataHandler other;

    GameController gameController;
    AudioController audioController;
    GameUI gameUI;

    //0 -> pre start
    //1 -> move
    //2 -> choose bluff or pass
    //3 -> sacrifice
    //4 -> game finished
    //5 -> checking bluff
    //6 -> true bluff claim
    //7 -> setup phase
    //8 -> round finished
    //9 -> enemy left

    [SerializeField] GameObject[] unitPrefabs;

    public bool isBot = false;

    //changed file

    void Start()
    {


        //revert for black player
        if(BoltNetwork.IsClient && entity.IsOwner)
        {
            FindObjectOfType<Camera>().transform.Rotate(Vector3.forward,180);
            RotateUnits();
            //FindObjectOfType<GameUI>().RotateWorld();
        }

        
    }


    void Update()
    {

        if(!entity.IsOwner)return;
        if (isBot)
        {
            if(state.blackNickName != "Bot" && state.blackAvatarIndex < 5)
            {
                state.blackNickName = "Bot";
                int botAvatarIndex = Random.Range(9, 15);
                state.blackAvatarIndex = botAvatarIndex;

            }

            

            return;
        }

        //they work once
        SetStartVaribles();
        if (!audioController) audioController = FindObjectOfType<AudioController>();
        SetInitialData();


        if(!gameController) return;

        if(!other)
        {
            DataHandler[] dataHandlers = FindObjectsOfType<DataHandler>();
            foreach(DataHandler dataHandler in dataHandlers)
            {
                if(dataHandler != this)other = dataHandler;
            }
        }

        HandleTimes();
        Replicate(); 

    }



    

    void RotateUnits()
    {
        Unit[] units = FindObjectsOfType<Unit>();
        foreach(Unit unit in units)
        {
            unit.transform.Rotate(Vector3.forward,180);
        }


    }


    public string GetUnitTypeInCachedBlock()
    {
        if(state.cachedBlockIndex < 0) return "";
        if (GetBlocksUnitIndex(state.cachedBlockIndex) < 0) return "";
        return gameController.allUnits[GetBlocksUnitIndex(state.cachedBlockIndex)].unitType;
    }

    public void ResetRound(int roundNo)
    {
        if(roundNo % 2 == 0) state.whoseTurn = 1;
        else state.whoseTurn = 2;


        state.gameState = 7;
        state.cachedUnitIndex = -1;
        state.cachedBlockIndex = -1;    
        state.lastMoveWasBluff = false;
        state.timeLeftBlack = 30;
        state.timeLeftWhite = 30;
        state.kingRevealed = false;


        for(int i=0;i<16;i++)
        {
            if(gameController.allUnits[i])Destroy(gameController.allUnits[i].gameObject);

            GameObject unitObject = Instantiate(unitPrefabs[i],transform.position,Quaternion.identity);
            Unit unit = unitObject.GetComponent<Unit>();
            gameController.allUnits[i] = unit;
        }

        for(int i=0;i<36;i++)state.blocksUnitIndices[i] = -1;
        for(int i=0;i<16;i++)state.unitsBlockIndices[i] = -1;

        state.blocksUnitIndices[0] = 0;
        state.unitsBlockIndices[0] = 0;

        state.blocksUnitIndices[1] = 1;
        state.unitsBlockIndices[1] = 1;

        state.blocksUnitIndices[2] = 2;
        state.unitsBlockIndices[2] = 2;

        state.blocksUnitIndices[3] = 3;
        state.unitsBlockIndices[3] = 3;

        state.blocksUnitIndices[4] = 4;
        state.unitsBlockIndices[4] = 4;

        state.blocksUnitIndices[8] = 5;
        state.unitsBlockIndices[5] = 8;

        state.blocksUnitIndices[6] = 6;
        state.unitsBlockIndices[6] = 6;

        state.blocksUnitIndices[7] = 7;
        state.unitsBlockIndices[7] = 7;


        state.blocksUnitIndices[27] = 13;
        state.unitsBlockIndices[13] = 27;

        state.blocksUnitIndices[28] = 15;
        state.unitsBlockIndices[15] = 28;

        state.blocksUnitIndices[29] = 14;
        state.unitsBlockIndices[14] = 29;

        state.blocksUnitIndices[31] = 11;
        state.unitsBlockIndices[11] = 31;

        state.blocksUnitIndices[32] = 12;
        state.unitsBlockIndices[12] = 32;

        state.blocksUnitIndices[33] = 8;
        state.unitsBlockIndices[8] = 33;

        state.blocksUnitIndices[34] = 10;
        state.unitsBlockIndices[10] = 34;

        state.blocksUnitIndices[35] = 9;
        state.unitsBlockIndices[9] = 35;




        if(BoltNetwork.IsClient && entity.IsOwner)RotateUnits();

        FindObjectOfType<GameUI>().ClearGraveyard();



        if(FindObjectOfType<GameUI>().myPlayer.playerNo == 2)
        {
            for(int i=0;i<5;i++)
            {
                gameController.allUnits[i].GetHide();
            }
        }
        else
        {
            for(int i=8;i<13;i++)
            {
                gameController.allUnits[i].GetHide();
            }
        }


        gameController.roundOverCoroutineStarted = false;


    }

    void SetInitialData()
    {
        if(!gameController)
        {
            gameController = FindObjectOfType<GameController>();
            if(gameController)
            {
                state.gameState = 0;
                state.whoseTurn = 1;
                state.actionNumber = 0;
                state.cachedUnitIndex = -1;
                state.cachedBlockIndex = -1;
                state.lastMoveWasBluff = false;
                state.timeLeftBlack = 30;
                state.timeLeftWhite = 30;
                state.kingRevealed = false;

                
                if (FindObjectOfType<GameUI>().myPlayer.playerNo == 1)
                {
                    state.whiteNickName = FindObjectOfType<Settings>().nickName;
                    state.whiteAvatarIndex = FindObjectOfType<Settings>().avatarIndex;
                }
                else
                {
                    state.blackNickName = FindObjectOfType<Settings>().nickName;
                    state.blackAvatarIndex = FindObjectOfType<Settings>().avatarIndex;
                }
                

                

                for(int i=0;i<36;i++)state.blocksUnitIndices[i] = -1;

                state.blocksUnitIndices[0] = 0;
                state.unitsBlockIndices[0] = 0;

                state.blocksUnitIndices[1] = 1;
                state.unitsBlockIndices[1] = 1;

                state.blocksUnitIndices[2] = 2;
                state.unitsBlockIndices[2] = 2;

                state.blocksUnitIndices[3] = 3;
                state.unitsBlockIndices[3] = 3;

                state.blocksUnitIndices[4] = 4;
                state.unitsBlockIndices[4] = 4;

                state.blocksUnitIndices[8] = 5;
                state.unitsBlockIndices[5] = 8;

                state.blocksUnitIndices[6] = 6;
                state.unitsBlockIndices[6] = 6;

                state.blocksUnitIndices[7] = 7;
                state.unitsBlockIndices[7] = 7;


                state.blocksUnitIndices[27] = 13;
                state.unitsBlockIndices[13] = 27;

                state.blocksUnitIndices[28] = 15;
                state.unitsBlockIndices[15] = 28;

                state.blocksUnitIndices[29] = 14;
                state.unitsBlockIndices[14] = 29;
        
                state.blocksUnitIndices[31] = 11;
                state.unitsBlockIndices[11] = 31;

                state.blocksUnitIndices[32] = 12;
                state.unitsBlockIndices[12] = 32;

                state.blocksUnitIndices[33] = 8;
                state.unitsBlockIndices[8] = 33;

                state.blocksUnitIndices[34] = 10;
                state.unitsBlockIndices[10] = 34;

                state.blocksUnitIndices[35] = 9;
                state.unitsBlockIndices[9] = 35;


            }

        }


        if(!gameUI)
        {
            gameUI = FindObjectOfType<GameUI>();
            if(gameUI)
            {
                if(BoltNetwork.IsClient)gameUI.RotateWorld();
                
            }
        }

        


    }

    public void MakeCacheDied()
    {
        //caller block was empty
        
        if(state.cachedUnitIndex != -1)
        {
            state.unitsBlockIndices[state.cachedUnitIndex] = -2;
            if(gameController.allUnits[state.cachedUnitIndex].unitType == "Queen")
            {
                MakeUnitDied(GetBlocksUnitIndex(state.cachedBlockIndex),false);
            }
        }

        state.cachedUnitIndex = -1;
    }

    public string GetCachedUnitType()
    {
        if (state.cachedUnitIndex < 0) return "";
        return gameController.allUnits[state.cachedUnitIndex].unitType;
    }
    public void MakeUnitDied(int unitIndex,bool sacrified)
    {
        

        if(unitIndex == -1)return;
        state.blocksUnitIndices[state.unitsBlockIndices[unitIndex]] = -1;

        if(sacrified)state.unitsBlockIndices[unitIndex] = -3;
        else state.unitsBlockIndices[unitIndex] = -2
        ;

    }

    public void MakeUnitCached(int unitIndex)
    {
        //caller block was empty
        if(unitIndex == -1)return;
        state.cachedUnitIndex = unitIndex;
        state.unitsBlockIndices[unitIndex] = -1;
    }


    public void MakeBlockCached(int blockIndex)
    {
        //
        state.cachedBlockIndex = blockIndex;
    }

    public void MakeCacheReturn()
    {
        //kill existing unit
        state.unitsBlockIndices[state.blocksUnitIndices[state.cachedBlockIndex]] = -2;
        state.blocksUnitIndices[state.cachedBlockIndex] = -1;

        if(state.cachedUnitIndex != -1)
        {
            state.unitsBlockIndices[state.cachedUnitIndex] = state.cachedBlockIndex;
            state.blocksUnitIndices[state.cachedBlockIndex] = state.cachedUnitIndex;
            state.cachedUnitIndex = -1;
        }

        state.cachedBlockIndex = -1;

    }

    public void SetLastMoveBluff(bool bluff)
    {
        state.lastMoveWasBluff = bluff;
    }

    public int GetUnitsBlockIndex(int unitIndex)
    {
        return state.unitsBlockIndices[unitIndex];
    }

    public int GetBlocksUnitIndex(int blockIndex)
    {
        return state.blocksUnitIndices[blockIndex];
    }



    void SetStartVaribles()
    {
        Player[] players = FindObjectsOfType<Player>();
        if(players.Length < 2) return;

        Player player1;
        Player player2;
        if(players[0].playerNo == 1)
        {
            player1 = players[0];
            player2 = players[1];
        }
        else
        {
            player1 = players[1];
            player2 = players[0];
        }

        if(isBot)
        {
            //nothing with UI
        }
        else
        {
            if (BoltNetwork.IsServer)
            {
                FindObjectOfType<GameUI>().myPlayer = player1;

            }
            if (BoltNetwork.IsClient)
            {
                FindObjectOfType<GameUI>().myPlayer = player2;
                
            }
        }

        

    }

    public void SetBlockIndexToUnit(int unitIndex,int blockIndex)
    {
        state.unitsBlockIndices[unitIndex] = blockIndex;
    }

    public void SetUnitIndexToBlock(int unitIndex,int blockIndex)
    {
        state.blocksUnitIndices[blockIndex] = unitIndex;
    }

    public void SetKingClaimed(bool claimed)
    {
        state.kingClaimed = claimed;
    }

    public bool GetKingClaimed()
    {
        return state.kingClaimed;
    }

    public float GetTimeLeftWhite()
    {
        return state.timeLeftWhite;
    }

    public float GetTimeLeftBlack()
    {
        return state.timeLeftBlack;

    }


    void HandleTimes()
    {
        if(state.gameState == 0 || state.gameState == 4 || state.gameState == 5 || state.gameState == 6 || state.gameState == 8 || state.gameState == 9) return;

        if (state.whoseTurn == 1)
        {
            if ((int)state.timeLeftWhite > (int)(state.timeLeftWhite - Time.deltaTime) && state.timeLeftWhite < 5.9f) audioController.PlaySound("tickSound");
            state.timeLeftWhite -= Time.deltaTime;

        }
        else
        {
            if ((int)state.timeLeftBlack > (int)(state.timeLeftBlack - Time.deltaTime) && state.timeLeftBlack < 5.9f) audioController.PlaySound("tickSound");
            state.timeLeftBlack -= Time.deltaTime;
        }
        

    }


    void Replicate()
    {
        if(!other) return;


        if(other.state.actionNumber > state.actionNumber)
        {
            int i = 0;
            foreach(var index in other.state.unitsBlockIndices)
            {
                state.unitsBlockIndices[i] = index;
                i++;
            }


            
            i = 0;
            foreach(var index in other.state.blocksUnitIndices)
            {
                state.blocksUnitIndices[i] = index;
                i++;
            }

            if (state.gameState == 7 && other.state.gameState == 1)FindObjectOfType<GameUI>().InActivateBoardInfo();


            //to replicate dialogue
            if (state.gameState == 2 && other.state.gameState == 1) FindObjectOfType<GameUI>().TriggerEnemyDialoguePopUp("Pass");






            //--------



            if (state.gameState == 2 && other.state.gameState == 1) audioController.PlaySound("passSound");



            state.whoseTurn = other.state.whoseTurn;
            state.gameState = other.state.gameState;
            state.actionNumber = other.state.actionNumber;
            state.cachedUnitIndex = other.state.cachedUnitIndex;
            state.cachedBlockIndex = other.state.cachedBlockIndex;
            state.lastMoveWasBluff = other.state.lastMoveWasBluff;
            state.timeLeftBlack = other.state.timeLeftBlack;
            state.timeLeftWhite = other.state.timeLeftWhite;
            state.kingClaimed = other.state.kingClaimed;
            state.kingRevealed = other.state.kingRevealed;

            //to replicate sounds
            if (state.gameState == 3) audioController.PlaySound("wasNotBluffSound");
            else if (state.gameState == 6) audioController.PlaySound("wasBluffSound");
            else if (state.gameState == 2) audioController.PlaySound("moveSound");
            else if (state.gameState == 4) audioController.PlaySound("gameOverSound");
            else if (state.gameState == 8) audioController.PlaySound("roundOverSound");
            else if (state.gameState == 5) audioController.PlaySound("bluffClaimedSound");

            if (state.gameState == 2 && state.kingClaimed) audioController.PlaySound("claimKingSound");
            if (state.whoseTurn == FindObjectOfType<GameUI>().myPlayer.playerNo) audioController.PlaySound("turnOnYouSound");
            //--------------------


            //to replicate dialogue
            if (state.gameState == 5 && state.whoseTurn != FindObjectOfType<GameUI>().myPlayer.playerNo)
                                    FindObjectOfType<GameUI>().TriggerEnemyDialoguePopUp("Claim Bluff");

            if (state.gameState == 2 && state.whoseTurn == FindObjectOfType<GameUI>().myPlayer.playerNo && state.kingClaimed)
                FindObjectOfType<GameUI>().TriggerEnemyDialoguePopUp("Claim King");





            //--------




            OnStepFinish();
        }   




    }  


    public string GetWhiteNickName()
    {
        
        if (FindObjectOfType<GameUI>().myPlayer.playerNo == 1) return state.whiteNickName;
        else if (!other) return "";
        else return other.state.whiteNickName;

    }

    public string GetBlackNickName()
    {
        if (FindObjectOfType<GameUI>().myPlayer.playerNo == 2) return state.blackNickName;
        else if (!other) return "";
        else return other.state.blackNickName;

    }

    public int GetWhiteAvatarIndex()
    {
        if (FindObjectOfType<GameUI>().myPlayer.playerNo == 1) return state.whiteAvatarIndex;
        else if (!other) return -1;
        else return other.state.whiteAvatarIndex;
    }

    public int GetBlackAvatarIndex()
    {
        if (FindObjectOfType<GameUI>().myPlayer.playerNo == 2) return state.blackAvatarIndex;
        else if (!other) return -1;
        else return other.state.blackAvatarIndex;
    }

    public void AlternateTurn()
    {

        if(state.whoseTurn == 1)
        {
            state.whoseTurn = 2;
        }
        else if(state.whoseTurn == 2)
        {
            state.whoseTurn = 1;
        }




        if (state.whoseTurn == FindObjectOfType<GameUI>().myPlayer.playerNo) audioController.PlaySound("turnOnYouSound");


    }   

    public void SetGameState(int gameState)
    {
        //0 -> pre start
        //1 -> move
        //2 -> choose bluff or pass
        //3 -> sacrifice
        //4 -> game finished
        //5 -> checking bluff
        //6 -> true bluff claim
        //7 -> setup phase
        //8 -> round finished
        //9 -> enemy left
        if (gameState == 3) audioController.PlaySound("wasNotBluffSound");
        else if (gameState == 6) audioController.PlaySound("wasBluffSound");
        else if (gameState == 2)
        {
            if(state.kingClaimed) audioController.PlaySound("claimKingSound");
            audioController.PlaySound("moveSound");
        }
        else if (gameState == 4) audioController.PlaySound("gameOverSound");
        else if (gameState == 8) audioController.PlaySound("roundOverSound");


        if(state.gameState == 7 && gameState == 1) FindObjectOfType<GameUI>().InActivateBoardInfo();



        state.gameState = gameState;
    }


    public void IncrementActionNumber()
    {

        if (other.state.actionNumber > state.actionNumber) state.actionNumber = other.state.actionNumber;
        state.actionNumber++;

        OnStepFinish();

    }


    //when action number increase (works simultaneously in both server and client)
    void OnStepFinish()
    {
        gameController.MakeAllBlocksDefaultColor();
        gameController.DisableAllMoveObjectsOfUnits();
    }

    public int GetGameState()
    {
        return state.gameState;
    }

    public int GetWhoseTurn()
    {
        return state.whoseTurn;
    }

    public bool GetLastMoveWasBluff()
    {
        return state.lastMoveWasBluff;
    }
}
