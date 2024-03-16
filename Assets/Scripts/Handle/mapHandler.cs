using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : Handler
{
    [SerializeField]
    protected List<Transform> gatekeeperSpawns;

    [SerializeField]
    protected List<Transform> pathblockSpawns;

    [SerializeField]
    protected List<Pathblocker> pathblockers;

    protected List<Gatekeeper> currentGKs;

    // getters and setters
    public List<Transform> GatekeeperSpawns {
        get { return gatekeeperSpawns; }
    }

    public List<Gatekeeper> CurrentGKs {
        get { return currentGKs; }
        set { currentGKs = value; }
    }

    public List<Transform> PathblockSpawns {
        get { return pathblockSpawns; }
    }

    public List<Pathblocker> Pathblockers {
        get { return Pathblockers; }
    }

    protected void Start() {
        currentGKs = new List<Gatekeeper>();
    }

    // Spawn pathblockers here
    public void spawnPathblockers(Gatekeeper owner, int i) {
        Pathblocker temp;
        temp = Instantiate(pathblockers[Random.Range(0, pathblockers.Count -1)], pathblockSpawns[i].position, Quaternion.identity);
        if (temp != null) {
            owner.OwnedPathblocker = temp;
        }
    }

    // TO-DO: update so that after checking goals, road blocks can be removed
    public override void goalCompleted (Goal goal) {
        
        // get target from goal
        Gatekeeper temp = (Gatekeeper) goal.Target;
        
        Gatekeeper affectedGK;
        if (temp != null) {
            temp.openPathblocker();
            temp.releasePathBlocker();
        }
     }



    public override void goalFailed (Goal goal) { }


}
