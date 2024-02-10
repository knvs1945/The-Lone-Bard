using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : Handler
{
    
    [SerializeField]
    protected PlayerHandler playerHandle;
    [SerializeField]
    protected RhythmHandler rhythmHandle;
    [SerializeField]
    protected EnemyHandler enemyHandle;
    [SerializeField]
    protected GoalHandler goalHandle;
    private bool stagePrepFlag;


    // Start is called before the first frame update

    void Start()
    {
        // fix framerate to default FPS (30);
        Application.targetFrameRate = FPS;

        // test setBPM
        RhythmHandler.setBPM(60);
        consoleUI.Log("State: " + GameHandler.getGameState());

        // move this somewhere else once startLevel works
        startLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    // get gameState
    public static states getGameState() {
        return gameState;
    }

    public static bool isGamePaused() {
        return pauseGame;
    }

    /* 
    *
    *   Start the stage sequences
    *
    */
    public bool startLevel() {
        stagePrepFlag = false;
        if (gameState != states.inStage) {
            // only change the gameState if the stage preps completes
            StartCoroutine(startStagePreps(result => {
                if (result) {
                    Debug.Log("Start Stage Preparations completed");
                    gameState = states.inStage;
                }
                else Debug.Log ("An error occurred during stage preparation.");
            }));
        }
        return stagePrepFlag;
    }

    // prep sequences
    IEnumerator startStagePreps(Action<bool> checkPrepDone) {
        yield return StartCoroutine(playerStagePrep());
        
        checkPrepDone(stagePrepFlag); // all coroutines returned properly so we return true
    }
    
    IEnumerator playerStagePrep() {
        bool success = false;
        if (playerHandle.startStageSequence()) {
            // call rhythm stage prep next
            yield return StartCoroutine(rhythmStagePrep());
            success = true;
        }
        else stagePrepFlag = true;
        yield return success;
    }

    IEnumerator rhythmStagePrep() {
        
        yield return new WaitForSeconds(stageIntroTimer + 1);
        bool success = false;
        rhythmHandle.playMetronome();
        rhythmHandle.playTrack(1);
        enemyHandle.startGenerator(0,1); // start spawning monsters    
        success = true;
        // yield return success;
    }


    
}
