using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using UnityEngine.SceneManagement;
using UdpKit;

public class GameController : GlobalEventListener
{

    [SerializeField] public Block[] allBlocks;
    [SerializeField] public Unit[] allUnits;


    DataHandler dataHandler;

    IEnumerator gameOverCoroutine;


    IEnumerator roundOverCoroutine;
    public bool roundOverCoroutineStarted = false;

    IEnumerator bluffClaimedCoroutine;

    IEnumerator trueBluffClaimCoroutine;

    IEnumerator leaveGameCoroutine;


    //hardcoded king indices
    int whiteKingIndex = 1;
    int blackKingIndex = 9;


    public int whitePlayerScore = 0;
    public int blackPlayerScore = 0;


    public int winner = -1;

    public int roundNo = 0;

    public string roundOverCause;

    public int roundWinner = -1;

    AudioController audioController;
    void Start()
    {
        audioController = FindObjectOfType<AudioController>();

    }

    void Update()
    {
        if(!dataHandler)
        {
            DataHandler[] dataHandlers = FindObjectsOfType<DataHandler>();
            foreach(DataHandler dataHandler in dataHandlers)
            {
                if(dataHandler.entity.IsOwner)this.dataHandler = dataHandler;
            }
        }


        CheckSetups();
        CheckRoundFinished();

    }

    public override void BoltShutdownBegin(AddCallback registerDoneCallback, UdpConnectionDisconnectReason disconnectReason)
    {
        dataHandler.SetGameState(9);
        FindObjectOfType<GameUI>().gameInfoTMP.text = "Enemy left.\nLeaving Game...";
        LeaveGameAfter(2);
    }

    public void LeaveGame()
    {
        BoltLauncher.Shutdown();
        SceneManager.LoadScene("Menu");
    }


    private IEnumerator LeaveGameAfterC(float after)
    {
        BoltLauncher.Shutdown();
        yield return new WaitForSeconds(after);
        LeaveGame();
    }

    public void LeaveGameAfter(float seconds)
    {
        leaveGameCoroutine = LeaveGameAfterC(3f);
        StartCoroutine(leaveGameCoroutine);
    }


    public int GetBlockIndexAtLocation(Vector3 pos)
    {
        int result = -1;


        for(int i=0;i<allBlocks.Length;i++)
        {
            if((allBlocks[i].transform.position-pos).magnitude < 0.1f)
            {
                result = i;
                break;
            }
        }

        return result;
    }

    void CheckSetups()
    {
        if(!dataHandler) return;

        if(dataHandler.GetGameState() != 7) return;


        if(roundNo % 2 == 0)
        {
            if(dataHandler.GetTimeLeftWhite() < 0 && dataHandler.GetWhoseTurn() == 1)
            {
                if(FindObjectOfType<GameUI>().myPlayer.playerNo == dataHandler.GetWhoseTurn())
                {
                    dataHandler.IncrementActionNumber();
                    dataHandler.AlternateTurn();
                }
                
            }

            if(dataHandler.GetTimeLeftBlack() < 0)
            {
                if(FindObjectOfType<GameUI>().myPlayer.playerNo == dataHandler.GetWhoseTurn())
                {
                    dataHandler.SetGameState(1);
                    dataHandler.AlternateTurn();
                    
                    dataHandler.state.timeLeftWhite = 90;
                    dataHandler.state.timeLeftBlack = 90;
                    dataHandler.IncrementActionNumber();
                }
                
            }
        }
        else
        {
            if(dataHandler.GetTimeLeftBlack() < 0 && dataHandler.GetWhoseTurn() == 2)
            {
                if (FindObjectOfType<GameUI>().myPlayer.playerNo == dataHandler.GetWhoseTurn())
                {
                    dataHandler.IncrementActionNumber();
                    dataHandler.AlternateTurn();
                }
            }

            if(dataHandler.GetTimeLeftWhite() < 0)
            {

                if (FindObjectOfType<GameUI>().myPlayer.playerNo == dataHandler.GetWhoseTurn())
                {
                    dataHandler.SetGameState(1);
                    dataHandler.AlternateTurn();
                    
                    dataHandler.state.timeLeftWhite = 90;
                    dataHandler.state.timeLeftBlack = 90;
                    dataHandler.IncrementActionNumber();
                }
            }
        }

        

    }


