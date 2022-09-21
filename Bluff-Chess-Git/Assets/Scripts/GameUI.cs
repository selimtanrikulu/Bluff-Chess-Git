using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Bolt.Matchmaking;

public class GameUI : MonoBehaviour
{


    [SerializeField] public TextMeshPro gameInfoTMP;
    [SerializeField] TextMeshPro myTimeTMP;
    [SerializeField] TextMeshPro gameIDTMP;
    [SerializeField] TextMeshPro gameIDTMPStatic;

    [SerializeField] TextMeshPro enemyTimeTMP;
    [SerializeField] float startGameDelay;
    [SerializeField] MyButton claimKingButton;
    [SerializeField] MyButton dontClaimKingButton;
    [SerializeField] MyButton claimBluffButton;
    [SerializeField] MyButton passButton;
    [SerializeField] MyButton randomizeButton;
    [SerializeField] MyButton readyButton;
    [SerializeField] MyButton leaveButton;

    [SerializeField] SpriteRenderer myScore1;
    [SerializeField] SpriteRenderer myScore2;
    [SerializeField] SpriteRenderer enemyScore1;
    [SerializeField] SpriteRenderer enemyScore2;

    [SerializeField] Sprite scoreSprite;
    [SerializeField] Sprite scorelessSprite;

    [SerializeField] SpriteRenderer[] whiteGraveyardImages;
    [SerializeField] SpriteRenderer[] blackGraveyardImages;

    int currentWhiteGraveyard = 0;
    int currentBlackGraveyard = 0;

    [SerializeField] Sprite blackKingSprite;
    [SerializeField] Sprite blackBishopSprite;
    [SerializeField] Sprite blackRookSprite;
    [SerializeField] Sprite blackQueenSprite;
    [SerializeField] Sprite blackPawnSprite;
    [SerializeField] Sprite blackKnightSprite;

    [SerializeField] Sprite whiteKingSprite;
    [SerializeField] Sprite whiteBishopSprite;
    [SerializeField] Sprite whiteRookSprite;
    [SerializeField] Sprite whiteQueenSprite;
    [SerializeField] Sprite whitePawnSprite;
    [SerializeField] Sprite whiteKnightSprite;

    [SerializeField] Sprite emptyBlackSprite;
    [SerializeField] Sprite emptyWhiteSprite;


    [SerializeField] TextMeshPro myNickName;
    [SerializeField] TextMeshPro enemyNickName;


    [SerializeField] float graveyardAnimationSpeed;

    [SerializeField] float popupDuration;
    [SerializeField] SpriteRenderer enemyDialogueImage;
    [SerializeField] TextMeshPro enemyDialogueTMP;
    [SerializeField] float enemyDialogueTransparencyChange;


    IEnumerator graveyardAnimationCoroutine;
    IEnumerator enemyPopupCoroutine;

    public Player myPlayer;


    GameController gameController;

    DataHandler dataHandler;


    IEnumerator getDisabledCoroutine;

    IEnumerator activateBoardInfoTMPcoroutine;

    IEnumerator inActivateBoardInfoTMPCoroutine;
    [SerializeField] TextMeshPro boardInfoTMP;
    [SerializeField] GameObject boardInfoShadow;
    [SerializeField] float boardInfoActivationTime;

    IEnumerator carryGainingScoreCoroutine;
    [SerializeField] GameObject gainingScore;
    Vector3 gainingScoreStartPos;

    IEnumerator activateGainingScoreCoroutine;

    AudioController audioController;

    Settings settings;

    [SerializeField] Sprite[] avatarSprites;
    [SerializeField] SpriteRenderer myAvatarImage;
    [SerializeField] SpriteRenderer myAvatarBackground;
    [SerializeField] SpriteRenderer enemyAvatarImage;
    [SerializeField] SpriteRenderer enemyAvatarBackground;


    //just to rotate
    [SerializeField] GameObject clockImage1;
    [SerializeField] GameObject clockImage2;
    [SerializeField] GameObject boardShadow;
    [SerializeField] GameObject topShadow;
    [SerializeField] GameObject background;
    [SerializeField] GameObject boardBackground;
    [SerializeField] GameObject gameLogBackground;

