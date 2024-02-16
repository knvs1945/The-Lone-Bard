using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectFireball : Projectiles
{
    // public values on editor
    public float fireballSpd, fireballRange;

    protected string targetTag;
    protected float pushbackMin = 1f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        spd = fireballSpd;
        range = fireballRange;
    }

    // Update is called once per frame
    void Update()
    {
        moveUntilCollide();
    }

    // check if the fireballs hit anything
    protected override void doOnCollideTarget(Collision2D collision) {

        Debug.Log("collision tag: " + collision.gameObject.tag);
        targetTag = collision.gameObject.tag; 
        if (targets.Contains(targetTag)) {
            pushBack(collision.collider, transform, pushbackMin);
            switch (targetTag) {
                case "Enemy":
                case "enemy":
                case "Boss":
                case "boss":
                    Enemy enemy = collision.collider.GetComponent<Enemy>();
                    if (enemy != null) enemy.takesDamage(DMG);
                    animRemoveEffect();
                    break;
            }
        }

        // check if it hits obstacles and remove it.
        if (checkIfCollideWithObstacle(collision.gameObject.tag)) animRemoveEffect();
    }

    // remove game object after reaching max range;
    protected override void doOnMaxRange()
    {
        Destroy(gameObject);
    }

    

}
