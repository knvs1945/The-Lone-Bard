using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for effects that contain damaging properties
public class Projectiles : Effects
{
    public string[] targets;

    protected Vector2 startPos;
    protected float dmg, spd, range = 10f;

    public float DMG {
        get { return dmg; }
        set {
            if (value >= 0) dmg = value; 
        }
    }

    public float SPEED {
        get { return spd; }
        set { 
            spd = value; 
            Debug.Log("SPEED UPDATED: " + spd);
        }
    }

    public float RANGE {
        get { return range; }
        set {
            if (value >= 0) range = value;
        }
    }

    // Called when projectile hits a target
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision);
        doOnHitTarget(collision);
    }

    // apply pushback onto a target
    protected virtual void pushBack(Collider2D target, Transform source, float power)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null) {
            Vector2 direction = target.transform.position - source.position;
            direction.Normalize();
            rb.AddForce(direction * (rb.drag + power), ForceMode2D.Impulse);
        }
    }

    
    // move the projectile until it collides or disappears
    protected virtual void moveUntilCollide()
    {
        if (getDistanceFromOrigin() <= range) transform.Translate(Vector2.down * spd * Time.deltaTime);
        else { doOnMaxRange(); }
    }

    // get distance from Origin
    protected virtual float getDistanceFromOrigin() {
        return Vector2.Distance(transform.position, startPos);
    }


    // overrideable behavior for children
    protected virtual void doOnHitTarget(Collider2D collision) {}
    protected virtual void doOnMaxRange() {}


    }
