using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : Handler
{
    [SerializeField]
    protected List<Transform> gatekeeperSpawns;

    protected List<Gatekeeper> currentGKs;

    // getters and setters
    public List<Transform> GatekeeperSpawns {
        get { return gatekeeperSpawns; }
    }

    public List<Gatekeeper> CurrentGKs {
        get { return currentGKs; }
        set { currentGKs = value; }
    }

    protected void Start() {
        currentGKs = new List<Gatekeeper>();
    }

    // TO-DO: update so that after checking goals, road blocks can be removed
    public override void goalCompleted (Goal goal) { }
    public override void goalFailed (Goal goal) { }


}
