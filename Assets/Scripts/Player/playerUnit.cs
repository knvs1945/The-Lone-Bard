using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUnit : gameUnit
{    
    protected playerControls controls;

    [SerializeField]
    private playerUnit player;

    // Event Delegates here
    public delegate void onPlayerTakesDamage(); 

    // Events start here
    public static event onPlayerTakesDamage updatePlayerHPBar; 

    // Start is called before the first frame update
    protected virtual void Start()
    {
        isImmune = false;
        Debug.Log("Player check: " + player);
    }

    public playerControls Controls {
        get { return controls; }
        set { 
            controls = value; 
            if (player != null) player.Controls = controls;    
        }
    }

    public playerUnit Player {
        get { return player;  }
        set { player = value; }
    }

    // common functions
    protected bool checkHPifAlive() {
        if (isAlive && !isNPC) {
            return isAlive;
        }
        return false;
    }

    protected void updateHPBar() {
        // if (updatePlayerHPBar != null) {
        consoleUI.Log("Updating HP Bar: " + HP);
        playerUnit.updatePlayerHPBar();    // inform the playerHandler that the HP bar needs updating
        // }
    }

}
