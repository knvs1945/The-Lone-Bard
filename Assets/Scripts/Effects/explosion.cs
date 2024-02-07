using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : effects
{
    public string targetTag = "Enemy";

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public void setExplosionTarget(string tag, float DMG)
    {
        Debug.Log("Explosion Damage Set: " + DMG);
        addSplashDMG(DMG);
        targetTag = tag;
    }

    // call this to explode 
    public void explodeOnTargets()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        // only do damage if the area damage used is set more than 0 and if there are targets found
        
        if (splashDMG > 0 && targets.Length > 0) {
            for (int i = 0; i < targets.Length; i++) {
                if (checkTargetsAroundSplash(targets[i].transform)) {
                    targets[i].GetComponent<gameUnit>().takesDamage(splashDMG); // deal damage to that unit
                }
            }
        }
    }
    
}