    void CheckRoundFinished()
    {
        if(!dataHandler) return;
        //if(dataHandler.GetGameState() != 1 && dataHandler.GetGameState() != 8 && !dataHandler.state.kingRevealed)return;



        if (dataHandler.GetGameState() == 0 || dataHandler.GetGameState() == 7) return;


        int whiteDeathCount = 0;
        int blackDeathCount = 0;
        bool whiteKingDead = false;
        bool blackKingDead = false;
        bool whiteTimeOut = false;
        bool blackTimeOut = false;

        for(int i=0;i<8;i++)
        {
            if(dataHandler.GetUnitsBlockIndex(i) == -2 || dataHandler.GetUnitsBlockIndex(i) == -3)whiteDeathCount ++;
        }

        for(int i=8;i<16;i++)
        {
            if(dataHandler.GetUnitsBlockIndex(i) == -2 || dataHandler.GetUnitsBlockIndex(i) == -3)blackDeathCount ++;
        }  


        if(dataHandler.GetUnitsBlockIndex(whiteKingIndex) == -2) whiteKingDead = true;
        if(dataHandler.GetUnitsBlockIndex(blackKingIndex) == -2) blackKingDead = true;


        if(dataHandler.GetTimeLeftWhite() <= 0) whiteTimeOut = true;
        if(dataHandler.GetTimeLeftBlack() <= 0) blackTimeOut = true;


        if(whiteDeathCount >= 5 || blackDeathCount >= 5 || blackKingDead || whiteKingDead ||whiteTimeOut || blackTimeOut || dataHandler.state.kingRevealed)
        {

            if(!roundOverCoroutineStarted)
            {

                if (whiteDeathCount >= 5 && blackDeathCount >= 5)
                {
                    roundOverCause = "Both side have less than 4 pieces.";


                }
                else if (blackKingDead || blackDeathCount >= 5 || blackTimeOut || (dataHandler.state.kingRevealed && dataHandler.GetWhoseTurn() == 2))
                {
                    if (blackKingDead) roundOverCause = "Black king died.";
                    else if (blackDeathCount >= 5) roundOverCause = "Black has less than 4 pieces";
                    else if (blackTimeOut) roundOverCause = "Black timed out.";
                    else if (dataHandler.state.kingRevealed && dataHandler.GetWhoseTurn() == 2) roundOverCause = "White king revealed";
                    whitePlayerScore++;
                    FindObjectOfType<GameUI>().CarryGainingScore(1,whitePlayerScore);
                    roundWinner = 1;
                }
                else
                {
                    if (whiteKingDead) roundOverCause = "White king died.";
                    else if (whiteDeathCount >= 5) roundOverCause = "White has less than 4 pieces";
                    else if (whiteTimeOut) roundOverCause = "White timed out.";
                    else if (dataHandler.state.kingRevealed && dataHandler.GetWhoseTurn() == 1) roundOverCause = "Black king revealed";

                    blackPlayerScore++;
                    FindObjectOfType<GameUI>().CarryGainingScore(2,blackPlayerScore);
                    roundWinner = 2;
                }

                roundOverCoroutine = RoundOver(3.0f);
                StartCoroutine(roundOverCoroutine);
                roundOverCoroutineStarted = true;
            }

            
        }

    }

    private IEnumerator RoundOver(float roundOverDelay)
    {
        dataHandler.SetGameState(8);
        roundNo ++;


        //Unhide units
        foreach(Unit unit in allUnits)
        {
            if(unit)
            {
                if(unit.unitType != "Pawn")
                unit.UnHideForSeconds(roundOverDelay);
            }
        }

        //get default color block
        foreach(Block block in allBlocks)
        {
            block.GetDefaultColor();
        }

        FindObjectOfType<GameUI>().ActivateBoardInfo("ROUND OVER",roundOverDelay/2);

        


        yield return new WaitForSeconds(roundOverDelay);

        
        FindObjectOfType<GameUI>().ActivateBoardInfo("SETUP PHASE",-1);

        if(whitePlayerScore > 1)
        {
            winner = 1;
            gameOverCoroutine = GameOver(3f);
            StartCoroutine(gameOverCoroutine);
        }
        else if(blackPlayerScore > 1)
        {
            winner = 2;
            gameOverCoroutine = GameOver(3f);
            StartCoroutine(gameOverCoroutine);
        }
        else
        {
            dataHandler.ResetRound(roundNo);
        }

       

    }

    private IEnumerator GameOver(float gameOverDelay)
    {
        dataHandler.SetGameState(4);
        dataHandler.IncrementActionNumber();

        FindObjectOfType<GameUI>().ActivateBoardInfo("GAME OVER",gameOverDelay);

        yield return new WaitForSeconds(gameOverDelay);


        LeaveGame();
       
    }

    


