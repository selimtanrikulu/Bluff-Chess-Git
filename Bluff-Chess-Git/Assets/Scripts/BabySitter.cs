using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BabySitter : MonoBehaviour
{
    [SerializeField] GameObject babySitterBaloon;
    [SerializeField] TextMeshPro babySitterTMP;
    [SerializeField] float tipDuration;
    [SerializeField] float forceShowTipDuration;
    [SerializeField] float transparencyChangeSpeed;

    IEnumerator showUpTipCoroutine;
    IEnumerator inActivateTipCoroutine;

    bool tipActive;

    Color babySitterTMPStartColor;
    Color babySitterBaloonStartColor;


    bool forceClose;
    bool forceShowed;

    DataHandler dataHandler;

    int lastGameState = -1;
    int lastTurn = -1;

    GameUI gameUI;
    GameController gameController;

    void Start()
    {
        babySitterBaloonStartColor = babySitterBaloon.GetComponent<SpriteRenderer>().color;
        babySitterTMPStartColor = babySitterTMP.color;
        
        Color color1 = babySitterBaloonStartColor;
        color1.a = 0;
        babySitterBaloonStartColor = color1;

        Color color2 = babySitterTMPStartColor;
        color2.a = 0;
        babySitterTMPStartColor = color2;

        gameUI = FindObjectOfType<GameUI>();
        gameController = FindObjectOfType<GameController>();

        //ShowUpTip("Set your units");
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

        if(!dataHandler) return;

        BrowseGame();



    }

    bool GameStateChanged()
    {
        if(lastGameState != dataHandler.GetGameState())return true;
        return false;
    }

    bool TurnChanged()
    {
        if(lastTurn != dataHandler.GetWhoseTurn()) return true;
        return false;
    }


    void BrowseGame()
    {
        if((!GameStateChanged() && !TurnChanged())) return;

        forceClose = true; 

        
        

        if(dataHandler.GetGameState() == 0) ShowUpTip("Wait for your opponent to connect",false);


        else if(dataHandler.GetGameState() == 1 && lastGameState == 2 && dataHandler.GetWhoseTurn() != gameUI.myPlayer.playerNo)ShowUpTip("Your opponent passed your move",true);

        else if(dataHandler.GetGameState() == 1 && lastGameState == 2 && dataHandler.GetWhoseTurn() == gameUI.myPlayer.playerNo)ShowUpTip("You passed your opponent's move",true);

        



        if(dataHandler.GetGameState() == 7 && dataHandler.GetWhoseTurn() == gameUI.myPlayer.playerNo) ShowUpTip("Set your pieces",false);
        else if(dataHandler.GetGameState() == 7 && dataHandler.GetWhoseTurn() != gameUI.myPlayer.playerNo) ShowUpTip("Wait your opponent to set pieces",false);


        else if(dataHandler.GetGameState() == 1 && dataHandler.GetWhoseTurn() == gameUI.myPlayer.playerNo) ShowUpTip("Make your move !",false);
        else if(dataHandler.GetGameState() == 1 && dataHandler.GetWhoseTurn() != gameUI.myPlayer.playerNo) ShowUpTip("Wait your opponent to move",false);

        else if(dataHandler.GetGameState() == 2 && dataHandler.GetWhoseTurn() != gameUI.myPlayer.playerNo)
        {
            if(dataHandler.GetLastMoveWasBluff()) ShowUpTip("You made a bluff !",false);
            else ShowUpTip("You made a legal move !",false);
        }

        else if(dataHandler.GetGameState() == 2 && dataHandler.GetWhoseTurn() == gameUI.myPlayer.playerNo)ShowUpTip("Your opponent moved. Make your choice !",false);


        else if(dataHandler.GetGameState() == 5 && dataHandler.GetWhoseTurn() != gameUI.myPlayer.playerNo) ShowUpTip("Your opponent claimed bluff !",false);
        else if(dataHandler.GetGameState() == 5 && dataHandler.GetWhoseTurn() == gameUI.myPlayer.playerNo) ShowUpTip("You claimed bluff !",false);

        else if(dataHandler.GetGameState() == 3 && dataHandler.GetWhoseTurn() == gameUI.myPlayer.playerNo) ShowUpTip("Your claim was wrong. Sacrifice a piece",false);
        else if(dataHandler.GetGameState() == 3 && dataHandler.GetWhoseTurn() != gameUI.myPlayer.playerNo) ShowUpTip("Wrong claim ! Your opponent will sacrifice a piece !",false);

        else if(dataHandler.GetGameState() == 8)
        {
            if(gameController.roundWinner == gameUI.myPlayer.playerNo)ShowUpTip("You won the round !"  + gameController.roundOverCause,false);
            else ShowUpTip("You lost the round." + gameController.roundOverCause,false);
        }

        else if(dataHandler.GetGameState() == 4)
        {
            if(gameController.roundWinner == gameUI.myPlayer.playerNo)ShowUpTip("You won the game !",false);
            else ShowUpTip("You lost the game.",false);
        }


        else if(dataHandler.GetGameState() == 6 && dataHandler.GetWhoseTurn() == gameUI.myPlayer.playerNo)ShowUpTip("You caught your opponent's bluff !",false);
        else if(dataHandler.GetGameState() == 6 && dataHandler.GetWhoseTurn() != gameUI.myPlayer.playerNo)ShowUpTip("Your opponent caught your bluff",false);

    }

    void ShowUpTip(string tip,bool forceShow)
    {
        if(tipActive)return;
        if(forceShow && forceShowed) return;

        tipActive = true;

        forceShowed = forceShow;

        if(!forceShow)
        {
            lastGameState = dataHandler.GetGameState();
            lastTurn = dataHandler.GetWhoseTurn();
        }

        

        forceClose = false;

        
        showUpTipCoroutine = ShowUpTipC(tip,forceShow);
        StartCoroutine(showUpTipCoroutine);
    }



    private IEnumerator ShowUpTipC(string tip,bool forceShow)
    {
        babySitterTMP.text = tip;
        babySitterTMP.color = babySitterTMPStartColor;
        babySitterBaloon.GetComponent<SpriteRenderer>().color = babySitterBaloonStartColor;

        float alpha = 0;

        float duration;
        if(forceShow)duration = forceShowTipDuration;
        else duration = tipDuration;

        //Alpha up
        while(alpha < 1)
        {
            alpha += Time.deltaTime * transparencyChangeSpeed;
            duration -= Time.deltaTime;

            Color color1 = babySitterTMP.color;
            color1.a = alpha;
            babySitterTMP.color = color1;

            Color color2 = babySitterBaloon.GetComponent<SpriteRenderer>().color;
            color2.a = alpha;
            babySitterBaloon.GetComponent<SpriteRenderer>().color = color2;

            yield return new WaitForSeconds(Time.deltaTime);


            if(forceClose && !forceShow)break;

        }

        while(duration > 1/transparencyChangeSpeed)
        {
            duration -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);  

            if(forceClose && !forceShow)break; 
        }
        
        while(alpha > 0)
        {
            alpha -= Time.deltaTime * transparencyChangeSpeed;
            duration -= Time.deltaTime;

            Color color1 = babySitterTMP.color;
            color1.a = alpha;
            babySitterTMP.color = color1;

            Color color2 = babySitterBaloon.GetComponent<SpriteRenderer>().color;
            color2.a = alpha;
            babySitterBaloon.GetComponent<SpriteRenderer>().color = color2;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        tipActive = false;

    }


}
