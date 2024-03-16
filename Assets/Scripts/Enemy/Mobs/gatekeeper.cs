using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatekeeper : Enemy
{
    public List<Enemy> defenders = new List<Enemy>();

    protected Pathblocker ownedPathblocker;
    protected bool hasProtectors = true;

    // Getters and Setters
    public Pathblocker OwnedPathblocker {
        get { return OwnedPathblocker; }
        set { ownedPathblocker = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        isImmune = false;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!isEnemyEnabled) return;
        checkDefendersStatus();
    }

    // add to defenders list
    public void addAsDefender(Enemy mob) {
        defenders.Add(mob);
        hasProtectors = true;
    }

    // check if defenders are still active
    protected void checkDefendersStatus() {
        defenders.RemoveAll(mob => !mob.IsAlive); // remove any defenders that are not alive
        if (defenders.Count <= 0) {
            hasProtectors = false;
        }
    }

    // open assigned pathblocker if available
    public bool openPathblocker() {
        Debug.Log("Opening pathblocker: " + ownedPathblocker);
        if (ownedPathblocker != null) ownedPathblocker.open();
        return true;
    }

    // release assigned pathblocker
    public bool releasePathBlocker() {
        if (ownedPathblocker != null) {
            // ownedPathblocker.remove();
            ownedPathblocker = null;
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    // obelisk gets damaged. Only takes damage if it has no more defenders
    protected override void doOnTakeDamage(float DMG) {
        if (hasProtectors) {
            // TODO - Add line creation animation to point out where the rest of the guards are
            Debug.Log("Gatekeeper still has guards: " + defenders.Count);
        }
        else if (HP > 0) {
            HP -= DMG;
            if (HP <= 0) {
                this.isAlive = false;
            }
        }
    }
}
