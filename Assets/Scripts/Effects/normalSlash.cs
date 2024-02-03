using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalSlash : projectiles
{
    // Start is called before the first frame update
    protected string targetTag;
    protected override void Start()
    {
      flashEffect = false;
      base.Start();   
    }

    // Update is called once per frame
    /* void Update()
    {
        
    } */

    // slash has hit a targets
    protected override void doOnHitTarget(Collider2D collision) {
      if (targets.Length > 0) {
        for (int i = 0; i < targets.Length; i++) {
          targetTag = collision.tag; 
          Debug.Log("Target collided: " + targetTag + " - " + targets[i] );
          if (targetTag == targets[i]) {
            collision.GetComponent<enemy>().takesDamage(DMG);
          }
        }
      }
    }
}
