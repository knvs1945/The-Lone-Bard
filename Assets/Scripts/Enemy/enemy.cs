using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : gameUnit
{
    protected static bool isDamageTextVisible = true;

    public float engageRange;
    public dmgEffect damageText;

    protected gameUnit currentTarget;
    protected Transform target;
    protected dmgEffect tempEffect;
    protected bool isBoss;
    protected string description;
    

    // getters & setters
    public bool IsBoss {
        get { return isBoss; }
    }

    public string Description {
        get { return description; }
    }

    public gameUnit CurrentTarget {
        get { return currentTarget; }
        set { currentTarget = value; }
    }


    // Start is called before the first frame update
    protected virtual void Start() {
        
    }

    // common functions
    protected bool checkHPifAlive() {
        if (isAlive && !isNPC) {
            return HP > 0 ? true : false;
        }
        return false;
    }

    // always call if the target is updated     
    public void UpdateTarget() {
        if (currentTarget) {
            target = currentTarget.transform;
        }
    }

    protected virtual float getTargetDistance() {
        return Vector2.Distance(transform.position, target.position);
    }

    // show damage text on the spot
    protected virtual void showDamage(float value) {
        // create damage text on the spot
        if (!isDamageTextVisible) return;
        tempEffect = Instantiate(damageText, transform.position, Quaternion.identity);
        tempEffect.setText(value);
    }

    // show damage text on a specific point
    protected virtual void showDamage(float value, Vector2 position) {
        // create damage text on the position
        if (!isDamageTextVisible) return;
        tempEffect = Instantiate(damageText, position, Quaternion.identity);
        tempEffect.setText(value);
    }

    // Overrideable functions
    // move towards target using base movespeed;
    protected virtual void chaseCurrentTarget() {
        if (currentTarget) {
            
            if (currentTarget.IsAlive) {
                doOnChaseTarget();
                // keep moving until target is in engaging distance
                if (getTargetDistance() >= engageRange) transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                else doOnReachTarget();
            }
        }
    }

    // damage the target 
    protected virtual void damageTarget(float value) {
        if (currentTarget.IsAlive) {
            currentTarget.takesDamage(value);
        }
    }

    protected virtual void doOnChaseTarget(){}
    protected virtual void doOnReachTarget(){}

}
