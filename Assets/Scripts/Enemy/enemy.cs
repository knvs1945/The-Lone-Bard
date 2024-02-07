using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : gameUnit
{
    protected static bool isDamageTextVisible = true;

    public dmgEffect damageText;
    public effects deathEffect;
    public float engageRange, chaseRange, decisionDelay = 1, roamRangeX, roamRangeY, roamDelay = 3;
    

    protected gameUnit currentTarget;
    protected Transform target;
    protected Vector2 targetPosition;
    protected dmgEffect tempEffect;
    protected bool isBoss;
    protected string description;
    protected float roamTimer;
    

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
    
    // used mainly for free roam change target point
    protected virtual float getTargetDistance(Vector2 targetPoint) {
        return Vector2.Distance(transform.position, targetPoint);
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
    // free roam e.g. when not within chasing range
    protected virtual void freeRoamToPoint() {
        // keep roaming until position is reached or roam timer runs out, then change roaming behavior again 
        if (getTargetDistance(targetPosition) > 1f && roamTimer <= Time.time) transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        else {
            targetPosition = setNewTargetPosition(roamRangeX, roamRangeY);
            roamTimer = Time.time + roamDelay;
        }
    }

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
    protected virtual void doOnDeath(){}

    // miscenalleneous functions
    
    // change roam target based on random value near current position
    protected Vector2 setNewTargetPosition(float rangeX, float rangeY) {
        return new Vector2( Random.Range(transform.position.x - rangeX, transform.position.x + rangeX), 
                            Random.Range(transform.position.y - rangeY, transform.position.y + rangeY));
    }

    // change roam target based on min-max X and Y range
    protected Vector2 setNewTargetPosition(float minX, float minY, float maxX, float maxY) {
        return new Vector2( Random.Range(minX, maxX),
                            Random.Range(minY, maxY));
    }


}
