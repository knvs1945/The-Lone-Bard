using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHandler : handler
{
    
    [SerializeField]
    protected playerHandler playerHandle;
    [SerializeField]
    protected rhythmHandler rhythmHandle;
    [SerializeField]
    protected enemyHandler enemyHandle;
    // Start is called before the first frame update

    void Start()
    {
        // fix framerate to default FPS (30);
        Application.targetFrameRate = FPS;

        // test setBPM
        rhythmHandler.setBPM(120);
        rhythmHandle.playMetronome();
        rhythmHandle.playTrack(1);
        enemyHandle.startGenerator(0,1); // start spawning monsters

        consoleUI.Log("State: " + gameHandler.getGameState());
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
}
