using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSlash : Projectiles
{
    // Start is called before the first frame update
    protected string targetTag;
    protected float pushbackMin = 1f;
    protected override void Start()
    {
      flashEffect = false;
      base.Start();   
    }

    // Update is called once per frame
    /* void Update()
    {
        
    } */

    // slash has hit a target. targets is from projectiles superclass
    protected override void doOnHitTarget(Collider2D collision) {
      Debug.Log("collision tag: " + collision.tag);
      targetTag = collision.tag; 
      if (targets.Contains(targetTag)) {
          // Debug.Log("Target collided: " + targetTag + " - " + targets[i] + " - " + DMG);
          pushBack(collision, transform, pushbackMin);
          switch (targetTag) {
              case "Enemy":
              case "enemy":
              case "Boss":
              case "boss":
                  Enemy enemy = collision.GetComponent<Enemy>();
                  if (enemy != null) enemy.takesDamage(DMG);
                  animRemoveEffect();
                  break;
          }
      }
    }
}
