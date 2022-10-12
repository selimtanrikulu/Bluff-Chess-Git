using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialMaker : MonoBehaviour
{


    BabySitter babySitter;

    [SerializeField] GameObject[] allBlocks;
    [SerializeField] GameObject[] allForeGrounds;
    [SerializeField] GameObject[] allForeForeGrounds;

    [SerializeField] GameObject[] allUnits;
    [SerializeField] GameObject[] allMoveObjects;

    [SerializeField] Sprite actionTracerSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite moveSprite;
    [SerializeField] Sprite emptyBlackSprite;
    [SerializeField] Sprite killableSprite;
    [SerializeField] Sprite moveRookSprite;
    [SerializeField] Sprite blackRookSprite;
    [SerializeField] Sprite moveBishopSprite;
    [SerializeField] Sprite blackKingSprite;
    [SerializeField] Sprite kingMoveSprite;



    [SerializeField] Sprite blackPawnSprite;
     [SerializeField] Sprite whitePawnSprite;
    [SerializeField] Sprite  whiteQueenSprite;
    [SerializeField] Sprite whiteKingSprite;
[SerializeField] Sprite  blackBishopSprite;
[SerializeField] Sprite  whiteBishopSprite;

[SerializeField] Sprite  whiteKnightSprite;
[SerializeField] Sprite  whiteRookSprite;
[SerializeField] Sprite  blackKnightSprite;
[SerializeField] Sprite   blackQueenSprite;
[SerializeField] Sprite emptyWhiteSprite;



    int tutorialState = 0;

    float counter;


    [SerializeField] MyButton claimKingButton;
    [SerializeField] MyButton dontClaimKingButton;
    [SerializeField] MyButton claimBluffButton;
    [SerializeField] MyButton passButton;
    [SerializeField] MyButton randomizeButton;
    [SerializeField] MyButton readyButton;
    [SerializeField] MyButton leaveButton;

    [SerializeField] MyButton nextButton;

    [SerializeField] ArrowImage arrowImage;



    IEnumerator activateBoardInfoTMPcoroutine;

    IEnumerator inActivateBoardInfoTMPCoroutine;
    [SerializeField] TextMeshPro boardInfoTMP;
    [SerializeField] GameObject boardInfoShadow;
    [SerializeField] float boardInfoActivationTime;

    IEnumerator carryGainingScoreCoroutine;
    [SerializeField] GameObject gainingScore;
    Vector3 gainingScoreStartPos;

    IEnumerator activateGainingScoreCoroutine;




    [SerializeField] TextMeshPro enemyDialogueTMP;
    [SerializeField] SpriteRenderer enemyDialogueImage;
    [SerializeField] float enemyDialogueTransparencyChange;
    [SerializeField] float popupDuration;
    IEnumerator enemyPopupCoroutine;


    [SerializeField] SpriteRenderer[] whiteGraveyardImages;
    [SerializeField] SpriteRenderer[] blackGraveyardImages;
    [SerializeField] float graveyardAnimationSpeed;

    int currentWhiteGraveyard = 0;
    int currentBlackGraveyard = 0;

    IEnumerator graveyardAnimationCoroutine;


    


    void Start()
    {
        babySitter = FindObjectOfType<BabySitter>();


        for(int i=8;i<13;i++)
        {
            allUnits[i].GetComponent<SpriteRenderer>().sprite = emptyBlackSprite;
        }

        
    }

    void Update()
    {
        Debug.Log(tutorialState);
        
        HandleTips();
        HandleTutorialStates();

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


    void HandleTutorialStates()
    {
        if(tutorialState == 5)
        {
            arrowImage.gameObject.SetActive(true);
            SetArrowPosition(new Vector2(0.45f,-0.48f));
            ActivateBoardInfo("SETUP PHASE",-1);
            nextButton.gameObject.SetActive(false);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[9])
                {
                    tutorialState ++;
                }
            }
        }

        else if(tutorialState == 6)
        {
            arrowImage.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);

            allForeGrounds[6].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
            allForeGrounds[7].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
            allForeGrounds[10].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
            allForeGrounds[9].GetComponent<SpriteRenderer>().sprite = selectedSprite;

        }

        else if(tutorialState == 8)
        {
            arrowImage.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);

            SetArrowPosition(new Vector2(-1.4f,-0.48f));

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[7])
                {
                    allForeGrounds[6].GetComponent<SpriteRenderer>().sprite =null;
                    allForeGrounds[7].GetComponent<SpriteRenderer>().sprite = moveSprite;
                    allForeGrounds[10].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[9].GetComponent<SpriteRenderer>().sprite = moveSprite;




                    allUnits[6].transform.position = allBlocks[7].transform.position;
                    arrowImage.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);

                    tutorialState ++;
                }
            }
            

        }

        else if(tutorialState == 10)
        {
            allForeGrounds[7].GetComponent<SpriteRenderer>().sprite = null;
            allForeGrounds[9].GetComponent<SpriteRenderer>().sprite = null;
            arrowImage.gameObject.SetActive(true);
            SetArrowPosition(new Vector2(-2.35f,-1.41f));
            ActivateBoardInfo("SETUP PHASE",-1);
            nextButton.gameObject.SetActive(false);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[0])
                {
                    arrowImage.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);
                    allForeGrounds[5].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[0].GetComponent<SpriteRenderer>().sprite = selectedSprite;
                    tutorialState ++;
                }
            }
        }

        else if(tutorialState == 12)
        {
            SetArrowPosition(new Vector2(2.35f,-1.47f));
            arrowImage.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
            
            

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[5])
                {
                    allForeGrounds[0].GetComponent<SpriteRenderer>().sprite =moveSprite;
                    allForeGrounds[5].GetComponent<SpriteRenderer>().sprite = moveSprite;
                    allUnits[0].transform.position = allBlocks[5].transform.position;
                    arrowImage.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);

                    tutorialState ++;
                }
            }
            

        }
        else if(tutorialState == 14)
        {

            allForeGrounds[0].GetComponent<SpriteRenderer>().sprite = null;
            allForeGrounds[5].GetComponent<SpriteRenderer>().sprite = null;

            readyButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);

        }

        else if(tutorialState == 15)
        {
            readyButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);


            counter += Time.deltaTime;

            if(counter >= 2)
            {
                counter = 0;
                tutorialState ++;
            }
        }

        else if(tutorialState == 16)
        {
            counter += Time.deltaTime;

            if(counter >= 3)
            {
                counter = 0;
                tutorialState ++;
                nextButton.gameObject.SetActive(true);
            }
        }

        else if(tutorialState == 17)
        {
            allUnits[14].transform.position = allBlocks[24].transform.position;
            allUnits[12].transform.position = allBlocks[30].transform.position;


            InActivateBoardInfo();
        }

        else if(tutorialState == 18)
        {
            nextButton.gameObject.SetActive(false);
            readyButton.gameObject.SetActive(false);
            SetArrowPosition(new Vector2(0.5f,-1.411f));
            arrowImage.gameObject.SetActive(true);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[3])
                {
                    allForeGrounds[3].GetComponent<SpriteRenderer>().sprite = selectedSprite;
                    allForeForeGrounds[9].SetActive(true);
                    allForeForeGrounds[15].SetActive(true);
                    allForeForeGrounds[21].SetActive(true);

                    allForeGrounds[9].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[15].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[21].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[14].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;

                    allForeGrounds[10].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[16].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[17].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;

                     allForeGrounds[27].GetComponent<SpriteRenderer>().sprite = killableSprite;

                    arrowImage.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);

                    tutorialState ++;
                }
            }

        } 



        else if(tutorialState == 20)
        {
            SetArrowPosition(new Vector2(0.51f,2.335f));

            arrowImage.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[27])
                {
                    allForeGrounds[3].GetComponent<SpriteRenderer>().sprite = moveSprite;
                    allForeForeGrounds[9].SetActive(false);
                    allForeForeGrounds[15].SetActive(false);
                    allForeForeGrounds[21].SetActive(false);

                    allForeGrounds[9].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[15].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[21].GetComponent<SpriteRenderer>().sprite = null;


                    allForeGrounds[10].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[16].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[17].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[14].GetComponent<SpriteRenderer>().sprite = null;
                    

                    allUnits[3].transform.position = allBlocks[27].transform.position;

                    allUnits[13].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);

                    allForeGrounds[27].GetComponent<SpriteRenderer>().sprite = moveSprite;

                    arrowImage.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(false);

                    tutorialState ++;
                }
            }



        }


        else if(tutorialState == 21)
        {
            counter += Time.deltaTime;

            if(counter > 2)
            {
                counter = 0;
                tutorialState++;

                allForeGrounds[27].GetComponent<SpriteRenderer>().sprite = null;
                allForeGrounds[3].GetComponent<SpriteRenderer>().sprite = null;

                nextButton.gameObject.SetActive(true);
                TriggerEnemyDialoguePopUp("Pass");

                
                AddImageToGraveyard("Pawn",2,allUnits[13].transform.position);
            }

        }

        else if(tutorialState == 25)
        {
            nextButton.gameObject.SetActive(false);


            counter += Time.deltaTime;

            if(counter > 3)
            {
                counter = 0;
                tutorialState++;

                allForeGrounds[31].GetComponent<SpriteRenderer>().sprite = moveSprite;
                allForeGrounds[7].GetComponent<SpriteRenderer>().sprite = moveSprite;

                allUnits[8].transform.position = allBlocks[7].transform.position;
                allUnits[6].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);


                allMoveObjects[8].SetActive(true);
                allMoveObjects[8].GetComponent<SpriteRenderer>().sprite = moveRookSprite;

                nextButton.gameObject.SetActive(true);
            }

        }


        else if(tutorialState == 30)
        {
            nextButton.gameObject.SetActive(false);
            claimBluffButton.gameObject.SetActive(true);

        }

        else if(tutorialState == 31)
        {
            claimBluffButton.gameObject.SetActive(false);


            counter += Time.deltaTime;

            if(counter > 2)
            {
                counter = 0;
                tutorialState++;



                
                AddImageToGraveyard("Pawn",1,allUnits[6].transform.position);

            }

        }

        else if(tutorialState == 32)
        {
            nextButton.gameObject.SetActive(true);

            allForeGrounds[31].GetComponent<SpriteRenderer>().sprite = null;
            allForeGrounds[7].GetComponent<SpriteRenderer>().sprite = null;
            allMoveObjects[8].SetActive(false);
            allMoveObjects[8].GetComponent<SpriteRenderer>().sprite = moveRookSprite;


            allUnits[8].GetComponent<SpriteRenderer>().sprite = blackRookSprite;

        }

        else if(tutorialState == 33)
        {
            allUnits[8].GetComponent<SpriteRenderer>().sprite = emptyBlackSprite;

        }

        else if(tutorialState == 34)
        {


        }

        else if(tutorialState == 35)
        {
            nextButton.gameObject.SetActive(false);

            allForeGrounds[2].GetComponent<SpriteRenderer>().sprite = killableSprite;
            allForeGrounds[4].GetComponent<SpriteRenderer>().sprite = killableSprite;
            allForeGrounds[5].GetComponent<SpriteRenderer>().sprite = killableSprite;
            allForeGrounds[8].GetComponent<SpriteRenderer>().sprite = killableSprite;
            allForeGrounds[11].GetComponent<SpriteRenderer>().sprite = killableSprite;
            allForeGrounds[27].GetComponent<SpriteRenderer>().sprite = killableSprite;

            SetArrowPosition(new Vector2(-0.44f,-0.46f));
            arrowImage.gameObject.SetActive(true);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[8])
                {
                    allForeGrounds[2].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[4].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[5].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[8].GetComponent<SpriteRenderer>().sprite =  null;
                    allForeGrounds[11].GetComponent<SpriteRenderer>().sprite =  null;
                    allForeGrounds[27].GetComponent<SpriteRenderer>().sprite =  null;
                    allUnits[5].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
                    arrowImage.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);
                    tutorialState ++;


                    
                    AddImageToGraveyard("Pawn",1,allUnits[5].transform.position);
                }
            }



        }


        else if(tutorialState == 42)
        {
            nextButton.gameObject.SetActive(false);
            SetArrowPosition(new Vector2(-1.389f,-1.41f));
            arrowImage.gameObject.SetActive(true);


            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[1])
                {
                    allForeGrounds[1].GetComponent<SpriteRenderer>().sprite = selectedSprite;
                    
                    allForeGrounds[6].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[0].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[7].GetComponent<SpriteRenderer>().sprite = killableSprite;
                    allForeGrounds[8].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;


                    allForeGrounds[15].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[22].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[12].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[14].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[9].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;

                    arrowImage.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(true);
                    tutorialState ++;
                }
            }


        }


        else if(tutorialState == 47)
        {
            nextButton.gameObject.SetActive(false);
            claimKingButton.gameObject.SetActive(true);

        }


        else if(tutorialState == 48)
        {
            allForeGrounds[15].GetComponent<SpriteRenderer>().sprite = null;
            allForeGrounds[22].GetComponent<SpriteRenderer>().sprite = null;
            allForeGrounds[12].GetComponent<SpriteRenderer>().sprite = null;
            allForeGrounds[14].GetComponent<SpriteRenderer>().sprite = null;
            allForeGrounds[9].GetComponent<SpriteRenderer>().sprite = null;


            claimKingButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);

            allForeForeGrounds[6].SetActive(true);
            allForeForeGrounds[0].SetActive(true);
            allForeForeGrounds[8].SetActive(true);

        }

        else if(tutorialState == 49)
        {
            nextButton.gameObject.SetActive(false);
            SetArrowPosition(new Vector2(-1.41f,-0.51f));
            arrowImage.gameObject.SetActive(true);


            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[7])
                {
                    allForeForeGrounds[6].SetActive(false);
                    allForeForeGrounds[0].SetActive(false);
                    allForeForeGrounds[8].SetActive(false);


                    allForeGrounds[1].GetComponent<SpriteRenderer>().sprite = moveSprite;
                    
                    allForeGrounds[6].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[0].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[7].GetComponent<SpriteRenderer>().sprite = moveSprite;
                    allForeGrounds[8].GetComponent<SpriteRenderer>().sprite = null;
                    

                    allUnits[1].transform.position = allBlocks[7].transform.position;
                    allUnits[8].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);

                    arrowImage.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(false);
                    tutorialState ++;
                }
            }


        }

        else if(tutorialState == 50)
        {
        
            counter += Time.deltaTime;


            if(counter > 2)
            {
                counter = 0;
                tutorialState++;
                TriggerEnemyDialoguePopUp("Pass");
                allForeGrounds[1].GetComponent<SpriteRenderer>().sprite = null;
                allForeGrounds[7].GetComponent<SpriteRenderer>().sprite = null;


                nextButton.gameObject.SetActive(true);


                
                AddImageToGraveyard("Rook",2,allUnits[1].transform.position);

            }

        }

        


        else if(tutorialState == 52)
        {
            nextButton.gameObject.SetActive(false);
            counter += Time.deltaTime;


            if(counter > 2)
            {
                counter = 0;
                tutorialState++;

                allUnits[12].transform.position = allBlocks[5].transform.position;
                allForeGrounds[5].GetComponent<SpriteRenderer>().sprite = moveSprite;
                allForeGrounds[30].GetComponent<SpriteRenderer>().sprite = moveSprite;

                allUnits[0].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);

                allMoveObjects[12].GetComponent<SpriteRenderer>().sprite = moveBishopSprite;
                allMoveObjects[12].SetActive(true);


                passButton.gameObject.SetActive(true);
            }

        }


        else if(tutorialState == 54)
        {
            passButton.gameObject.SetActive(false);


            allMoveObjects[12].SetActive(false);
            allForeGrounds[5].GetComponent<SpriteRenderer>().sprite = null;
            allForeGrounds[30].GetComponent<SpriteRenderer>().sprite = null;


            SetArrowPosition(new Vector2(-0.49f,-1.4f));
            arrowImage.gameObject.SetActive(true);


            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[2])
                {
                    allForeGrounds[2].GetComponent<SpriteRenderer>().sprite = selectedSprite;

                    allForeGrounds[8].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[14].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[20].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[26].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[32].GetComponent<SpriteRenderer>().sprite = killableSprite;

                    allForeGrounds[9].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[16].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[23].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[15].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[10].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[6].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[3].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[0].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[1].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;
                    allForeGrounds[13].GetComponent<SpriteRenderer>().sprite = actionTracerSprite;


                    arrowImage.gameObject.SetActive(false);
                    tutorialState ++;
                }
            }


        }


        else if(tutorialState == 55)
        {

            SetArrowPosition(new Vector2(-0.49f,3.23f));
            arrowImage.gameObject.SetActive(true);


            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(GetBlockUnderCursor() == allBlocks[32])
                {
                    allForeGrounds[2].GetComponent<SpriteRenderer>().sprite = moveSprite;
                    allForeGrounds[32].GetComponent<SpriteRenderer>().sprite = moveSprite;

                    allForeGrounds[8].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[14].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[20].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[26].GetComponent<SpriteRenderer>().sprite = null;

                    allForeGrounds[9].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[16].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[23].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[15].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[10].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[6].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[3].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[0].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[1].GetComponent<SpriteRenderer>().sprite = null;
                    allForeGrounds[13].GetComponent<SpriteRenderer>().sprite = null;


                    allUnits[2].transform.position = allBlocks[32].transform.position;
                    allUnits[9].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);



                    arrowImage.gameObject.SetActive(false);
                    nextButton.gameObject.SetActive(false);
                    tutorialState ++;
                }
            }


        }


        else if(tutorialState == 56)
        {
        
            counter += Time.deltaTime;


            if(counter > 2)
            {
                counter = 0;
                tutorialState++;
                TriggerEnemyDialoguePopUp("Pass");
                allForeGrounds[2].GetComponent<SpriteRenderer>().sprite = null;
                allForeGrounds[32].GetComponent<SpriteRenderer>().sprite = null;


                nextButton.gameObject.SetActive(true);


                
                AddImageToGraveyard("Bishop",2,allUnits[9].transform.position);

            }

        }


        else if(tutorialState == 58)
        {
            nextButton.gameObject.SetActive(false);

            counter += Time.deltaTime;


            if(counter > 2)
            {
                counter = 0;
                tutorialState++;

                allUnits[10].transform.position = allBlocks[32].transform.position;
                allUnits[2].GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);

                allMoveObjects[10].GetComponent<SpriteRenderer>().sprite = kingMoveSprite;
                allMoveObjects[10].SetActive(true);

                allForeGrounds[32].GetComponent<SpriteRenderer>().sprite = moveSprite;
                allForeGrounds[33].GetComponent<SpriteRenderer>().sprite = moveSprite;


                nextButton.gameObject.SetActive(true);
            }   

        }

        else if(tutorialState == 61)
        {
            nextButton.gameObject.SetActive(false);
            passButton.gameObject.SetActive(true);

        }
        else if(tutorialState == 62)
        {
            allMoveObjects[10].SetActive(false);
            allForeGrounds[32].GetComponent<SpriteRenderer>().sprite = null;
            allForeGrounds[33].GetComponent<SpriteRenderer>().sprite = null;


            nextButton.gameObject.SetActive(true);
            passButton.gameObject.SetActive(false);

            allUnits[10].GetComponent<SpriteRenderer>().sprite = null;


        }


        else if(tutorialState == 73)
        {
            LeaveTutorialButtonOnClick();
        }




    }

    

    public void ReadyButtonOnClick()
    {
        tutorialState ++;
    }

    public void ClaimBluffButtonOnClick()
    {
        tutorialState ++;
    }

    public void ClaimKingButtonOnClick()
    {
        tutorialState ++;
    }


    public void LeaveTutorialButtonOnClick()
    {
        SceneManager.LoadScene("Menu");
    }

    void HandleTips()
    {
        if(tutorialState == 0)babySitter.ShowUpTipTutorial("Welcome to Bluff Chess !");
        else if(tutorialState == 1)babySitter.ShowUpTipTutorial("I'm Robotip, your favorite fellow.");
        else if(tutorialState == 2)babySitter.ShowUpTipTutorial("In this tutorial, I will try to teach you the game mechanics.");
        else if(tutorialState == 3)babySitter.ShowUpTipTutorial("Follow me to become a great bluffer !");

        else if(tutorialState == 4)babySitter.ShowUpTipTutorial("Each game starts with a\n \"SETUP\" phase");
        else if(tutorialState == 5)babySitter.ShowUpTipTutorial("Your turn ! Click your pawn to move");
        else if(tutorialState == 6)babySitter.ShowUpTipTutorial("Great ! Now you can see the little circles on the movable blocks");
        else if(tutorialState == 7)babySitter.ShowUpTipTutorial("In the setup phase, you can move your pawns inside the 2'nd row.");
        else if(tutorialState == 8)babySitter.ShowUpTipTutorial("Now place the pawn into the shown location.");
        else if(tutorialState == 9)babySitter.ShowUpTipTutorial("Great ! Let's set one more piece");
        else if(tutorialState == 10)babySitter.ShowUpTipTutorial("Click your knight to move");
        else if(tutorialState == 11)babySitter.ShowUpTipTutorial("In setup phase, you can move your non-pawn pieces inside the first row");
        else if(tutorialState == 12)babySitter.ShowUpTipTutorial("Move your knight to the shown location.");
        else if(tutorialState == 13)babySitter.ShowUpTipTutorial("I think you are ready now.");
        else if(tutorialState == 14)babySitter.ShowUpTipTutorial("Press\nready button !");
        else if(tutorialState == 15)babySitter.ShowUpTipTutorial("Wait your opponent to set pieces");
        else if(tutorialState == 16)babySitter.ShowUpTipTutorial("As you can see, your opponent's all non-pawn units are hidden !");
        else if(tutorialState == 17)babySitter.ShowUpTipTutorial("Your opponent is ready too. Now the round starts.");
        else if(tutorialState == 18)babySitter.ShowUpTipTutorial("Click your rook to make your first move");
        else if(tutorialState == 19)babySitter.ShowUpTipTutorial("Notice that the legal movable blocks are indicated slightly differently.");
        else if(tutorialState == 20)babySitter.ShowUpTipTutorial("Now make a legal move to the shown location");
        else if(tutorialState == 21)babySitter.ShowUpTipTutorial("Great ! Now wait for your opponent to make a decision");
        else if(tutorialState == 22)babySitter.ShowUpTipTutorial("It seems your opponent passed your move.");
        else if(tutorialState == 23)babySitter.ShowUpTipTutorial("Which means your opponent thinks your move was not bluff");
        else if(tutorialState == 24)babySitter.ShowUpTipTutorial("That was a correct guess. Anyway we captured a piece.");
        else if(tutorialState == 25)babySitter.ShowUpTipTutorial("Opponent's turn. Wait for your opponent to move.");
        else if(tutorialState == 26)babySitter.ShowUpTipTutorial("The opponent made the move. Now it is time to make a decision.");
        else if(tutorialState == 27)babySitter.ShowUpTipTutorial("The claim is done by the opponent shown at top of the moving piece.");
        else if(tutorialState == 28)babySitter.ShowUpTipTutorial("So, your opponent's claim is that piece is a rook");
        else if(tutorialState == 29)babySitter.ShowUpTipTutorial("It may be. But I think it seems a sneaky move.");
        else if(tutorialState == 30)babySitter.ShowUpTipTutorial("Go and decide on \"Claim Bluff\" !");
        else if(tutorialState == 31)babySitter.ShowUpTipTutorial("Let's see whether the move was a bluff or not.");
        else if(tutorialState == 32)babySitter.ShowUpTipTutorial("The piece is now visible. It is really a rook. We made a wrong guess.");
        else if(tutorialState == 33)babySitter.ShowUpTipTutorial("The piece is hidden again. It is a rook keep it in mind !");
        else if(tutorialState == 34)babySitter.ShowUpTipTutorial("As we blamed our opponent unfairly we will sacrifice a piece");
        else if(tutorialState == 35)babySitter.ShowUpTipTutorial("Click on the shown piece to sacrifice.");
        else if(tutorialState == 36)babySitter.ShowUpTipTutorial("We lost a piece as a penalty");
        else if(tutorialState == 37)babySitter.ShowUpTipTutorial("It is usually better to sacrifice your pawns first.");
        else if(tutorialState == 38)babySitter.ShowUpTipTutorial("Because you cannot move your pawns after the round starts.");
        else if(tutorialState == 39)babySitter.ShowUpTipTutorial("Now it is our turn to move");
        else if(tutorialState == 40)babySitter.ShowUpTipTutorial("Our king is in danger. If we lose it, we lose the round.");
        else if(tutorialState == 41)babySitter.ShowUpTipTutorial("We have to kill that rook we kept in mind.");
        else if(tutorialState == 42)babySitter.ShowUpTipTutorial("Now select your king to move !");
        else if(tutorialState == 43)babySitter.ShowUpTipTutorial("Now, if you move your king directly, it will be a claim of rook move");
        else if(tutorialState == 44)babySitter.ShowUpTipTutorial("We usually don't want to make an illegal move with our king.");
        else if(tutorialState == 44)babySitter.ShowUpTipTutorial("Because if we get caught, we lose our king and lose the round.");
        else if(tutorialState == 45)babySitter.ShowUpTipTutorial("To make a legal move with your king, you have to press the Claim King button");
        else if(tutorialState == 46)babySitter.ShowUpTipTutorial("In this way, your move claim will be KING");
        else if(tutorialState == 47)babySitter.ShowUpTipTutorial("Now, press the Claim King button");
        else if(tutorialState == 48)babySitter.ShowUpTipTutorial("We are now ready to make a legal move with our King.");
        else if(tutorialState == 49)babySitter.ShowUpTipTutorial("Capture that rook !");
        else if(tutorialState == 50)babySitter.ShowUpTipTutorial("Great ! Wait for your opponent to decide");
        else if(tutorialState == 51)babySitter.ShowUpTipTutorial("Your opponent passed your move again.");
        else if(tutorialState == 52)babySitter.ShowUpTipTutorial("It is opponent's turn. Wait for your opponent to move.");
        else if(tutorialState == 53)babySitter.ShowUpTipTutorial("The opponent has moved. Now use the pass button");
        else if(tutorialState == 54)babySitter.ShowUpTipTutorial("It is your turn. Click on your bomb to move.");
        else if(tutorialState == 55)babySitter.ShowUpTipTutorial("Attack to the shown location !");
        else if(tutorialState == 56)babySitter.ShowUpTipTutorial("Wait for the opponent to decide");
        else if(tutorialState == 57)babySitter.ShowUpTipTutorial("Your opponent passed your\nmove ! We bluffed it.");
        else if(tutorialState == 58)babySitter.ShowUpTipTutorial("It is your opponent's turn");
        else if(tutorialState == 58)babySitter.ShowUpTipTutorial("Umm. Your opponent made a King claim");
        else if(tutorialState == 59)babySitter.ShowUpTipTutorial("If it is really the King, it is gonna die anyway");
        else if(tutorialState == 60)babySitter.ShowUpTipTutorial("Because our bomb will blow it up !");
        else if(tutorialState == 61)babySitter.ShowUpTipTutorial("I think it is better to pass now.");
        else if(tutorialState == 62)babySitter.ShowUpTipTutorial("It was really the King !");
        else if(tutorialState == 63)babySitter.ShowUpTipTutorial("We blew it up with our bomb.");
        else if(tutorialState == 64)babySitter.ShowUpTipTutorial("We win the round as we killed the opponent's king");
        else if(tutorialState == 65)babySitter.ShowUpTipTutorial("Who wins 2 rounds, wins the game.");
        else if(tutorialState == 66)babySitter.ShowUpTipTutorial("Last one more important info:");
        else if(tutorialState == 67)babySitter.ShowUpTipTutorial("There are 3 conditions overs the round");
        else if(tutorialState == 68)babySitter.ShowUpTipTutorial("1-One of the Kings die");
        else if(tutorialState == 69)babySitter.ShowUpTipTutorial("2-One of the sides has less than 4 pieces");
        else if(tutorialState == 70)babySitter.ShowUpTipTutorial("3-One of the sides' King reveals. If you claim bluff to a legal King move, you lose.");
        else if(tutorialState == 71)babySitter.ShowUpTipTutorial("I think you are ready to beat your opponents !");
        else if(tutorialState == 72)babySitter.ShowUpTipTutorial("Have fun !");




    }

    public void NextButtonOnClick()
    {
        tutorialState ++;
    }

    public void PassButtonOnClick()
    {
        if(tutorialState == 53)
        {
            
            AddImageToGraveyard("Knight",1,allUnits[0].transform.position);
        }
        else if(tutorialState == 61)
        {
            AddImageToGraveyard("Queen",1,allUnits[2].transform.position);
            AddImageToGraveyard("King",2,allUnits[2].transform.position);

        }


        tutorialState ++;
    }

    void SetArrowPosition(Vector2 pos)
    {
        arrowImage.centerX = pos.x;
        arrowImage.centerY = pos.y;

        
        if(Mathf.Abs(arrowImage.transform.position.x - arrowImage.centerX) > 0.1f)
        {
            arrowImage.transform.position = pos;
        }



    }

    GameObject GetBlockUnderCursor()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
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


        //Vector3 targetLoc;

        if(playerNo == 1/*myPlayer.playerNo*/)
        {
            if(scoreNo == 1)
            {
               // targetLoc = myScore1.transform.position;
            }
            else
            {
              //  targetLoc = myScore2.transform.position;
            }
        }
        else
        {
            if(scoreNo == 1)
            {
              //  targetLoc = enemyScore1.transform.position;
            }
            else
            {
               // targetLoc = enemyScore2.transform.position;
            }
        }




       // graveyardAnimationCoroutine = CarryImageFromTo(gainingScore.GetComponent<SpriteRenderer>(),gainingScore.transform.position,targetLoc,0.3f);
       // StartCoroutine(graveyardAnimationCoroutine);
        yield return new WaitForSeconds(1.5f);
        gainingScore.GetComponentInChildren<Blink>().onlyDown = true;
        yield return new WaitForSeconds(2f);

        gainingScore.GetComponentInChildren<Blink>().Reset();
        gainingScore.SetActive(false);
        gainingScore.transform.position = gainingScoreStartPos;
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
}
