using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handler for players
public class playerHandler : handler
{
    protected Slider playerHP;

    [SerializeField]
    protected playerUnit playerObj;

    // Player's controls
    protected playerControls controlPlayer1;
    protected string[] defaultTapKeys = {"h","j","k","l"};

    // constructor
    public playerHandler() {
    }

    // Start is called before the first frame update
    void Start()
    {
        // create default controls for player
        controlPlayer1 = new playerControls(
            "w", "s", "a", "d",
            "u", "i", "space", "shift", "p",
            defaultTapKeys
        );
        
        // assign the control set to player 1
        playerObj.Controls = controlPlayer1;

        // get the HP bar of the player
        playerHP = GameObject.Find("PlayerHP").GetComponent<Slider>();
        playerHP.value = playerObj.Player.HP;

        do {
            if (playerObj != null) {
                // register to event when player gets damaged
                playerUnit.updatePlayerHPBar += updatePlayerHPBar;
                Debug.Log("Registering updatePlayerHPBar event...");
            }
        } while (playerObj == null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // update HP Bar after getting damaged;
    protected void updatePlayerHPBar() {
        // Debug.Log("Updating Player HP Bar: " + playerObj.Player.HP);
        playerHP.value = playerObj.Player.HP;
    }
}

// class to contain player controls
public class playerControls {
    protected string moveUp, moveDown, moveLeft, moveRight, attack, defend, skillsync, dodge, pause;
    protected string[] tapButtons = new string[4];

    // constructor
    public playerControls(
        string _up, string _down, string _left, string _right, 
        string _attack, string _defend, string _sync, string _dodge, string _pause,
        string[] tapbuttons
    ) {
        moveUp = _up;
        moveDown = _down;
        moveLeft = _left;
        moveRight = _right;
        attack = _attack;
        defend = _defend;
        skillsync = _sync;
        dodge = _dodge;
        pause = _pause;
        tapButtons = tapbuttons;
    }

    // getters & setters
    public string MoveUp {
        get { return moveUp; }
        set { moveUp = value; }
    }
    public string MoveDown {
        get { return moveDown; }
        set { moveDown = value; }
    }
    public string MoveLeft {
        get { return moveLeft; }
        set { moveLeft = value; }
    }
    public string MoveRight {
        get { return moveRight; }
        set { moveRight = value; }
    }
    public string Attack {
        get { return attack; }
        set { attack = value; }
    }
    public string Defend {
        get { return defend; }
        set { defend = value; }
    }
    public string Skillsync {
        get { return skillsync; }
        set { skillsync = value; }
    }
    public string Dodge {
        get { return dodge; }
        set { dodge = value; }
    }
    public string Pause {
        get { return pause; }
        set { pause = value; }
    }
    public string[] TapButtons {
        get { return tapButtons; }
        set { tapButtons = value; }
    }
}

