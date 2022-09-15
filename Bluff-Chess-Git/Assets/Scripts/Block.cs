using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class Block : MonoBehaviour
{
    GameController gameController;
    int myRow;
    int myColumn;
    public int myIndex;


    Block[] allBlocks;
    List<Block> horizontalBlocks;
    List<Block> verticalBlocks;
    List<Block> diagonalBlocks;
    List<Block> LBlocks;


    List<Block> rightHorizontalBlocks;
    List<Block> leftHorizontalBlocks;
    List<Block> upVerticalBlocks;
    List<Block> downVerticalBlocks;
    List<Block> topRightDiagonalBlocks;
    List<Block> topLeftDiagonalBlocks;
    List<Block> downRightDiagonalBlocks;
    List<Block> downLeftDiagonalBlocks;


    [SerializeField] public GameObject foreGround;
    [SerializeField] public GameObject foreForeGround;

    Player[] players;
    [SerializeField] Sprite foreGroundHighlightSprite;
    [SerializeField] Sprite foreGroundKillHighlightSprite;
    [SerializeField] Sprite foreGroundSelectedHighlightSprite;
    [SerializeField] Sprite foreGroundMoveHighlightSprite;


    DataHandler dataHandler;


 
    // Start is called before the first frame update
    void Start()
    {   
        

        gameController = FindObjectOfType<GameController>();
        allBlocks = gameController.allBlocks;

        horizontalBlocks = new List<Block>();
        verticalBlocks = new List<Block>();
        diagonalBlocks = new List<Block>();
        LBlocks = new List<Block>();

        rightHorizontalBlocks = new List<Block>();
        leftHorizontalBlocks = new List<Block>();
        upVerticalBlocks = new List<Block>();
        downVerticalBlocks = new List<Block>();

        topRightDiagonalBlocks = new List<Block>();
        topLeftDiagonalBlocks = new List<Block>();
        downRightDiagonalBlocks = new List<Block>();
        downLeftDiagonalBlocks = new List<Block>();


        SetMyIndexMyRowAndMyColumn();
        SetHorizontalBlocks();
        SetVerticalBlocks();
        SetDiagonalBlocks();
        SetLBlocks();


        players = FindObjectsOfType<Player>();

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
            return;
        }
    }

    public List<Block> GetHorizontalBlocks()
    {
        return horizontalBlocks;
    }

    public List<Block> GetVerticalBlocks()
    {
        return verticalBlocks;
    }

    public List<Block> GetDiagonalBlocks()
    {
        return diagonalBlocks;
    }

    public List<Block> GetLBlocks()
    {
        return LBlocks;
    }
    public string WhatTypeCanMoveToBlock(Block target)
    {
        string result = "";

        if(horizontalBlocks.Contains(target) || verticalBlocks.Contains(target)) result = "Rook";
        else if(diagonalBlocks.Contains(target)) result = "Bishop";
        else if(LBlocks.Contains(target)) result = "Knight";

        return result;
    }

    public List<Block> GetNonBluffBlocks()
    {
        List<Block> result = new List<Block>();

        if(dataHandler.GetBlocksUnitIndex(myIndex) == -1) return result;

        

        if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "Knight")
        {
            foreach(Block block in LBlocks)
            {
                result.Add(block);
            }
        }
        else if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "Rook")
        {
            foreach(Block block in horizontalBlocks)
            {
                result.Add(block);
            }
            foreach(Block block in verticalBlocks)
            {
                result.Add(block);
            }
        }
        else if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "Queen")
        {
            
        }
        else if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "King")
        {
            
        }
        else if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "Bishop")
        {
            foreach(Block block in diagonalBlocks)
            {
                result.Add(block);
            }
        }

        

        return result;
    }

    public void GetRed()
    {
        foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundKillHighlightSprite;
    }

    public void GetDefaultColor()
    {
        foreGround.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void GetYellow()
    {
        foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundMoveHighlightSprite;
    }


    public List<Block> GetMovableBlocks()
    {
        List<Block> result = new List<Block>();

        foreach(Block block in leftHorizontalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1)
            {
                if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo)result.Add(block);
                break;
            }
            result.Add(block);
            
        }


        foreach(Block block in rightHorizontalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1)
            {
                if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo)result.Add(block);
                break;
            }
            result.Add(block);
        }


        foreach(Block block in upVerticalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1)
            {
                if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo)result.Add(block);
                break;
            }
            result.Add(block);
        }
        foreach(Block block in downVerticalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1)
            {
                if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo)result.Add(block);
                break;
            }
            result.Add(block);

        }


        foreach(Block block in topLeftDiagonalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1)
            {
                if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo)result.Add(block);
                break;
            }
            result.Add(block);
        }
        foreach(Block block in topRightDiagonalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1)
            {
                if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo)result.Add(block);
                break;
            }
            result.Add(block);
        }
        foreach(Block block in downLeftDiagonalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1)
            {
                if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo)result.Add(block);
                break;
            }
            result.Add(block);
        }
        foreach(Block block in downRightDiagonalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1)
            {
                if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo)result.Add(block);
                break;
            }
            result.Add(block);
        }
        

        foreach(Block block in LBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1 && gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo == gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo)continue;
            result.Add(block);
        }


        return result;
    }


    public List<Block> GetKingMovableBlocks()
    {
        List<Block> temp = new List<Block>();
        if(rightHorizontalBlocks.Count > 0)temp.Add(rightHorizontalBlocks[0]);
        if(leftHorizontalBlocks.Count > 0)temp.Add(leftHorizontalBlocks[0]);
        if(upVerticalBlocks.Count > 0)temp.Add(upVerticalBlocks[0]);
        if(downVerticalBlocks.Count > 0)temp.Add(downVerticalBlocks[0]);
        if(topRightDiagonalBlocks.Count > 0)temp.Add(topRightDiagonalBlocks[0]);
        if(topLeftDiagonalBlocks.Count > 0)temp.Add(topLeftDiagonalBlocks[0]);
        if(downLeftDiagonalBlocks.Count > 0)temp.Add(downLeftDiagonalBlocks[0]);
        if(downRightDiagonalBlocks.Count > 0)temp.Add(downRightDiagonalBlocks[0]);

        List<Block> result = new List<Block>();

        foreach(Block block in temp)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1 && gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo == gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo) continue;

            result.Add(block);
        }




        return result;
    }

    public void HighlightKing()
    {
        foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundSelectedHighlightSprite;

        List<Block> kingMovableBlocks = GetKingMovableBlocks();

        foreach(Block block in kingMovableBlocks)
        {
            block.foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundHighlightSprite;

            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1 && gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo) block.foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundKillHighlightSprite;

            if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "King")
            {
                block.foreForeGround.SetActive(true);
            }

        }

        

    }

    public List<Block> GetLegalMoves()
    {
        List<Block> result = new List<Block>();
        Unit myUnit = gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)];
        if (myUnit.unitType == "King")
        {
        }
        else if (myUnit.unitType == "Queen")
        {
        }
        else if (myUnit.unitType == "Knight")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (GetLBlocks().Contains(block))
                {
                    result.Add(block);
                }
            }
        }
        else if (myUnit.unitType == "Rook")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (
                    (GetVerticalBlocks().Contains(block) || GetHorizontalBlocks().Contains(block)))
                {
                    result.Add(block);
                }
            }
        }
        else if (myUnit.unitType == "Bishop")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (
                    GetDiagonalBlocks().Contains(block))
                {
                    result.Add(block);
                }
            }
        }

        return result;
    }

    public List<Block> GetIllegalMoves()
    {
        List<Block> result = new List<Block>();
        Unit myUnit = gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)];

        if (myUnit.unitType == "King")
        {
            result = GetMovableBlocks();
        }
        else if (myUnit.unitType == "Queen")
        {
            result = GetMovableBlocks();
        }
        else if (myUnit.unitType == "Rook")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (
                    (!GetVerticalBlocks().Contains(block) && !GetHorizontalBlocks().Contains(block))) result.Add(block);
            }
        }
        else if (myUnit.unitType == "Bishop")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (
                    !GetDiagonalBlocks().Contains(block)) result.Add(block);
            }
        }
        else if (myUnit.unitType == "Knight")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (
                    !GetLBlocks().Contains(block)) result.Add(block);
            }
        }


        return result;
    }


    public List<Block> GetLegalEnemyBlocks()
    {
        List<Block> result = new List<Block>();
        Unit myUnit = gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)];
        if(myUnit.unitType == "King")
        {
        }
        else if(myUnit.unitType == "Queen")
        {
        }
        else if(myUnit.unitType == "Knight")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != myUnit.teamNo &&
                    GetLBlocks().Contains(block))
                {
                    result.Add(block);
                }
            }
        }
        else if(myUnit.unitType == "Rook")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != myUnit.teamNo &&
                    (GetVerticalBlocks().Contains(block) || GetHorizontalBlocks().Contains(block)))
                {
                    result.Add(block);
                }
            }
        }
        else if(myUnit.unitType == "Bishop")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != myUnit.teamNo &&
                    GetDiagonalBlocks().Contains(block))
                {
                    result.Add(block);
                }
            }
        }

        return result;
    }

    public List<Block> GetIllegalEnemyMoves()
    {
        List<Block> result = new List<Block>();
        Unit myUnit = gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)];

        if(myUnit.unitType == "King")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != myUnit.teamNo) result.Add(block);
            }
        }
        else if(myUnit.unitType == "Queen")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != myUnit.teamNo) result.Add(block);
            }
        }
        else if(myUnit.unitType == "Rook")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != myUnit.teamNo &&
                    (!GetVerticalBlocks().Contains(block) && !GetHorizontalBlocks().Contains(block))) result.Add(block);
            }
        }
        else if (myUnit.unitType == "Bishop")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != myUnit.teamNo &&
                    !GetDiagonalBlocks().Contains(block)) result.Add(block);
            }
        }
        else if (myUnit.unitType == "Knight")
        {
            foreach (Block block in GetMovableBlocks())
            {
                if (dataHandler.GetBlocksUnitIndex(block.myIndex) < 0) continue;
                if (gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != myUnit.teamNo &&
                    !GetLBlocks().Contains(block)) result.Add(block);
            }
        }


        return result;
    }

    public void HighlightHorizontal()
    {
        foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundSelectedHighlightSprite;

        foreach(Block block in horizontalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) == -1)block.foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundHighlightSprite;
        }

    }

    public void Highlight()
    {
        foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundSelectedHighlightSprite;

        List<Block> movableBlocks = GetMovableBlocks();


        foreach(Block block in movableBlocks)
        {
            block.foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundHighlightSprite;

            if(dataHandler.GetBlocksUnitIndex(block.myIndex) != -1 && gameController.allUnits[dataHandler.GetBlocksUnitIndex(block.myIndex)].teamNo != gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].teamNo) block.foreGround.GetComponent<SpriteRenderer>().sprite = foreGroundKillHighlightSprite;
        }

        

        if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "Knight")
        {
            foreach(Block block in LBlocks)
            {
                if(movableBlocks.Contains(block))block.foreForeGround.SetActive(true);
            }
        }
        else if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "Queen")
        {

        }
        else if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "Rook")
        {
            foreach(Block block in horizontalBlocks)
            {
                if(movableBlocks.Contains(block))block.foreForeGround.SetActive(true);
                
            }
            foreach(Block block in verticalBlocks)
            {
                if(movableBlocks.Contains(block))block.foreForeGround.SetActive(true);
            }
        }
        else if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "Bishop")
        {
            foreach(Block block in diagonalBlocks)
            {
                if(movableBlocks.Contains(block))block.foreForeGround.SetActive(true);
            }
        }
        else if(gameController.allUnits[dataHandler.GetBlocksUnitIndex(myIndex)].unitType == "King")
        {
            
        }

    }

    public void Lowlight()
    {
        foreGround.GetComponent<SpriteRenderer>().sprite = null;

        List<Block> movableBlocks = GetMovableBlocks();

        foreach(Block block in movableBlocks)
        {
            block.foreGround.GetComponent<SpriteRenderer>().sprite = null;
            block.foreForeGround.SetActive(false);
        }

    }


    public void LowlightHorizontal()
    {

        foreGround.GetComponent<SpriteRenderer>().sprite = null;

        foreach(Block block in horizontalBlocks)
        {
            if(dataHandler.GetBlocksUnitIndex(block.myIndex) == -1)block.foreGround.GetComponent<SpriteRenderer>().sprite = null;
        }



    }

    void SetMyIndexMyRowAndMyColumn()
    {
        //myIndex
        for(int i=0;i<36;i++)
        {
            if(allBlocks[i] == this)
            {
                myIndex = i;
                break;
            }
        }

        //myColumn
        if(myIndex % 6 == 0) myColumn = 0;
        else if(myIndex % 6 == 1) myColumn = 1;
        else if(myIndex % 6 == 2) myColumn = 2;
        else if(myIndex % 6 == 3) myColumn = 3;
        else if(myIndex % 6 == 4) myColumn = 4;
        else if(myIndex % 6 == 5) myColumn = 5;


        //myRow
        if(myIndex < 6)myRow = 0;
        else if(myIndex < 12)myRow = 1;
        else if(myIndex < 18)myRow = 2;
        else if(myIndex < 24)myRow = 3;
        else if(myIndex < 30)myRow = 4;
        else if(myIndex < 36)myRow = 5;
    }


    void SetHorizontalBlocks()
    {
        horizontalBlocks = new List<Block>();
        if(myRow == 0)
        {
            horizontalBlocks.Add(allBlocks[0]);
            horizontalBlocks.Add(allBlocks[1]);
            horizontalBlocks.Add(allBlocks[2]);
            horizontalBlocks.Add(allBlocks[3]);
            horizontalBlocks.Add(allBlocks[4]);
            horizontalBlocks.Add(allBlocks[5]);

        }
        else if(myRow == 1)
        {
            horizontalBlocks.Add(allBlocks[6]);
            horizontalBlocks.Add(allBlocks[7]);
            horizontalBlocks.Add(allBlocks[8]);
            horizontalBlocks.Add(allBlocks[9]);
            horizontalBlocks.Add(allBlocks[10]);
            horizontalBlocks.Add(allBlocks[11]);
        }
        else if(myRow == 2)
        {
            horizontalBlocks.Add(allBlocks[12]);
            horizontalBlocks.Add(allBlocks[13]);
            horizontalBlocks.Add(allBlocks[14]);
            horizontalBlocks.Add(allBlocks[15]);
            horizontalBlocks.Add(allBlocks[16]);
            horizontalBlocks.Add(allBlocks[17]);
        }
        else if(myRow == 3)
        {
            horizontalBlocks.Add(allBlocks[18]);
            horizontalBlocks.Add(allBlocks[19]);
            horizontalBlocks.Add(allBlocks[20]);
            horizontalBlocks.Add(allBlocks[21]);
            horizontalBlocks.Add(allBlocks[22]);
            horizontalBlocks.Add(allBlocks[23]);
        }
        else if(myRow == 4)
        {
            horizontalBlocks.Add(allBlocks[24]);
            horizontalBlocks.Add(allBlocks[25]);
            horizontalBlocks.Add(allBlocks[26]);
            horizontalBlocks.Add(allBlocks[27]);
            horizontalBlocks.Add(allBlocks[28]);
            horizontalBlocks.Add(allBlocks[29]);
        }
        else if(myRow == 5)
        {
            horizontalBlocks.Add(allBlocks[30]);
            horizontalBlocks.Add(allBlocks[31]);
            horizontalBlocks.Add(allBlocks[32]);
            horizontalBlocks.Add(allBlocks[33]);
            horizontalBlocks.Add(allBlocks[34]);
            horizontalBlocks.Add(allBlocks[35]);
        }

        horizontalBlocks.Remove(this);

        //setting right and left
        foreach(Block block in horizontalBlocks)
        {
            if(block.transform.position.x < transform.position.x) leftHorizontalBlocks.Add(block);
            else rightHorizontalBlocks.Add(block);
        }

        //sorting right and left
        for(int i=0;i<rightHorizontalBlocks.Count;i++)
        {
            for(int j=0;j<rightHorizontalBlocks.Count-1;j++)
            {
                if(rightHorizontalBlocks[j].transform.position.x > rightHorizontalBlocks[j+1].transform.position.x)
                {
                    Block temp = rightHorizontalBlocks[j];
                    rightHorizontalBlocks[j] = rightHorizontalBlocks[j+1];
                    rightHorizontalBlocks[j+1] = temp;
                }
            }
        }

        for(int i=0;i<leftHorizontalBlocks.Count;i++)
        {
            for(int j=0;j<leftHorizontalBlocks.Count-1;j++)
            {
                if(leftHorizontalBlocks[j].transform.position.x < leftHorizontalBlocks[j+1].transform.position.x)
                {
                    Block temp = leftHorizontalBlocks[j];
                    leftHorizontalBlocks[j] = leftHorizontalBlocks[j+1];
                    leftHorizontalBlocks[j+1] = temp;
                }
            }
        }


    }


    void SetVerticalBlocks()
    {
        verticalBlocks = new List<Block>();
        if(myColumn == 0)
        {
            verticalBlocks.Add(allBlocks[0]);
            verticalBlocks.Add(allBlocks[6]);
            verticalBlocks.Add(allBlocks[12]);
            verticalBlocks.Add(allBlocks[18]);
            verticalBlocks.Add(allBlocks[24]);
            verticalBlocks.Add(allBlocks[30]);
        }
        else if(myColumn == 1)
        {
            verticalBlocks.Add(allBlocks[1]);
            verticalBlocks.Add(allBlocks[7]);
            verticalBlocks.Add(allBlocks[13]);
            verticalBlocks.Add(allBlocks[19]);
            verticalBlocks.Add(allBlocks[25]);
            verticalBlocks.Add(allBlocks[31]);
        }
        else if(myColumn == 2)
        {
            verticalBlocks.Add(allBlocks[2]);
            verticalBlocks.Add(allBlocks[8]);
            verticalBlocks.Add(allBlocks[14]);
            verticalBlocks.Add(allBlocks[20]);
            verticalBlocks.Add(allBlocks[26]);
            verticalBlocks.Add(allBlocks[32]);
        }
        else if(myColumn == 3)
        {
            verticalBlocks.Add(allBlocks[3]);
            verticalBlocks.Add(allBlocks[9]);
            verticalBlocks.Add(allBlocks[15]);
            verticalBlocks.Add(allBlocks[21]);
            verticalBlocks.Add(allBlocks[27]);
            verticalBlocks.Add(allBlocks[33]);
        }
        else if(myColumn == 4)
        {
            verticalBlocks.Add(allBlocks[4]);
            verticalBlocks.Add(allBlocks[10]);
            verticalBlocks.Add(allBlocks[16]);
            verticalBlocks.Add(allBlocks[22]);
            verticalBlocks.Add(allBlocks[28]);
            verticalBlocks.Add(allBlocks[34]);
        }
        else if(myColumn == 5)
        {
            verticalBlocks.Add(allBlocks[5]);
            verticalBlocks.Add(allBlocks[11]);
            verticalBlocks.Add(allBlocks[17]);
            verticalBlocks.Add(allBlocks[23]);
            verticalBlocks.Add(allBlocks[29]);
            verticalBlocks.Add(allBlocks[35]);
        }

        verticalBlocks.Remove(this);

        //set down and up vertical blocks
        foreach(Block block in verticalBlocks)
        {
            if(block.transform.position.y < transform.position.y) downVerticalBlocks.Add(block);
            else upVerticalBlocks.Add(block);
        }

        //sort
        for(int i=0;i<downVerticalBlocks.Count;i++)
        {
            for(int j=0;j<downVerticalBlocks.Count-1;j++)
            {
                if(downVerticalBlocks[j].transform.position.y < downVerticalBlocks[j+1].transform.position.y)
                {
                    Block temp = downVerticalBlocks[j];
                    downVerticalBlocks[j] = downVerticalBlocks[j+1];
                    downVerticalBlocks[j+1] = temp;
                }
            }
        }

        for(int i=0;i<upVerticalBlocks.Count;i++)
        {
            for(int j=0;j<upVerticalBlocks.Count-1;j++)
            {
                if(upVerticalBlocks[j].transform.position.y > upVerticalBlocks[j+1].transform.position.y)
                {
                    Block temp = upVerticalBlocks[j];
                    upVerticalBlocks[j] = upVerticalBlocks[j+1];
                    upVerticalBlocks[j+1] = temp;
                }
            }
        }


    }

    void SetDiagonalBlocks()
    {
        diagonalBlocks = new List<Block>();

        if(myIndex == 0 || myIndex == 7 || myIndex == 14 || myIndex == 21 || myIndex == 28 || myIndex == 35)
        {
            diagonalBlocks.Add(allBlocks[0]);
            diagonalBlocks.Add(allBlocks[7]);
            diagonalBlocks.Add(allBlocks[14]);
            diagonalBlocks.Add(allBlocks[21]);
            diagonalBlocks.Add(allBlocks[28]);
            diagonalBlocks.Add(allBlocks[35]);


        }
        else if(myIndex == 1 || myIndex == 8 || myIndex == 15 || myIndex == 22 || myIndex == 29)
        {
            diagonalBlocks.Add(allBlocks[1]);
            diagonalBlocks.Add(allBlocks[8]);
            diagonalBlocks.Add(allBlocks[15]);
            diagonalBlocks.Add(allBlocks[22]);
            diagonalBlocks.Add(allBlocks[29]);

        }
        else if(myIndex == 2 || myIndex == 9 || myIndex == 16 || myIndex == 23)
        {
            diagonalBlocks.Add(allBlocks[2]);
            diagonalBlocks.Add(allBlocks[9]);
            diagonalBlocks.Add(allBlocks[16]);
            diagonalBlocks.Add(allBlocks[23]);
        }
        else if(myIndex == 3 || myIndex == 10 || myIndex == 17)
        {
            diagonalBlocks.Add(allBlocks[3]);
            diagonalBlocks.Add(allBlocks[10]);
            diagonalBlocks.Add(allBlocks[17]);

        }
        else if(myIndex == 4 || myIndex == 11)
        {
            diagonalBlocks.Add(allBlocks[4]);
            diagonalBlocks.Add(allBlocks[11]);
        }



        if(myIndex == 5 || myIndex == 10 || myIndex == 15 || myIndex == 20 || myIndex == 25 || myIndex == 30)
        {
            diagonalBlocks.Add(allBlocks[5]);
            diagonalBlocks.Add(allBlocks[10]);
            diagonalBlocks.Add(allBlocks[15]);
            diagonalBlocks.Add(allBlocks[20]);
            diagonalBlocks.Add(allBlocks[25]);
            diagonalBlocks.Add(allBlocks[30]);
        }

        else if(myIndex == 4 || myIndex == 9 || myIndex == 14 || myIndex == 19 || myIndex == 24)
        {
            diagonalBlocks.Add(allBlocks[4]);
            diagonalBlocks.Add(allBlocks[9]);
            diagonalBlocks.Add(allBlocks[14]);
            diagonalBlocks.Add(allBlocks[19]);
            diagonalBlocks.Add(allBlocks[24]);
        }

        else if(myIndex == 3 || myIndex == 8 || myIndex == 13 || myIndex == 18)
        {
            diagonalBlocks.Add(allBlocks[3]);
            diagonalBlocks.Add(allBlocks[8]);
            diagonalBlocks.Add(allBlocks[13]);
            diagonalBlocks.Add(allBlocks[18]);
        }

        else if(myIndex == 2 || myIndex == 7 || myIndex == 12)
        {
            diagonalBlocks.Add(allBlocks[2]);
            diagonalBlocks.Add(allBlocks[7]);
            diagonalBlocks.Add(allBlocks[12]);
        }

        else if(myIndex == 1 || myIndex == 6)
        {
            diagonalBlocks.Add(allBlocks[1]);
            diagonalBlocks.Add(allBlocks[6]);
        }



        if(myIndex == 6 || myIndex == 13 || myIndex == 20 || myIndex == 27 || myIndex == 34)
        {
            diagonalBlocks.Add(allBlocks[6]);
            diagonalBlocks.Add(allBlocks[13]);
            diagonalBlocks.Add(allBlocks[20]);
            diagonalBlocks.Add(allBlocks[27]);
            diagonalBlocks.Add(allBlocks[34]);

        }

        else if(myIndex == 12 || myIndex == 19 || myIndex == 26 || myIndex == 33)
        {
            diagonalBlocks.Add(allBlocks[12]);
            diagonalBlocks.Add(allBlocks[19]);
            diagonalBlocks.Add(allBlocks[26]);
            diagonalBlocks.Add(allBlocks[33]);
        }

        else if(myIndex == 18 || myIndex == 25 || myIndex == 32)
        {
            diagonalBlocks.Add(allBlocks[18]);
            diagonalBlocks.Add(allBlocks[25]);
            diagonalBlocks.Add(allBlocks[32]);
        }
        else if(myIndex == 24 || myIndex == 31)
        {
            diagonalBlocks.Add(allBlocks[24]);
            diagonalBlocks.Add(allBlocks[31]);
        }  


        if(myIndex == 11 || myIndex == 16 || myIndex == 21 || myIndex == 26 || myIndex == 31)
        {
            diagonalBlocks.Add(allBlocks[11]);
            diagonalBlocks.Add(allBlocks[16]);
            diagonalBlocks.Add(allBlocks[21]);
            diagonalBlocks.Add(allBlocks[26]);
            diagonalBlocks.Add(allBlocks[31]);
        }

        else if(myIndex == 17 || myIndex == 22 || myIndex == 27 || myIndex == 32)
        {
            diagonalBlocks.Add(allBlocks[17]);
            diagonalBlocks.Add(allBlocks[22]);
            diagonalBlocks.Add(allBlocks[27]);
            diagonalBlocks.Add(allBlocks[32]);
        }
        else if(myIndex == 23 || myIndex == 28 || myIndex == 33)
        {
            diagonalBlocks.Add(allBlocks[23]);
            diagonalBlocks.Add(allBlocks[28]);
            diagonalBlocks.Add(allBlocks[33]);
        }
        else if(myIndex == 29 || myIndex == 34)
        {
            diagonalBlocks.Add(allBlocks[29]);
            diagonalBlocks.Add(allBlocks[34]);
        }

        while(diagonalBlocks.Contains(this)) diagonalBlocks.Remove(this);

        //set top left-top right-down left-down right diagonals

        foreach(Block block in diagonalBlocks)
        {
            if(block.transform.position.x < transform.position.x)
            {
                if(block.transform.position.y < transform.position.y)
                {
                    downLeftDiagonalBlocks.Add(block);
                }
                else
                {
                    topLeftDiagonalBlocks.Add(block);
                }
            }
            else
            {
                if(block.transform.position.y < transform.position.y)
                {
                    downRightDiagonalBlocks.Add(block);
                }
                else
                {
                    topRightDiagonalBlocks.Add(block);
                }
            }
        }


        //sort

        for(int i=0;i<downLeftDiagonalBlocks.Count;i++)
        {
            for(int j=0;j<downLeftDiagonalBlocks.Count-1;j++)
            {
                if(downLeftDiagonalBlocks[j].transform.position.x < downLeftDiagonalBlocks[j+1].transform.position.x)
                {
                    Block temp = downLeftDiagonalBlocks[j];
                    downLeftDiagonalBlocks[j] = downLeftDiagonalBlocks[j+1];
                    downLeftDiagonalBlocks[j+1] = temp;
                }
            }
        }

        for(int i=0;i<topLeftDiagonalBlocks.Count;i++)
        {
            for(int j=0;j<topLeftDiagonalBlocks.Count-1;j++)
            {
                if(topLeftDiagonalBlocks[j].transform.position.x < topLeftDiagonalBlocks[j+1].transform.position.x)
                {
                    Block temp = topLeftDiagonalBlocks[j];
                    topLeftDiagonalBlocks[j] = topLeftDiagonalBlocks[j+1];
                    topLeftDiagonalBlocks[j+1] = temp;
                }
            }
        }


        for(int i=0;i<downRightDiagonalBlocks.Count;i++)
        {
            for(int j=0;j<downRightDiagonalBlocks.Count-1;j++)
            {
                if(downRightDiagonalBlocks[j].transform.position.x > downRightDiagonalBlocks[j+1].transform.position.x)
                {
                    Block temp = downRightDiagonalBlocks[j];
                    downRightDiagonalBlocks[j] = downRightDiagonalBlocks[j+1];
                    downRightDiagonalBlocks[j+1] = temp;
                }
            }
        }
        for(int i=0;i<topRightDiagonalBlocks.Count;i++)
        {
            for(int j=0;j<topRightDiagonalBlocks.Count-1;j++)
            {
                if(topRightDiagonalBlocks[j].transform.position.x > topRightDiagonalBlocks[j+1].transform.position.x)
                {
                    Block temp = topRightDiagonalBlocks[j];
                    topRightDiagonalBlocks[j] = topRightDiagonalBlocks[j+1];
                    topRightDiagonalBlocks[j+1] = temp;
                }
            }
        }




    }


    void SetLBlocks()
    {
        if(myIndex == 0)
        {
            LBlocks.Add(allBlocks[8]);
            LBlocks.Add(allBlocks[13]);
        }
        else if(myIndex == 1)
        {
            LBlocks.Add(allBlocks[12]);
            LBlocks.Add(allBlocks[14]);
            LBlocks.Add(allBlocks[9]);
        }
        else if(myIndex == 2)
        {
            LBlocks.Add(allBlocks[6]);
            LBlocks.Add(allBlocks[13]);
            LBlocks.Add(allBlocks[15]);
            LBlocks.Add(allBlocks[10]);

        }
        else if(myIndex == 3)
        {
            LBlocks.Add(allBlocks[7]);
            LBlocks.Add(allBlocks[14]);
            LBlocks.Add(allBlocks[16]);
            LBlocks.Add(allBlocks[11]);

        }
        else if(myIndex == 4)
        {
            LBlocks.Add(allBlocks[8]);
            LBlocks.Add(allBlocks[15]);
            LBlocks.Add(allBlocks[17]);
        }
        else if(myIndex == 5)
        {
            LBlocks.Add(allBlocks[9]);
            LBlocks.Add(allBlocks[16]);

        }
        else if(myIndex == 6)
        {
            LBlocks.Add(allBlocks[14]);
            LBlocks.Add(allBlocks[19]);
            LBlocks.Add(allBlocks[2]);
        }
        else if(myIndex == 7)
        {
            LBlocks.Add(allBlocks[18]);
            LBlocks.Add(allBlocks[20]);
            LBlocks.Add(allBlocks[15]);
            LBlocks.Add(allBlocks[3]);
        }
        else if(myIndex == 8)
        {
            LBlocks.Add(allBlocks[12]);
            LBlocks.Add(allBlocks[19]);
            LBlocks.Add(allBlocks[21]);
            LBlocks.Add(allBlocks[16]);
            LBlocks.Add(allBlocks[4]);
            LBlocks.Add(allBlocks[0]);
        }
        else if(myIndex == 9)
        {
            LBlocks.Add(allBlocks[13]);
            LBlocks.Add(allBlocks[20]);
            LBlocks.Add(allBlocks[22]);
            LBlocks.Add(allBlocks[17]);
            LBlocks.Add(allBlocks[5]);
            LBlocks.Add(allBlocks[1]);

        }
        else if(myIndex == 10)
        {
            LBlocks.Add(allBlocks[14]);
            LBlocks.Add(allBlocks[21]);
            LBlocks.Add(allBlocks[23]);
            LBlocks.Add(allBlocks[2]);
        }
        else if(myIndex == 11)
        {
            LBlocks.Add(allBlocks[15]);
            LBlocks.Add(allBlocks[22]);
            LBlocks.Add(allBlocks[3]);

        }
        else if(myIndex == 12)
        {
            LBlocks.Add(allBlocks[20]);
            LBlocks.Add(allBlocks[25]);
            LBlocks.Add(allBlocks[8]);
            LBlocks.Add(allBlocks[1]);
        }
        else if(myIndex == 13)
        {
            LBlocks.Add(allBlocks[24]);
            LBlocks.Add(allBlocks[26]);
            LBlocks.Add(allBlocks[21]);
            LBlocks.Add(allBlocks[9]);
            LBlocks.Add(allBlocks[0]);
            LBlocks.Add(allBlocks[2]);
        }
        else if(myIndex == 14)
        {
            LBlocks.Add(allBlocks[18]);
            LBlocks.Add(allBlocks[25]);
            LBlocks.Add(allBlocks[27]);
            LBlocks.Add(allBlocks[22]);
            LBlocks.Add(allBlocks[10]);
            LBlocks.Add(allBlocks[6]);
            LBlocks.Add(allBlocks[1]);
            LBlocks.Add(allBlocks[3]);

        }
        else if(myIndex == 15)
        {
            LBlocks.Add(allBlocks[19]);
            LBlocks.Add(allBlocks[26]);
            LBlocks.Add(allBlocks[28]);
            LBlocks.Add(allBlocks[23]);
            LBlocks.Add(allBlocks[11]);
            LBlocks.Add(allBlocks[7]);
            LBlocks.Add(allBlocks[2]);
            LBlocks.Add(allBlocks[4]);

        }
        else if(myIndex == 16)
        {
            LBlocks.Add(allBlocks[20]);
            LBlocks.Add(allBlocks[27]);
            LBlocks.Add(allBlocks[29]);
            LBlocks.Add(allBlocks[8]);
            LBlocks.Add(allBlocks[3]);
            LBlocks.Add(allBlocks[5]);

        }
        else if(myIndex == 17)
        {
            LBlocks.Add(allBlocks[21]);
            LBlocks.Add(allBlocks[28]);
            LBlocks.Add(allBlocks[9]);
            LBlocks.Add(allBlocks[4]);
        }
        else if(myIndex == 18)
        {
            LBlocks.Add(allBlocks[26]);
            LBlocks.Add(allBlocks[31]);
            LBlocks.Add(allBlocks[14]);
            LBlocks.Add(allBlocks[7]);

        }
        else if(myIndex == 19)
        {
            LBlocks.Add(allBlocks[30]);
            LBlocks.Add(allBlocks[32]);
            LBlocks.Add(allBlocks[27]);
            LBlocks.Add(allBlocks[15]);
            LBlocks.Add(allBlocks[6]);
            LBlocks.Add(allBlocks[8]);

        }
        else if(myIndex == 20)
        {
            LBlocks.Add(allBlocks[28]);
            LBlocks.Add(allBlocks[31]);
            LBlocks.Add(allBlocks[33]);
            LBlocks.Add(allBlocks[24]);
            LBlocks.Add(allBlocks[16]);
            LBlocks.Add(allBlocks[12]);
            LBlocks.Add(allBlocks[7]);
            LBlocks.Add(allBlocks[9]);

        }
        else if(myIndex == 21)
        {
            LBlocks.Add(allBlocks[25]);
            LBlocks.Add(allBlocks[32]);
            LBlocks.Add(allBlocks[34]);
            LBlocks.Add(allBlocks[29]);
            LBlocks.Add(allBlocks[17]);
            LBlocks.Add(allBlocks[13]);
            LBlocks.Add(allBlocks[8]);
            LBlocks.Add(allBlocks[10]);

        }
        else if(myIndex == 22)
        {
            LBlocks.Add(allBlocks[26]);
            LBlocks.Add(allBlocks[33]);
            LBlocks.Add(allBlocks[35]);
            LBlocks.Add(allBlocks[14]);
            LBlocks.Add(allBlocks[9]);
            LBlocks.Add(allBlocks[11]);

        }
        else if(myIndex == 23)
        {
            LBlocks.Add(allBlocks[27]);
            LBlocks.Add(allBlocks[34]);
            LBlocks.Add(allBlocks[15]);
            LBlocks.Add(allBlocks[10]);

        }
        else if(myIndex == 24)
        {
            LBlocks.Add(allBlocks[32]);
            LBlocks.Add(allBlocks[20]);
            LBlocks.Add(allBlocks[13]);

        }
        else if(myIndex == 25)
        {
            LBlocks.Add(allBlocks[33]);
            LBlocks.Add(allBlocks[21]);
            LBlocks.Add(allBlocks[12]);
            LBlocks.Add(allBlocks[14]);

        }
        else if(myIndex == 26)
        {
            LBlocks.Add(allBlocks[30]);
            LBlocks.Add(allBlocks[34]);
            LBlocks.Add(allBlocks[22]);
            LBlocks.Add(allBlocks[18]);
            LBlocks.Add(allBlocks[13]);
            LBlocks.Add(allBlocks[15]);

        }
        else if(myIndex == 27)
        {
            LBlocks.Add(allBlocks[31]);
            LBlocks.Add(allBlocks[35]);
            LBlocks.Add(allBlocks[23]);
            LBlocks.Add(allBlocks[19]);
            LBlocks.Add(allBlocks[14]);
            LBlocks.Add(allBlocks[16]);

        }
        else if(myIndex == 28)
        {
            LBlocks.Add(allBlocks[32]);
            LBlocks.Add(allBlocks[20]);
            LBlocks.Add(allBlocks[15]);
            LBlocks.Add(allBlocks[17]);

        }
        else if(myIndex == 29)
        {
            LBlocks.Add(allBlocks[33]);
            LBlocks.Add(allBlocks[21]);
            LBlocks.Add(allBlocks[16]);

        }
        else if(myIndex == 30)
        {
            LBlocks.Add(allBlocks[26]);
            LBlocks.Add(allBlocks[19]);

        }
        else if(myIndex == 31)
        {
            LBlocks.Add(allBlocks[27]);
            LBlocks.Add(allBlocks[18]);
            LBlocks.Add(allBlocks[20]);

        }
        else if(myIndex == 32)
        {
            LBlocks.Add(allBlocks[28]);
            LBlocks.Add(allBlocks[24]);
            LBlocks.Add(allBlocks[19]);
            LBlocks.Add(allBlocks[21]);

        }
        else if(myIndex == 33)
        {
            LBlocks.Add(allBlocks[29]);
            LBlocks.Add(allBlocks[25]);
            LBlocks.Add(allBlocks[20]);
            LBlocks.Add(allBlocks[22]);

        }
        else if(myIndex == 34)
        {
            LBlocks.Add(allBlocks[26]);
            LBlocks.Add(allBlocks[21]);
            LBlocks.Add(allBlocks[23]);

        }
        else if(myIndex == 35)
        {
            LBlocks.Add(allBlocks[27]);
            LBlocks.Add(allBlocks[22]);

        }





    }


}
