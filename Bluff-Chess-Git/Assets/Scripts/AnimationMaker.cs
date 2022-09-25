using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMaker : MonoBehaviour
{
    [SerializeField] MenuAnimationPiece[] pieces;


    class PieceAction
    {
        public int pieceID;
        public float actionTime;
        public int stepID;
        public int stepAmount;
        public float speedFactor;
        
        float startActionTime;
        public bool done;
        

        public PieceAction(int pieceID,float actionTime,int stepID,int stepAmount,float speedFactor)
        {
            this.pieceID = pieceID;
            this.actionTime = actionTime;
            this.stepID = stepID;
            this.stepAmount = stepAmount;
            this.speedFactor = speedFactor;
            startActionTime = actionTime;
        }


        public void ResetTime()
        {
            actionTime = startActionTime;
            done = false;
        }

    }

    IEnumerator resetCoroutine;
    bool coroutineActive = false;


    List<PieceAction> pieceActions = new List<PieceAction>();
    float timer = 0;


    void Start()
    {
        //PieceAction(int pieceID,float actionTime,int stepID,int stepAmount,float speedFactor)


        /*
        1 -> right down
        2 -> right up
        3 -> left down
        4 -> left up
        */

        pieceActions.Add(new PieceAction(0,0,1,2,1));
        pieceActions.Add(new PieceAction(2,0.3f,3,2,1));
        pieceActions.Add(new PieceAction(2,4.2f,2,2,1));
        pieceActions.Add(new PieceAction(4,2.1f,4,1,1));
        pieceActions.Add(new PieceAction(4,3.3f,1,1,1));
        pieceActions.Add(new PieceAction(0,2.4f,3,1,1));
        pieceActions.Add(new PieceAction(1,1.1f,2,3,1));
        pieceActions.Add(new PieceAction(1,3.5f,3,1,1));
        pieceActions.Add(new PieceAction(1,4.5f,3,2,1));
        pieceActions.Add(new PieceAction(3,2,3,1,1));
        pieceActions.Add(new PieceAction(3,3.4f,4,1,1));
        pieceActions.Add(new PieceAction(3,4.7f,1,1,1));
        pieceActions.Add(new PieceAction(3,5.9f,2,1,1));
        pieceActions.Add(new PieceAction(0,3.6f,4,1,1));
        pieceActions.Add(new PieceAction(0,4.4f,2,1,1));
        pieceActions.Add(new PieceAction(0,5.5f,4,1,1));
    }

    void Update()
    {
        CheckPieceActions();
    }


    void CheckPieceActions()
    {
        timer += Time.deltaTime;
        foreach(PieceAction pieceAction in pieceActions)
        {
            if(pieceAction.actionTime < timer && !pieceAction.done)
            {
                pieceAction.done = true;
                pieces[pieceAction.pieceID].MoveStepAndWait(pieceAction.stepID,pieceAction.stepAmount,pieceAction.speedFactor);
            }



        }
        if(AllActionsDone())ResetPieceActions();
    }


    bool AllActionsDone()
    {
        foreach(PieceAction pieceAction in pieceActions)
        {
            if(!pieceAction.done) return false;
        }

        return true;
    }

    IEnumerator ResetAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        foreach(PieceAction pieceAction in pieceActions)
        {
            pieceAction.ResetTime();
        }
        foreach(MenuAnimationPiece piece in pieces)
        {
            piece.ResetPosition();
        }
        timer = 0;
        coroutineActive = false;
    }

    void ResetPieceActions()
    {
        if(coroutineActive) return;
        
        coroutineActive = true;
        resetCoroutine = ResetAfter(1);
        StartCoroutine(resetCoroutine);   
    }



}
