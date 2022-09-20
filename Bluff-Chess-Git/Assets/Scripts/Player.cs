using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public int playerNo;

    public Block selectedBlock;


    GameController gameController;

    public bool claimKing = false;


    DataHandler dataHandler;


    bool startHideDone = false;


    public bool isBot = false;
    public bool botDecisionDone = false;
    [SerializeField] float botDecisionMinTime;
    [SerializeField] float botDecisionMaxTime;
    IEnumerator applyDecisionCoroutine;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        
    }


    void Update()
    {
        //works only for bot
        AIHandler();




        if(!dataHandler)
        {
            DataHandler[] dataHandlers = FindObjectsOfType<DataHandler>();
            foreach(DataHandler dataHandler in dataHandlers)
            {

                if (dataHandler.entity.IsOwner && !dataHandler.isBot)
                {
                    this.dataHandler = dataHandler;
                }

                
            }
        }

        if (BoltNetwork.IsClient && playerNo == 1) return;
        if (BoltNetwork.IsServer && playerNo == 2) return;


        if (!startHideDone)
        {
            if(playerNo == 2)
            {
                for(int i=0;i<8;i++)
                {
                    if(gameController.allUnits[i].unitType != "Pawn")gameController.allUnits[i].GetHide();
                }
            }
            else
            {
                for(int i=8;i<16;i++)
                {
                    if(gameController.allUnits[i].unitType != "Pawn")gameController.allUnits[i].GetHide();
                }
            }

            startHideDone = true;
        }


        if(!gameController) return;
        if(!dataHandler) return;

        


        if(dataHandler.GetGameState() == 7 && dataHandler.GetWhoseTurn() == playerNo) HandleSetupPhase();
        if(dataHandler.GetGameState() == 1 && dataHandler.GetWhoseTurn() == playerNo) HandleSelection();
        if(dataHandler.GetGameState() == 3 && dataHandler.GetWhoseTurn() == playerNo) SacrificeUnit();
    }

    void AIHandler()
    {
        if(isBot)
        {
            if(playerNo == dataHandler.GetWhoseTurn())
            {
                if (botDecisionDone) return;


                float delay = Random.Range(botDecisionMinTime, botDecisionMaxTime);

                if(dataHandler.GetGameState() == 0)
                {
                    //cannot be
                }
                else if (dataHandler.GetGameState() == 1)
                {
                    applyDecisionCoroutine = ApplyDecisionAfter(delay, "Move");
                    StartCoroutine(applyDecisionCoroutine);
                    botDecisionDone = true;
                }
                else if (dataHandler.GetGameState() == 2)
                {
                    float rand = Random.Range(0.0f, 1.0f);

                    //if king is eaten, directly claim bluff
                    if (dataHandler.GetCachedUnitType() == "King") rand = 1;
                    if (GetAliveUnits().Count < 4) rand = 1;

                    if(dataHandler.GetLastMoveWasBluff())
                    {
                        //torpil
                        rand += 0.3f;
                    }
                    else
                    {
                        rand -= 0.3f;
                    }

                    if(rand < 0.5f)
                    {
                        applyDecisionCoroutine = ApplyDecisionAfter(delay, "Pass");
                        StartCoroutine(applyDecisionCoroutine);
                    }
                    else
                    {
                        applyDecisionCoroutine = ApplyDecisionAfter(delay, "Claim Bluff");
                        StartCoroutine(applyDecisionCoroutine);
                    }
                    botDecisionDone = true;
                }
                else if (dataHandler.GetGameState() == 3)
                {
                    applyDecisionCoroutine = ApplyDecisionAfter(delay, "Sacrifice");
                    StartCoroutine(applyDecisionCoroutine);
                    botDecisionDone = true;
                }
                else if (dataHandler.GetGameState() == 4)
                {

                }
                else if (dataHandler.GetGameState() == 5)
                {

                }
                else if (dataHandler.GetGameState() == 6)
                {

                }
                else if (dataHandler.GetGameState() == 7)
                {
                    applyDecisionCoroutine = ApplyDecisionAfter(delay, "Randomize units and get ready");
                    StartCoroutine(applyDecisionCoroutine);
                    botDecisionDone = true;
                }
                else if (dataHandler.GetGameState() == 8)
                {

                }
                else if (dataHandler.GetGameState() == 9)
                {

                }




            }
        }
    }

    List<int> GetAliveUnits()
    {
        List<int> result = new List<int>();

        if(playerNo == 1)
        {
            for(int i=0;i<8;i++)
            {
                if (dataHandler.GetUnitsBlockIndex(i) > -1) result.Add(i);
            }
        }
        else
        {
            for (int i = 8; i < 16; i++)
            {
                if (dataHandler.GetUnitsBlockIndex(i) > -1) result.Add(i);
            }
        }
        return result;
    }

    List<int> GetNonPawnAliveUnits()
    {
        List<int> result = new List<int>();

        if (playerNo == 1)
        {
            for (int i = 0; i < 8; i++)
            {
                if (dataHandler.GetUnitsBlockIndex(i) > -1 && gameController.allUnits[i].unitType != "Pawn") result.Add(i);
            }
        }
        else
        {
            for (int i = 8; i < 16; i++)
            {
                if (dataHandler.GetUnitsBlockIndex(i) > -1 && gameController.allUnits[i].unitType != "Pawn") result.Add(i);
            }
        }
        return result;
    }

    private IEnumerator ApplyDecisionAfter(float delay,string decision)
    {
       
        yield return new WaitForSeconds(delay);

        if (dataHandler.GetGameState() == 4 || dataHandler.GetGameState() == 8)
        {
            botDecisionDone = false;
        }
        else
        {
            if (decision == "Randomize units and get ready")
            {
                RandomizeUnits();
                GetReady();
            }
            else if (decision == "Pass")
            {
                gameController.Pass();
                FindObjectOfType<GameUI>().TriggerEnemyDialoguePopUp("Pass");
            }
            else if (decision == "Claim Bluff")
            {
                gameController.BluffClaimed();
                FindObjectOfType<GameUI>().TriggerEnemyDialoguePopUp("Claim Bluff");
            }
            else if (decision == "Sacrifice")
            {
                List<int> aliveBotUnits = new List<int>();
                for (int i = 8; i < 16; i++)
                {
                    if (dataHandler.GetUnitsBlockIndex(i) > -1 && gameController.allUnits[i].unitType != "King")
                    {
                        aliveBotUnits.Add(i);
                    }
                }

                int rand = Random.Range(0, aliveBotUnits.Count);
                int randUnitIndex = aliveBotUnits[rand];
                gameController.UnitSacrificed(randUnitIndex);
            }
            else if (decision == "Move")
            {
                List<int> nonPawnAliveUnits = GetNonPawnAliveUnits();

                while (true)
                {
                    int rand = Random.Range(0, nonPawnAliveUnits.Count);
                    float randMove = Random.Range(0.0f, 1.0f);

                    int unit1 = GetAUnitHasLegalEnemyMove();
                    int unit2 = GetAUnitHasIllegalEnemyMove();
                    int randUnit = nonPawnAliveUnits[rand];


                    
                    if (gameController.allUnits[randUnit].unitType == "King")
                    {
                        if (nonPawnAliveUnits.Count > 1)
                        {
                            while(gameController.allUnits[randUnit].unitType != "King")
                            {
                                rand = Random.Range(0, nonPawnAliveUnits.Count);
                                randUnit = nonPawnAliveUnits[rand];
                            }
                            
                        }
                    }

                    if (unit1 != -1 && randMove < 0.9f)
                    {
                        Block source = gameController.allBlocks[dataHandler.GetUnitsBlockIndex(unit1)];
                        int rand2 = Random.Range(0, source.GetLegalEnemyBlocks().Count);
                        Block target = source.GetLegalEnemyBlocks()[rand2];
                        gameController.ConquerBlock(source, target, false);
                        break;
                    }
                    else if (randMove < 0.9f)
                    {
                        Block source = gameController.allBlocks[dataHandler.GetUnitsBlockIndex(randUnit)];
                        int rand2 = Random.Range(0, source.GetLegalMoves().Count);
                        if (source.GetLegalMoves().Count > 0)
                        {
                            Block target = source.GetLegalMoves()[rand2];
                            gameController.ConquerBlock(source, target, false);
                            break;
                        }

                    }

                    else if (unit2 != -1)
                    {
                        Block source = gameController.allBlocks[dataHandler.GetUnitsBlockIndex(unit2)];
                        int rand2 = Random.Range(0, source.GetIllegalEnemyMoves().Count);
                        Block target = source.GetIllegalEnemyMoves()[rand2];
                        gameController.ConquerBlock(source, target, true);
                        break;

                    }
                    else
                    {
                        Block source = gameController.allBlocks[dataHandler.GetUnitsBlockIndex(randUnit)];
                        int rand2 = Random.Range(0, source.GetIllegalMoves().Count);
                        Block target = source.GetIllegalMoves()[rand2];
                        gameController.ConquerBlock(source, target, true);
                        break;
                    }



                }


            }
        }



        
    }


    int GetAUnitHasLegalEnemyMove()
    {

        List<int> all = new List<int>();

        List<int> nonPawnAliveUnits = GetNonPawnAliveUnits();


        foreach(int i in nonPawnAliveUnits)
        {
            if (gameController.allBlocks[dataHandler.GetUnitsBlockIndex(i)].GetLegalEnemyBlocks().Count > 0) all.Add(i);
        }

        if (all.Count < 1) return -1;
        else
        {
            int rand = Random.Range(0, all.Count);
            return all[rand];
        }

    }


    int GetAUnitHasIllegalEnemyMove()
    {

        List<int> all = new List<int>();

        List<int> nonPawnAliveUnits = GetNonPawnAliveUnits();


        foreach (int i in nonPawnAliveUnits)
        {
            if (gameController.allBlocks[dataHandler.GetUnitsBlockIndex(i)].GetIllegalEnemyMoves().Count > 0) all.Add(i);
        }

        if (all.Count < 1) return -1;
        else
        {
            int rand = Random.Range(0, all.Count);
            return all[rand];
        }

    }

    //Not the king
    void MakeBlockOfUnitsRed()
    {
        if(playerNo == 1)
        {
            for(int i=0;i<8;i++)
            {
                if(gameController.allUnits[i] && dataHandler.GetUnitsBlockIndex(i) > -1 && i!=1)gameController.allBlocks[dataHandler.GetUnitsBlockIndex(i)].GetRed();
            }
        }
        else
        {
            for(int i=8;i<16;i++)
            {
                if(gameController.allUnits[i] && dataHandler.GetUnitsBlockIndex(i) > -1 && i!=9)gameController.allBlocks[dataHandler.GetUnitsBlockIndex(i)].GetRed();
            }
        }
    }


    void SacrificeUnit()
    {
        MakeBlockOfUnitsRed();

        
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Block targetBlock = GetBlockUnderCursor();
            if(targetBlock && dataHandler.GetBlocksUnitIndex(targetBlock.myIndex) != -1 && gameController.allUnits[dataHandler.GetBlocksUnitIndex(targetBlock.myIndex)].teamNo == playerNo
                                                                                        && gameController.allUnits[dataHandler.GetBlocksUnitIndex(targetBlock.myIndex)].unitType != "King")
            {
                gameController.UnitSacrificed(dataHandler.GetBlocksUnitIndex(targetBlock.myIndex));
            }
            
        }

    }

    void HandleSetupPhase()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(selectedBlock)
            {
                selectedBlock.LowlightHorizontal();



                Block targetBlock = GetBlockUnderCursor();

                if(targetBlock && selectedBlock.GetHorizontalBlocks().Contains(targetBlock) &&
                        (dataHandler.GetBlocksUnitIndex(targetBlock.myIndex) == -1 || 
                        gameController.allUnits[dataHandler.GetBlocksUnitIndex(targetBlock.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(selectedBlock.myIndex)].teamNo))
                {
                    gameController.SetupUnit(selectedBlock,targetBlock);
                }


                selectedBlock = null;

            }
            else
            {
                selectedBlock = GetBlockUnderCursor();
                if(selectedBlock)
                {
                    
                    if(dataHandler.GetBlocksUnitIndex(selectedBlock.myIndex) != -1)
                    {

                        if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(selectedBlock.myIndex)].teamNo == playerNo)
                        {
                            selectedBlock.HighlightHorizontal();
                        }
                        else
                        {
                            selectedBlock = null;
                        }
                    }
                    
                    else
                    {
                        selectedBlock = null;
                    }
                }
            }
            

        }



    }

    bool ClickedOusiteBoard()
    {   
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Mathf.Abs(mousePos.y) > 3;
    }



    public void HightlightKingSelected()
    {
        selectedBlock.Lowlight();
        selectedBlock.HighlightKing();
    }

    public void HighlightSelected()
    {
        selectedBlock.Lowlight();
        selectedBlock.Highlight();
    }

    void HandleSelection()
    {

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {   
            if(ClickedOusiteBoard())return;

            if(selectedBlock)
            {
                selectedBlock.Lowlight();

                Block targetBlock = GetBlockUnderCursor();
                
                if(claimKing)
                {
                    if(targetBlock && selectedBlock.GetKingMovableBlocks().Contains(targetBlock) && (dataHandler.GetBlocksUnitIndex(targetBlock.myIndex) == -1 ||
                                                              gameController.allUnits[dataHandler.GetBlocksUnitIndex(targetBlock.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(selectedBlock.myIndex)].teamNo))
                    {
                        if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(selectedBlock.myIndex)].unitType == "King")gameController.ConquerBlock(selectedBlock,targetBlock,false);
                        else gameController.ConquerBlock(selectedBlock,targetBlock,true);
                    }
                }
                else
                {
                    if(targetBlock && selectedBlock.GetMovableBlocks().Contains(targetBlock) && (dataHandler.GetBlocksUnitIndex(targetBlock.myIndex) == -1 || 
                                                            gameController.allUnits[dataHandler.GetBlocksUnitIndex(targetBlock.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(selectedBlock.myIndex)].teamNo))
                    {
                        if(selectedBlock.GetNonBluffBlocks().Contains(targetBlock))gameController.ConquerBlock(selectedBlock,targetBlock,false);
                        else gameController.ConquerBlock(selectedBlock,targetBlock,true);
                    }
                }

                selectedBlock = null;
                claimKing = false;

            }
            else
            {
                selectedBlock = GetBlockUnderCursor();
                if(selectedBlock)
                {
                    
                    if(dataHandler.GetBlocksUnitIndex(selectedBlock.myIndex) != -1)
                    {

                        if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(selectedBlock.myIndex)].teamNo == playerNo && gameController.allUnits[dataHandler.GetBlocksUnitIndex(selectedBlock.myIndex)].unitType != "Pawn")
                        {
                            if(claimKing)selectedBlock.HighlightKing();
                            else selectedBlock.Highlight();
                        }
                        else
                        {
                            selectedBlock = null;
                        }
                    }
                    
                    else
                    {
                        selectedBlock = null;
                    }
                }
            }
            

        }
    }

    Block GetBlockUnderCursor()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject);
            return hit.collider.GetComponent<Block>();
        }
        return null;
    }



    public void RandomizeUnits()
    {

        if(playerNo == 1)
        {
            for(int i=0;i<8;i++)
            {
                dataHandler.SetBlockIndexToUnit(i,-1);
                
            }
            for(int i=0;i<12;i++)
            {
                dataHandler.SetUnitIndexToBlock(-1,i);
            }

            for(int i=0;i<5;i++)
            {
                while(dataHandler.GetUnitsBlockIndex(i) == -1)
                {
                    int rand = Random.Range(0,6);
                    if(dataHandler.GetBlocksUnitIndex(rand) != -1) continue;
                    else
                    {
                        dataHandler.SetBlockIndexToUnit(i,rand);
                        dataHandler.SetUnitIndexToBlock(i,rand);
                    }
                }
            }

            for(int i=5;i<8;i++)
            {
                while(dataHandler.GetUnitsBlockIndex(i) == -1)
                {
                    int rand = Random.Range(6,12);
                    if(dataHandler.GetBlocksUnitIndex(rand) != -1) continue;
                    else
                    {
                        dataHandler.SetBlockIndexToUnit(i,rand);
                        dataHandler.SetUnitIndexToBlock(i,rand);
                    }
                }
            }


        }
        else
        {
            for(int i=8;i<16;i++)
            {
                dataHandler.SetBlockIndexToUnit(i,-1);
            }
            for(int i=24;i<36;i++)
            {
                dataHandler.SetUnitIndexToBlock(-1,i);
            }

            for(int i=8;i<13;i++)
            {
                while(dataHandler.GetUnitsBlockIndex(i) == -1)
                {
                    int rand = Random.Range(30,36);
                    if(dataHandler.GetBlocksUnitIndex(rand) != -1) continue;
                    else
                    {
                        dataHandler.SetBlockIndexToUnit(i,rand);
                        dataHandler.SetUnitIndexToBlock(i,rand);
                    }
                }
            }

            for(int i=13;i<16;i++)
            {
                while(dataHandler.GetUnitsBlockIndex(i) == -1)
                {
                    int rand = Random.Range(24,30);
                    if(dataHandler.GetBlocksUnitIndex(rand) != -1) continue;
                    else
                    {
                        dataHandler.SetBlockIndexToUnit(i,rand);
                        dataHandler.SetUnitIndexToBlock(i,rand);
                    }
                }
            }


        }


    }

    public void GetReady()
    {
        dataHandler.AlternateTurn();

        if((gameController.roundNo % 2 == 1 && playerNo == 1) || (gameController.roundNo % 2 == 0 && playerNo == 2))
        {
            dataHandler.SetGameState(1);
            dataHandler.state.timeLeftWhite = 90;
            dataHandler.state.timeLeftBlack = 90;
        }

        dataHandler.IncrementActionNumber();

        botDecisionDone = false;

    }

}
