using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameUnit
{
    public static bool isEnemyEnabled = false;
    protected static bool isDamageTextVisible = true;

    public DmgEffect damageText;
    public Effects deathEffect;
    public float engageRange, chaseRange, decisionDelay = 1, roamRangeX, roamRangeY, roamDelay = 3;
    
    protected GameUnit currentTarget;
    protected Transform target;
    protected Vector2 targetPosition, Direction;
    protected DmgEffect tempEffect;
    protected bool isBoss, isFlipped = false;
    protected string description;
    protected float roamTimer;
    

    // getters & setters
    public bool IsBoss {
        get { return isBoss; }
    }

    public string Description {
        get { return description; }
    }

    public GameUnit CurrentTarget {
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

    // Make the sprite look left or right based on mouse position
    protected void flipSprite()    
    {
        if (!isFlipped) {
            if (checkTargetPos() <= 0)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                isFlipped = true;
            }
        }
        else {
            if (checkTargetPos() > 0)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                isFlipped = false;
            }
        }
    }

    // Flip the sprite based on the target point it is going
    protected void flipSprite(Vector2 targetPos)    
    {
        if (!isFlipped) {
            if (checkTargetPos(targetPos) <= 0)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                isFlipped = true;
            }
        }
        else {
            if (checkTargetPos(targetPos) > 0)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                isFlipped = false;
            }
        }
    }

    // make the enemy check where the target is
    protected int checkTargetPos()
    {
        return (checkXDirection(currentTarget.transform.position) == 1) ? 1 : 0;
    }

    // check where the specific spot is
    protected int checkTargetPos(Vector2 targetPosition)
    {
        return (checkXDirection(targetPosition) == 1) ? 1 : 0;
    }

    // check if the player is left or right of the player and adjust accordingly
    protected int checkXDirection (Vector2 targetPoint)
    {   
        Direction = new Vector2(targetPoint.x - transform.position.x, 0);
        if (Direction.normalized.x >= 0) return 0; // 1 for right
        return 1;
    }


}