    //------

    [SerializeField] GameObject myTurnCircle;
    [SerializeField] GameObject enemyTurnCircle;


    [SerializeField] GameInfoBlink gameInfoBlink;
    [SerializeField] GameObject babySitter;


    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        settings = FindObjectOfType<Settings>();

        if(settings.privateGame)
        {
            gameIDTMP.text =(BoltMatchmaking.CurrentSession.HostName).ToString();
            
        }
        else
        {
            gameIDTMP.gameObject.SetActive(false);
            gameIDTMPStatic.gameObject.SetActive(false);
        }

        
        audioController = FindObjectOfType<AudioController>();
        
        gainingScoreStartPos = gainingScore.transform.position;
        
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

        HandleStartGame();
        HandleClaimDontClaimKingButtons();
        HandleClaimBluffPassButtons();
        HandleReadyRandomizeButtons();
        //HandleScores();
        HandleTimeTexts();
        HandlegameInfoTMP();
        HandleLeaveButton();
        HandleNickNames();
        HandleAvatars();
        HandleTurnCircles();

    }

    public void ActivateBoardInfo(string text,float duration)
    {
        activateBoardInfoTMPcoroutine = ActivateBoardInfoC(text,duration);
        StartCoroutine(activateBoardInfoTMPcoroutine);
    }

    public void InActivateBoardInfo()
    {
        inActivateBoardInfoTMPCoroutine = InActivateBoardInfoC();
        StartCoroutine(inActivateBoardInfoTMPCoroutine);
    }

    public void CarryGainingScore(int playerNo,int scoreNo)
    {
        carryGainingScoreCoroutine = CarryGainingScoreC(playerNo,scoreNo);
        StartCoroutine(carryGainingScoreCoroutine);
    }

    IEnumerator ActivateBoardInfoC(string text,float duration)
    {   
        boardInfoTMP.text = text;


        while(boardInfoTMP.color.a < 1)
        {
            Color color = boardInfoTMP.color;
            color.a += Time.deltaTime * (1/boardInfoActivationTime);

            boardInfoTMP.color = color;
            boardInfoShadow.GetComponent<SpriteRenderer>().color = color;
            duration -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if(duration < 0)
        {

        }
        else
        {
            while(duration > boardInfoActivationTime)
            {
                duration -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }


            while(duration > 0)
            {
                Color color = boardInfoTMP.color;
                color.a -= Time.deltaTime * (1/boardInfoActivationTime);

                boardInfoTMP.color = color;
                boardInfoShadow.GetComponent<SpriteRenderer>().color = color;
                duration -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

    IEnumerator InActivateBoardInfoC()
    {   
        while(boardInfoTMP.color.a > 0)
        {
            Color color = boardInfoTMP.color;
            color.a -= Time.deltaTime * (1/boardInfoActivationTime);

            boardInfoTMP.color = color;
            boardInfoShadow.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }        
    }

    IEnumerator ActivateGainingScore()
    {   
        gainingScore.SetActive(true);
        gainingScore.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);

        Color color = gainingScore.GetComponent<SpriteRenderer>().color;


        while(color.a < 1)
        {
            color.a += Time.deltaTime * 4;
            gainingScore.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }        
    }

    IEnumerator CarryGainingScoreC(int playerNo,int scoreNo)
    {   
        yield return new WaitForSeconds(0.5f);
        activateGainingScoreCoroutine = ActivateGainingScore();
        StartCoroutine(activateGainingScoreCoroutine);
        yield return new WaitForSeconds(0.5f);


        Vector3 targetLoc;

        if(playerNo == myPlayer.playerNo)
        {
            if(scoreNo == 1)
            {
                targetLoc = myScore1.transform.position;
            }
            else
            {
                targetLoc = myScore2.transform.position;
            }
        }
        else
        {
            if(scoreNo == 1)
            {
                targetLoc = enemyScore1.transform.position;
            }
            else
            {
                targetLoc = enemyScore2.transform.position;
            }
        }




        graveyardAnimationCoroutine = CarryImageFromTo(gainingScore.GetComponent<SpriteRenderer>(),gainingScore.transform.position,targetLoc,0.3f);
        StartCoroutine(graveyardAnimationCoroutine);
        yield return new WaitForSeconds(1.5f);
        gainingScore.GetComponentInChildren<Blink>().onlyDown = true;
        yield return new WaitForSeconds(2f);
        HandleScores();
        gainingScore.GetComponentInChildren<Blink>().Reset();
        gainingScore.SetActive(false);
        gainingScore.transform.position = gainingScoreStartPos;
    }


    void HandleNickNames()
    {
        myNickName.text = FindObjectOfType<Settings>().nickName;
        if(myPlayer.playerNo == 1)
        {
            enemyNickName.text = dataHandler.GetBlackNickName();
        }
        else
        {
            enemyNickName.text = dataHandler.GetWhiteNickName();
        }
        
    }

    void HandleAvatars()
    {
        myAvatarImage.sprite = avatarSprites[FindObjectOfType<Settings>().avatarIndex];
        if (myPlayer.playerNo == 1)
        {
            int index = dataHandler.GetBlackAvatarIndex();
            if(index != -1)
            {
                enemyAvatarImage.sprite = avatarSprites[index];
                enemyAvatarImage.color = new Color(1, 1, 1, 1);
                enemyAvatarBackground.color = new Color(1, 1, 1, 1);
            }
            else
            {
                enemyAvatarBackground.color = new Color(1, 1, 1, 0);
                enemyAvatarImage.color = new Color(1, 1, 1, 0);
            }
            
        }
        else
        {
            int index = dataHandler.GetWhiteAvatarIndex();
            if (index != -1)
            {
                enemyAvatarImage.sprite = avatarSprites[index];
                enemyAvatarImage.color = new Color(1, 1, 1, 1);
                enemyAvatarBackground.color = new Color(1, 1, 1, 1);
            }
            else
            {
                enemyAvatarBackground.color = new Color(1, 1, 1, 0);
                enemyAvatarImage.color = new Color(1, 1, 1, 0);
            }
        }
    }


    
    void HandlegameInfoTMP()
    {
        string currentGameInfo = gameInfoTMP.text;
        
        if(dataHandler.GetGameState()==0)
        {
            gameInfoTMP.text = "Waiting for the opponent to connect";
            return;
        }  

        if(dataHandler.GetGameState() == 1)
        {
            if(dataHandler.GetWhoseTurn() == 1)
            {
                gameInfoTMP.text = "White player\nwill move";
            }   
            else
            {
                gameInfoTMP.text = "Black player\nwill move";
            }
        }

        else if(dataHandler.GetGameState() == 2)
        {
            if(dataHandler.GetWhoseTurn() == 1)
            {
                gameInfoTMP.text = "White player will decide to claim bluff / pass";
            }   
            else
            {
                gameInfoTMP.text = "Black player will decide to claim bluff / pass";
            }

        }

        else if(dataHandler.GetGameState() == 3)
        {
            if(dataHandler.GetWhoseTurn() == 1)
            {
                gameInfoTMP.text = "It was NOT bluff. White player will sacrifice a piece";
            }   
            else
            {
                gameInfoTMP.text = "It was NOT bluff. Black player will sacrifice a piece";
            }
        }

        else if(dataHandler.GetGameState() == 4)
        {
            if(gameController.winner == 1)gameInfoTMP.text = "Game over \nWhite won";
            else gameInfoTMP.text = "Game over \nBlack won";
            
        }
        
        else if(dataHandler.GetGameState() == 5)
        {
            if(dataHandler.GetWhoseTurn() == 1)
            {
                gameInfoTMP.text = "White player has claimed bluff. Checking ...";
            }
            else
            {
                gameInfoTMP.text = "Black player has claimed bluff. Checking ...";
            }

        }

        else if(dataHandler.GetGameState() == 6)
        {
            if(dataHandler.GetWhoseTurn() == 1)
            {
                gameInfoTMP.text = "It was bluff !\nThe black piece will die";
            }
            else
            {
                gameInfoTMP.text = "It was bluff !\nThe white piece will die";
            }

        }

        else if(dataHandler.GetGameState() == 7)
        {
            if(dataHandler.GetWhoseTurn() == 1)
            {
                gameInfoTMP.text = "White player is setting";
            }
            else
            {
                gameInfoTMP.text = "Black player is setting";
            }

        }

        else if(dataHandler.GetGameState() == 8)
        {
            gameInfoTMP.text = "Round over.\n" + gameController.roundOverCause;
        }
        

        if(currentGameInfo != gameInfoTMP.text)
        {
            gameInfoBlink.BlinkOneTime();
        }

    }

    void HandleTurnCircles()
    {
        if(dataHandler.GetWhoseTurn() == myPlayer.playerNo)
        {
            myTurnCircle.SetActive(true);
            enemyTurnCircle.SetActive(false);
        }
        else
        {
            myTurnCircle.SetActive(false);
            enemyTurnCircle.SetActive(true);
        }
    }

    void HandleScores()
    {
        if(!dataHandler) return;
        if(!myPlayer) return;
        if(!gameController) return;

        int myScore;
        int enemyScore;

        if(myPlayer.playerNo == 1)
        {
            myScore = gameController.whitePlayerScore;
            enemyScore = gameController.blackPlayerScore;

        }
        else
        {
            myScore = gameController.blackPlayerScore;
            enemyScore = gameController.whitePlayerScore;
        }

        if(myScore == 0)
        {
            myScore1.sprite = scorelessSprite;
            myScore2.sprite= scorelessSprite;
        }
        else if(myScore == 1)
        {
            myScore1.sprite = scoreSprite;
            myScore2.sprite = scorelessSprite;

        }
        else if(myScore == 2)
        {
            myScore1.sprite = scoreSprite;
            myScore2.sprite= scoreSprite;

        }
        if(enemyScore == 0)
        {   
            enemyScore1.sprite = scorelessSprite;
            enemyScore2.sprite = scorelessSprite;
        }
        else if(enemyScore == 1)
        {
            enemyScore1.sprite = scoreSprite;
            enemyScore2.sprite = scorelessSprite;
        }
        else if(enemyScore == 2)
        {
            enemyScore1.sprite = scoreSprite;
            enemyScore2.sprite = scoreSprite;
        }


        //enabling enemy score
            if(FindObjectsOfType<DataHandler>().Length > 1)
            {
                enemyScore1.gameObject.SetActive(true);
                enemyScore2.gameObject.SetActive(true);
            }
            else
            {
                enemyScore1.gameObject.SetActive(false);
                enemyScore2.gameObject.SetActive(false);
            }






        //----------


    }

    void HandleStartGame()
    {
        if(dataHandler.GetGameState() != 0) return;
        
        if(FindObjectsOfType<DataHandler>().Length < 2) return;
        
        if(startGameDelay > 0)
        {
            startGameDelay -= Time.deltaTime;
            gameInfoTMP.text =((int)(startGameDelay) + 1).ToString();
        }
        else
        {
            gameInfoTMP.text = "";
            gameController.StartSetupPhase();
            gameIDTMP.gameObject.SetActive(false);
            gameIDTMPStatic.gameObject.SetActive(false);

            ActivateBoardInfo("SETUP PHASE",-1);

        }


    }

    private IEnumerator GetDisable(Button button,float after)
    {
        yield return new WaitForSeconds(after);
        
        button.gameObject.SetActive(false);

    }

    void HandleClaimDontClaimKingButtons()
    {
        if(dataHandler.GetGameState() == 1 && dataHandler.GetWhoseTurn() == myPlayer.playerNo && myPlayer.selectedBlock)
        {
            if(myPlayer.claimKing)
            {
                dontClaimKingButton.gameObject.SetActive(true);
                claimKingButton.gameObject.SetActive(false);
            }
            else
            {
                dontClaimKingButton.gameObject.SetActive(false);
                claimKingButton.gameObject.SetActive(true);
            }
        }
        else
        {
            claimKingButton.gameObject.SetActive(false);
            dontClaimKingButton.gameObject.SetActive(false);
        }
    }

    void HandleClaimBluffPassButtons()
    {                                  
                                    
        if(dataHandler.GetGameState() == 2 && dataHandler.GetWhoseTurn() == myPlayer.playerNo)
        {
            claimBluffButton.gameObject.SetActive(true);
            passButton.gameObject.SetActive(true);
        }
        else
        {
            claimBluffButton.gameObject.SetActive(false);
            passButton.gameObject.SetActive(false);
        }

    }


    void HandleReadyRandomizeButtons()
    {
        if(dataHandler.GetGameState() == 7 && dataHandler.GetWhoseTurn() == myPlayer.playerNo)
        {
            readyButton.gameObject.SetActive(true);
            randomizeButton.gameObject.SetActive(true);
        }
        else
        {
            readyButton.gameObject.SetActive(false);
            randomizeButton.gameObject.SetActive(false);
        }



    }


    void HandleTimeTexts()
    {
        if(!myPlayer) return;

        if(!dataHandler) return;

        if(dataHandler.GetGameState() == 0)return;

        if(myPlayer.playerNo == 1)
        {
            myTimeTMP.text = Mathf.Max(((int)(dataHandler.GetTimeLeftWhite())),0).ToString();
            enemyTimeTMP.text = Mathf.Max(((int)(dataHandler.GetTimeLeftBlack())),0).ToString();
        }
        else
        {
            myTimeTMP.text = Mathf.Max(((int)(dataHandler.GetTimeLeftBlack())),0).ToString();
            enemyTimeTMP.text = Mathf.Max(((int)(dataHandler.GetTimeLeftWhite())),0).ToString();
        }


    }


    void HandleLeaveButton()
    {
        if(!dataHandler)return;

        if(dataHandler.GetGameState() == 0) leaveButton.gameObject.SetActive(true);
        else leaveButton.gameObject.SetActive(false);
    }



    IEnumerator CarryImageFromTo(SpriteRenderer image, Vector2 start,Vector2 finish,float speedConfigure)
    {   
        image.transform.position = start;

        Vector2 imagePos = image.transform.position;

        while((imagePos - finish).magnitude>0.005f)
        {
            if((imagePos-finish).magnitude < 0.3f)
            {
                imagePos += (finish - imagePos).normalized * graveyardAnimationSpeed * Time.deltaTime * 0.01f;
                image.transform.position = imagePos;
            }
            else
            {
                imagePos += (finish - imagePos).normalized * graveyardAnimationSpeed * Time.deltaTime * speedConfigure;
                image.transform.position = imagePos;
            }

            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        

    }

    public void ClearGraveyard()
    {
        for(int i=0;i<whiteGraveyardImages.Length;i++)
        {
            whiteGraveyardImages[i].color = new Color(1,1,1,0);
        }
        currentWhiteGraveyard = 0;

        for(int i=0;i<blackGraveyardImages.Length;i++)
        {
            blackGraveyardImages[i].color = new Color(1,1,1,0);
        }
        currentBlackGraveyard = 0;


    }


    public void AddImageToGraveyard(string unitType,int teamNo,Vector3 fromWorldSpace)
    {
        Vector3 screenPos = fromWorldSpace;
        

        if(teamNo == 1)
        {
            if(unitType == "Pawn")
            {
                whiteGraveyardImages[currentWhiteGraveyard].sprite = whitePawnSprite;
            }
            else if(unitType == "Knight")
            {
                whiteGraveyardImages[currentWhiteGraveyard].sprite = whiteKnightSprite;
            }
            else if(unitType == "Queen")
            {
                whiteGraveyardImages[currentWhiteGraveyard].sprite = whiteQueenSprite;
            }
            else if(unitType == "King")
            {
                whiteGraveyardImages[currentWhiteGraveyard].sprite = whiteKingSprite;
            }
            else if(unitType == "Bishop")
            {
                whiteGraveyardImages[currentWhiteGraveyard].sprite = whiteBishopSprite;
            }
            else if(unitType == "Rook")
            {
                whiteGraveyardImages[currentWhiteGraveyard].sprite = whiteRookSprite;
            }
            else if(unitType == "Empty")
            {
                whiteGraveyardImages[currentWhiteGraveyard].sprite = emptyWhiteSprite;
            }

            whiteGraveyardImages[currentWhiteGraveyard].color = new Color(1,1,1,1);
            graveyardAnimationCoroutine = CarryImageFromTo(whiteGraveyardImages[currentWhiteGraveyard],screenPos,whiteGraveyardImages[currentWhiteGraveyard].transform.position,1);
            StartCoroutine(graveyardAnimationCoroutine);

            currentWhiteGraveyard ++;
        }
        else
        {
            if(unitType == "Pawn")
            {
                blackGraveyardImages[currentBlackGraveyard].sprite = blackPawnSprite;
            }
            else if(unitType == "Knight")
            {
                blackGraveyardImages[currentBlackGraveyard].sprite = blackKnightSprite;
            }
            else if(unitType == "Queen")
            {
                blackGraveyardImages[currentBlackGraveyard].sprite = blackQueenSprite;
            }
            else if(unitType == "King")
            {
                blackGraveyardImages[currentBlackGraveyard].sprite = blackKingSprite;
            }
            else if(unitType == "Bishop")
            {
                blackGraveyardImages[currentBlackGraveyard].sprite = blackBishopSprite;
            }
            else if(unitType == "Rook")
            {
                blackGraveyardImages[currentBlackGraveyard].sprite = blackRookSprite;
            }
            else if(unitType == "Empty")
            {
                blackGraveyardImages[currentBlackGraveyard].sprite = emptyBlackSprite;
            }

            blackGraveyardImages[currentBlackGraveyard].color = new Color(1,1,1,1);

            blackGraveyardImages[currentBlackGraveyard].color = new Color(1,1,1,1);
            graveyardAnimationCoroutine = CarryImageFromTo(blackGraveyardImages[currentBlackGraveyard],screenPos,blackGraveyardImages[currentBlackGraveyard].transform.position,1);
            StartCoroutine(graveyardAnimationCoroutine);

            currentBlackGraveyard ++;
        }

        
    }

    public void RotateWorld()
    {
        List<GameObject> worldObjectsToRotate = new List<GameObject>();
        //worldObjectsToRotate.Add(gameInfoTMP.gameObject);
        worldObjectsToRotate.Add(enemyScore1.gameObject);
        worldObjectsToRotate.Add(enemyScore2.gameObject);
        worldObjectsToRotate.Add(myScore1.gameObject);
        worldObjectsToRotate.Add(myScore2.gameObject);
        worldObjectsToRotate.Add(leaveButton.gameObject);
        worldObjectsToRotate.Add(claimBluffButton.gameObject);
        worldObjectsToRotate.Add(dontClaimKingButton.gameObject);
        worldObjectsToRotate.Add(randomizeButton.gameObject);
        worldObjectsToRotate.Add(passButton.gameObject);
        worldObjectsToRotate.Add(readyButton.gameObject);
        worldObjectsToRotate.Add(claimKingButton.gameObject);

        worldObjectsToRotate.Add(myAvatarBackground.gameObject);
        worldObjectsToRotate.Add(enemyAvatarBackground.gameObject);
        worldObjectsToRotate.Add(gameIDTMP.gameObject);
        worldObjectsToRotate.Add(myNickName.gameObject);
        worldObjectsToRotate.Add(enemyNickName.gameObject);
        worldObjectsToRotate.Add(gameIDTMPStatic.gameObject);
        worldObjectsToRotate.Add(myTimeTMP.gameObject);
        worldObjectsToRotate.Add(enemyTimeTMP.gameObject);
        worldObjectsToRotate.Add(clockImage1);
        worldObjectsToRotate.Add(clockImage2);
        worldObjectsToRotate.Add(boardShadow);
        worldObjectsToRotate.Add(topShadow);
        worldObjectsToRotate.Add(background);
        worldObjectsToRotate.Add(boardBackground);

        worldObjectsToRotate.Add(myTurnCircle);
        worldObjectsToRotate.Add(enemyTurnCircle);

        worldObjectsToRotate.Add(boardInfoTMP.gameObject);
        worldObjectsToRotate.Add(boardInfoShadow);
        worldObjectsToRotate.Add(gainingScore);
        worldObjectsToRotate.Add(babySitter);
        

        worldObjectsToRotate.Add(gameLogBackground);

        worldObjectsToRotate.Add(enemyDialogueImage.gameObject);
        foreach(GameObject obj in worldObjectsToRotate)
        {
            obj.transform.position = new Vector3(-obj.transform.position.x,-obj.transform.position.y,obj.transform.position.z);
            obj.transform.Rotate(Vector3.forward,180);
        }

        foreach(SpriteRenderer obj in blackGraveyardImages)
        {
            obj.transform.position = new Vector3(obj.transform.position.x,-obj.transform.position.y,obj.transform.position.z);
            obj.transform.Rotate(Vector3.forward,180);
        }
        foreach(SpriteRenderer obj in whiteGraveyardImages)
        {
            obj.transform.position = new Vector3(obj.transform.position.x,-obj.transform.position.y,obj.transform.position.z);
            obj.transform.Rotate(Vector3.forward,180);
        }



        //change gaining score start pos
        gainingScoreStartPos.x *= -1;
        gainingScoreStartPos.y *= -1;


        //--


    }

    IEnumerator EnemyDialoguePopUpCoroutine(string text,float duration)
    {

        enemyDialogueTMP.text = text;    

        while(enemyDialogueImage.color.a < 1)
        {
            enemyDialogueImage.color += new Color(0,0,0,Time.deltaTime * enemyDialogueTransparencyChange);
            enemyDialogueTMP.color += new Color(0,0,0,Time.deltaTime * enemyDialogueTransparencyChange) ;
            yield return new WaitForSeconds(Time.deltaTime);

        }

        yield return new WaitForSeconds(duration - ((1/enemyDialogueTransparencyChange) * 2));

        while(enemyDialogueImage.color.a > 0)
        {
            enemyDialogueImage.color -= new Color(0,0,0,Time.deltaTime * enemyDialogueTransparencyChange);
            enemyDialogueTMP.color -= new Color(0,0,0,Time.deltaTime * enemyDialogueTransparencyChange) ;
            yield return new WaitForSeconds(Time.deltaTime);

        }

    }



    public void TriggerEnemyDialoguePopUp(string dialogue)
    {

        if(dialogue == "Claim Bluff")
        {
            enemyPopupCoroutine = EnemyDialoguePopUpCoroutine("It's Bluff !", popupDuration);
            StartCoroutine(enemyPopupCoroutine);
        }
        else if(dialogue == "Pass")
        {
            enemyPopupCoroutine = EnemyDialoguePopUpCoroutine("Pass", popupDuration);
            StartCoroutine(enemyPopupCoroutine);
        }
        else if(dialogue == "Claim King")
        {
            enemyPopupCoroutine = EnemyDialoguePopUpCoroutine("I Claim King !", popupDuration);
            StartCoroutine(enemyPopupCoroutine);
        }



    }


    public void ClaimKingButtonOnClick()
    {
        myPlayer.claimKing = true;
        myPlayer.HightlightKingSelected();
    }

    public void DontClaimKingButtonOnClick()
    {
        myPlayer.claimKing = false;
        myPlayer.HighlightSelected();
    }

    public void ClaimBluffButtonOnClick()
    {
        gameController.BluffClaimed();
    }

    public void PassButtonOnClick()
    {
        gameController.Pass();
        

    }


    public void RandomizeButtonOnClick()
    {
        myPlayer.RandomizeUnits();

        audioController.PlaySound("randomizeSound");

    }

    public void ReadyButtonOnClick()
    {
        myPlayer.GetReady();


    }


    public void LeaveButtonOnClick()
    {
        gameController.LeaveGame();

        

    }

}
