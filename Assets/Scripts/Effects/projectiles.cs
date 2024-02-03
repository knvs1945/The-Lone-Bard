using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for effects that contain damaging properties
public class projectiles : effects
{
    public string[] targets;

    protected float dmg, spd;

    public float DMG {
        get { return dmg; }
        set {
            if (value >= 0) dmg = value; 
        }
    }

    public float SPEED {
        get { return spd; }
        set { dmg = spd; }
    }

    // Called when projectile hits a target
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision);
        doOnHitTarget(collision);
    }

    // overrideable behavior for children
    protected virtual void doOnHitTarget(Collider2D collision) {}

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

}
