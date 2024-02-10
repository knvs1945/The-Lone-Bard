using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Game States
public enum states
{
    MainMenu,
    inStage,
    inEliteBattle,
    inBossBattle,
    inMap,
    Victory,
    Defeat,
    GameWin,
    GameLoss,
    GameEnd,
    Cutscene,
    Preload
}

public class Handler : MonoBehaviour
{
    protected static states gameState = states.Preload; // default the gameState to preload

    protected static bool pauseGame = false;
    protected static int FPS = 30;
    protected static float stageIntroTimer = 3f;
}