    public void StartSetupPhase()
    {

        dataHandler.SetGameState(7);

    }



    public void ConquerBlock(Block source,Block target,bool bluff)
    {
        if (dataHandler.GetGameState() != 1)
        {
            ResetBotDecisionDone();
            return;
        }


        dataHandler.SetLastMoveBluff(bluff);
        dataHandler.MakeUnitCached(dataHandler.GetBlocksUnitIndex(target.myIndex));
        dataHandler.MakeBlockCached(target.myIndex);
        dataHandler.SetKingClaimed(FindObjectOfType<GameUI>().myPlayer.claimKing);

        allUnits[dataHandler.GetBlocksUnitIndex(source.myIndex)].MoveToBlockIndex(target.myIndex);
        dataHandler.SetGameState(2);
        dataHandler.AlternateTurn();

        dataHandler.IncrementActionNumber();
        ResetBotDecisionDone();
    }

    public void SetupUnit(Block source,Block target)
    {
        audioController.PlaySound("moveSound");
        allUnits[dataHandler.GetBlocksUnitIndex(source.myIndex)].MoveToBlockIndex(target.myIndex);
        //dataHandler.IncrementActionNumber();
    }



    public void BluffClaimed()
    {
        audioController.PlaySound("bluffClaimedSound");

        bluffClaimedCoroutine = BluffClaimedC(2);
        StartCoroutine(bluffClaimedCoroutine);

    }


    IEnumerator BluffClaimedC(float doAfter)
    {
        allUnits[dataHandler.GetBlocksUnitIndex(dataHandler.state.cachedBlockIndex)].UnHideForSeconds(1.8f);
        dataHandler.SetGameState(5);

        dataHandler.IncrementActionNumber();

        yield return new WaitForSeconds(doAfter);


        if(dataHandler.GetLastMoveWasBluff())
        {
            trueBluffClaimCoroutine = TrueBluffClaimed(2);
            StartCoroutine(trueBluffClaimCoroutine);  
        } 
        else 
        {
            dataHandler.MakeCacheDied();
            if(dataHandler.GetUnitTypeInCachedBlock() == "King")
            {
                dataHandler.state.kingRevealed = true;
            }
            else
            {
                dataHandler.SetGameState(3);
            }

            dataHandler.IncrementActionNumber();


            //for bot
            ResetBotDecisionDone();
        }
    }


    void ResetBotDecisionDone()
    {
        foreach (Player player in FindObjectsOfType<Player>())
        {
            player.botDecisionDone = false;
        }
    }
    

    void MakeBlockOfUnitsDefault(int playerNo)
    {
        if(playerNo == 1)
        {
            for(int i=0;i<8;i++)
            {
                if(allUnits[i] && dataHandler.GetUnitsBlockIndex(i) > -1)allBlocks[dataHandler.GetUnitsBlockIndex(i)].GetDefaultColor();
            }
        }
        else
        {
            for(int i=8;i<16;i++)
            {
                if(allUnits[i] && dataHandler.GetUnitsBlockIndex(i) > -1)allBlocks[dataHandler.GetUnitsBlockIndex(i)].GetDefaultColor();
            }
        }

    }


    public void MakeAllBlocksDefaultColor()
    {
        for(int i=0;i<36;i++)
        {
            allBlocks[i].GetDefaultColor();
        }
    }

    public void DisableAllMoveObjectsOfUnits()
    {
        foreach(Unit unit in allUnits)
        {
            if(unit)unit.moveImageObject.SetActive(false);
        }
    }

    IEnumerator TrueBluffClaimed(float doAfter)
    {
        dataHandler.SetGameState(6);
        dataHandler.IncrementActionNumber();

        yield return new WaitForSeconds(doAfter);

        dataHandler.MakeCacheReturn();
        dataHandler.SetGameState(1);
        dataHandler.IncrementActionNumber();


        ResetBotDecisionDone();
    }

    public void Pass()
    {
        dataHandler.SetGameState(1);
        
        dataHandler.MakeCacheDied();

        dataHandler.IncrementActionNumber();


        ResetBotDecisionDone();

        audioController.PlaySound("passSound");
    }


    public void UnitSacrificed(int unitIndex)
    {
        if(dataHandler.GetGameState() == 8) return;


        MakeBlockOfUnitsDefault(dataHandler.GetWhoseTurn());
        dataHandler.MakeUnitDied(unitIndex,true);
        dataHandler.SetGameState(1);
        dataHandler.IncrementActionNumber();


        ResetBotDecisionDone();
    }

}
