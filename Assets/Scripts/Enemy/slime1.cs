using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime1 : enemy
{
    public gameUnit tempTarget; // use for testing only
    public bool isExplosive, isShocking, isVenomous;

    private float distance, atkAnimSpd, burstMoveSpeed;

    // constructor
    public slime1 (int level = 1) {
        if (currentTarget == null) currentTarget = tempTarget;
        Level = level;
        atkAnimSpd = 3;
        ATKTimer = 0;
        ATKdelay = 1;
        ATKbase = 5;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        atkAnimSpd = 3;
        ATKTimer = 0;
        ATKdelay = 1;
        ATKbase = 5;
        // currentTarget = tempTarget;
        targetPosition = setNewTargetPosition(roamRangeX, roamRangeY); // setup a roam range
        roamTimer = Time.time + roamDelay; // setup a roam timer if the roam position is not reachable\
        UpdateTarget();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // roam anywhere until 
        if (getTargetDistance() >= chaseRange) freeRoamToPoint();
        else chaseCurrentTarget();
    }

    // called within the slime's run animation
    public void addBurstMoveSpeed()
    {
        burstMoveSpeed = moveSpeed;
    }

    // override the normal roam behavior and make it "jump" during its animation instead
    protected override void freeRoamToPoint() {
        // keep roaming until position is reached or roam timer runs out, then change roaming behavior again 
        if (Time.time < roamTimer) {
            
            if (getTargetDistance(targetPosition) > 1f) {
                // transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, burstMoveSpeed * Time.deltaTime);
                burstMoveSpeed = burstMoveSpeed > 0 ? burstMoveSpeed - 0.5f : 0; // gradually remove the burst movespeed until it is 0
            }
        }
        else {
            targetPosition = setNewTargetPosition(roamRangeX, roamRangeY);
            roamTimer = Time.time + roamDelay;
        }
    }

    // override the normal chase behavior and make it "jump" during its animation instead
    protected override void chaseCurrentTarget() {
        if (currentTarget) {
            if (currentTarget.IsAlive) {
                doOnChaseTarget();
                // keep moving until target is in engaging distance
                if (getTargetDistance() >= engageRange) {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, burstMoveSpeed * Time.deltaTime);
                    burstMoveSpeed = burstMoveSpeed > 0 ? burstMoveSpeed - 0.5f : 0; // gradually remove the burst movespeed until it is 0
                }
                else doOnReachTarget();
            }
        }
    }

    // bat 1 behavior
    protected override void doOnReachTarget() {
        if (currentTarget.IsAlive) {
            distance = getTargetDistance();
            if (distance <= engageRange) {
                // bite the player with an animation sequence
                if (Time.time >= ATKTimer) {
                    ATKTimer = Time.time + ATKdelay; // delay the next attack using the ATKDelay
                    if (!isExplosive) {
                        StartCoroutine(biteAttack());
                        showDamage(ATKbase, target.position);
                        damageTarget(ATKbase);
                    }
                }
            }
        }
    }

    protected IEnumerator biteAttack ()
    {
        Vector2 startPos = transform.position, endPos = target.position;
        float percent = 0, animFormula;
        while (percent <= 1) {
            percent += (atkAnimSpd * Time.deltaTime);
            animFormula = (-Mathf.Pow(percent,3) + percent) * 4;
            transform.position = Vector2.Lerp(startPos, endPos, animFormula);
            yield return null;  // don't return any time to make the animation instant;
        }
    }

    // bat 1 gets damaged
    protected override void doOnTakeDamage(float DMG) {        
        if (HP >= 0) {
            HP -= DMG;
            if (HP <= 0) {
                this.isAlive = false;
                doOnDeath();
            }
        }
    }

    // Check if the monster is explosive or shocking before destroying object
    protected override void doOnDeath() {
        explosion boomOnDeath;
        if (isExplosive) {
            boomOnDeath = (explosion)Instantiate(deathEffect, transform.position, transform.rotation);
            boomOnDeath.setExplosionTarget("Player", ATKbase);
        }
        Destroy(gameObject);
    }

}
